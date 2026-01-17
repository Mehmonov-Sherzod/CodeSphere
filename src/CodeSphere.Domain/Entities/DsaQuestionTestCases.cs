using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Entities
{
    public class DsaQuestionTestCases
    {
        public Guid Id { get; set; }
        public string Input { get; set; }
        public string ExpectedOutput { get; set; }
        public bool IsHidden { get; set; }
        public Guid DsaQuestionId { get; set; }
        public DsaQuestions DsaQuestion { get; set; }
        
    }
}
