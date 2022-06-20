using Mahamma.Base.Dto.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;

namespace Mahamma.Identity.Infrastructure.EntityConfigurations
{
    public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Role.Entity.Role>
    {
        public void Configure(EntityTypeBuilder<Domain.Role.Entity.Role> Role)
        {
            var entityMetadata = Role.Metadata;
            var indexes = Role.Metadata.GetIndexes().ToList();
            foreach (var index in indexes)
            {
                entityMetadata.RemoveIndex(index);
            }
            Role.HasIndex(r => new { r.NormalizedName, r.CompanyId }).HasDatabaseName("CompanyRoleNameIndex").IsUnique();
            Role.Property(r => r.CompanyId).HasColumnName("CompanyId");
            Role.Property(r => r.DeletedStatus).IsRequired();
            Role.Property(r => r.CreationDate).HasColumnType("datetime").IsRequired();

        

            Role.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
