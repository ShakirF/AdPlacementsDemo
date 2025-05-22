using AdPlacements.Application.Contracts;

namespace AdPlacements.Application.Interfaces
{
    public interface IPlatformService
    {
        Task UploadAsync(UploadPlatformsRequest request, CancellationToken ct);
        Task<IReadOnlyCollection<string>> SearchAsync(SearchPlatformsQuery query);
    }
}
