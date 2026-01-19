using CodeSphere.Application.Models.DsaQuestionDefinition;
using CodeSphere.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DsaQuestionDefinitionController : ControllerBase
    {
        private readonly IDsaQuestionDefinitionService _dsaQuestionDefinitionService;

        public DsaQuestionDefinitionController(IDsaQuestionDefinitionService dsaQuestionDefinitionService)
        {
            _dsaQuestionDefinitionService = dsaQuestionDefinitionService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateDsaQuestionDefinition(CreateDsaQuestionDefinition createDsaQuestionDefinition)
        {
            var result = await _dsaQuestionDefinitionService.CreateDsaQuestionDefinition(createDsaQuestionDefinition);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDsaQuestionDefinitionById(Guid id)
        {
            var result = await _dsaQuestionDefinitionService.GetDsaQuestionDefinitionById(id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet("by-question/{dsaQuestionId}")]
        public async Task<IActionResult> GetDsaQuestionDefinitionByQuestionId(Guid dsaQuestionId)
        {
            var result = await _dsaQuestionDefinitionService.GetDsaQuestionDefinitionByQuestionId(dsaQuestionId);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDsaQuestionDefinition(UpdateDsaQuestionDefinition updateDsaQuestionDefinition, Guid id)
        {
            var result = await _dsaQuestionDefinitionService.UpdateDsaQuestionDefinition(updateDsaQuestionDefinition, id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDsaQuestionDefinition(Guid id)
        {
            var result = await _dsaQuestionDefinitionService.DeleteDsaQuestionDefinition(id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
