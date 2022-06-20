using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.SetCompaany
{
    public class SetCompanyCommandValidator : AbstractValidator<SetCompanyCommand>
    {
        public SetCompanyCommandValidator()
        {
            RuleFor(command => command.CompanyId).GreaterThan(0).WithMessage("Company Id Is Less than 1");
        }
    }
}
