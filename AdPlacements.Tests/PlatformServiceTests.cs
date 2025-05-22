using AdPlacements.Application.Contracts;
using AdPlacements.Application.Services;
using AdPlacements.Infrastructure.Parsing;
using AdPlacements.Infrastructure.Repositories;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace AdPlacements.Tests
{
    public class PlatformServiceTests
    {
        [Fact]
        public async Task SearchAsync_Forwards_Request_To_Repository()
        {
            // arrange
            var repoMock = new Mock<IPlatformRepository>();
            repoMock.Setup(r => r.FindByLocation(It.IsAny<AdPlacements.Domain.Entities.Location>()))
                    .Returns(new[] { "X" });

            var dummyParser = new PlatformFileParser(NullLogger<PlatformFileParser>.Instance);
            var service = new PlatformService(repoMock.Object, dummyParser,
                                              NullLogger<PlatformService>.Instance);

            // act
            var result = await service.SearchAsync(new SearchPlatformsQuery { Location = "/ru" });

            // assert
            Assert.Single(result);
            Assert.Contains("X", result);
            repoMock.Verify(r => r.FindByLocation(It.IsAny<AdPlacements.Domain.Entities.Location>()), Times.Once);
        }
    }
}
