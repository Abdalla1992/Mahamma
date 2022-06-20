using Mahamma.Base.Dto.Enum;
using Mahamma.Notification.Domain.Notification.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Infrastructure.EntityConfiguration
{
    class NotificationContentEntityTypeConfiguration : IEntityTypeConfiguration<NotificationContent>
    {
        public void Configure(EntityTypeBuilder<NotificationContent> NotificationContent)
        {
            NotificationContent.ToTable("NotificationContent");
            NotificationContent.HasKey(a => a.Id);
            NotificationContent.Property(a => a.Title).HasColumnName("Title");
            NotificationContent.Property(a => a.Body).HasColumnName("Body");
            NotificationContent.Property(a => a.LanguageId).HasColumnName("LanguageId");
            NotificationContent.Property(a => a.NotificationId).HasColumnName("NotificationId");
            NotificationContent.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            NotificationContent.Property(a => a.DeletedStatus).IsRequired();

            NotificationContent.HasOne(n => n.Notification)
            .WithMany(n => n.NotificationContents).IsRequired(true).HasForeignKey(c => c.NotificationId);


            NotificationContent.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);

        }
    }
}
