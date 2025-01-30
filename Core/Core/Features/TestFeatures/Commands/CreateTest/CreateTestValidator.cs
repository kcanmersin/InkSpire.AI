using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.TestFeatures.Commands.CreateTest
{
    public class CreateTestValidator :AbstractValidator<CreateTestCommand>
    {
        public CreateTestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.BookId).NotEmpty();
        }
    }
}
