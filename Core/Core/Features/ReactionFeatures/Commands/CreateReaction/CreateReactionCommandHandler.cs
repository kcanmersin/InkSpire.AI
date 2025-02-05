using Core.Data;
using Core.Shared;
using InkSpire.Core.Data.Entity;
using MediatR;
using Microsoft.EntityFrameworkCore;
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
            var existingReaction = await _context.Reactions
                .SingleOrDefaultAsync(r =>
                    r.UserId == request.UserId &&
                    r.BookId == request.BookId &&
                    ((r.CommentId == null && request.CommentId == null) ||
                     (r.CommentId != null && r.CommentId == request.CommentId)),
                    cancellationToken);

            // Eğer reaction zaten varsa ve aynı reaction tekrar veriliyorsa, reaction'ı sil
            if (existingReaction != null)
            {
                if (existingReaction.ReactionType == request.ReactionType)
                {
                    _context.Reactions.Remove(existingReaction);
                    await _context.SaveChangesAsync(cancellationToken);
                    return Result.Success<CreateReactionCommandResponse>(null);
                }

                // Eğer farklı bir reaction veriliyorsa, mevcut reaction'ı güncelle
                existingReaction.ReactionType = request.ReactionType;
                await _context.SaveChangesAsync(cancellationToken);

                var updatedResponse = new CreateReactionCommandResponse
                {
                    ReactionId = existingReaction.Id,
                    UserId = existingReaction.UserId,
                    ReactionType = existingReaction.ReactionType
                };

                return Result.Success(updatedResponse);
            }

            // Eğer reaction yoksa, yeni bir tane ekle
            var newReaction = new Reaction
            {
                UserId = request.UserId,
                ReactionType = request.ReactionType,
                BookId = request.BookId,
                CommentId = request.CommentId
            };
            await _context.Reactions.AddAsync(newReaction, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new CreateReactionCommandResponse
            {
                ReactionId = newReaction.Id,
                UserId = newReaction.UserId,
                ReactionType = newReaction.ReactionType
            };

            return Result.Success(response);
        }
    }
}
