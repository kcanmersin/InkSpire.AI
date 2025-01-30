using Core.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.TestFeatures.Commands.CreateTest
{
    public class CreateTestCommand :IRequest<Result<CreateTestResponse>>
    {
        //userid bookid
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
    }
}
