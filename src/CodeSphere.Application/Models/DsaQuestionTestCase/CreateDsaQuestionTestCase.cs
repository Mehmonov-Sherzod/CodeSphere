using System;

namespace CodeSphere.Application.Models.DsaQuestionTestCase
{
    public class CreateDsaQuestionTestCase
    {
        public string Input { get; set; }
        public string ExpectedOutput { get; set; }
        public bool IsHidden { get; set; }
        public Guid DsaQuestionId { get; set; }
    }
}
