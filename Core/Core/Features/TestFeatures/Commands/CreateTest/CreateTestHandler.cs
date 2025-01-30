using Core.Data;
using Core.Shared;
using FluentValidation;
using InkSpire.Core.Data.Entity;
using InkSpire.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TestFeatures.Commands.CreateTest
{
    public class CreateTestHandler : IRequestHandler<CreateTestCommand, Result<CreateTestResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<CreateTestCommand> _validator;
        private readonly IGroqLLM _groqLLMService;

        public CreateTestHandler(
            ApplicationDbContext context,
            IValidator<CreateTestCommand> validator,
            IGroqLLM groqLLMService)
        {
            _context = context;
            _validator = validator;
            _groqLLMService = groqLLMService;
        }

        public async Task<Result<CreateTestResponse>> Handle(CreateTestCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<CreateTestResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.BookId, cancellationToken);
            if (book == null)
            {
                return Result.Failure<CreateTestResponse>(
                    new Error("NotFound", "The specified book was not found."));
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user == null)
            {
                return Result.Failure<CreateTestResponse>(
                    new Error("NotFound", "The specified user was not found."));
            }

            var generatedTest = await _groqLLMService.GenerateTestAsync(
                book: book,
                user: user,
                level: book.Level,
                content: book.Content,
                language: book.Language
            );

            if (generatedTest.Questions == null || !generatedTest.Questions.Any())
            {
                return Result.Failure<CreateTestResponse>(
                    new Error("TestGenerationFailed", "Failed to generate test questions."));
            }

            await _context.Test.AddAsync(generatedTest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new CreateTestResponse
            {
                TestId = generatedTest.Id,
            };

            return Result.Success(response);
        }
    }
}
