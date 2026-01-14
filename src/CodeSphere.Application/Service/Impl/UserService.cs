using CodeSphere.Application.Models;
using CodeSphere.Application.Models.Jwt;
using CodeSphere.Application.Models.LoginResponse;
using CodeSphere.Application.Models.User;
using CodeSphere.DataAccess.Persistence;
using CodeSphere.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Service.Impl
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IConfiguration _configuration;
        private readonly JwtSettings _jwtSettings;  
        public UserService(AppDbContext appDbContext , IConfiguration configuration ,IOptions<JwtSettings> jwt)
        {
            _appDbContext = appDbContext;
            _configuration = configuration;
            _jwtSettings = jwt.Value;
        }

        public async Task<ApiResult<LoginResponse>> LoginRegister(TelegramAuth auth , string token)
        {
           if(VerifyHash (auth, token))
            {
                var user = await _appDbContext.Users
                    .FirstOrDefaultAsync(u => u.TelegramId == auth.Id.ToString());
                if (user == null)
                {
                    var newUser = new User
                    {
                        FullName = auth.First_name,
                        PhoneNumber = "",
                        TelegramId = auth.Id.ToString(),
                        UserRoles = new List<UserRole>()
                    };
                    var userRole = new UserRole
                    {
                        RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // Default role ID
                        UserId = newUser.Id
                    };
                    newUser.UserRoles.Add(userRole);
                    _appDbContext.Users.Add(newUser);
                    await _appDbContext.SaveChangesAsync();
                    var issuer = _jwtSettings.Issuer;
                    var audience = _jwtSettings.Audience;
                    var key = _jwtSettings.Key;
                    double tokenValidityMins = (double)_jwtSettings.TokenValidityMins;
                    var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, newUser.TelegramId),
                        new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString())
                    };
                    var userRoles = await _appDbContext.UserRoles
                        .Where(ur => ur.UserId == newUser.Id)
                        .Include(ur => ur.Role)
                        .FirstOrDefaultAsync();

                    claims.Add(new Claim(ClaimTypes.Role, userRoles.Role.Name));
                    var tokenDescription = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = tokenExpiryTimeStamp,
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescription);
                    var accessToken = tokenHandler.WriteToken(securityToken);
                    return ApiResult<LoginResponse>
                        .Success(new LoginResponse
                        {
                            AccessTime = (int)tokenValidityMins,
                            AccessToken = accessToken,
                            TelegramId = newUser.TelegramId
                        });
                }
                else
                {
                    var issuer = _jwtSettings.Issuer;
                    var audience = _jwtSettings.Audience;
                    var key = _jwtSettings.Key;
                    double tokenValidityMins = (double)_jwtSettings.TokenValidityMins;
                    var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidityMins);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.TelegramId),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };
                    var userRoles = await _appDbContext.UserRoles
                        .Where(ur => ur.UserId == user.Id)
                        .Include(ur => ur.Role)
                        .FirstOrDefaultAsync();

                    claims.Add(new Claim(ClaimTypes.Role, userRoles.Role.Name));
                    var tokenDescription = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = tokenExpiryTimeStamp,
                        Issuer = issuer,
                        Audience = audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key)), SecurityAlgorithms.HmacSha256)
                    };
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.CreateToken(tokenDescription);
                    var accessToken = tokenHandler.WriteToken(securityToken);
                    return ApiResult<LoginResponse>
                        .Success(new LoginResponse
                        {
                            AccessTime = (int)tokenValidityMins,
                            AccessToken = accessToken,
                            TelegramId = user.TelegramId
                        });
                }
                
            }
           else
            {
                return ApiResult<LoginResponse>.Failure(new List<string> { "Invalid Telegram authentication data." });
            }
        }
        public async Task<ApiResult<PaginationResult<GetUser>>> GetAllUserPage(PageOption pageOption)
        {
            var query = _appDbContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(pageOption.Search))
            {
                query = query.Where(u => u.FullName.Contains(pageOption.Search) ||
                                         u.TelegramId.Contains(pageOption.Search) ||
                                         u.PhoneNumber.Contains(pageOption.Search));
            }

            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((pageOption.PageNumber - 1) * pageOption.PageSize)
                .Take(pageOption.PageSize)
                .Select(u => new GetUser
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    PhoneNumber = u.PhoneNumber,
                    TelegramId = u.TelegramId
                })
                .ToListAsync();

            var result = new PaginationResult<GetUser>
            {
                Values = users,
                PageNumber = pageOption.PageNumber,
                PageSize = pageOption.PageSize,
                TotalCount = totalCount
            };

            return ApiResult<PaginationResult<GetUser>>.Success(result);
        }

        public async Task<ApiResult<GetUser>> GetUserById(Guid id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return ApiResult<GetUser>.Failure(new List<string> { "User not found" });
            }

            var getUser = new GetUser
            {
                Id = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                TelegramId = user.TelegramId
            };

            return ApiResult<GetUser>.Success(getUser);
        }

        public async Task<ApiResult<GetUser>> UpdateUser(UpdateUser updateUser, Guid id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return ApiResult<GetUser>.Failure(new List<string> { "User not found" });
            }

            user.FullName = updateUser.FullName;
            user.PhoneNumber = updateUser.PhoneNumber;

            await _appDbContext.SaveChangesAsync();

            var getUser = new GetUser
            {
                Id = user.Id,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                TelegramId = user.TelegramId
            };

            return ApiResult<GetUser>.Success(getUser);
        }

        public async Task<ApiResult<bool>> DeleteUser(Guid id)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return ApiResult<bool>.Failure(new List<string> { "User not found" });
            }

            _appDbContext.Users.Remove(user);
            await _appDbContext.SaveChangesAsync();

            return ApiResult<bool>.Success(true);
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
