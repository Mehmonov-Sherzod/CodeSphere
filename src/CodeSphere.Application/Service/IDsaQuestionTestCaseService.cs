using CodeSphere.Application.Models;
using CodeSphere.Application.Models.DsaQuestionTestCase;
using System;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service
{
    public interface IDsaQuestionTestCaseService
    {
        Task<ApiResult<string>> CreateTestCase(CreateDsaQuestionTestCase createTestCase);
        Task<ApiResult<PaginationResult<GetDsaQuestionTestCase>>> GetAllTestCasesPage(PageOption pageOption);
        Task<ApiResult<GetDsaQuestionTestCase>> GetTestCaseById(Guid id);
        Task<ApiResult<string>> UpdateTestCase(UpdateDsaQuestionTestCase updateTestCase, Guid id);
        Task<ApiResult<string>> DeleteTestCase(Guid id);
        Task<ApiResult<PaginationResult<GetDsaQuestionTestCase>>> GetTestCasesByQuestionId(Guid questionId, PageOption pageOption);
    }
}
