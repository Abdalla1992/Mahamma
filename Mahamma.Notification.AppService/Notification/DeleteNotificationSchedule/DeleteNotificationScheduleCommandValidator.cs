using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.DeleteNotificationSchedule
{
    public class DeleteNotificationScheduleCommandValidator : AbstractValidator<DeleteNotificationScheduleCommand>
    {
        public DeleteNotificationScheduleCommandValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Invalid Notificatio nSchedule Id");
        }
    }
}
