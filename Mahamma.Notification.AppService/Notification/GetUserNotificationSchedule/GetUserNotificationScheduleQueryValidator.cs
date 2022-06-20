using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.GetUserNotificationSchedule
{
    public class GetUserNotificationScheduleQueryValidator : AbstractValidator<GetUserNotificationScheduleQuery>
    {
        public GetUserNotificationScheduleQueryValidator()
        {
            RuleFor(command => command.UserId).GreaterThan(0).WithMessage("USerId Id Is Less than 1");
        }
    }
}
