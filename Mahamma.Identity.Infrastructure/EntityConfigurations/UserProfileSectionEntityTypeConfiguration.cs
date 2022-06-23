using Mahamma.Base.Dto.Enum;
using Mahamma.Identity.Domain.Language.Entity;
using Mahamma.Identity.Domain.Language.Enum;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.Domain.User.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.EntityConfigurations
{
    public class UserProfileSectionEntityTypeConfiguration : IEntityTypeConfiguration<UserProfileSection>
    {
        public void Configure(EntityTypeBuilder<UserProfileSection> entity)
        {
            entity.ToTable("UserProfileSection");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.OrderId).IsRequired();
            entity.Property(a => a.SectionId).IsRequired();
            entity.Property(a => a.DeletedStatus).IsRequired();
            entity.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            entity.Property(a => a.UserId).HasColumnName("UserId").IsRequired();

           // entity.HasOne<User>().WithMany().IsRequired(true).HasForeignKey(c => c.UserId);


            entity.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
