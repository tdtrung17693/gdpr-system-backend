﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core.Interfaces.Services;

namespace Web.Api.Infrastructure.Data.EntityFramework
{
    public class DbContextFactory : DesignTimeDbContextFactoryBase<ApplicationDbContext>
    {
        private IHttpContextAccessor _httpContext ;
    public DbContextFactory() { }
        public DbContextFactory(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        protected override ApplicationDbContext CreateNewInstance(DbContextOptions<ApplicationDbContext> options)
        {
            //return new ApplicationDbContext(options, _authService);
            return new ApplicationDbContext(options, _httpContext);
        }
    }

    //public class DbContextFactory : DesignTimeDbContextFactoryBase<GdprContext>
    //{
    //    protected override GpdrContext CreateNewInstance(DbContextOptions<GdprContext> options)
    //    {
    //        return new AppliContext(options);
    //    }
    //}
}
