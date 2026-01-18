using System;
using System.Collections.Generic;

namespace CodeSphere.Application.Models.DsaQuestion
{
    public class CreateDsaQuestion
    {
        public string QuestionTitle { get; set; }
        public string QuestionDescription { get; set; }
        public string DifficultyLevel { get; set; }
        public string? SolutionUrl { get; set; }
        public List<Guid>? TopicIds { get; set; }
        public List<CreateTestCase>? TestCases { get; set; }
        public CreateDefinition? Definition { get; set; }
    }

    public class CreateTestCase
    {
        public string Input { get; set; }
        public string ExpectedOutput { get; set; }
        public bool IsHidden { get; set; }
    }

    public class CreateDefinition
    {
        public string ClassName { get; set; } = "Solution";
        public string MethodName { get; set; }
        public string ReturnType { get; set; }
        public List<CreateDefinitionParameter>? Parameters { get; set; }
    }

    public class CreateDefinitionParameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
