using System;
using System.Collections.Generic;

namespace CodeSphere.Application.Models.DsaTopic
{
    public class GetDsaTopic
    {
        public Guid Id { get; set; }
        public string TopicName { get; set; }
        public int TopicLevel { get; set; }
        public int QuestionCount { get; set; }
    }

    public class GetDsaTopicWithQuestions
    {
        public Guid Id { get; set; }
        public string TopicName { get; set; }
        public int TopicLevel { get; set; }
        public List<DsaQuestionSummary> Questions { get; set; }
    }

    public class DsaQuestionSummary
    {
        public Guid Id { get; set; }
        public string QuestionTitle { get; set; }
        public string DifficultyLevel { get; set; }
    }
}
