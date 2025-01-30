using Core.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.CommentFeatures.Commands.CreateComment
{
    public class CreateCommentCommand : IRequest<Result<CreateCommentResponse>>
    {
        public string Text { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
    }
}
