using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.UpdateUserProfileSection
{
    public class UpdateUserProfileSectionCommandValidator : AbstractValidator<UpdateUserProfileSectionCommand>
    {
        public UpdateUserProfileSectionCommandValidator()
        {
            RuleFor(command => command.UserId).GreaterThan(0).WithMessage("User Id is required");
            RuleFor(command => command.UserProfileSections).NotEmpty().WithMessage("User Profile Sections required");
            RuleForEach(command => command.UserProfileSections).ChildRules(ps =>
            {
                ps.RuleFor(x => x.SectionId).GreaterThan(0).WithMessage("Section Id is required");
                ps.RuleFor(x => x.OrderId).GreaterThan(0).WithMessage("Order Id is required");

            });
        }
    }
}
