using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.EntityConfigurations
{
    public class UserRoleEntityTypeConfiguration : IEntityTypeConfiguration<Domain.UserRole.Entity.UserRole>
    {
        public void Configure(EntityTypeBuilder<Domain.UserRole.Entity.UserRole> UserRole)
        {
            UserRole.HasKey(p => new { p.UserId, p.RoleId });
            UserRole.Property(a => a.DeletedStatus).IsRequired();
            UserRole.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
        }
    }
}
