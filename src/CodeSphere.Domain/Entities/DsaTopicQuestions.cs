using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Entities
{
    public class DsaTopicQuestions
    {
        public Guid Id { get; set; }
        public Guid DsaTopicsId { get; set; }
        public DsaTopics DsaTopics { get; set; }
        public Guid DsaQuestionsId { get; set; }
        public DsaQuestions DsaQuestions { get; set; }
    }
}
