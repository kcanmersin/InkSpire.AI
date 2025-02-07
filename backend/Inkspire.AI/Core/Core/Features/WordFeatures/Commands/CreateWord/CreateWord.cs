using Core.Data;
using Core.Data.Entity.User;
using Core.Features.UserFeatures.Login;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.WordFeatures.Commands.CreateWord
{
    public class CreateWordCommand : IRequest<Result<CreateWordResponse>>
    {
        public Guid UserId { get; set; }
        public string WordText { get; set; }
        public string TranslatedText { get; set; }
        public List<string> Examples { get; set; } = new();
        public List<string> ExampleDescriptions { get; set; } = new();
    }

    public class CreateWordHandler : IRequestHandler<CreateWordCommand, Result<CreateWordResponse>>
    {
        private readonly ApplicationDbContext _context;

        public CreateWordHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CreateWordResponse>> Handle(CreateWordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Include(u => u.Words).FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user == null)
                return Result.Failure<CreateWordResponse>(new Error("NotFound", "User not found."));

            if (request.Examples.Count != request.ExampleDescriptions.Count)
                    return Result.Failure<CreateWordResponse>(new Error("InvalidInput", "Examples and example descriptions must have the same count."));

            var word = new Word
            {
                UserId = request.UserId,
                WordText = request.WordText,
                TranslatedText = request.TranslatedText,
                Examples = request.Examples,
                ExampleDescriptions = request.ExampleDescriptions
            };

            await _context.Words.AddAsync(word, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success(new CreateWordResponse
            {
                Id = word.Id,
                WordText = word.WordText,
                TranslatedText = word.TranslatedText,
                Examples = word.Examples,
                ExampleDescriptions = word.ExampleDescriptions
            });
        }
    }

    public class CreateWordResponse
    {
        public Guid Id { get; set; }
        public string WordText { get; set; }
        public string TranslatedText { get; set; }
        public List<string> Examples { get; set; } = new();
        public List<string> ExampleDescriptions { get; set; } = new();
    }
}
