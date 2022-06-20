using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.AddNotificationList
{
    class AddNotificationListCommandValidator : AbstractValidator<AddNotificationListCommand>
    {
        public AddNotificationListCommandValidator()
        {
            RuleFor(command => command.NotificationList).NotEmpty().WithMessage("Notification List Is  Empty");
        }
    }
}
