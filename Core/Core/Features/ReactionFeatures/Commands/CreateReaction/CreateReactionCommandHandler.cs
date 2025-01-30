using Core.Data;
using Core.Shared;
using InkSpire.Core.Data.Entity;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.ReactionFeatures.Commands.CreateReaction
{
    public class CreateReactionCommandHandler : IRequestHandler<CreateReactionCommand, Result<CreateReactionCommandResponse>>
    {
        private readonly ApplicationDbContext _context;

        public CreateReactionCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CreateReactionCommandResponse>> Handle(CreateReactionCommand request, CancellationToken cancellationToken)
        {
            var reaction = new Reaction
            {
                UserId = request.UserId,
                ReactionType = request.ReactionType,
                BookId = request.BookId,
                CommentId = request.CommentId
            };

            await _context.Reactions.AddAsync(reaction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new CreateReactionCommandResponse
            {
                ReactionId = reaction.Id,
                UserId = reaction.UserId,
                ReactionType = reaction.ReactionType
            };

            return Result.Success(response);
        }
    }
}
