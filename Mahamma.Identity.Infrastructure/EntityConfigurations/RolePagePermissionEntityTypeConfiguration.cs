using Mahamma.Base.Dto.Enum;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.Domain.Role.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.EntityConfigurations
{
    public class RolePagePermissionEntityTypeConfiguration : IEntityTypeConfiguration<RolePagePermission>
    {
        public void Configure(EntityTypeBuilder<RolePagePermission> RolePagePermission)
        {
            RolePagePermission.ToTable("RolePagePermission");
            RolePagePermission.HasKey(a => a.Id);
            RolePagePermission.Property(a => a.RoleId).HasColumnName("RoleId").IsRequired();
            RolePagePermission.Property(a => a.PagePermissionId).HasColumnName("PagePermissionId").IsRequired();
            RolePagePermission.Property(a => a.DeletedStatus).IsRequired();
            RolePagePermission.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();

            RolePagePermission.HasOne(rp => rp.PagePermission).WithMany(r=>r.RolePagePermissions).IsRequired(true).HasForeignKey(c => c.PagePermissionId);

            RolePagePermission.HasOne<Role>().WithMany(r => r.RolePagePermissions).IsRequired(true).HasForeignKey(c => c.RoleId);


            RolePagePermission.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
