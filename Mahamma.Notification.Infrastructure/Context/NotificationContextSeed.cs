using Mahamma.Notification.Domain.Language.Enum;
using Mahamma.Notification.Domain.Notification.Enum;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Infrastructure.Context
{
   public static class NotificationContextSeed
    {

        public static void BuildEnums(this ModelBuilder modelBuilder)
        {
            #region NotificationSendingStatus
            modelBuilder.Entity<NotificationSendingStatus>().HasData(new NotificationSendingStatus(NotificationSendingStatus.New.Id, NotificationSendingStatus.New.Name));
            modelBuilder.Entity<NotificationSendingStatus>().HasData(new NotificationSendingStatus(NotificationSendingStatus.Sent.Id, NotificationSendingStatus.Sent.Name));
            modelBuilder.Entity<NotificationSendingStatus>().HasData(new NotificationSendingStatus(NotificationSendingStatus.Faild.Id, NotificationSendingStatus.Faild.Name));
            #endregion

            #region NotificationSendingType

            modelBuilder.Entity<NotificationSendingType>().HasData(new NotificationSendingType(NotificationSendingType.Email.Id, NotificationSendingType.Email.Name));
            modelBuilder.Entity<NotificationSendingType>().HasData(new NotificationSendingType(NotificationSendingType.PushNotification.Id, NotificationSendingType.PushNotification.Name));
            modelBuilder.Entity<NotificationSendingType>().HasData(new NotificationSendingType(NotificationSendingType.DeviceNotification.Id, NotificationSendingType.DeviceNotification.Name));
            modelBuilder.Entity<NotificationSendingType>().HasData(new NotificationSendingType(NotificationSendingType.All.Id, NotificationSendingType.All.Name));
            #endregion

            #region NotificationType
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.AddProject.Id, NotificationType.AddProject.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.ArchiveProject.Id, NotificationType.ArchiveProject.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.AssignMemberToProject.Id, NotificationType.AssignMemberToProject.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.DeleteProject.Id, NotificationType.DeleteProject.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.UpdateProject.Id, NotificationType.UpdateProject.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.AddComment.Id, NotificationType.AddComment.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.AddTask.Id, NotificationType.AddTask.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.ArchiveTask.Id, NotificationType.ArchiveTask.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.AssignMemberToTask.Id, NotificationType.AssignMemberToTask.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.DeleteTask.Id, NotificationType.DeleteTask.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.LikeComment.Id, NotificationType.LikeComment.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.SubmitTask.Id, NotificationType.SubmitTask.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.UpdateTask.Id, NotificationType.UpdateTask.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.AssignMemberToWorkspace.Id, NotificationType.AssignMemberToWorkspace.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.AddWorkspace.Id, NotificationType.AddWorkspace.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.UpdateWorkspace.Id, NotificationType.UpdateWorkspace.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.AddSubTask.Id, NotificationType.AddSubTask.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.AcceptedTask.Id, NotificationType.AcceptedTask.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.RejectTask.Id, NotificationType.RejectTask.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.MentionComment.Id, NotificationType.MentionComment.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.MeetingAdded.Id, NotificationType.MeetingAdded.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.MeetingUpdated.Id, NotificationType.MeetingUpdated.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.MeetingCanceled.Id, NotificationType.MeetingCanceled.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.UpdateSubTask.Id, NotificationType.UpdateSubTask.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.DeleteSubTask.Id, NotificationType.DeleteSubTask.Name));
            modelBuilder.Entity<NotificationType>().HasData(new NotificationType(NotificationType.MinuteOfMeetingAdded.Id, NotificationType.MinuteOfMeetingAdded.Name));
            #endregion

            #region Language
            modelBuilder.Entity<Language>().HasData(new Language(Language.English.Id, Language.English.Name));
            modelBuilder.Entity<Language>().HasData(new Language(Language.Arabic.Id, Language.Arabic.Name));
            #endregion

            #region NotificationSheduleType
            modelBuilder.Entity<NotificationSheduleType>().HasData(new NotificationSheduleType(NotificationSheduleType.Daily.Id, NotificationSheduleType.Daily.Name));
            modelBuilder.Entity<NotificationSheduleType>().HasData(new NotificationSheduleType(NotificationSheduleType.Weekly.Id, NotificationSheduleType.Weekly.Name));
            modelBuilder.Entity<NotificationSheduleType>().HasData(new NotificationSheduleType(NotificationSheduleType.Monthly.Id, NotificationSheduleType.Monthly.Name));
            #endregion

        }
    }
}
