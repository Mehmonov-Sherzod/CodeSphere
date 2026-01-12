using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Entities
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; }
        public string Author { get; set; }
        public List<Topic> topics { get; set; } 
    }
}
