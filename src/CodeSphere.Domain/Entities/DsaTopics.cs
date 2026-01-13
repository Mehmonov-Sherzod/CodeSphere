using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Entities
{
    public class DsaTopics
    {
        public Guid Id { get; set; }
        public string TopicName { get; set; }
        public int TopicLevel { get; set; }
       
    }
}
