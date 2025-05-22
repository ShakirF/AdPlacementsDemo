using AdPlacements.Application.Contracts;
using AdPlacements.Application.Interfaces;
using AdPlacements.Domain.Entities;
using AdPlacements.Infrastructure.Parsing;
using AdPlacements.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdPlacements.Application.Services
{
    public sealed class PlatformService(
         IPlatformRepository repository,
         PlatformFileParser parser,
         ILogger<PlatformService> logger) : IPlatformService
    {
        public Task<IReadOnlyCollection<string>> SearchAsync(SearchPlatformsQuery query)
        {
            var location = new Location(query.Location);
            var result = repository.FindByLocation(location);
            return Task.FromResult(result);
        }

        public async Task UploadAsync(UploadPlatformsRequest request, CancellationToken ct)
        {
            await using var stream = request.File.OpenReadStream();
            var platforms = parser.Parse(stream).ToList();
            repository.ReplaceAll(platforms);
            logger.LogInformation("Upload completed, total platforms: {Count}", platforms.Count);
        }
    }
}
