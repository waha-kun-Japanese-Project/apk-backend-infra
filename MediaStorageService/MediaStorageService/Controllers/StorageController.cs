using Media.ServiceAbstraction;
using Microsoft.AspNetCore.Mvc;

namespace MediaStorageService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StorageController(IStorageService storageService): ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile(IFormFile file,[FromQuery] string folder, CancellationToken cancellationToken)
        {
            var result = await storageService.UploadFileAsync(file, folder, cancellationToken);
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteFile([FromQuery] string objectName, CancellationToken cancellationToken)
        {
            await storageService.DeleteAsync(objectName, cancellationToken);
            return NoContent();
        }
        [HttpGet]
        public async Task<IActionResult> DownloadFile([FromQuery]string objectName, CancellationToken cancellationToken)
        {
            var stream = await storageService.DownloadAsync(objectName, cancellationToken);
            return File(stream, "application/octet-stream", objectName);
        }
        [HttpGet("get")]
        public async Task<IActionResult> GetFile(string objectName, CancellationToken cancellationToken)
        {
            var result = await storageService.GetAsync(objectName, cancellationToken);
            return Ok(result);
        }
    }
}
