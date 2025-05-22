using AdPlacements.Domain.Exceptions;

namespace AdPlacements.Domain.Entities
{
    public sealed class AdPlatform
    {
        public string Name { get; }
        public IReadOnlyCollection<Location> Locations { get; }

        public AdPlatform(string name, IEnumerable<Location> locations)
        {
            Name = string.IsNullOrWhiteSpace(name)
                ? throw new DomainException("Platform name can't be empty")
                : name.Trim();

            var locs = locations.ToArray();
            if (locs.Length == 0)
                throw new DomainException("Platform must contain at least one location");

            Locations = locs;
        }
    }
}

