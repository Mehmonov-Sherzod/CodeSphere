using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Application.Models.Topic
{
    public class CreateTopic
    {
        public string Name { get; set; }
        public Guid CourseId { get; set; }
    }
}
