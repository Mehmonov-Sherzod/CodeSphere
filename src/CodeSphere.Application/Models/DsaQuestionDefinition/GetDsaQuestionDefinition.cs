using System;
using System.Collections.Generic;

namespace CodeSphere.Application.Models.DsaQuestionDefinition
{
    public class GetDsaQuestionDefinition
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string ReturnType { get; set; }
        public Guid DsaQuestionsId { get; set; }
        public List<GetDsaQuestionDefinitionParameter> Parameters { get; set; }
    }

    public class GetDsaQuestionDefinitionParameter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
