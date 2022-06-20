using Mahamma.Base.Dto.Enum;
using Mahamma.Notification.Domain.Notification.Entity;
using Mahamma.Notification.Domain.Notification.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Infrastructure.EntityConfiguration
{
    class NotificationSheduleEntityTypeConfiguration : IEntityTypeConfiguration<NotificationShedule>
    {
        public void Configure(EntityTypeBuilder<NotificationShedule> NotificationShedule)
        {
            NotificationShedule.ToTable("NotificationShedule");
            NotificationShedule.HasKey(a => a.Id);
            NotificationShedule.Property(a => a.From).HasColumnName("From").HasColumnType("datetime").IsRequired();
            NotificationShedule.Property(a => a.To).HasColumnName("To").HasColumnType("datetime").IsRequired();
            NotificationShedule.Property(a => a.UserId).HasColumnName("UserId").IsRequired();
            NotificationShedule.Property(a => a.NotificationScheduleTypeId).HasColumnName("NotificationScheduleTypeId").IsRequired();
            NotificationShedule.Property(a => a.WeekDayId).HasColumnName("WeekDayId");
            NotificationShedule.Property(a => a.MonthDayId).HasColumnName("MonthDayId");
            NotificationShedule.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            NotificationShedule.Property(a => a.DeletedStatus).IsRequired();


            NotificationShedule.HasOne<NotificationSheduleType>().
                WithMany().IsRequired(true).HasForeignKey(c => c.NotificationScheduleTypeId);


            NotificationShedule.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);

        }
    }
}
