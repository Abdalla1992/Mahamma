using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.UpdateUserProfileSetting
{
    public class UpdateUserProfileSettingCommandValidator : AbstractValidator<UpdateUserProfileSettingCommand>
    {
        public UpdateUserProfileSettingCommandValidator()
        {
            RuleFor(command => command.FullName).NotEmpty().WithMessage("Full Name Is Required");
            RuleFor(command => command.JobTitle).NotEmpty().WithMessage("JobTitle Is Required");
            RuleFor(command => command.LanguageId).GreaterThan(0).NotEmpty().WithMessage("LanguageId Must Be Greater Than 0");
            RuleFor(command => command.Email).NotEmpty().WithMessage("Email Is Required");
           // RuleFor(command => command.Bio).NotEmpty().WithMessage("Bio Is Required");
           // RuleFor(command => command.ProfileImage).NotEmpty().WithMessage("Profile Image Is Required");
           //RuleFor(command => command.Skills).NotEmpty().WithMessage("Skills Is Required");
        }
    }
}
