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

        public async Task<ApiResult<string>> DeleteCourse(Guid id)
        {
            var course  = await _appDbContext.Courses.FirstOrDefaultAsync();

            if(course == null)
                return ApiResult<string>.Failure(new List<string> { "Course Error" });

            _appDbContext.Courses.Remove(course);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("Course - Certificate");
        }

        public async Task<ApiResult<PaginationResult<GetCourse>>> GetAllCoursePage(PageOption pageOption)
        {
            var query = _appDbContext.Courses.AsQueryable();

            if (!string.IsNullOrEmpty(pageOption.Search))
            {
                query = query.Where(s => s.Name.Contains(pageOption.Search));
            }

            List<GetCourse> course = await query
               .Skip(pageOption.PageSize * (pageOption.PageNumber - 1))
               .Take(pageOption.PageSize)
               .Select(x => new GetCourse
               {
                   Id = x.Id,
                   Name = x.Name,
                   Author = x.Author,
                   Image = x.ImageUrl

               }).ToListAsync();

            int total = _appDbContext.Courses.Count();

            var pagination = new PaginationResult<GetCourse>
            {
                PageSize = pageOption.PageSize,
                PageNumber = pageOption.PageNumber,
                TotalCount = total,
                Values = course
            };

            return ApiResult<PaginationResult<GetCourse>>.Success(pagination);
        }

        public async Task<ApiResult<string>> UpdateCourse(UpdateCourse updateCourse, Guid Id)
        {
            string? urlImage = null;

            if (updateCourse.Image != null && updateCourse.Image.Length > 0)
            {
                var extension = Path.GetExtension(updateCourse.Image.FileName);
                var objectName = $"{Guid.NewGuid()}{extension}";
                using var mystream = updateCourse.Image.OpenReadStream();
                urlImage = await _fileStoreageService.UploadFileAsync(
                    "questions-image",
                    objectName,
                    mystream,
                    updateCourse.Image.ContentType
                );
            }
            var course = await _appDbContext.Courses.FirstOrDefaultAsync(x => x.Id == Id);

            if (course == null)
                return ApiResult<string>.Failure(new List<string> { "Course Error" });

            course.Name = updateCourse.Name;
            course.Author = updateCourse.Author;
            course.ImageUrl = urlImage;

            _appDbContext.Update(course);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("Course Create ");

        }
    }
}
