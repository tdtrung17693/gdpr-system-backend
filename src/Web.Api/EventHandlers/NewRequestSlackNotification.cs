using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly ApplicationDbContext _context;
        private IConfiguration _configuration;

        public NewRequestSlackNotification(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task HandleAsync(RequestCreated ev)
        {
            var slackToken = _configuration.GetValue<string>("Slack:Token");
            var slackWebHook = _configuration.GetValue<string>("Slack:WebHook");
            
            var admin = await _context.User
                .Include(user => user.Role)
                .Where(user => user.Role.Name == "Administrator")
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

            await Task.Run(async () =>
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