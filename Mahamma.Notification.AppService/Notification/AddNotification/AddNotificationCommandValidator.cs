using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Notification.AddNotification
{
     public class AddNotificationCommandValidator : AbstractValidator<AddNotificationCommand>
    {
        public AddNotificationCommandValidator()
        {
            //RuleFor(command => command.WorkSpaceID);
            //RuleFor(command => command.ProjectID);
            //RuleFor(command => command.TaskID);
            RuleFor(command => command.NotificationSendingStatusId).GreaterThan(0).WithMessage("NotificationSendingStatus Id Is Less Than 1");
            RuleFor(command => command.NotificationSendingTypeId).GreaterThan(0).WithMessage("NotificationSendingType Id Is Less Than 1");
            RuleFor(command => command.NotificationTypeId).GreaterThan(0).WithMessage("NotificationType Id Is Less Than 1");
            RuleFor(command => command.ReceiverUserId).GreaterThan(0).WithMessage("ReceiverUser Id Is Less Than 1");
            RuleFor(command => command.SenderUserId).GreaterThan(0).WithMessage("SenderUser Id Is Less Than 1");
            //RuleFor(command => command.IsRead)


        }
    }
}
