using Mahamma.Base.Dto.Enum;
using Mahamma.Identity.Domain.Language.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<Domain.User.Entity.User>
    {
        public void Configure(EntityTypeBuilder<Domain.User.Entity.User> User)
        {
            
            User.Property(a => a.ProfileImage).HasColumnName("ProfileImage");
            User.Property(a => a.FullName).HasColumnName("FullName");
            User.Property(a => a.JobTitle).HasColumnName("JobTitle");
            User.Property(a => a.WorkingDays).HasColumnName("WorkingDays");
            User.Property(a => a.WorkingHours).HasColumnName("WorkingHours");
            User.Property(a => a.CompanyId).HasColumnName("CompanyId");
            User.Property(a => a.UserProfileStatusId).HasColumnName("UserProfileStatusId");
            User.Property(a => a.DeletedStatus).IsRequired();
            User.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            User.Property(a => a.LanguageId).HasColumnName("LanguageId");
            User.Property(a => a.Bio).HasColumnName("Bio");
            User.Property(a => a.Skills).HasColumnName("Skills");

            User.HasOne<Language>().WithMany(r => r.UserList).IsRequired(true).HasForeignKey(c => c.LanguageId);

            User.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);

        }
    }
}
