using System;
using System.Collections.Generic;

namespace CodeSphere.Application.Models.DsaQuestion
{
    public class UpdateDsaQuestion
    {
        public string QuestionTitle { get; set; }
        public string QuestionDescription { get; set; }
        public string DifficultyLevel { get; set; }
        public string? SolutionUrl { get; set; }
        public List<Guid>? TopicIds { get; set; }
        public List<UpdateTestCase>? TestCases { get; set; }
    }

    public class UpdateTestCase
    {
        public Guid? Id { get; set; }
        public string Input { get; set; }
        public string ExpectedOutput { get; set; }
        public bool IsHidden { get; set; }
    }
}
