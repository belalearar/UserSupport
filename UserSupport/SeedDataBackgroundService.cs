using UserSupport.Model;
using UserSupport.Services;

namespace UserSupport
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
                        new() {Id=1,Name="Bilal",Seniority=Enums.Seniority.Lead },
                        new() {Id=2,Name="Adam",Seniority=Enums.Seniority.MidLevel},
                        new() {Id=2,Name="Sanad",Seniority=Enums.Seniority.MidLevel },
                        new() {Id=2,Name="Sid",Seniority=Enums.Seniority.Jonior }
                    ],
                        Name = "A",
                        ShiftFrom = new TimeOnly(8, 0),
                        ShiftTo = new TimeOnly(16, 0),
                    },
                    new Team
                    {
                        TeamMembers =
                    [
                        //new() {Id=1,Name="Dav",Seniority=Enums.Seniority.Senior },
                        //new() {Id=2,Name="Kavin",Seniority=Enums.Seniority.MidLevel },
                        //new() {Id=2,Name="John",Seniority=Enums.Seniority.Jonior },
                        new() {Id=2,Name="Sarah",Seniority=Enums.Seniority.Jonior }
                    ],
                        Name = "B",
                        ShiftFrom = new TimeOnly(16, 0),
                        ShiftTo = new TimeOnly(23, 59,59),
                    },new Team
                    {
                        TeamMembers =
                    [
                        new() {Id=1,Name="Saed",Seniority=Enums.Seniority.MidLevel },
                        new() {Id=2,Name="Suzy",Seniority=Enums.Seniority.MidLevel }
                    ],
                        Name = "C",
                        ShiftFrom = new TimeOnly(0, 0),
                        ShiftTo = new TimeOnly(7, 59,59),
                    },new Team
                    {
                        TeamMembers =
                    [
                        new() {Id=1,Name="Sam",Seniority=Enums.Seniority.Jonior },
                        new() {Id=2,Name="Kim",Seniority=Enums.Seniority.Jonior },
                        new() {Id=2,Name="Andey",Seniority=Enums.Seniority.Jonior },
                        new() {Id=2,Name="Andrew",Seniority=Enums.Seniority.Jonior },
                        new() {Id=2,Name="Andreas",Seniority=Enums.Seniority.Jonior },
                        new() {Id=2,Name="Ezz",Seniority=Enums.Seniority.Jonior }
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