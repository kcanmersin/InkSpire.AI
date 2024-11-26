using Core.Data;
using MediatR;

namespace Core.Features.ReactionFeatures.Commands.DeleteReaction
{
    public class DeleteReactionHandler : IRequestHandler<DeleteReactionCommand, bool>
    {
        private readonly ApplicationDbContext _context;

        public DeleteReactionHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteReactionCommand request, CancellationToken cancellationToken)
        {
            var reaction = await _context.Reactions.FindAsync(new object[] { request.Id }, cancellationToken);
            if (reaction == null) throw new Exception("Reaction not found.");

            _context.Reactions.Remove(reaction);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
