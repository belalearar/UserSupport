using System.Collections.Concurrent;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
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

            var team = await teamResponseMessage.Content.ReadFromJsonAsync<Team>();

            for (int i = 0; i <= (team!.QueueSize + team.Capacity); i++)
            {
                var userName = "AliAAA" + DateTime.Now.ToString();
                var responseMessage = await client.PostAsJsonAsync("/create-session", userName);

                responseMessage.EnsureSuccessStatusCode();

                if (i > team.Capacity)
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

            var team = await teamResponseMessage.Content.ReadFromJsonAsync<Team>();

            for (int i = 0; i <= (team!.QueueSize + team.Capacity); i++)
            {
                var userName = "AliAAA" + DateTime.Now.ToString();
                var responseMessage = await client.PostAsJsonAsync("/create-session", userName);

                responseMessage.EnsureSuccessStatusCode();

                if (i > team.Capacity)
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
    }
}