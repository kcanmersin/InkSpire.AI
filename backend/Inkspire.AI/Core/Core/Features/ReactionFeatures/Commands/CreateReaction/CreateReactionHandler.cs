using Core.Data;
using Core.Data.Entity;
using FluentValidation;
using MediatR;

namespace Core.Features.ReactionFeatures.Commands.CreateReaction
{
    public class CreateReactionHandler : IRequestHandler<CreateReactionCommand, CreateReactionResponse>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<CreateReactionCommand> _validator;

        public CreateReactionHandler(ApplicationDbContext context, IValidator<CreateReactionCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<CreateReactionResponse> Handle(CreateReactionCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            var reaction = new Reaction
            {
                Type = request.Type,
                StoryId = request.StoryId,
                CommentId = request.CommentId,
                CreatedById = request.CreatedById,
                CreatedDate = DateTime.UtcNow
            };

            _context.Reactions.Add(reaction);
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateReactionResponse
            {
                Id = reaction.Id,
                Type = reaction.Type,
                StoryId = reaction.StoryId,
                CommentId = reaction.CommentId,
                CreatedById = reaction.CreatedById,
                CreatedDate = reaction.CreatedDate
            };
        }
    }
}
