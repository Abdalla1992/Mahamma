using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.UpdateUserProfileStatus
{
    public class UpdateUserProfileStatusCommandValidator : AbstractValidator<UpdateUserProfileStatusCommand>
    {
        public UpdateUserProfileStatusCommandValidator()
        {
            RuleFor(command => command.UserProfileStatusId).GreaterThan(0).WithMessage("User Profile Status Id Is Less than 1");
        }
    }
}
