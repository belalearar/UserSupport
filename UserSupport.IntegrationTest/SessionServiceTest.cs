using System.Collections.Concurrent;
using System.Net.Http.Json;
using UserSupport.Common.Helpers;
using UserSupport.Common.Model;

namespace UserSupport.IntegrationTest
{
    public class SessionServiceTest
    {
        [Fact]
        public async Task CreateSessionTest()
        {
            var application = new UserSupportWebAppFactory();
            var client = application.CreateClient();

            var responseMessage = await client.PostAsJsonAsync("/create-session", "AliAAA");

            responseMessage.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.Created, responseMessage.StatusCode);
        }

        [Fact]
        public async Task PollTest()
        {
            var application = new UserSupportWebAppFactory();
            var client = application.CreateClient();

            var responseMessage = await client.PostAsJsonAsync("/poll", "AliAAA");

            responseMessage.EnsureSuccessStatusCode();

            Assert.Equal(System.Net.HttpStatusCode.NoContent, responseMessage.StatusCode);
        }

        [Fact]
        // Test Poll is working fine by adding the delay to the queue if poll happened
        public async Task PollTest_ShouldRemainInQueue()
        {
            var application = new UserSupportWebAppFactory();
            var client = application.CreateClient();

            var teamResponseMessage = await client.GetAsync("team-on-shift");
            var overflowTeamResponseMessage = await client.GetAsync("team-overflow");

            var team = await teamResponseMessage.Content.ReadFromJsonAsync<Team>();
            var overflowTeam = await overflowTeamResponseMessage.Content.ReadFromJsonAsync<Team>();

            var isDuringWorkingHours = TimeHepler.DuringWorkingHours();

            var overflowCapacity = (isDuringWorkingHours && overflowTeam != null) ? overflowTeam?.Capacity : 0;
            for (int i = 0; i <= (team!.QueueSize + team.Capacity + overflowCapacity); i++)
            {
                var userName = "AliAAA" + DateTime.Now.ToString();
                var responseMessage = await client.PostAsJsonAsync("/create-session", userName);

                responseMessage.EnsureSuccessStatusCode();

                if (i > team.Capacity + overflowCapacity)
                {
                    await Task.Delay(2000);
                    await client.PostAsJsonAsync("/poll", userName);
                    var newQueueMessage = await client.GetAsync("/queue");
                    var newQueueResult = await newQueueMessage.Content.ReadFromJsonAsync<ConcurrentDictionary<string, DateTime>>();
                    Assert.Single(newQueueResult!);
                    break;
                }
            }
        }

        [Fact]
        // Test Poll is working fine by adding the delay by 5 seconds to the queue if poll happened
        public async Task PollTest_ShouldRemoveFromQueue()
        {
            var application = new UserSupportWebAppFactory();
            var client = application.CreateClient();

            var teamResponseMessage = await client.GetAsync("team-on-shift");
            var overflowTeamResponseMessage = await client.GetAsync("team-overflow");

            var team = await teamResponseMessage.Content.ReadFromJsonAsync<Team>();
            var overflowTeam = await overflowTeamResponseMessage.Content.ReadFromJsonAsync<Team>();

            var isDuringWorkingHours = TimeHepler.DuringWorkingHours();

            var overflowCapacity = (isDuringWorkingHours && overflowTeam != null) ? overflowTeam?.Capacity : 0;

            for (int i = 0; i <= (team!.QueueSize + team.Capacity + overflowCapacity); i++)
            {
                var userName = "AliAAA" + DateTime.Now.ToString();
                var responseMessage = await client.PostAsJsonAsync("/create-session", userName);

                responseMessage.EnsureSuccessStatusCode();

                if (i > team.Capacity + overflowCapacity)
                {
                    await Task.Delay(5000);
                    await client.PostAsJsonAsync("/poll", userName);
                    var newQueueMessage = await client.GetAsync("/queue");
                    var newQueueResult = await newQueueMessage.Content.ReadFromJsonAsync<ConcurrentDictionary<string, DateTime>>();
                    Assert.Empty(newQueueResult!);
                    break;
                }
            }
        }


        [Fact]
        // Test Team Queue Capacity Should Be Team Capacity * 1.5
        public async Task CheckTeamQueueCapacity_ShouldTeamQueueCapacity()
        {
            var application = new UserSupportWebAppFactory();
            var client = application.CreateClient();

            var teamResponseMessage = await client.GetAsync("team-on-shift");

            var team = await teamResponseMessage.Content.ReadFromJsonAsync<Team>();

            Assert.Equal(team!.Capacity * 1.5, team.QueueSize);
        }
    }
}