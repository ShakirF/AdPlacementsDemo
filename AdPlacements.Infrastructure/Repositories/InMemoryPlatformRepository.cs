using AdPlacements.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AdPlacements.Infrastructure.Repositories
{
    public sealed class InMemoryPlatformRepository(ILogger<InMemoryPlatformRepository> logger) : IPlatformRepository
    {
        private readonly object _lock = new();
        private Dictionary<Location, List<string>> _index = new();
        public IReadOnlyCollection<string> FindByLocation(Location location)
         => _index.TryGetValue(location, out var list) ? list : Array.Empty<string>();

        public void ReplaceAll(IEnumerable<AdPlatform> platforms)
        {
            var newIndex = new Dictionary<Location, List<string>>();

            foreach (var p in platforms)
            {
                foreach (var loc in p.Locations)
                {
                    foreach (var prefix in loc.GetPrefixes())
                    {
                        if (!newIndex.TryGetValue(prefix, out var list))
                            newIndex[prefix] = list = [];

                        list.Add(p.Name);
                    }
                }
            }

            lock (_lock)
            {
                _index = newIndex;
            }

            logger.LogInformation("Loaded {Count} platforms", platforms.Count());
        }
    }
}
