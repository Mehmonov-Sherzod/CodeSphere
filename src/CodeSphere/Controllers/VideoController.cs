using CodeSphere.Application.Models;
using CodeSphere.Application.Models.Video;
using CodeSphere.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;

        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        [HttpPost("create-video")]
        [RequestSizeLimit(524288000)] // 500MB
        [RequestFormLimits(MultipartBodyLengthLimit = 524288000)]
        public async Task<IActionResult> CreateVideo(CreateVideo createVideo)
        {
            var result = await _videoService.CreateVideo(createVideo);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("get-all-video-page")]
        public async Task<IActionResult> GetAllVideoPage(PageOption pageOption)
        {
            var result = await _videoService.GetAllVideoPage(pageOption);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        [RequestSizeLimit(524288000)] // 500MB
        [RequestFormLimits(MultipartBodyLengthLimit = 524288000)]
        public async Task<IActionResult> UpdateVideo(UpdateVideo updateVideo, Guid id)
        {
            var result = await _videoService.UpdateVideo(updateVideo, id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo(Guid id)
        {
            var result = await _videoService.DeleteVideo(id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
