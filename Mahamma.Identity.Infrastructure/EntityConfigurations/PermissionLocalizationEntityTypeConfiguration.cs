using Mahamma.Identity.Domain.Role.Enum;
using Mahamma.Base.Dto.Enum;
using Mahamma.Identity.Domain.Role.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mahamma.Identity.Domain.Language.Enum;
using Mahamma.Identity.Domain.Language.Entity;

namespace Mahamma.Identity.Infrastructure.EntityConfigurations
{
    public class PermissionLocalizationEntityTypeConfiguration : IEntityTypeConfiguration<PermissionLocalization>
    {
        public void Configure(EntityTypeBuilder<PermissionLocalization> permissionLocalization)
        {

            permissionLocalization.ToTable("PermissionLocalization");
            permissionLocalization.HasKey(a => a.Id);
            permissionLocalization.Property(a => a.DisplayName).HasColumnName("DisplayName").IsRequired();
            permissionLocalization.Property(a => a.LanguageId).HasColumnName("LanguageId").IsRequired();
            permissionLocalization.Property(a => a.PermissionId).HasColumnName("PermissionId").IsRequired();
            permissionLocalization.Property(a => a.DeletedStatus).IsRequired();
            permissionLocalization.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();

            permissionLocalization.HasOne<Permission>().WithMany().IsRequired(true).HasForeignKey(c => c.PermissionId);
            permissionLocalization.HasOne<Language>().WithMany().IsRequired(true).HasForeignKey(c => c.LanguageId);


            permissionLocalization.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
