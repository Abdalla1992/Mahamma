using Mahamma.Base.Dto.Enum;
using Mahamma.Notification.Domain.UserPushNotificationTokens.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Infrastructure.EntityConfiguration
{
    public class FirebaseNotificationTokensEntityTypeConfiguration : IEntityTypeConfiguration<FirebaseNotificationTokens>
    {
        public void Configure(EntityTypeBuilder<FirebaseNotificationTokens> FirebaseToken)
        {
            FirebaseToken.ToTable("FirebaseNotificationTokens");
            FirebaseToken.HasKey(a => a.Id);
            FirebaseToken.Property(a => a.FirebaseToken).HasColumnName("FirebaseToken").IsRequired();
            FirebaseToken.Property(a => a.UserId).HasColumnName("UserId").IsRequired();

            FirebaseToken.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            FirebaseToken.Property(a => a.DeletedStatus).IsRequired();

            FirebaseToken.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
