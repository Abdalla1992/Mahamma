using FluentValidation;
using Mahamma.Notification.Domain.Notification.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.AddNotificationShedule
{
    public class AddNotificationsheduleCommandValidator : AbstractValidator<AddNotificationScheduleCommand>
    {
        public AddNotificationsheduleCommandValidator()
        {
            RuleFor(command => command.From).NotNull().WithMessage("Time Must Not To Be Empty");
            RuleFor(command => command.To).NotNull().WithMessage("Time Must Not To Be Empty");
            RuleFor(command => command.NotificationScheduleTypeId).GreaterThan(0).WithMessage("NotificationScheduleTypeId Id Is Less Than 1");
            RuleFor(command => command).Must(ValidateWeekDayId).WithMessage("Invalid Week Day Id");
            RuleFor(command => command).Must(ValidateMonthDayId).WithMessage("Invalid Month Day Id");
            RuleFor(command => command.UserId).NotNull().GreaterThan(0).WithMessage("Invalid User Id");
        }

        private bool ValidateWeekDayId(AddNotificationScheduleCommand addNotificationsheduleCommand)
        {
            if (addNotificationsheduleCommand.NotificationScheduleTypeId== NotificationSheduleType.Weekly.Id)
            {
                return NotificationWeekDay.List().Any(e => e.Id == addNotificationsheduleCommand.WeekDayId);
            }
            else
            {
                return true;
            }
        }
        private bool ValidateMonthDayId(AddNotificationScheduleCommand addNotificationsheduleCommand)
        {
            if (addNotificationsheduleCommand.NotificationScheduleTypeId == NotificationSheduleType.Monthly.Id)
            {
                return addNotificationsheduleCommand.MonthDayId >= 1 && addNotificationsheduleCommand.MonthDayId <= 31;
            }
            else
            {
                return true;
            }
        }

    }
}
