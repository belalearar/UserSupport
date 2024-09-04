using Microsoft.Extensions.Hosting;
using UserSupport.Common.Services;

namespace UserSupport.Infrastructure.BackgroundServices
{
    public class AgentChatCordinatorService(IChatService chatService) : BackgroundService
    {
        private readonly PeriodicTimer periodicTimer = new(TimeSpan.FromSeconds(1));
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested && await periodicTimer.WaitForNextTickAsync(stoppingToken))
            {
                chatService.PickAndAssignToAgent();
            }
        }
    }
}