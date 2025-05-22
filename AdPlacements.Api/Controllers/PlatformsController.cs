using AdPlacements.Application.Contracts;
using AdPlacements.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdPlacements.Api.Controllers
{
    [ApiController]
    [Route("api/platforms")]
    public class PlatformsController(IPlatformService service, ILogger<PlatformsController> logger) : ControllerBase
    {
        /// POST api/platforms/upload
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] UploadPlatformsRequest request, CancellationToken ct)
        {
            logger.LogInformation("File upload requested: {Size} bytes", request.File.Length);
            await service.UploadAsync(request, ct);
            return NoContent();
        }

        /// GET api/platforms/search?location=/ru/svrd
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] SearchPlatformsQuery query)
        {
            var platforms = await service.SearchAsync(query);
            return Ok(platforms);
        }
    }
}
