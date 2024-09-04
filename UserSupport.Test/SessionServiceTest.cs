using Microsoft.Extensions.Options;
using Moq;
using UserSupport.Common.Configs;
using UserSupport.Common.Model;
using UserSupport.Common.Services;
using UserSupport.Infrastructure.Services;

namespace UserSupport.Test
{
    public class SessionServiceTest
    {
        private const string UserName = "userName";

        [Fact]
        public void CreateSessionTest()
        {
            //Arrange
            var options = new Mock<IOptions<SystemProfile>>();
            var chatService = new Mock<IChatService>();
            options.Setup(a => a.Value).Returns(new SystemProfile
            {
                PollTimeInSeconds = 3
            });
            var queue = new System.Collections.Concurrent.ConcurrentDictionary<string, DateTime>();
            queue["Ahmad"] = DateTime.UtcNow;
            var team = new Team
            {
                Name = "Test",
                ShiftFrom = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(-1)),
                ShiftTo = TimeOnly.FromDateTime(DateTime.UtcNow.AddHours(1)),
                TeamMembers = [new TeamMember { Id = 1, Name = "Ali", NumberOfUsers = 0, Seniority = Common.Enums.Seniority.Jonior }],
            };
            chatService.Setup(a => a.GetQueue()).Returns(queue);
            chatService.Setup(a => a.AddToQueue(It.IsAny<string>()));
            chatService.Setup(a => a.GetOnShiftTeam()).Returns(team);
            var service = new SessionService(options.Object, chatService.Object);

            //Act
            var result = service.CreateSession(UserName);
            //Assert
            Assert.True(result);
            chatService.Verify(a => a.GetQueue());
            chatService.Verify(a => a.AddToQueue(UserName), Times.Once);
            chatService.Verify(a => a.GetOnShiftTeam(), Times.Once);
        }
    }
}