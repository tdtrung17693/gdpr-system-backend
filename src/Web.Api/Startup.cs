using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Web.Api.Core;
using Web.Api.Core.Interfaces.UseCases;
using Web.Api.Core.UseCases;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Infrastructure.Data.EntityFramework.Repositories;
using Web.Api.Extensions;
using Web.Api.Infrastructure;
using Web.Api.Infrastructure.Auth;
using Web.Api.Infrastructure.Data.EntityFramework;
using Web.Api.Presenters;
using Web.Api.Auth;
using Web.Api.Auth.RequirementHandlers;
using Web.Api.Infrastructure.Helpers;
using Web.Api.Core.Interfaces.Services;
using System.Collections.Generic;
using Web.Api.Core.Dto;
using Web.Api.Serialization;
using Web.Api.Core.Interfaces.UseCases.ServerInterface;

namespace Web.Api
{
  public class Startup
  {
    private const string SecretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
    private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
    readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public IServiceProvider ConfigureServices(IServiceCollection services)
    {
      CheckRequiredConfiguration();

      //Config CORS
      services.AddCors(options =>
      {
        options.AddPolicy(name: MyAllowSpecificOrigins, build =>
          {
            build.WithOrigins(
                "http://localhost:3000",
                "http://localhost:3000/servers"
              )
              .AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
          });
      });

      // Add framework services.
      var connectionString = Configuration.GetConnectionString("Default");
      services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(
        connectionString, b => b.MigrationsAssembly("Web.Api.Infrastructure")));
      // jwt wire up
      // Get options from app settings
      var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

      // Configure JwtIssuerOptions
      services.Configure<JwtIssuerOptions>(options =>
      {
        options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
        options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
        options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
      });

      var tokenValidationParameters = new TokenValidationParameters
      {
        ValidateIssuer = true,
        ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

        ValidateAudience = true,
        ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = _signingKey,

        RequireExpirationTime = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
      };

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

      }).AddJwtBearer(configureOptions =>
      {
        configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
        configureOptions.TokenValidationParameters = tokenValidationParameters;
        configureOptions.SaveToken = true;
        configureOptions.Events = new JwtBearerEvents
        {
          OnTokenValidated = async context =>
                {
                  var uid = context
                            .Principal
                            .Claims
                            .Single(c => c.Type == Constants.Strings.JwtClaimIdentifiers.Id).Value;
                  var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
                  await authService.LogIn(Guid.Parse(uid));
                }
        };
      });

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
      services.AddMvc()
        .AddJsonOptions(
            options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );
      services.AddAutoMapper();
      services.AddSingleton<IAuthorizationPolicyProvider, HavePermissionProvider>();
      services.AddSingleton(typeof(ResourcePresenter<>), typeof(ResourcePresenter<>));
      services.AddScoped<IAuthorizationHandler, PermissionHandler>();

      // Register the Swagger generator, defining 1 or more Swagger documents
      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "GDPR System API", Version = "v1" });
      });

      // Now register our services with Autofac container.
      var builder = new ContainerBuilder();

      builder.RegisterModule(new CoreModule());
      builder.RegisterModule(new InfrastructureModule());

      // Presenters
      builder.RegisterType<RegisterUserPresenter>().SingleInstance();
      builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => t.Name.EndsWith("Presenter")).SingleInstance();

      builder.Populate(services);
      var container = builder.Build();
      // Create the IServiceProvider based on the container.
      return new AutofacServiceProvider(container);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseExceptionHandler(
          builder =>
          {
            builder.Run(
                      async context =>
                      {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                          context.Response.AddApplicationError(error.Error.Message);
                          await context.Response.WriteAsync(JsonSerializer.SerializeObject(new
                          {
                            Error = new Error(Error.Codes.UNKNOWN, error.Error.Message)
                          })).ConfigureAwait(false);
                        }
                      });
          });

      //add file link swagger
      app.UseStaticFiles();
      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "GDPR System API V1");
      });


      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();
      app.UseAuthentication();
      // app.UseJwtTokenMiddleware();


      app.UseCors(MyAllowSpecificOrigins);
      app.UseMvc();

    }

    protected void CheckRequiredConfiguration()
    {
      var configuration = Configuration;
      var requiredConfiguration = new List<string> {
        "Mail:StmpServer"
      };

      //requiredConfiguration.All(c => configuration.Contains(c));
    }
  }
}