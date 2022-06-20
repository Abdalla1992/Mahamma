using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Project.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    class ProjectCharterEntityTypeConfiguration : IEntityTypeConfiguration<ProjectCharter>
    {
        public void Configure(EntityTypeBuilder<ProjectCharter> projectCharter)
        {
            projectCharter.ToTable("ProjectCharter");
            projectCharter.HasKey(a => a.Id);
            projectCharter.Property(a => a.ProjectId).HasColumnName("ProjectId").IsRequired();
            projectCharter.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            projectCharter.Property(a => a.DeletedStatus).IsRequired();
            projectCharter.HasOne<Project>().WithOne(x => x.ProjectCharter).IsRequired(true);
            projectCharter.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
