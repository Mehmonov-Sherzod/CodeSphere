using CodeSphere.Application.Models;
using CodeSphere.Application.Models.Topic;
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
    public class TopicService : ITopicService
    {
        private readonly AppDbContext _appDbContext;

        public TopicService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ApiResult<string>> CreateTopic(CreateTopic createTopic)
        {
            var course = await _appDbContext.Courses.FirstOrDefaultAsync(x => x.Id == createTopic.CourseId);

            if (course == null)
                return ApiResult<string>.Failure(new List<string> { "Course not found" });

            var topic = new Topic
            {
                Name = createTopic.Name,
                CourseId = createTopic.CourseId
            };

            await _appDbContext.Topic.AddAsync(topic);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("Topic has been created");
        }

        public async Task<ApiResult<PaginationResult<GetTopic>>> GetAllTopicPage(PageOption pageOption)
        {
            var query = _appDbContext.Topic.AsQueryable();

            if (!string.IsNullOrEmpty(pageOption.Search))
            {
                query = query.Where(s => s.Name.Contains(pageOption.Search));
            }

            List<GetTopic> topics = await query
               .Skip(pageOption.PageSize * (pageOption.PageNumber - 1))
               .Take(pageOption.PageSize)
               .Select(x => new GetTopic
               {
                   Id = x.Id,
                   Name = x.Name,
                   CourseId = x.CourseId
               }).ToListAsync();

            int total = await _appDbContext.Topic.CountAsync();

            var pagination = new PaginationResult<GetTopic>
            {
                PageSize = pageOption.PageSize,
                PageNumber = pageOption.PageNumber,
                TotalCount = total,
                Values = topics
            };

            return ApiResult<PaginationResult<GetTopic>>.Success(pagination);
        }

        public async Task<ApiResult<string>> UpdateTopic(UpdateTopic updateTopic, Guid Id)
        {
            var topic = await _appDbContext.Topic.FirstOrDefaultAsync(x => x.Id == Id);

            if (topic == null)
                return ApiResult<string>.Failure(new List<string> { "Topic not found" });

            var course = await _appDbContext.Courses.FirstOrDefaultAsync(x => x.Id == updateTopic.CourseId);

            if (course == null)
                return ApiResult<string>.Failure(new List<string> { "Course not found" });

            topic.Name = updateTopic.Name;
            topic.CourseId = updateTopic.CourseId;

            _appDbContext.Update(topic);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("Topic has been updated");
        }

        public async Task<ApiResult<string>> DeleteTopic(Guid Id)
        {
            var topic = await _appDbContext.Topic.FirstOrDefaultAsync(x => x.Id == Id);

            if (topic == null)
                return ApiResult<string>.Failure(new List<string> { "Topic not found" });

            _appDbContext.Topic.Remove(topic);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("Topic has been deleted");
        }
    }
}
