namespace UserSupport.Common.Services
{
    public interface ISessionService
    {
        bool CreateSession(string username);
        void MonitorUsers();
        void Poll(string username);
    }
}