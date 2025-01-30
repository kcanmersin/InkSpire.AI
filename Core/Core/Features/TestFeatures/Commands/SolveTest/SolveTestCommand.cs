using Core.Shared;
using MediatR;

namespace Core.Features.TestFeatures.Commands.SolveTest
{
    public class SolveTestCommand : IRequest<Result<SolveTestResponse>>
    {
        public Guid TestId { get; set; }
        public List<QuestionAnswerDto> Answers { get; set; } = new List<QuestionAnswerDto>();
    }

    public class QuestionAnswerDto
    {
        public Guid QuestionId { get; set; }
        public string Answer { get; set; }
    }
}
