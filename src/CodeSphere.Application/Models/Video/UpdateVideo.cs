using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Models.Video
{
    public class UpdateVideo
    {
        public string Name { get; set; }
        public IFormFile? Image { get; set; }
        public IFormFile? Video { get; set; }
        public Guid TopicId { get; set; }
    }
}
