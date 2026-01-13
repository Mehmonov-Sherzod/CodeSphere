using CodeSphere.Application.Models;
using CodeSphere.Application.Models.Course;
using CodeSphere.DataAccess.Persistence;
using CodeSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service.Impl
{
    public class CourseService : ICourseService
    {
        private readonly IFileStoreageService _fileStoreageService;

        private readonly AppDbContext _appDbContext;

        public CourseService(IFileStoreageService fileStoreageService, AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _fileStoreageService = fileStoreageService;
        }
        public async Task<ApiResult<string>> CreateCourse(CreateCourse createCourse)
        {
            string? urlImage = null;

            if (createCourse.Image != null && createCourse.Image.Length > 0)
            {
                var extension = Path.GetExtension(createCourse.Image.FileName);
                var objectName = $"{Guid.NewGuid()}{extension}";
                using var mystream = createCourse.Image.OpenReadStream();
                urlImage = await _fileStoreageService.UploadFileAsync(
                    "questions-image",
                    objectName,
                    mystream,
                    createCourse.Image.ContentType
                );
            }
            var Courses = new Course
            {
                Name = createCourse.Name,
                ImageUrl = urlImage,
                Author = createCourse.Author,

            };

            await _appDbContext.Courses.AddAsync(Courses);
            await _appDbContext.SaveChangesAsync();
            return ApiResult<string>.Success("Course has been created");
        }
    }
}
