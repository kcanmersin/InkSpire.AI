using Core.Features.BookFeatures.Commands.CreateBook;
using MediatR;
using Core.Shared;

namespace API.GraphQL
{
    public class BookMutation
    {
        private readonly IMediator _mediator;

        public BookMutation(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateBookResponse> CreateBook(
            string title,
            string language,
            string level,
            Guid authorId,
            [Service] IMediator mediator)
        {
            var command = new CreateBookCommand
            {
                AuthorId = authorId,
                Title = title,
                Language = language,
                Level = level
            };

            var result = await mediator.Send(command);

            if (!result.IsSuccess)
            {
                throw new Exception($"Book creation failed: {result.Error.Message}");
            }

            return result.Value;
        }
    }
}
