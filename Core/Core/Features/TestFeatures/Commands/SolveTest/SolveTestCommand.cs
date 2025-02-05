using Core.Shared;
using MediatR;
using System;
using System.Collections.Generic;

namespace Core.Features.TestFeatures.Commands.SolveTest
{
    public class SolveTestCommand : IRequest<Result<SolveTestResponse>>
    {
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public List<QuestionAnswerDto> Answers { get; set; } = new List<QuestionAnswerDto>();
    }

    public class QuestionAnswerDto
    {
        public string QuestionText { get; set; }
        public string Answer { get; set; }
    }
}
