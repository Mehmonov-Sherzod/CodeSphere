using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Entities
{
    public class DsaQuestionDefinition
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; } = "Solution";
        public string MethodName { get; set; } 
        public string ReturnType { get; set; }
        public List<DsaQuestionDefinitionParameteres> Parameters { get; set; }
        public Guid DsaQuestionsId  { get; set; }
        public DsaQuestions DsaQuestions { get; set; }
    }
}
