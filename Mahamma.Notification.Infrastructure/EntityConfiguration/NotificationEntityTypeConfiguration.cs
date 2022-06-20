using Mahamma.Base.Dto.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Infrastructure.EntityConfiguration
{
    public class NotificationEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Notification.Entity.Notification>
    {
        public void Configure(EntityTypeBuilder<Domain.Notification.Entity.Notification> Notification)
        {
            Notification.ToTable("Notification");
            Notification.HasKey(a => a.Id);
            Notification.Property(a => a.WorkSpaceId).HasColumnName("WorkSpaceId");
            Notification.Property(a => a.ProjectId).HasColumnName("ProjectId");
            Notification.Property(a => a.TaskId).HasColumnName("TaskId");
            Notification.Property(a => a.MeetingId).HasColumnName("MeetingId");
            Notification.Property(a => a.NotificationSendingStatusId).HasColumnName("NotificationSendingStatusId").IsRequired();
            Notification.Property(a => a.NotificationSendingTypeId).HasColumnName("NotificationSendingTypeId").IsRequired();
            Notification.Property(a => a.NotificationTypeId).HasColumnName("NotificationTypeId").IsRequired();
            Notification.Property(a => a.ReceiverUserId).HasColumnName("ReceiverUserId").IsRequired();
            Notification.Property(a => a.SenderUserId).HasColumnName("SenderUserId").IsRequired();
            Notification.Property(a => a.IsRead).HasDefaultValue(false).HasColumnName("IsRead").IsRequired();

            Notification.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            Notification.Property(a => a.DeletedStatus).IsRequired();


            Notification.HasOne<Domain.Notification.Enum.NotificationSendingStatus>()
            .WithMany().IsRequired(true).HasForeignKey(c => c.NotificationSendingStatusId);


            Notification.HasOne<Domain.Notification.Enum.NotificationSendingType>()
            .WithMany().IsRequired(true).HasForeignKey(c => c.NotificationSendingTypeId);


            Notification.HasOne<Domain.Notification.Enum.NotificationType>()
            .WithMany().IsRequired(true).HasForeignKey(c => c.NotificationTypeId);

            Notification.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);

        }
    }
}
