using Core.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.TestFeatures.Queries.GetTestByBookId
{
    public class GetTestByBookIdQuery : IRequest<Result<GetTestByBookIdQueryResponse>>
    {
        public Guid BookId { get; set; }

        public Guid? UserId { get; set; }
    }
}
