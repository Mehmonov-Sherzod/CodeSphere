using CodeSphere.Application.Models;
using CodeSphere.Application.Models.Course;
using CodeSphere.Application.Service;
using Microsoft.AspNetCore.Mvc;

namespace CodeSphere.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService; 
        }

        [HttpPost("create-course")]

        public async Task<IActionResult> CreateCourse(CreateCourse createCourse)
        {
            var result = await _courseService.CreateCourse(createCourse);

            if (result.Succeeded)
            {
                return Ok(result);  
            }
            return BadRequest(result);
        }

        [HttpPost("get-all-course-page")]

        public async Task<IActionResult> GetAllCoursePage(PageOption pageOption)
        {
            var result = await _courseService.GetAllCoursePage(pageOption);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateCourse(UpdateCourse updateCourse, Guid Id)
        {
            var result = await _courseService.UpdateCourse(updateCourse, Id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteUpdate(Guid Id)
        {
            var result = await _courseService.DeleteCourse(Id);

            if (result.Succeeded)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
