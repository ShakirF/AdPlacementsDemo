using AdPlacements.Domain.Entities;

namespace AdPlacements.Infrastructure.Repositories
{
    public interface IPlatformRepository
    {
        void ReplaceAll(IEnumerable<AdPlatform> platforms);
        IReadOnlyCollection<string> FindByLocation(Location location);
    }
}
