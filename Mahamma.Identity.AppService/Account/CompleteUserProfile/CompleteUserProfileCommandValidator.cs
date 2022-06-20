using FluentValidation;

namespace Mahamma.Identity.AppService.Account.CompleteUserProfile
{
    public class CompleteUserProfileCommandValidator : AbstractValidator<CompleteUserProfileCommand>
    {
        public CompleteUserProfileCommandValidator()
        {
            RuleFor(command => command.FullName).NotEmpty().WithMessage("Full Name Is Empty");
            RuleFor(command => command.JobTitle).NotEmpty().WithMessage("Job Title Is Empty");
        }
    }
}
