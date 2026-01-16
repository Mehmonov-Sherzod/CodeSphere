using CodeSphere.Application.Models;
using CodeSphere.Application.Models.Topic;
using CodeSphere.Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpPost("create-topic")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTopic(CreateTopic createTopic)
        {
            var result = await _topicService.CreateTopic(createTopic);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("get-all-topic-page")]
        //[Authorize]
        public async Task<IActionResult> GetAllTopicPage(PageOption pageOption)
        {
            var result = await _topicService.GetAllTopicPage(pageOption);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("update-topic")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTopic(UpdateTopic updateTopic, Guid Id)
        {
            var result = await _topicService.UpdateTopic(updateTopic, Id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("delete-topic")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTopic(Guid Id)
        {
            var result = await _topicService.DeleteTopic(Id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
