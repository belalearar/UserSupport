using System.Collections.Concurrent;
using UserSupport.Common.Enums;
using UserSupport.Common.Helpers;
using UserSupport.Common.Model;
using UserSupport.Common.Services;

namespace UserSupport.Infrastructure.Services
{
    public class ChatService() : IChatService
    {
        private readonly ConcurrentDictionary<string, DateTime> _queue = [];
        private readonly ConcurrentDictionary<string, Team> _teams = [];
        private Team _currentTeam = null!;
        private Team _overflowTeam = null!;

        public ConcurrentDictionary<string, DateTime> GetQueue()
        {
            return _queue;
        }

        public bool RemoveFromQueue(string userName)
        {
            _queue.Remove(userName, out _);
            return true;
        }

        public void AddToQueue(string userName)
        {
            _queue[userName] = DateTime.UtcNow;
        }

        public void UpdateQueue(string userName)
        {
            if (_queue.TryGetValue(userName, out _))
            {
                _queue[userName] = DateTime.UtcNow;
            }
        }

        public void SetOnShiftTeam(Team team)
        {
            _currentTeam = team;
        }

        public Team GetOnShiftTeam()
        {
            return _currentTeam;
        }

        public void SetOverflowTeam(Team team)
        {
            _overflowTeam = team;
        }

        public Team GetOverflowTeam()
        {
            return _overflowTeam;
        }

        public void PickAndAssignToAgent()
        {
            bool duringWorkingHours = TimeHepler.DuringWorkingHours();
            foreach (var userName in _queue.Keys.Reverse())
            {
                var joniorMember = _currentTeam.TeamMembers
                     .Where(a => a.Seniority is Seniority.Jonior && a.NumberOfUsers < a.Capicity)
                     .OrderByDescending(a => a.Capicity)
                     .FirstOrDefault();
                if (joniorMember != null)
                {
                    joniorMember.NumberOfUsers++;
                    _queue.Remove(userName, out _);
                    return;
                }
                var midMember = _currentTeam.TeamMembers
                     .Where(a => a.Seniority is Seniority.MidLevel && a.NumberOfUsers < a.Capicity)
                     .OrderByDescending(a => a.Capicity)
                     .FirstOrDefault();
                if (midMember != null)
                {
                    midMember.NumberOfUsers++;
                    _queue.Remove(userName, out _);
                    return;
                }
                var seniorMember = _currentTeam.TeamMembers
                     .Where(a => a.Seniority is Seniority.Senior && a.NumberOfUsers < a.Capicity)
                     .OrderByDescending(a => a.Capicity)
                     .FirstOrDefault();
                if (seniorMember != null)
                {
                    seniorMember.NumberOfUsers++;
                    _queue.Remove(userName, out _);
                    return;
                }

                var leadMember = _currentTeam.TeamMembers
                    .Where(a => a.Seniority is Seniority.Lead && a.NumberOfUsers < a.Capicity)
                    .OrderByDescending(a => a.Capicity)
                    .FirstOrDefault();
                if (leadMember != null)
                {
                    leadMember.NumberOfUsers++;
                    _queue.Remove(userName, out _);
                    return;
                }

                if (duringWorkingHours)
                {
                    var overflowMember = _currentTeam.TeamMembers.Where(a => a.Name.Equals("Overflow") && a.NumberOfUsers < a.Capicity)
                    .OrderByDescending(a => a.Capicity)
                    .FirstOrDefault();
                    if (overflowMember != null)
                    {
                        overflowMember.NumberOfUsers++;
                        _queue.Remove(userName, out _);
                        return;
                    }
                }

            }
        }
    }
}