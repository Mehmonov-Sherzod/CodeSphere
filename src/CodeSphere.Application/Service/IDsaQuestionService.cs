using CodeSphere.Application.Models;
using CodeSphere.Application.Models.DsaQuestion;
using System;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service
{
    public interface IDsaQuestionService
    {
        Task<ApiResult<string>> CreateDsaQuestion(CreateDsaQuestion createDsaQuestion);
        Task<ApiResult<PaginationResult<GetDsaQuestion>>> GetAllDsaQuestionsPage(PageOption pageOption);
        Task<ApiResult<GetDsaQuestion>> GetDsaQuestionById(Guid id);
        Task<ApiResult<string>> UpdateDsaQuestion(UpdateDsaQuestion updateDsaQuestion, Guid id);
        Task<ApiResult<string>> DeleteDsaQuestion(Guid id);
        Task<ApiResult<PaginationResult<GetDsaQuestion>>> GetDsaQuestionsByTopicId(Guid topicId, PageOption pageOption);
    }
}
