using System.ComponentModel.DataAnnotations;

namespace API.Contracts.Reaction
{
    public class DeleteReactionRequest
    {
        [Required]
        public Guid Id { get; set; }
    }
}
