using Core.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.BookFeatures.Queries.GetBookById
{
    public class GetBookByIdQuery : IRequest<Result<GetBookByIdQueryResponse>>
    {
        public Guid BookId { get; set; }
    }
}
