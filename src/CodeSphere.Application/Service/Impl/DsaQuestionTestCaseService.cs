using CodeSphere.Application.Models;
using CodeSphere.Application.Models.DsaQuestionTestCase;
using CodeSphere.DataAccess.Persistence;
using CodeSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service.Impl
{
    public class DsaQuestionTestCaseService : IDsaQuestionTestCaseService
    {
        private readonly AppDbContext _appDbContext;

        public DsaQuestionTestCaseService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ApiResult<string>> CreateTestCase(CreateDsaQuestionTestCase createTestCase)
        {
            var questionExists = await _appDbContext.DsaQuestions.AnyAsync(q => q.Id == createTestCase.DsaQuestionId);
            if (!questionExists)
                return ApiResult<string>.Failure(new List<string> { "DSA Question not found" });

            var testCase = new DsaQuestionTestCases
            {
                Id = Guid.NewGuid(),
                Input = createTestCase.Input,
                ExpectedOutput = createTestCase.ExpectedOutput,
                IsHidden = createTestCase.IsHidden,
                DsaQuestionId = createTestCase.DsaQuestionId
            };

            await _appDbContext.DsaQuestionTestCases.AddAsync(testCase);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("Test case has been created");
        }

        public async Task<ApiResult<PaginationResult<GetDsaQuestionTestCase>>> GetAllTestCasesPage(PageOption pageOption)
        {
            var query = _appDbContext.DsaQuestionTestCases
                .Include(tc => tc.DsaQuestion)
                .AsQueryable();

            if (!string.IsNullOrEmpty(pageOption.Search))
            {
                query = query.Where(tc => tc.Input.Contains(pageOption.Search) ||
                                          tc.ExpectedOutput.Contains(pageOption.Search));
            }

            var totalCount = await query.CountAsync();

            var testCases = await query
                .Skip(pageOption.PageSize * (pageOption.PageNumber - 1))
                .Take(pageOption.PageSize)
                .Select(tc => new GetDsaQuestionTestCase
                {
                    Id = tc.Id,
                    Input = tc.Input,
                    ExpectedOutput = tc.ExpectedOutput,
                    IsHidden = tc.IsHidden,
                    DsaQuestionId = tc.DsaQuestionId,
                    QuestionTitle = tc.DsaQuestion.QuestionTitle
                }).ToListAsync();

            var pagination = new PaginationResult<GetDsaQuestionTestCase>
            {
                PageSize = pageOption.PageSize,
                PageNumber = pageOption.PageNumber,
                TotalCount = totalCount,
                Values = testCases
            };

            return ApiResult<PaginationResult<GetDsaQuestionTestCase>>.Success(pagination);
        }

        public async Task<ApiResult<GetDsaQuestionTestCase>> GetTestCaseById(Guid id)
        {
            var testCase = await _appDbContext.DsaQuestionTestCases
                .Include(tc => tc.DsaQuestion)
                .FirstOrDefaultAsync(tc => tc.Id == id);

            if (testCase == null)
                return ApiResult<GetDsaQuestionTestCase>.Failure(new List<string> { "Test case not found" });

            var result = new GetDsaQuestionTestCase
            {
                Id = testCase.Id,
                Input = testCase.Input,
                ExpectedOutput = testCase.ExpectedOutput,
                IsHidden = testCase.IsHidden,
                DsaQuestionId = testCase.DsaQuestionId,
                QuestionTitle = testCase.DsaQuestion?.QuestionTitle
            };

            return ApiResult<GetDsaQuestionTestCase>.Success(result);
        }

        public async Task<ApiResult<string>> UpdateTestCase(UpdateDsaQuestionTestCase updateTestCase, Guid id)
        {
            var testCase = await _appDbContext.DsaQuestionTestCases.FirstOrDefaultAsync(tc => tc.Id == id);

            if (testCase == null)
                return ApiResult<string>.Failure(new List<string> { "Test case not found" });

            testCase.Input = updateTestCase.Input;
            testCase.ExpectedOutput = updateTestCase.ExpectedOutput;
            testCase.IsHidden = updateTestCase.IsHidden;

            _appDbContext.DsaQuestionTestCases.Update(testCase);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("Test case has been updated");
        }

        public async Task<ApiResult<string>> DeleteTestCase(Guid id)
        {
            var testCase = await _appDbContext.DsaQuestionTestCases.FirstOrDefaultAsync(tc => tc.Id == id);

            if (testCase == null)
                return ApiResult<string>.Failure(new List<string> { "Test case not found" });

            _appDbContext.DsaQuestionTestCases.Remove(testCase);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<string>.Success("Test case has been deleted");
        }

        public async Task<ApiResult<PaginationResult<GetDsaQuestionTestCase>>> GetTestCasesByQuestionId(Guid questionId, PageOption pageOption)
        {
            var questionExists = await _appDbContext.DsaQuestions.AnyAsync(q => q.Id == questionId);
            if (!questionExists)
                return ApiResult<PaginationResult<GetDsaQuestionTestCase>>.Failure(new List<string> { "DSA Question not found" });

            var query = _appDbContext.DsaQuestionTestCases
                .Include(tc => tc.DsaQuestion)
                .Where(tc => tc.DsaQuestionId == questionId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(pageOption.Search))
            {
                query = query.Where(tc => tc.Input.Contains(pageOption.Search));
            }

            var totalCount = await query.CountAsync();

            var testCases = await query
                .Skip(pageOption.PageSize * (pageOption.PageNumber - 1))
                .Take(pageOption.PageSize)
                .Select(tc => new GetDsaQuestionTestCase
                {
                    Id = tc.Id,
                    Input = tc.Input,
                    ExpectedOutput = tc.ExpectedOutput,
                    IsHidden = tc.IsHidden,
                    DsaQuestionId = tc.DsaQuestionId,
                    QuestionTitle = tc.DsaQuestion.QuestionTitle
                }).ToListAsync();

            var pagination = new PaginationResult<GetDsaQuestionTestCase>
            {
                PageSize = pageOption.PageSize,
                PageNumber = pageOption.PageNumber,
                TotalCount = totalCount,
                Values = testCases
            };

            return ApiResult<PaginationResult<GetDsaQuestionTestCase>>.Success(pagination);
        }
    }
}
