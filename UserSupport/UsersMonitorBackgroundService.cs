using UserSupport.Services;

namespace UserSupport
{
    public class UsersMonitorBackgroundService(ISessionService sessionServeic) : BackgroundService
    {
        private PeriodicTimer timer = new(TimeSpan.FromMilliseconds(300));
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                sessionServeic.MonitorUsers();
            } while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken));
        }
    }
}