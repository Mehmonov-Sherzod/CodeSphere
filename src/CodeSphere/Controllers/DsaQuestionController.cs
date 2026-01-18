using CodeSphere.Application.Models;
using CodeSphere.Application.Models.DsaQuestion;
using CodeSphere.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DsaQuestionController : ControllerBase
    {
        private readonly IDsaQuestionService _dsaQuestionService;

        public DsaQuestionController(IDsaQuestionService dsaQuestionService)
        {
            _dsaQuestionService = dsaQuestionService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDsaQuestion(CreateDsaQuestion createDsaQuestion)
        {
            var result = await _dsaQuestionService.CreateDsaQuestion(createDsaQuestion);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("get-all-page")]
        public async Task<IActionResult> GetAllDsaQuestionsPage(PageOption pageOption)
        {
            var result = await _dsaQuestionService.GetAllDsaQuestionsPage(pageOption);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDsaQuestionById(Guid id)
        {
            var result = await _dsaQuestionService.GetDsaQuestionById(id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDsaQuestion(UpdateDsaQuestion updateDsaQuestion, Guid id)
        {
            var result = await _dsaQuestionService.UpdateDsaQuestion(updateDsaQuestion, id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDsaQuestion(Guid id)
        {
            var result = await _dsaQuestionService.DeleteDsaQuestion(id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("by-topic/{topicId}")]
        public async Task<IActionResult> GetDsaQuestionsByTopicId(Guid topicId, PageOption pageOption)
        {
            var result = await _dsaQuestionService.GetDsaQuestionsByTopicId(topicId, pageOption);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
