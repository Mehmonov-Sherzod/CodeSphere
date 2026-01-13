using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Entities
{
    public class Topic
    {
        public Guid Id { get; set; }    

        public string Name { get; set; }

        public Course Course { get; set; }

        public Guid CourseId { get; set; }  
    }
}
