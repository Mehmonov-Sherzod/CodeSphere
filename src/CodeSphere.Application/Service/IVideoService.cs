using CodeSphere.Application.Models;
using CodeSphere.Application.Models.Video;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service
{
    public interface IVideoService
    {
        Task<ApiResult<string>> CreateVideo(CreateVideo createVideo);
        Task<ApiResult<PaginationResult<GetVideo>>> GetAllVideoPage(PageOption pageOption);
        Task<ApiResult<string>> UpdateVideo(UpdateVideo updateVideo, Guid id);
        Task<ApiResult<string>> DeleteVideo(Guid id);
    }
}
