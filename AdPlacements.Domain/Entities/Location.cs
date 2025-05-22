using AdPlacements.Domain.Exceptions;

namespace AdPlacements.Domain.Entities
{
    public sealed class Location
    {
        public string Path { get; }

        public Location(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw) || raw[0] != '/')
                throw new DomainException("Location must start with '/'");

            Path = Normalise(raw);
        }

        /* Вложенность определяем по префиксу */
        public bool IsPrefixOf(Location other) => other.Path.StartsWith(Path, StringComparison.OrdinalIgnoreCase);

        public IEnumerable<Location> GetPrefixes()
        {
            var current = Path;
            while (true)
            {
                yield return new Location(current);

                var idx = current.LastIndexOf('/');
                if (idx <= 0) break;

                current = current[..idx];
            }
        }

        private static string Normalise(string raw) => raw.Trim().TrimEnd('/').ToLowerInvariant();

        public override string ToString() => Path;

        /* value-object equality */
        public override bool Equals(object? obj) => obj is Location l && l.Path == Path;
        public override int GetHashCode() => Path.GetHashCode();
    }
}
