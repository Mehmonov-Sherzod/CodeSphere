using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Models.Course
{
    public class GetCourse
    {
        public string Name { get; set; }
        public string Author { get; set; }
        public IFormFile? Image { get; set; }
    }
}
