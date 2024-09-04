using Microsoft.Extensions.Hosting;
using UserSupport.Common.Services;

namespace UserSupport.Infrastructure.BackgroundServices
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