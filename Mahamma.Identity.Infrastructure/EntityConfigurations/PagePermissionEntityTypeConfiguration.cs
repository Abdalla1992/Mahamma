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
    public class PagePermissionEntityTypeConfiguration : IEntityTypeConfiguration<PagePermission>
    {
        public void Configure(EntityTypeBuilder<PagePermission> PagePermission)
        {
            PagePermission.ToTable("PagePermission");
            PagePermission.HasKey(a => a.Id);
            PagePermission.Property(a => a.PageId).HasColumnName("PageId").IsRequired();
            PagePermission.Property(a => a.PermissionId).HasColumnName("PermissionId").IsRequired();
            PagePermission.Property(a => a.DeletedStatus).IsRequired();
            PagePermission.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();

            PagePermission.HasOne<Permission>().WithMany().IsRequired(true).HasForeignKey(c => c.PermissionId);

            PagePermission.HasOne(p => p.Page).WithMany(r => r.PagePermissions).IsRequired(true).HasForeignKey(c => c.PageId);


            PagePermission.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
