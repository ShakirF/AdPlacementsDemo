using AdPlacements.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AdPlacements.Infrastructure.Parsing
{
    public sealed class PlatformFileParser(ILogger<PlatformFileParser> logger)
    {
        public IEnumerable<AdPlatform> Parse(Stream stream)
        {
            using var reader = new StreamReader(stream, Encoding.UTF8, leaveOpen: true);

            string? line;
            var lineNo = 0;

            while ((line = reader.ReadLine()) is not null)
            {
                lineNo++;
                if (string.IsNullOrWhiteSpace(line) || !line.Contains(':'))
                    continue;

                var parts = line.Split(':', 2, StringSplitOptions.TrimEntries);

                AdPlatform? platform = null;   

                try
                {
                    var name = parts[0];
                    var locs = parts[1]
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(raw => new Location(raw));

                    platform = new AdPlatform(name, locs);  
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Skip invalid line {LineNo}", lineNo);
                }

                if (platform is not null)
                    yield return platform;
            }
        }
    }
}
