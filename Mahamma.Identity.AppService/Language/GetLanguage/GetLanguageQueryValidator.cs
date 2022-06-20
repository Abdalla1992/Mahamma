using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Language.GetLanguage
{
    public class GetLanguageQueryValidator : AbstractValidator<GetLanguageQuery>
    {
        public GetLanguageQueryValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Language Id is required");
        }
    }
}
