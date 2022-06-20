using Mahamma.Domain.Meeting.Enum;
using Mahamma.Domain.Task.Enum;
using Microsoft.EntityFrameworkCore;

namespace Mahamma.Infrastructure.Context
{
    public static class MahammaContextSeed
    {
        public static void BuildEnums(this ModelBuilder modelBuilder)
        {
            #region Task Priority
            modelBuilder.Entity<TaskPriority>().HasData(new TaskPriority(TaskPriority.Normal.Id, TaskPriority.Normal.Name));
            modelBuilder.Entity<TaskPriority>().HasData(new TaskPriority(TaskPriority.Major.Id, TaskPriority.Major.Name));
            modelBuilder.Entity<TaskPriority>().HasData(new TaskPriority(TaskPriority.Urgent.Id, TaskPriority.Urgent.Name));
            #endregion

            #region Task Status
            modelBuilder.Entity<TaskStatus>().HasData(new TaskStatus(TaskStatus.New.Id, TaskStatus.New.Name));
            modelBuilder.Entity<TaskStatus>().HasData(new TaskStatus(TaskStatus.InProgress.Id, TaskStatus.InProgress.Name));
            modelBuilder.Entity<TaskStatus>().HasData(new TaskStatus(TaskStatus.InProgressWithDelay.Id, TaskStatus.InProgressWithDelay.Name));
            modelBuilder.Entity<TaskStatus>().HasData(new TaskStatus(TaskStatus.CompletedOnTime.Id, TaskStatus.CompletedOnTime.Name));
            modelBuilder.Entity<TaskStatus>().HasData(new TaskStatus(TaskStatus.CompletedEarly.Id, TaskStatus.CompletedEarly.Name));
            modelBuilder.Entity<TaskStatus>().HasData(new TaskStatus(TaskStatus.CompletedLate.Id, TaskStatus.CompletedLate.Name));
            #endregion

            #region Meeting Roles
            modelBuilder.Entity<MeetingRole>().HasData(new MeetingRole(MeetingRole.Creator.Id, MeetingRole.Creator.Name));
            modelBuilder.Entity<MeetingRole>().HasData(new MeetingRole(MeetingRole.Speaker.Id, MeetingRole.Speaker.Name));
            modelBuilder.Entity<MeetingRole>().HasData(new MeetingRole(MeetingRole.Presenter.Id, MeetingRole.Presenter.Name));
            modelBuilder.Entity<MeetingRole>().HasData(new MeetingRole(MeetingRole.MinuteOfMeetingWriter.Id, MeetingRole.MinuteOfMeetingWriter.Name));
            #endregion
        }
    }
}
