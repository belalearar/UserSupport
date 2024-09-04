using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using UserSupport.Common.Configs;
using UserSupport.Common.Helpers;
using UserSupport.Common.Model;
using UserSupport.Common.Services;

namespace UserSupport.Infrastructure.Services
{
    public class SessionService(IOptions<SystemProfile> options, IChatService chatService) : ISessionService
    {
        private readonly SystemProfile _profile = options.Value;

        public bool CreateSession(string userName)
        {
            //Check
            bool duringWorkingHours = TimeHepler.DuringWorkingHours();
            var queue = chatService.GetQueue();
            var currentTeam = chatService.GetOnShiftTeam();
            if (queue.Count >= currentTeam.QueueSize && !duringWorkingHours)
            {
                return false;
            }
            else if (queue.Count >= currentTeam.QueueSize && duringWorkingHours)
            {
                var isAvailable = CheckOverflowTeamAvailability(userName, currentTeam);
                if (!isAvailable) { return false; }
            }
            //Add
            chatService.AddToQueue(userName);
            //return
            return true;
        }

        private bool CheckOverflowTeamAvailability(string userName, Team currentTeam)
        {
            var overflowTeam = chatService.GetOverflowTeam();
            var maxCapacity = currentTeam.QueueSize + overflowTeam.Capacity;
            var currentCapacity = currentTeam.QueueSize + overflowTeam.TeamMembers.Sum(a => a.NumberOfUsers);
            if (overflowTeam != null && currentCapacity < maxCapacity)
            {
                chatService.AddToQueue(userName);
                return true;
            }
            return false;
        }

        public void Poll(string username)
        {
            chatService.UpdateQueue(username);
        }

        public void MonitorUsers()
        {
            var currentTime = DateTime.UtcNow;
            var queue = chatService.GetQueue();
            foreach (var record in queue)
            {
                if ((currentTime - record.Value).TotalSeconds >= _profile.PollTimeInSeconds)
                {
                    chatService.RemoveFromQueue(record.Key);
                }
            }
        }
    }
}