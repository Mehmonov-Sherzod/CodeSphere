using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Entities
{
    public   class DsaQuestionDefinitionParameteres
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public Guid DsaQuestionDefinitionId { get; set; }
        public DsaQuestionDefinition DsaQuestionDefinition { get; set; }
    }
}
