using Microsoft.Extensions.Hosting;
using UserSupport.Common.Enums;
using UserSupport.Common.Model;
using UserSupport.Common.Services;

namespace UserSupport.Infrastructure.BackgroundServices
{
    public class SeedDataBackgroundService(IChatService chatService) : BackgroundService
    {
        private readonly List<Team> _teams = [];
        private readonly PeriodicTimer _periodicTimer = new PeriodicTimer(TimeSpan.FromMinutes(1));
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                if (_teams.Count == 0)
                {
                    _teams.AddRange([new Team
                    {
                        TeamMembers =
                    [
                        new() {Id=1,Name="Bilal",Seniority=Seniority.Lead },
                        new() {Id=2,Name="Adam",Seniority=Seniority.MidLevel},
                        new() {Id=2,Name="Sanad",Seniority=Seniority.MidLevel },
                        new() {Id=2,Name="Sid",Seniority=Seniority.Jonior }
                    ],
                        Name = "A",
                        ShiftFrom = new TimeOnly(8, 0),
                        ShiftTo = new TimeOnly(16, 0),
                    },
                    new Team
                    {
                        TeamMembers =
                    [
                        //new() {Id=1,Name="Dav",Seniority=Seniority.Senior },
                        //new() {Id=2,Name="Kavin",Seniority=Seniority.MidLevel },
                        //new() {Id=2,Name="John",Seniority=Seniority.Jonior },
                        new() {Id=2,Name="Sarah",Seniority=Seniority.Jonior }
                    ],
                        Name = "B",
                        ShiftFrom = new TimeOnly(16, 0),
                        ShiftTo = new TimeOnly(23, 59,59),
                    },new Team
                    {
                        TeamMembers =
                    [
                        new() {Id=1,Name="Saed",Seniority=Seniority.MidLevel },
                        new() {Id=2,Name="Suzy",Seniority=Seniority.MidLevel }
                    ],
                        Name = "C",
                        ShiftFrom = new TimeOnly(0, 0),
                        ShiftTo = new TimeOnly(7, 59,59),
                    },new Team
                    {
                        TeamMembers =
                    [
                        new() {Id=1,Name="Sam",Seniority=Seniority.Jonior },
                        new() {Id=2,Name="Kim",Seniority=Seniority.Jonior },
                        new() {Id=2,Name="Andey",Seniority=Seniority.Jonior },
                        new() {Id=2,Name="Andrew",Seniority=Seniority.Jonior },
                        new() {Id=2,Name="Andreas",Seniority=Seniority.Jonior },
                        new() {Id=2,Name="Ezz",Seniority=Seniority.Jonior }
                    ],
                        Name = "Overflow",
                        ShiftFrom = new TimeOnly(9, 0),
                        ShiftTo = new TimeOnly(18, 0),
                    }]);
                    var overflowTeam = _teams.FirstOrDefault(a => a.Name.Equals("Overflow"));
                    if (overflowTeam != null)
                    {
                        chatService.SetOverflowTeam(overflowTeam);
                    }
                }
                var currentTeam = _teams.First(a => !a.Name.Equals("Overflow") && TimeOnly.FromDateTime(DateTime.UtcNow) > a.ShiftFrom && TimeOnly.FromDateTime(DateTime.UtcNow) < a.ShiftTo);
                chatService.SetOnShiftTeam(currentTeam);
            } while (!stoppingToken.IsCancellationRequested && await _periodicTimer.WaitForNextTickAsync(stoppingToken));
        }
    }
}