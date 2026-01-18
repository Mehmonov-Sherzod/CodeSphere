using CodeSphere.Application.Models;
using CodeSphere.Application.Models.DsaTopic;
using CodeSphere.DataAccess.Persistence;
using CodeSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service.Impl
{
    public class DsaTopicService : IDsaTopicService
    {
        private readonly AppDbContext _appDbContext;

        public DsaTopicService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ApiResult<string>> CreateDsaTopic(CreateDsaTopic createDsaTopic)
        {
            var dsaTopic = new DsaTopics
            {
                Id = Guid.NewGuid(),
                TopicName = createDsaTopic.TopicName,
                TopicLevel = createDsaTopic.TopicLevel,
                TopicQuestions = new List<DsaTopicQuestions>()
            };

            await _appDbContext.DsaTopics.AddAsync(dsaTopic);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("DSA Topic has been created");
        }

        public async Task<ApiResult<PaginationResult<GetDsaTopic>>> GetAllDsaTopicsPage(PageOption pageOption)
        {
            var query = _appDbContext.DsaTopics
                .Include(t => t.TopicQuestions)
                .AsQueryable();

            if (!string.IsNullOrEmpty(pageOption.Search))
            {
                query = query.Where(t => t.TopicName.Contains(pageOption.Search));
            }

            var totalCount = await query.CountAsync();

            var topics = await query
                .Skip(pageOption.PageSize * (pageOption.PageNumber - 1))
                .Take(pageOption.PageSize)
                .Select(t => new GetDsaTopic
                {
                    Id = t.Id,
                    TopicName = t.TopicName,
                    TopicLevel = t.TopicLevel,
                    QuestionCount = t.TopicQuestions.Count
                }).ToListAsync();

            var pagination = new PaginationResult<GetDsaTopic>
            {
                PageSize = pageOption.PageSize,
                PageNumber = pageOption.PageNumber,
                TotalCount = totalCount,
                Values = topics
            };

            return ApiResult<PaginationResult<GetDsaTopic>>.Success(pagination);
        }

        public async Task<ApiResult<GetDsaTopicWithQuestions>> GetDsaTopicById(Guid id)
        {
            var topic = await _appDbContext.DsaTopics
                .Include(t => t.TopicQuestions)
                    .ThenInclude(tq => tq.DsaQuestions)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (topic == null)
                return ApiResult<GetDsaTopicWithQuestions>.Failure(new List<string> { "DSA Topic not found" });

            var result = new GetDsaTopicWithQuestions
            {
                Id = topic.Id,
                TopicName = topic.TopicName,
                TopicLevel = topic.TopicLevel,
                Questions = topic.TopicQuestions?.Select(tq => new DsaQuestionSummary
                {
                    Id = tq.DsaQuestions.Id,
                    QuestionTitle = tq.DsaQuestions.QuestionTitle,
                    DifficultyLevel = tq.DsaQuestions.DifficultyLevel
                }).ToList() ?? new List<DsaQuestionSummary>()
            };

            return ApiResult<GetDsaTopicWithQuestions>.Success(result);
        }

        public async Task<ApiResult<string>> UpdateDsaTopic(UpdateDsaTopic updateDsaTopic, Guid id)
        {
            var topic = await _appDbContext.DsaTopics.FirstOrDefaultAsync(t => t.Id == id);

            if (topic == null)
                return ApiResult<string>.Failure(new List<string> { "DSA Topic not found" });

            topic.TopicName = updateDsaTopic.TopicName;
            topic.TopicLevel = updateDsaTopic.TopicLevel;

            _appDbContext.DsaTopics.Update(topic);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("DSA Topic has been updated");
        }

        public async Task<ApiResult<string>> DeleteDsaTopic(Guid id)
        {
            var topic = await _appDbContext.DsaTopics
                .Include(t => t.TopicQuestions)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (topic == null)
                return ApiResult<string>.Failure(new List<string> { "DSA Topic not found" });

            // Remove topic-question associations
            _appDbContext.DsaTopicQuestions.RemoveRange(topic.TopicQuestions);
            _appDbContext.DsaTopics.Remove(topic);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("DSA Topic has been deleted");
        }
    }
}
