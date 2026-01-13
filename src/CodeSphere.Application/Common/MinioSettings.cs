using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Common
{
    public class MinioSettings
    {
        public string Endpoint { get; set; } = null!;
        public string AccessKey { get; set; } = null!;
        public string SecretKey { get; set; } = null!;
        public bool UseSsl { get; set; } // Agar HTTPS ishlatayotgan bo'lsangiz true, aks holda false
    }
}
