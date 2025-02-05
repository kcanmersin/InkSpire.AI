using Core.Data;
using Core.Shared;
using FluentValidation;
using InkSpire.Core.Data.Entity;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.CommentFeatures.Commands.CreateComment
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, Result<CreateCommentResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<CreateCommentCommand> _validator;

        public CreateCommentHandler(ApplicationDbContext context, IValidator<CreateCommentCommand> validator)
        {
            _context = context;
            _validator = validator;
        }

        public async Task<Result<CreateCommentResponse>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            var comment = new Comment
            {
                Text = request.Text,
                UserId = request.UserId,
                BookId = request.BookId
            };
            await _context.Comments.AddAsync(comment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new CreateCommentResponse
            {
                Id = comment.Id,
                Text = comment.Text,
                UserId = comment.UserId,
                BookId = comment.BookId
            };

            return Result.Success(response);
        }
    }
}
