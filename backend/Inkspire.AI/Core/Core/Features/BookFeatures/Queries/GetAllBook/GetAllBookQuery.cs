using MediatR;
using System.Collections.Generic;

namespace Core.Features.BookFeatures.Queries.GetAllBook
{
    public class GetAllBookQuery : IRequest<IEnumerable<GetAllBookQueryResponse>>
    {
    }
}
