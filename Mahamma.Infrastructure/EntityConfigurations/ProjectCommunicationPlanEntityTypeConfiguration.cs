using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Project.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class ProjectCommunicationPlanEntityTypeConfiguration : IEntityTypeConfiguration<ProjectCommunicationPlan>
    {
        public void Configure(EntityTypeBuilder<ProjectCommunicationPlan> projectCommunicationPlan)
        {
            projectCommunicationPlan.ToTable("ProjectCommunicationPlan");
            projectCommunicationPlan.HasKey(a => a.Id);
            projectCommunicationPlan.Property(a => a.ProjectId).HasColumnName("ProjectId").IsRequired();
            projectCommunicationPlan.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            projectCommunicationPlan.Property(a => a.DeletedStatus).IsRequired();
            projectCommunicationPlan.HasOne<Project>().WithMany(x => x.ProjectCommunicationPlans).IsRequired(true);
            projectCommunicationPlan.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
