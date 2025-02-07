using InkSpire.Core.Data.Entity;
using System;

namespace Core.Features.ReactionFeatures.Commands.CreateReaction
{
    public class CreateReactionCommandResponse
    {
        public Guid ReactionId { get; set; }
        public Guid UserId { get; set; }
        public ReactionType ReactionType { get; set; }
    }
}
