using Core.Data.Entity.Game;
using Core.Shared;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using InkSpire.Application.Abstractions;

namespace Core.Features.GameFeatures.CheckGameAnswer
{
    public class CheckGameAnswerQueryResponse
    {
        public string Word { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public string Feedback { get; set; } = string.Empty;
    }

    public class CheckGameAnswerQuery : IRequest<Result<CheckGameAnswerQueryResponse>>
    {
        public string Word { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }

    public class CheckGameAnswerHandler : IRequestHandler<CheckGameAnswerQuery, Result<CheckGameAnswerQueryResponse>>
    {
        private readonly IGroqLLM _groqService;

        public CheckGameAnswerHandler(IGroqLLM groqService)
        {
            _groqService = groqService;
        }

        public async Task<Result<CheckGameAnswerQueryResponse>> Handle(CheckGameAnswerQuery request, CancellationToken cancellationToken)
        {
            var answerResult = await _groqService.CheckAnswerAsync(request.Word, request.Topic, request.Language);

            if (answerResult == null)
            {
                return Result.Failure<CheckGameAnswerQueryResponse>(new Error("AnswerCheckFailed", "Failed to check the answer."));
            }

            return Result.Success(new CheckGameAnswerQueryResponse
            {
                Word = answerResult.Word,
                IsCorrect = answerResult.IsCorrect,
                Feedback = answerResult.Feedback
            });
        }
    }
}
