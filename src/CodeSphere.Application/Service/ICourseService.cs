using CodeSphere.Application.Models;
using CodeSphere.Application.Models.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service
{
    public interface ICourseService
    {
        Task<ApiResult<string>> CreateCourse(CreateCourse createCourse);
        Task<ApiResult<PaginationResult<GetCourse>>> GetAllCoursePage(PageOption pageOption);

        Task<ApiResult<string>> UpdateCourse(UpdateCourse updateCourse, Guid Id);

        Task<ApiResult<string>> DeleteCourse(Guid id);
    }
}
