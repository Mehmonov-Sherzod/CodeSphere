using CodeSphere.Application.Models;
using CodeSphere.Application.Models.DsaTopic;
using System;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service
{
    public interface IDsaTopicService
    {
        Task<ApiResult<string>> CreateDsaTopic(CreateDsaTopic createDsaTopic);
        Task<ApiResult<PaginationResult<GetDsaTopic>>> GetAllDsaTopicsPage(PageOption pageOption);
        Task<ApiResult<GetDsaTopicWithQuestions>> GetDsaTopicById(Guid id);
        Task<ApiResult<string>> UpdateDsaTopic(UpdateDsaTopic updateDsaTopic, Guid id);
        Task<ApiResult<string>> DeleteDsaTopic(Guid id);
    }
}
