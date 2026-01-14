using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeSphere.Application.Models;
using CodeSphere.Application.Models.User;
using CodeSphere.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CodeSphere.Application.Service.Impl
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;
        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ApiResult<bool>> LoginRegister(TelegramAuth auth , string token)
        {
           if(VerifyHash (auth, token))
            {
                //Reg or login logic here
                return ApiResult<bool>.Success(true);
            }
           else
            {
                return ApiResult<bool>.Failure(new List<string> { "Invalid Telegram authentication data." });
            }
        }
        private static bool VerifyHash(TelegramAuth auth, string botToken)
        {
            var dataCheckString = new StringBuilder();
            var properties = new Dictionary<string, string>
            {
                { "auth_date", auth.Auth_date.ToString() },
                { "first_name", auth.First_name },
                { "id", auth.Id.ToString() },
                { "photo_url", auth.Photo_url },
                { "username", auth.Username }
            };
            foreach (var prop in properties.OrderBy(p => p.Key))
            {
                if (dataCheckString.Length > 0)
                    dataCheckString.Append("\n");
                dataCheckString.Append($"{prop.Key}={prop.Value}");
            }
            using var hmac = new System.Security.Cryptography.HMACSHA256(System.Text.Encoding.UTF8.GetBytes(
                System.BitConverter.ToString(
                    System.Security.Cryptography.SHA256.HashData(
                        System.Text.Encoding.UTF8.GetBytes(botToken)
                    )
                ).Replace("-", "").ToLower()
            ));
            var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(dataCheckString.ToString()));
            var computedHash = System.BitConverter.ToString(hash).Replace("-", "").ToLower();
            return computedHash == auth.Hash;
        }
    }
}
