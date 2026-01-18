using System;
using System.Collections.Generic;

namespace CodeSphere.Application.Models.DsaQuestion
{
    public class GetDsaQuestion
    {
        public Guid Id { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionDescription { get; set; }
        public string DifficultyLevel { get; set; }
        public string? SolutionUrl { get; set; }
        public List<GetTestCase> TestCases { get; set; }
        public List<GetDsaTopicInfo> Topics { get; set; }
        public GetDefinition? Definition { get; set; }
    }

    public class GetTestCase
    {
        public Guid Id { get; set; }
        public string Input { get; set; }
        public string ExpectedOutput { get; set; }
        public bool IsHidden { get; set; }
    }

    public class GetDsaTopicInfo
    {
        public Guid Id { get; set; }
        public string TopicName { get; set; }
    }

    public class GetDefinition
    {
        public Guid Id { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public string ReturnType { get; set; }
        public List<GetDefinitionParameter> Parameters { get; set; }
    }

    public class GetDefinitionParameter
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
