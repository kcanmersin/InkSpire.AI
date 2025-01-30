using Core.Data;
using Core.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Data.Entity;
using Core.Features.TestFeatures.Commands.SolveTest;
using InkSpire.Application.Abstractions;  // Suppose you have IGroqLLM here

namespace Core.Features.TestFeatures.Commands.SolveTest
{
    public class SolveTestHandler : IRequestHandler<SolveTestCommand, Result<SolveTestResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<SolveTestCommand> _validator;
        private readonly IGroqLLM _groqLLMService; // We assume you have a service to check test

        public SolveTestHandler(
            ApplicationDbContext context,
            IValidator<SolveTestCommand> validator,
            IGroqLLM groqLLMService)
        {
            _context = context;
            _validator = validator;
            _groqLLMService = groqLLMService;
        }

        public async Task<Result<SolveTestResponse>> Handle(SolveTestCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                return Result.Failure<SolveTestResponse>(
                    new Error("ValidationFailed", validationResult.Errors.First().ErrorMessage));
            }

            var test = await _context.Test
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.Id == request.TestId, cancellationToken);
            if (test == null)
            {
                return Result.Failure<SolveTestResponse>(
                    new Error("NotFound", "Test not found."));
            }

            foreach (var qa in request.Answers)
            {
                var question = test.Questions.FirstOrDefault(q => q.Id == qa.QuestionId);
                if (question != null)
                {
                    question.Answer = qa.Answer ?? "";
                }
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == test.BookId, cancellationToken);
            if (book == null)
            {
                return Result.Failure<SolveTestResponse>(
                    new Error("NotFound", "Associated Book not found."));
            }

            test = await _groqLLMService.CheckTestAsync(test, book.Content);

            await _context.SaveChangesAsync(cancellationToken);

            var response = new SolveTestResponse
            {
                TestId = test.Id,
                TotalScore = test.TotalScore,
                GeneralFeedback = test.GeneralFeedback
            };

            return Result.Success(response);
        }
    }
}
