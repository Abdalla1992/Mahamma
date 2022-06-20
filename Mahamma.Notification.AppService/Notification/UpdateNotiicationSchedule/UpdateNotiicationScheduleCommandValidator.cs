using FluentValidation;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Notification.Domain.Notification.Enum;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.UpdateNotiicationSchedule
{
    public class UpdateNotiicationScheduleCommandValidator : AbstractValidator<UpdateNotiicationScheduleCommand>
    {
        public UpdateNotiicationScheduleCommandValidator()
        {
            RuleFor(command => command.Id).GreaterThan(0).WithMessage("Invalid Notiication Schedule Id");
            RuleFor(command => command.From).NotNull().WithMessage("Time Must Not To Be Empty");
            RuleFor(command => command.To).NotNull().WithMessage("Time Must Not To Be Empty");
            RuleFor(command => command.NotificationScheduleTypeId).GreaterThan(0).WithMessage("NotificationScheduleTypeId Id Is Less Than 1");
            RuleFor(command => command).Must(ValidateWeekDayId).WithMessage("Invalid Week Day Id");
            RuleFor(command => command).Must(ValidateMonthDayId).WithMessage("Invalid Month Day Id");
            RuleFor(command => command.UserId).NotNull().GreaterThan(0).WithMessage("Invalid User Id");
        }

        private bool ValidateWeekDayId(UpdateNotiicationScheduleCommand updateNotificationsheduleCommand)
        {
            if (updateNotificationsheduleCommand.NotificationScheduleTypeId == NotificationSheduleType.Weekly.Id)
            {
                return NotificationWeekDay.List().Any(e => e.Id == updateNotificationsheduleCommand.WeekDayId);
            }
            else
            {
                return true;
            }
        }
        private bool ValidateMonthDayId(UpdateNotiicationScheduleCommand updateNotificationsheduleCommand)
        {
            if (updateNotificationsheduleCommand.NotificationScheduleTypeId == NotificationSheduleType.Monthly.Id)
            {
                return updateNotificationsheduleCommand.MonthDayId >= 1 && updateNotificationsheduleCommand.MonthDayId <= 31;
            }
            else
            {
                return true;
            }
        }
    }
}
