using InkSpire.Core.Data.Entity;
using System;

namespace Core.Features.BookFeatures.Queries.GetAllBook
{
    public class GetAllBookQueryResponse
    {
        public Guid BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public string Language { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;

        //return first image of book and content of book

        public string Content { get; set; } = string.Empty;

        public  BookImage images { get; set; }

    }
}
