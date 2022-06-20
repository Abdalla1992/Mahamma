using FluentValidation;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.User.Repository;
using Microsoft.AspNetCore.Identity;

namespace Mahamma.AppService.Task.AddComment
{
    public class RegistrationCommandValidator : AbstractValidator<RegistrationCommand>
    {
        private readonly IUserRepository _userRepository;
        public UserManager<User> _userManager { get; set; }
        public RegistrationCommandValidator(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            RuleFor(command => command.Email).EmailAddress().WithMessage("Your Email Must Be Email Format Like abc@example.com")
                .NotEmpty().WithMessage("Email Address Is Empty");
            RuleFor(command => command.Password).NotEmpty().WithMessage("Password Is Empty");
            RuleFor(command => command).Must(IsUniqueEmail).WithMessage("Email Already Exists");
            RuleFor(command => command).Must(IsPasswordEqualsConfirmPassword).WithMessage("Confirm Password Must be Equal Password");

        }
        private bool IsUniqueEmail(RegistrationCommand registrationCommand)
        {
            return !(_userRepository.CheckEmailExistence(registrationCommand.Email).Result);
        }
        private bool IsPasswordEqualsConfirmPassword(RegistrationCommand registrationCommand)
        {
            return (registrationCommand.Password.Equals(registrationCommand.ConfirmPassword));
        }
    }
}
