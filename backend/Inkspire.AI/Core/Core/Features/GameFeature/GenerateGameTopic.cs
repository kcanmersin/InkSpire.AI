using Core.Data.Entity.Game;
using Core.Shared;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using InkSpire.Application.Abstractions;

namespace Core.Features.GameFeatures.GenerateGameTopic
{
    public class GenerateGameTopicQueryResponse
    {
        public string Topic { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }

    public class GenerateGameTopicQuery : IRequest<Result<GenerateGameTopicQueryResponse>>
    {
        public string Language { get; set; } = string.Empty;
    }

    public class GenerateGameTopicHandler : IRequestHandler<GenerateGameTopicQuery, Result<GenerateGameTopicQueryResponse>>
    {
        private readonly IGroqLLM _groqService;

        public GenerateGameTopicHandler(IGroqLLM groqService)
        {
            _groqService = groqService;
        }

        public async Task<Result<GenerateGameTopicQueryResponse>> Handle(GenerateGameTopicQuery request, CancellationToken cancellationToken)
        {
            var topic = await _groqService.GenerateGameTopicAsync(request.Language);

            if (topic == null || string.IsNullOrWhiteSpace(topic.Topic))
            {
                return Result.Failure<GenerateGameTopicQueryResponse>(new Error("TopicGenerationFailed", "Failed to generate game topic."));
            }

            return Result.Success(new GenerateGameTopicQueryResponse
            {
                Topic = topic.Topic,
                Language = topic.Language
            });
        }
    }
}
