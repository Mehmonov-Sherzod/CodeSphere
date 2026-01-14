using CodeSphere.Application.Models;
using CodeSphere.Application.Models.User;

namespace CodeSphere.Application.Service
{
    public interface IUserService
    {
        Task<ApiResult<bool>> LoginRegister(TelegramAuth auth, string token);
    }
}
