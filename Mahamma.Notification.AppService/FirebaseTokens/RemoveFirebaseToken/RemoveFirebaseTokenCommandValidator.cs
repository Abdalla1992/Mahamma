using FluentValidation;

namespace Mahamma.Notification.AppService.FirebaseTokens.AddFirebaseToken
{
    public class RemoveFirebaseTokenCommandValidator : AbstractValidator<RemoveFirebaseTokenCommand>
    {
        public RemoveFirebaseTokenCommandValidator()
        {
            RuleFor(command => command.FirebaseToken).NotEmpty().WithMessage("Firebase Token must be provided");
        }
    }
}
