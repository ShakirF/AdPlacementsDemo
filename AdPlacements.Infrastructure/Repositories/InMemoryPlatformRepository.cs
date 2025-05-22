using AdPlacements.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace AdPlacements.Infrastructure.Repositories
{
    public sealed class InMemoryPlatformRepository(ILogger<InMemoryPlatformRepository> logger) : IPlatformRepository
    {
        private readonly object _lock = new();
        private Dictionary<Location, List<string>> _index = new();
        public IReadOnlyCollection<string> FindByLocation(Location location)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
                foreach (var prefix in location.GetPrefixes())      
                       if (_index.TryGetValue(prefix, out var list))
                           foreach (var p in list)
                                    result.Add(p);
            
                return result.ToList();
        }

        public void ReplaceAll(IEnumerable<AdPlatform> platforms)
        {
            var newIndex = new Dictionary<Location, List<string>>();

            foreach (var p in platforms)
            {
                foreach (var loc in p.Locations)
                {
                    if (!newIndex.TryGetValue(loc, out var list))
                        newIndex[loc] = list = [];

                    list.Add(p.Name);
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
