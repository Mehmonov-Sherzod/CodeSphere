using System;
using System.Collections.Generic;

namespace CodeSphere.Application.Models.DsaQuestionDefinition
{
    public class UpdateDsaQuestionDefinition
    {
        public string ClassName { get; set; } = "Solution";
        public string MethodName { get; set; }
        public string ReturnType { get; set; }
        public List<UpdateDsaQuestionDefinitionParameter>? Parameters { get; set; }
    }

    public class UpdateDsaQuestionDefinitionParameter
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
