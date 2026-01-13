using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Entities
{
    public class Videos
    {
        public Guid Id { get; set; }    
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string VideoUrl { get; set; }
        public Topic Topic { get; set; }    
        public Guid TopicId { get; set; }   
    }
}
