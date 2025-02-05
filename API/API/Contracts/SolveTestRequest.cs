using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace API.Contracts
{
    public class SolveTestRequest
    {
        [System.ComponentModel.DefaultValue("5aa7b2aa-cef7-47bd-b4eb-28263306347f")]
        public Guid BookId { get; set; }

        [System.ComponentModel.DefaultValue("f7f7f7f7-f7f7-f7f7-f7f7-f7f7f7f7f7f7")]
        public Guid UserId { get; set; }

        public List<QuestionAnswerDto> Answers { get; set; } = new List<QuestionAnswerDto>();
    }

    public class QuestionAnswerDto
    {
        public string QuestionText { get; set; }
        public string Answer { get; set; }
    }
}
