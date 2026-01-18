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

            // Add test cases
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

            // Add topic associations
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

            // Add definition with parameters
            if (createDsaQuestion.Definition != null)
            {
                var definition = new DsaQuestionDefinition
                {
                    Id = Guid.NewGuid(),
                    ClassName = createDsaQuestion.Definition.ClassName, 
                    MethodName = createDsaQuestion.Definition.MethodName,
                    ReturnType = createDsaQuestion.Definition.ReturnType,
                    DsaQuestionsId = dsaQuestion.Id,
                    Parameters = new List<DsaQuestionDefinitionParameteres>()
                };

                if (createDsaQuestion.Definition.Parameters != null && createDsaQuestion.Definition.Parameters.Any())
                {
                    foreach (var param in createDsaQuestion.Definition.Parameters)
                    {
                        definition.Parameters.Add(new DsaQuestionDefinitionParameteres
                        {
                            Id = Guid.NewGuid(),
                            Name = param.Name,
                            Type = param.Type,
                            
                            DsaQuestionDefinitionId = definition.Id
                        });
                    }
                }

                dsaQuestion.Definition = definition;
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
                .Include(q => q.Definition)
                    .ThenInclude(d => d.Parameters)
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
                    }).ToList(),
                    Definition = q.Definition != null ? new GetDefinition
                    {
                        Id = q.Definition.Id,
                        ClassName = q.Definition.ClassName,
                        MethodName = q.Definition.MethodName,
                        ReturnType = q.Definition.ReturnType,
                        Parameters = q.Definition.Parameters.Select(p => new GetDefinitionParameter
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Type = p.Type,
                        
                        }).ToList()
                    } : null
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
                .Include(q => q.Definition)
                    .ThenInclude(d => d.Parameters)
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
                }).ToList() ?? new List<GetDsaTopicInfo>(),
                Definition = question.Definition != null ? new GetDefinition
                {
                    Id = question.Definition.Id,
                    ClassName = question.Definition.ClassName,
                    MethodName = question.Definition.MethodName,
                    ReturnType = question.Definition.ReturnType,
                    Parameters = question.Definition.Parameters?.Select(p => new GetDefinitionParameter
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Type = p.Type,
                    }).ToList() ?? new List<GetDefinitionParameter>()
                } : null
            };

            return ApiResult<GetDsaQuestion>.Success(result);
        }

        public async Task<ApiResult<string>> UpdateDsaQuestion(UpdateDsaQuestion updateDsaQuestion, Guid id)
        {
            var question = await _appDbContext.DsaQuestions
                .Include(q => q.TestCases)
                .Include(q => q.TopicQuestions)
                .Include(q => q.Definition)
                    .ThenInclude(d => d.Parameters)
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
                _appDbContext.DsaQuestionTestCases.RemoveRange(question.TestCases);

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
                _appDbContext.DsaTopicQuestions.RemoveRange(question.TopicQuestions);

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

            // Update definition
            if (updateDsaQuestion.Definition != null)
            {
                // Remove existing definition and parameters
                if (question.Definition != null)
                {
                    if (question.Definition.Parameters != null)
                    {
                        _appDbContext.DsaQuestionDefinitionParameteres.RemoveRange(question.Definition.Parameters);
                    }
                    _appDbContext.DsaQuestionDefinitions.Remove(question.Definition);
                }

                // Add new definition
                var definition = new DsaQuestionDefinition
                {
                    Id = updateDsaQuestion.Definition.Id ?? Guid.NewGuid(),
                    MethodName = updateDsaQuestion.Definition.MethodName,
                    ClassName = updateDsaQuestion.Definition.ClassName,
                    ReturnType = updateDsaQuestion.Definition.ReturnType,
                    DsaQuestionsId = id,
                    Parameters = new List<DsaQuestionDefinitionParameteres>()
                };

                if (updateDsaQuestion.Definition.Parameters != null && updateDsaQuestion.Definition.Parameters.Any())
                {
                    foreach (var param in updateDsaQuestion.Definition.Parameters)
                    {
                        definition.Parameters.Add(new DsaQuestionDefinitionParameteres
                        {
                            Id = param.Id ?? Guid.NewGuid(),
                            Name = param.Name,
                            Type = param.Type,
                            DsaQuestionDefinitionId = definition.Id
                        });
                    }
                }

                _appDbContext.DsaQuestionDefinitions.Add(definition);
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
                .Include(q => q.Definition)
                    .ThenInclude(d => d.Parameters)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
                return ApiResult<string>.Failure(new List<string> { "DSA Question not found" });

            // Remove definition and parameters
            if (question.Definition != null)
            {
                if (question.Definition.Parameters != null)
                {
                    _appDbContext.DsaQuestionDefinitionParameteres.RemoveRange(question.Definition.Parameters);
                }
                _appDbContext.DsaQuestionDefinitions.Remove(question.Definition);
            }

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
                .Include(q => q.Definition)
                    .ThenInclude(d => d.Parameters)
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
                    }).ToList(),
                    Definition = q.Definition != null ? new GetDefinition
                    {
                        Id = q.Definition.Id,
                        MethodName = q.Definition.MethodName,
                        ClassName = q.Definition.ClassName,
                        ReturnType = q.Definition.ReturnType,
                        Parameters = q.Definition.Parameters.Select(p => new GetDefinitionParameter
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Type = p.Type,
                        }).ToList()
                    } : null
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
