using System;
using System.Collections.Generic;

namespace CodeSphere.Application.Models.DsaQuestionDefinition
{
    public class CreateDsaQuestionDefinition
    {
        public string ClassName { get; set; } = "Solution";
        public string MethodName { get; set; }
        public string ReturnType { get; set; }
        public Guid DsaQuestionsId { get; set; }
        public List<CreateDsaQuestionDefinitionParameter>? Parameters { get; set; }
    }

    public class CreateDsaQuestionDefinitionParameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
