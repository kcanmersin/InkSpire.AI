using Core.Data;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Data.Entity;
using InkSpire.Application.Abstractions;

namespace Core.Features.TestFeatures.Queries.GetTestByBookId
{
    public class GetTestByBookIdQueryHandler
        : IRequestHandler<GetTestByBookIdQuery, Result<GetTestByBookIdQueryResponse>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IGroqLLM _groqLLMService;

        public GetTestByBookIdQueryHandler(ApplicationDbContext context, IGroqLLM groqLLMService)
        {
            _context = context;
            _groqLLMService = groqLLMService;
        }

        public async Task<Result<GetTestByBookIdQueryResponse>> Handle(
            GetTestByBookIdQuery request,
            CancellationToken cancellationToken)
        {
            var testQuery = _context.Test
                .Include(t => t.Questions)
                .Where(t => t.BookId == request.BookId);

            var userTest = await testQuery
                .Where(t => t.UserId == request.UserId)
                .Select(t => new GetTestByBookIdQueryResponse
                {
                    TestId = t.Id,
                    BookId = t.BookId,
                    UserId = t.UserId,
                    TotalScore = t.TotalScore,
                    GeneralFeedback = t.GeneralFeedback,
                    Questions = t.Questions.Select(q => new QuestionDto
                    {
                        QuestionId = q.Id,
                        QuestionText = q.QuestionText,
                        Score = q.Score,
                        Answer = q.Answer,
                        Choices = q.Choices,
                        QuestionType = q.QuestionType.ToString(),
                        Feedback = q.Feedback
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (userTest != null)
            {
                return Result.Success(userTest);
            }

            var generalTest = await testQuery
                .Where(t => t.UserId == null)
                .Select(t => new GetTestByBookIdQueryResponse
                {
                    TestId = t.Id,
                    BookId = t.BookId,
                    UserId = null,
                    TotalScore = t.TotalScore,
                    GeneralFeedback = t.GeneralFeedback,
                    Questions = t.Questions.Select(q => new QuestionDto
                    {
                        QuestionId = q.Id,
                        QuestionText = q.QuestionText,
                        Score = q.Score,
                        Answer = q.Answer,
                        Choices = q.Choices,
                        QuestionType = q.QuestionType.ToString(),
                        Feedback = q.Feedback
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (generalTest != null)
            {
                return Result.Success(generalTest);
            }

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == request.BookId, cancellationToken);
            if (book == null)
            {
                return Result.Failure<GetTestByBookIdQueryResponse>(
                    new Error("NotFound", "Book not found.")
                );
            }

            var generatedTest = await _groqLLMService.GenerateTestAsync(
                book: book,
                user: null,
                level: book.Level,
                content: book.Content,
                language: book.Language
            );

            if (generatedTest?.Questions == null || !generatedTest.Questions.Any())
            {
                return Result.Failure<GetTestByBookIdQueryResponse>(
                    new Error("TestGenerationFailed", "Failed to generate test for the book.")
                );
            }

            generatedTest.UserId = null;
            generatedTest.BookId = book.Id;

            await _context.Test.AddAsync(generatedTest, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var newTestResponse = new GetTestByBookIdQueryResponse
            {
                TestId = generatedTest.Id,
                BookId = generatedTest.BookId,
                UserId = null,
                TotalScore = generatedTest.TotalScore,
                GeneralFeedback = generatedTest.GeneralFeedback,
                Questions = generatedTest.Questions.Select(q => new QuestionDto
                {
                    QuestionId = q.Id,
                    QuestionText = q.QuestionText,
                    Score = q.Score,
                    Answer = q.Answer,
                    Choices = q.Choices,
                    QuestionType = q.QuestionType.ToString(),
                    Feedback = q.Feedback
                }).ToList()
            };

            return Result.Success(newTestResponse);
        }
    }
}
