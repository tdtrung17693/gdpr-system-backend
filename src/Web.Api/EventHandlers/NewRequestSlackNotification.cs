using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SlackAPI;
using SlackBotMessages;
using SlackBotMessages.Enums;
using SlackBotMessages.Models;
using Web.Api.Core.Domain.Event;
using Web.Api.Core.Dto.Requests;

using Web.Api.Core.Interfaces.Services.Event;
using Web.Api.Infrastructure.Data.EntityFramework;

namespace Web.Api.EventHandlers
{
    public class NewRequestSlackNotification : IEventHandler<RequestCreated>
    {
        private readonly ApplicationDbContext _context;

        public NewRequestSlackNotification(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task HandleAsync(RequestCreated ev)
        {
            var admin = await _context.User
                .Include(user => user.Role)
                .Where(user => user.Role.Name == "Administrator")
                .Select(u => u.Email)
                .ToListAsync();

            var token = "xoxb-1351514668807-1366258594290-m3Moo1XUvSrem1JcIol3BZ4d";
            var apiClient = new SlackTaskClient(token);
            await apiClient.ConnectAsync();
            var webHookUrl = "https://hooks.slack.com/services/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";
            var client = new SbmClient(webHookUrl);
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

            Task.Run(async () =>
            {
                foreach (var adminEmail in admin)
                {
                    var user = await apiClient.GetUserByEmailAsync(adminEmail);
                    if (!user.ok) continue;
                    message.Channel = $"@{user.user.name}";
                    await client.SendAsync(message);
                    await Task.Delay(2000);
                }
            });
        }
    }
}