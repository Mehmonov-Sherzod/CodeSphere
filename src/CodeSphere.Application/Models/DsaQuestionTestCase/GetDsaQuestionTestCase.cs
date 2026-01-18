using System;

namespace CodeSphere.Application.Models.DsaQuestionTestCase
{
    public class GetDsaQuestionTestCase
    {
        public Guid Id { get; set; }
        public string Input { get; set; }
        public string ExpectedOutput { get; set; }
        public bool IsHidden { get; set; }
        public Guid DsaQuestionId { get; set; }
        public string QuestionTitle { get; set; }
    }
}
