using CodeSphere.Application.Models;
using CodeSphere.Application.Models.DsaQuestionDefinition;
using System;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service
{
    public interface IDsaQuestionDefinitionService
    {
        Task<ApiResult<string>> CreateDsaQuestionDefinition(CreateDsaQuestionDefinition createDsaQuestionDefinition);
        Task<ApiResult<GetDsaQuestionDefinition>> GetDsaQuestionDefinitionById(Guid id);
        Task<ApiResult<GetDsaQuestionDefinition>> GetDsaQuestionDefinitionByQuestionId(Guid dsaQuestionId);
        Task<ApiResult<string>> UpdateDsaQuestionDefinition(UpdateDsaQuestionDefinition updateDsaQuestionDefinition, Guid id);
        Task<ApiResult<string>> DeleteDsaQuestionDefinition(Guid id);
    }
}
