using CodeSphere.Application.Models;
using CodeSphere.Application.Models.DsaQuestion;
using CodeSphere.DataAccess.Persistence;
using CodeSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service.Impl
{
    public class DsaQuestionService : IDsaQuestionService
    {
        private readonly AppDbContext _appDbContext;

        public DsaQuestionService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ApiResult<string>> CreateDsaQuestion(CreateDsaQuestion createDsaQuestion)
        {
            var dsaQuestion = new DsaQuestions
            {
                Id = Guid.NewGuid(),
                QuestionTitle = createDsaQuestion.QuestionTitle,
                QuestionDescription = createDsaQuestion.QuestionDescription,
                DifficultyLevel = createDsaQuestion.DifficultyLevel,
                SolutionUrl = createDsaQuestion.SolutionUrl,
                TestCases = new List<DsaQuestionTestCases>(),
                TopicQuestions = new List<DsaTopicQuestions>()
            };

            if (createDsaQuestion.TestCases != null && createDsaQuestion.TestCases.Any())
            {
                foreach (var testCase in createDsaQuestion.TestCases)
                {
                    dsaQuestion.TestCases.Add(new DsaQuestionTestCases
                    {
                        Id = Guid.NewGuid(),
                        Input = testCase.Input,
                        ExpectedOutput = testCase.ExpectedOutput,
                        IsHidden = testCase.IsHidden,
                        DsaQuestionId = dsaQuestion.Id
                    });
                }
            }

            if (createDsaQuestion.TopicIds != null && createDsaQuestion.TopicIds.Any())
            {
                foreach (var topicId in createDsaQuestion.TopicIds)
                {
                    var topicExists = await _appDbContext.DsaTopics.AnyAsync(t => t.Id == topicId);
                    if (topicExists)
                    {
                        dsaQuestion.TopicQuestions.Add(new DsaTopicQuestions
                        {
                            Id = Guid.NewGuid(),
                            DsaQuestionsId = dsaQuestion.Id,
                            DsaTopicsId = topicId
                        });
                    }
                }
            }

            await _appDbContext.DsaQuestions.AddAsync(dsaQuestion);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("DSA Question has been created");
        }

        public async Task<ApiResult<PaginationResult<GetDsaQuestion>>> GetAllDsaQuestionsPage(PageOption pageOption)
        {
            var query = _appDbContext.DsaQuestions
                .Include(q => q.TestCases)
                .Include(q => q.TopicQuestions)
                    .ThenInclude(tq => tq.DsaTopics)
                .AsQueryable();

            if (!string.IsNullOrEmpty(pageOption.Search))
            {
                query = query.Where(q => q.QuestionTitle.Contains(pageOption.Search) ||
                                         q.DifficultyLevel.Contains(pageOption.Search));
            }

            var totalCount = await query.CountAsync();

            var questions = await query
                .Skip(pageOption.PageSize * (pageOption.PageNumber - 1))
                .Take(pageOption.PageSize)
                .Select(q => new GetDsaQuestion
                {
                    Id = q.Id,
                    QuestionTitle = q.QuestionTitle,
                    QuestionDescription = q.QuestionDescription,
                    DifficultyLevel = q.DifficultyLevel,
                    SolutionUrl = q.SolutionUrl,
                    TestCases = q.TestCases.Select(tc => new GetTestCase
                    {
                        Id = tc.Id,
                        Input = tc.Input,
                        ExpectedOutput = tc.ExpectedOutput,
                        IsHidden = tc.IsHidden
                    }).ToList(),
                    Topics = q.TopicQuestions.Select(tq => new GetDsaTopicInfo
                    {
                        Id = tq.DsaTopics.Id,
                        TopicName = tq.DsaTopics.TopicName
                    }).ToList()
                }).ToListAsync();

            var pagination = new PaginationResult<GetDsaQuestion>
            {
                PageSize = pageOption.PageSize,
                PageNumber = pageOption.PageNumber,
                TotalCount = totalCount,
                Values = questions
            };

            return ApiResult<PaginationResult<GetDsaQuestion>>.Success(pagination);
        }

        public async Task<ApiResult<GetDsaQuestion>> GetDsaQuestionById(Guid id)
        {
            var question = await _appDbContext.DsaQuestions
                .Include(q => q.TestCases)
                .Include(q => q.TopicQuestions)
                    .ThenInclude(tq => tq.DsaTopics)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
                return ApiResult<GetDsaQuestion>.Failure(new List<string> { "DSA Question not found" });

            var result = new GetDsaQuestion
            {
                Id = question.Id,
                QuestionTitle = question.QuestionTitle,
                QuestionDescription = question.QuestionDescription,
                DifficultyLevel = question.DifficultyLevel,
                SolutionUrl = question.SolutionUrl,
                TestCases = question.TestCases?.Select(tc => new GetTestCase
                {
                    Id = tc.Id,
                    Input = tc.Input,
                    ExpectedOutput = tc.ExpectedOutput,
                    IsHidden = tc.IsHidden
                }).ToList() ?? new List<GetTestCase>(),
                Topics = question.TopicQuestions?.Select(tq => new GetDsaTopicInfo
                {
                    Id = tq.DsaTopics.Id,
                    TopicName = tq.DsaTopics.TopicName
                }).ToList() ?? new List<GetDsaTopicInfo>()
            };

            return ApiResult<GetDsaQuestion>.Success(result);
        }

        public async Task<ApiResult<string>> UpdateDsaQuestion(UpdateDsaQuestion updateDsaQuestion, Guid id)
        {
            var question = await _appDbContext.DsaQuestions
                .Include(q => q.TestCases)
                .Include(q => q.TopicQuestions)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
                return ApiResult<string>.Failure(new List<string> { "DSA Question not found" });

            question.QuestionTitle = updateDsaQuestion.QuestionTitle;
            question.QuestionDescription = updateDsaQuestion.QuestionDescription;
            question.DifficultyLevel = updateDsaQuestion.DifficultyLevel;
            question.SolutionUrl = updateDsaQuestion.SolutionUrl;

            // Update test cases
            if (updateDsaQuestion.TestCases != null)
            {
                // Remove existing test cases
                _appDbContext.DsaQuestionTestCases.RemoveRange(question.TestCases);

                // Add new test cases
                foreach (var testCase in updateDsaQuestion.TestCases)
                {
                    _appDbContext.DsaQuestionTestCases.Add(new DsaQuestionTestCases
                    {
                        Id = testCase.Id ?? Guid.NewGuid(),
                        Input = testCase.Input,
                        ExpectedOutput = testCase.ExpectedOutput,
                        IsHidden = testCase.IsHidden,
                        DsaQuestionId = id
                    });
                }
            }

            // Update topic associations
            if (updateDsaQuestion.TopicIds != null)
            {
                // Remove existing associations
                _appDbContext.DsaTopicQuestions.RemoveRange(question.TopicQuestions);

                // Add new associations
                foreach (var topicId in updateDsaQuestion.TopicIds)
                {
                    var topicExists = await _appDbContext.DsaTopics.AnyAsync(t => t.Id == topicId);
                    if (topicExists)
                    {
                        _appDbContext.DsaTopicQuestions.Add(new DsaTopicQuestions
                        {
                            Id = Guid.NewGuid(),
                            DsaQuestionsId = id,
                            DsaTopicsId = topicId
                        });
                    }
                }
            }

            _appDbContext.DsaQuestions.Update(question);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("DSA Question has been updated");
        }

        public async Task<ApiResult<string>> DeleteDsaQuestion(Guid id)
        {
            var question = await _appDbContext.DsaQuestions
                .Include(q => q.TestCases)
                .Include(q => q.TopicQuestions)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
                return ApiResult<string>.Failure(new List<string> { "DSA Question not found" });

            _appDbContext.DsaQuestionTestCases.RemoveRange(question.TestCases);
            _appDbContext.DsaTopicQuestions.RemoveRange(question.TopicQuestions);
            _appDbContext.DsaQuestions.Remove(question);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("DSA Question has been deleted");
        }

        public async Task<ApiResult<PaginationResult<GetDsaQuestion>>> GetDsaQuestionsByTopicId(Guid topicId, PageOption pageOption)
        {
            var topicExists = await _appDbContext.DsaTopics.AnyAsync(t => t.Id == topicId);
            if (!topicExists)
                return ApiResult<PaginationResult<GetDsaQuestion>>.Failure(new List<string> { "DSA Topic not found" });

            var query = _appDbContext.DsaQuestions
                .Include(q => q.TestCases)
                .Include(q => q.TopicQuestions)
                    .ThenInclude(tq => tq.DsaTopics)
                .Where(q => q.TopicQuestions.Any(tq => tq.DsaTopicsId == topicId))
                .AsQueryable();

            if (!string.IsNullOrEmpty(pageOption.Search))
            {
                query = query.Where(q => q.QuestionTitle.Contains(pageOption.Search));
            }

            var totalCount = await query.CountAsync();

            var questions = await query
                .Skip(pageOption.PageSize * (pageOption.PageNumber - 1))
                .Take(pageOption.PageSize)
                .Select(q => new GetDsaQuestion
                {
                    Id = q.Id,
                    QuestionTitle = q.QuestionTitle,
                    QuestionDescription = q.QuestionDescription,
                    DifficultyLevel = q.DifficultyLevel,
                    SolutionUrl = q.SolutionUrl,
                    TestCases = q.TestCases.Select(tc => new GetTestCase
                    {
                        Id = tc.Id,
                        Input = tc.Input,
                        ExpectedOutput = tc.ExpectedOutput,
                        IsHidden = tc.IsHidden
                    }).ToList(),
                    Topics = q.TopicQuestions.Select(tq => new GetDsaTopicInfo
                    {
                        Id = tq.DsaTopics.Id,
                        TopicName = tq.DsaTopics.TopicName
                    }).ToList()
                }).ToListAsync();

            var pagination = new PaginationResult<GetDsaQuestion>
            {
                PageSize = pageOption.PageSize,
                PageNumber = pageOption.PageNumber,
                TotalCount = totalCount,
                Values = questions
            };

            return ApiResult<PaginationResult<GetDsaQuestion>>.Success(pagination);
        }
    }
}
