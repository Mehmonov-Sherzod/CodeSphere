using CodeSphere.Application.Models;
using CodeSphere.Application.Models.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service
{
    public interface ITopicService
    {
        Task<ApiResult<string>> CreateTopic(CreateTopic createTopic);
        Task<ApiResult<PaginationResult<GetTopic>>> GetAllTopicPage(PageOption pageOption);
        Task<ApiResult<string>> UpdateTopic(UpdateTopic updateTopic, Guid Id);
        Task<ApiResult<string>> DeleteTopic(Guid Id);
    }
}
