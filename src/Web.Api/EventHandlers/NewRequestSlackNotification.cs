using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SlackAPI;
using SlackBotMessages;
using SlackBotMessages.Enums;
using SlackBotMessages.Models;

using Web.Api.Core.Domain.Event;
using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Infrastructure.Data.EntityFramework;

namespace Web.Api.EventHandlers
{
    public class NewRequestSlackNotification : IEventHandler<RequestCreated>
    {
        private IServiceProvider _provider;
        private IConfiguration _configuration;

        public NewRequestSlackNotification(IServiceProvider provider, IConfiguration configuration)
        {
            _provider = provider;
            _configuration = configuration;
        }

        public async Task HandleAsync(RequestCreated ev)
        {
            using (var scope = _provider.CreateScope())
            using (var ctx = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
            {
            var slackToken = _configuration.GetValue<string>("Slack:Token");
            var slackWebHook = _configuration.GetValue<string>("Slack:WebHook");

            if (string.IsNullOrEmpty(slackToken) || string.IsNullOrEmpty(slackWebHook)) return;
            
            var admin = await ctx.User
                .Include(user => user.Role)
                .Where(user => user.Role.Name == "Administrator" && user.Id != ev.RequesterId)
                .Select(u => u.Email)
                .ToListAsync();

            var apiClient = new SlackTaskClient(slackToken);
            await apiClient.ConnectAsync();
            var client = new SbmClient(slackWebHook);
            var message = new SlackBotMessages.Models.Message()
            {
                Username = $"{ev.RequesterUsername}",
                Text = $"Request access to the server *{ev.ServerName}*"
            };

            message.AddAttachment(new SlackBotMessages.Models.Attachment()
                .SetColor(Color.Green)
                .AddField("User", ev.RequesterUsername)
                .AddField("Created At", ev.CreatedAt.ToLocalTime().ToString("dd/MM/yyyy HH:mm"))
                .AddField("", new SlackLink($"http://localhost:3000/requests/editrequest/{ev.RequestId}","View Request").ToString()));

            foreach (var adminEmail in admin)
            {
                var user = await apiClient.GetUserByEmailAsync(adminEmail);
                if (!user.ok) continue;
                message.Channel = $"@{user.user.name}";
                await client.SendAsync(message);
                await Task.Delay(2000);
            }
            }
        }
    }
}
