using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Models.LoginResponse
{
    public class LoginResponse
    {
        public string? TelegramId { get; set; }
        public string? AccessToken { get; set; }
        public int AccessTime { get; set; }
    }
}
