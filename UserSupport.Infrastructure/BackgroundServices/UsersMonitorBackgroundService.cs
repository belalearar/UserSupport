using Microsoft.Extensions.Hosting;
using UserSupport.Common.Services;

namespace UserSupport.Infrastructure.BackgroundServices
{
    public class UsersMonitorBackgroundService(ISessionService sessionServeic) : BackgroundService
    {
        private readonly PeriodicTimer _timer = new(TimeSpan.FromMilliseconds(1000));
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                sessionServeic.MonitorUsers();
            } while (!stoppingToken.IsCancellationRequested && await _timer.WaitForNextTickAsync(stoppingToken));
        }
    }
}