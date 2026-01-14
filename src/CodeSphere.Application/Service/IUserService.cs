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
    }
}
