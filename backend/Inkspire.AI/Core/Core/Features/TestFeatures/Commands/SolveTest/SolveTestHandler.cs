using Core.Data;
using Core.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Core.Data.Entity;
using InkSpire.Application.Abstractions;

namespace Core.Features.TestFeatures.Commands.SolveTest
{
    public class SolveTestHandler : IRequestHandler<SolveTestCommand, Result<SolveTestResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IValidator<SolveTestCommand> _validator;
        private readonly IGroqLLM _groqLLMService;

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

            var generalTest = await _context.Test
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.BookId == request.BookId && t.UserId == null, cancellationToken);

            if (generalTest == null)
            {
                return Result.Failure<SolveTestResponse>(
                    new Error("NotFound", "General test for the book not found."));
            }

            var userTest = await _context.Test
                .Include(t => t.Questions)
                .FirstOrDefaultAsync(t => t.BookId == request.BookId && t.UserId == request.UserId, cancellationToken);

            if (userTest == null)
            {
                userTest = new Test
                {
                    BookId = generalTest.BookId,
                    UserId = request.UserId,
                    TotalScore = 0,
                    GeneralFeedback = "",
                    Questions = generalTest.Questions.Select(q => new Question
                    {
                        QuestionText = q.QuestionText,
                        QuestionType = q.QuestionType,
                        Score = q.Score,
                        Answer = "",
                        Choices = q.Choices,
                        Feedback = ""
                    }).ToList()
                };

                await _context.Test.AddAsync(userTest, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }

            foreach (var qa in request.Answers)
            {
                var question = userTest.Questions.FirstOrDefault(q => q.QuestionText == qa.QuestionText);
                if (question != null)
                {
                    question.Answer = qa.Answer ?? "";
                }
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == userTest.BookId, cancellationToken);
            if (book == null)
            {
                return Result.Failure<SolveTestResponse>(
                    new Error("NotFound", "Associated Book not found."));
            }

            userTest = await _groqLLMService.CheckTestAsync(userTest, book.Content);

            await _context.SaveChangesAsync(cancellationToken);

            var response = new SolveTestResponse
            {
                TestId = userTest.Id,
                TotalScore = userTest.TotalScore,
                GeneralFeedback = userTest.GeneralFeedback,
                Questions = userTest.Questions.ToList()
            };

            return Result.Success(response);
        }
    }
}
