using InkSpire.Core.Data.Entity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.BookFeatures.Queries.GetBookById
{
    public class GetBookByIdQueryResponse  
    {
        public Guid BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }

        public string Content { get; set; } = string.Empty;

        //all images
        public List<BookImage> images { get; set; } = new List<BookImage>();
    }
}
