using Microsoft.AspNetCore.Http;

namespace AdPlacements.Application.Contracts
{
    public sealed class UploadPlatformsRequest
    {
        public IFormFile File { get; init; } = default!;
    }
}
