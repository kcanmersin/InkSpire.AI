using Core.Features.TestFeatures.Commands.SolveTest;
using MediatR;
using System.ComponentModel;

namespace API.Contracts
{
    public class SolveTestRequest
    {
        [System.ComponentModel.DefaultValue("e230cf64-a2e9-46c5-9a63-acdf3e5ade38")]
            public Guid TestId { get; set; }
            public List<QuestionAnswerDto> Answers { get; set; } = new List<QuestionAnswerDto>();


    }
}
