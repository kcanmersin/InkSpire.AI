using Core.Shared;
using MediatR;
using System;

namespace Core.Features.BookFeatures.Commands.CreateBook
{
    public class CreateBookCommand : IRequest<Result<CreateBookResponse>>
    {
        public Guid AuthorId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = "English";
        public string Level { get; set; } = "Intermediate";
    }
}
