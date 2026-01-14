using CodeSphere.Application.Models;
using CodeSphere.Application.Models.LoginResponse;
using CodeSphere.Application.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service
{
    public interface IUserService
    {
        Task<ApiResult<LoginResponse>> LoginRegister(TelegramAuth auth, string token);
        Task<ApiResult<PaginationResult<GetUser>>> GetAllUserPage(PageOption pageOption);
        Task<ApiResult<GetUser>> GetUserById(Guid id);
        Task<ApiResult<GetUser>> UpdateUser(UpdateUser updateUser, Guid id);
        Task<ApiResult<bool>> DeleteUser(Guid id);
    }
}
