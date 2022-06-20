using FluentValidation;

namespace Mahamma.Notification.AppService.FirebaseTokens.AddFirebaseToken
{
    public class AddFirebaseTokenCommandValidator : AbstractValidator<AddFirebaseTokenCommand>
    {
        public AddFirebaseTokenCommandValidator()
        {
            RuleFor(command => command.FirebaseToken).NotEmpty().WithMessage("Firebase Token must be provided");
        }
    }
}
