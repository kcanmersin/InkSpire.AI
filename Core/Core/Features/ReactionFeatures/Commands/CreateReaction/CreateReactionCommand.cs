using Core.Shared; 
using MediatR;
using System;
using InkSpire.Core.Data.Entity;

namespace Core.Features.ReactionFeatures.Commands.CreateReaction
{
    public class CreateReactionCommand : IRequest<Result<CreateReactionCommandResponse>>
    {
        public Guid UserId { get; set; }
        public ReactionType ReactionType { get; set; }
        public Guid? BookId { get; set; }
        public Guid? CommentId { get; set; }
    }
}
