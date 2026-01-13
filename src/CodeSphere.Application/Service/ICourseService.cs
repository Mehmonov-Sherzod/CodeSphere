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
    }
}
