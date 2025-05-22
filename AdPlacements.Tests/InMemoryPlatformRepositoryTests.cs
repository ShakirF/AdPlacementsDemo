using AdPlacements.Domain.Entities;
using AdPlacements.Infrastructure.Repositories;
using Microsoft.Extensions.Logging.Abstractions;

namespace AdPlacements.Tests
{
    public class InMemoryPlatformRepositoryTests
    {

        private static readonly AdPlatform[] Platforms =
        {
            new("A", new[] { new Location("/ru") }),
            new("B", new[] { new Location("/ru/svrd") }),
            new("C", new[] { new Location("/ru/svrd/revda") })
        };

        private readonly InMemoryPlatformRepository _repo =  new(NullLogger<InMemoryPlatformRepository>.Instance);

        public InMemoryPlatformRepositoryTests() => _repo.ReplaceAll(Platforms);

        [Theory]
        [InlineData("/ru", new[] { "A" })]
        [InlineData("/ru/svrd", new[] { "A", "B" })]
        [InlineData("/ru/svrd/revda", new[] { "A", "B", "C" })]
        [InlineData("/unknown", new string[0])]
        public void FindByLocation_Returns_Correct_Set(string location, string[] expected)
        {
            var actual = _repo.FindByLocation(new Location(location)).OrderBy(x => x).ToArray();
            Assert.Equal(expected.OrderBy(x => x), actual);
        }
    }
}
