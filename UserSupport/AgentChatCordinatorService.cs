using UserSupport.Services;

namespace UserSupport
{
    public class AgentChatCordinatorService(IChatService chatService) : BackgroundService
    {
        private readonly PeriodicTimer periodicTimer = new(TimeSpan.FromSeconds(10));
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                chatService.PickAndAssignToAgent();
            }
            while (!stoppingToken.IsCancellationRequested && await periodicTimer.WaitForNextTickAsync());
        }
    }
}