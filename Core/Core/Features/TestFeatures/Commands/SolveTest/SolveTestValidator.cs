using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Features.TestFeatures.Commands.SolveTest
{
    public class SolveTestValidator  :AbstractValidator<SolveTestCommand>
    {
        public SolveTestValidator()
        {
            RuleFor(x => x.TestId).NotEmpty();
            RuleFor(x => x.Answers).NotEmpty();
        }

    }
}
