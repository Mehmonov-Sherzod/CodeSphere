using CodeSphere.Application.Models;
using CodeSphere.Application.Models.DsaQuestionTestCase;
using CodeSphere.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DsaQuestionTestCaseController : ControllerBase
    {
        private readonly IDsaQuestionTestCaseService _testCaseService;

        public DsaQuestionTestCaseController(IDsaQuestionTestCaseService testCaseService)
        {
            _testCaseService = testCaseService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateTestCase(CreateDsaQuestionTestCase createTestCase)
        {
            var result = await _testCaseService.CreateTestCase(createTestCase);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("get-all-page")]
        public async Task<IActionResult> GetAllTestCasesPage(PageOption pageOption)
        {
            var result = await _testCaseService.GetAllTestCasesPage(pageOption);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestCaseById(Guid id)
        {
            var result = await _testCaseService.GetTestCaseById(id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTestCase(UpdateDsaQuestionTestCase updateTestCase, Guid id)
        {
            var result = await _testCaseService.UpdateTestCase(updateTestCase, id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestCase(Guid id)
        {
            var result = await _testCaseService.DeleteTestCase(id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("by-question/{questionId}")]
        public async Task<IActionResult> GetTestCasesByQuestionId(Guid questionId, PageOption pageOption)
        {
            var result = await _testCaseService.GetTestCasesByQuestionId(questionId, pageOption);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
