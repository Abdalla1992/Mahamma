using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Project.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class ProjectRiskPlanEntityTypeConfiguration : IEntityTypeConfiguration<ProjectRiskPlan>
    {
        public void Configure(EntityTypeBuilder<ProjectRiskPlan> projectRiskPlan)
        {
            projectRiskPlan.ToTable("ProjectRiskPlan");
            projectRiskPlan.HasKey(a => a.Id);
            projectRiskPlan.Property(a => a.ProjectId).HasColumnName("ProjectId").IsRequired();
            projectRiskPlan.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            projectRiskPlan.Property(a => a.DeletedStatus).IsRequired();
            projectRiskPlan.HasOne<Project>().WithMany(x => x.ProjectRiskPlans).IsRequired(true);
            projectRiskPlan.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
