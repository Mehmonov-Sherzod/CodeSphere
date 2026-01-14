using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Models.User
{
    public class TelegramAuth
    {
        public long Id { get; set; }
        public string First_name { get; set; }
        public string Username { get; set; }
        public string Photo_url { get; set; }
        public long Auth_date { get; set; }
        public string Hash { get; set; }
    }
}
