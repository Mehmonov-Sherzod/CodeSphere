using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeSphere.Domain.Entities
{
    public class DsaQuestions
    {
        public Guid Id { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionDescription { get; set; }
        public string DifficultyLevel { get; set; }
        public string? SolutionUrl { get; set; }
        public List<DsaQuestionTestCases> TestCases { get; set; }
        public List<DsaTopicQuestions> TopicQuestions { get; set; }
        public DsaQuestionDefinition? Definition { get; set; }

    }
}
