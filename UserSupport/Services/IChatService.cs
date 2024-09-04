using System.Collections.Concurrent;
using UserSupport.Model;

namespace UserSupport.Services
{
    public interface IChatService
    {
        void AddToQueue(string userName);
        Team GetOnShiftTeam();
        Team GetOverflowTeam();
        ConcurrentDictionary<string, DateTime> GetQueue();
        void PickAndAssignToAgent();
        bool RemoveFromQueue(string userName);
        void SetOnShiftTeam(Team team);
        void SetOverflowTeam(Team team);
        void UpdateQueue(string userName);
    }
}