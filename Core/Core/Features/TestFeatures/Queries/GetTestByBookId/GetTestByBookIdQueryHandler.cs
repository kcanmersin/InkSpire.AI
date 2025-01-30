using Core.Data;
using Core.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Features.TestFeatures.Queries.GetTestByBookId
{
    public class GetTestByBookIdQueryHandler
        : IRequestHandler<GetTestByBookIdQuery, Result<GetTestByBookIdQueryResponse>>
    {
        private readonly ApplicationDbContext _context;

        public GetTestByBookIdQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<GetTestByBookIdQueryResponse>> Handle(
            GetTestByBookIdQuery request,
            CancellationToken cancellationToken)
        {
            var test = await _context.Test
                .Include(t => t.Questions)
                .Where(t => t.BookId == request.BookId && t.UserId == request.UserId)
                .Select(t => new GetTestByBookIdQueryResponse
                {
                    TestId = t.Id,
                    BookId = t.BookId,
                    UserId = t.UserId,
                    TotalScore = t.TotalScore,
                    Questions = t.Questions.Select(q => new QuestionDto
                    {
                        QuestionId = q.Id,
                        QuestionText = q.QuestionText,
                        Score = q.Score,
                        Answer = q.Answer,
                        Choices = q.Choices,
                        QuestionType = q.QuestionType.ToString()
                    }).ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);
            //get last one
            //.LastOrDefaultAsync(cancellationToken);

            if (test == null)
            {
                return Result.Failure<GetTestByBookIdQueryResponse>(
                    new Error("NotFound", "Test not found for the given BookId/UserId.")
                );
            }

            return Result.Success(test);
        }
    }
}
