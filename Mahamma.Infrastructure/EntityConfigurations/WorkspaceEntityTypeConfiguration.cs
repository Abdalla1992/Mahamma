using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Company.Entity;
using Mahamma.Domain.Workspace.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class WorkspaceEntityTypeConfiguration : IEntityTypeConfiguration<Workspace>
    {
        public void Configure(EntityTypeBuilder<Workspace> Workspace)
        {
            Workspace.ToTable("Workspace");
            Workspace.HasKey(a => a.Id);
            Workspace.Property(a => a.Name).HasColumnName("Name").IsRequired();
            Workspace.Property(a => a.ImageUrl).HasColumnName("ImageUrl");
            Workspace.Property(a => a.Color).HasColumnName("Color");
            Workspace.Property(a => a.CompanyId).HasColumnName("CompanyId");
            Workspace.Property(a => a.CreatorUserId).HasColumnName("CreatorUserId");
            Workspace.Property(a => a.DeletedStatus).IsRequired();
            Workspace.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();

            Workspace.HasOne<Company>().WithMany(x => x.Workspaces).IsRequired(true).HasForeignKey(c => c.CompanyId);

            Workspace.HasQueryFilter(c => c.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
