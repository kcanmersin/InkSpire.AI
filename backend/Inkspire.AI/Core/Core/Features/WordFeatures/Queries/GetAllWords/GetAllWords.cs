using Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Shared;

namespace Core.Features.WordFeatures.Queries.GetAllWords
{
    public class GetAllWordsQuery : IRequest<Result<IEnumerable<GetAllWordsQueryResponse>>> // Düzeltme burada
    {
        public Guid UserId { get; set; }
    }


    public class GetAllWordsQueryHandler : IRequestHandler<GetAllWordsQuery, Result<IEnumerable<GetAllWordsQueryResponse>>>
    {
        private readonly ApplicationDbContext _context;

        public GetAllWordsQueryHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<IEnumerable<GetAllWordsQueryResponse>>> Handle(GetAllWordsQuery request, CancellationToken cancellationToken)
        {
            var words = await _context.Words
                .AsNoTracking()
                .Where(w => w.UserId == request.UserId)
                .Select(w => new GetAllWordsQueryResponse
                {
                    Id = w.Id,
                    WordText = w.WordText,
                    TranslatedText = w.TranslatedText,
                    Examples = w.Examples,
                    ExampleDescriptions = w.ExampleDescriptions
                })
                .ToListAsync(cancellationToken);

            if (!words.Any())
                return Result.Failure<IEnumerable<GetAllWordsQueryResponse>>(new Error("NotFound", "No words found."));

            return words;
        }
    }


    public class GetAllWordsQueryResponse
    {
        public Guid Id { get; set; }
        public string WordText { get; set; }
        public string TranslatedText { get; set; }
        public List<string> Examples { get; set; } = new();
        public List<string> ExampleDescriptions { get; set; } = new();
    }
}
