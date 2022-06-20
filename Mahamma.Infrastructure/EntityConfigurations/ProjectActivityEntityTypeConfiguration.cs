using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.ProjectActivity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class ProjectActivityEntityTypeConfiguration : IEntityTypeConfiguration<ProjectActivity>
    {
        public void Configure(EntityTypeBuilder<ProjectActivity> projectActivity)
        {
            projectActivity.ToTable("ProjectActivity");
            projectActivity.HasKey(a => a.Id);
            projectActivity.Property(a => a.ProjectId).HasColumnName("ProjectId").IsRequired();
            projectActivity.Property(a => a.Action).HasColumnName("Action").IsRequired();
            projectActivity.Property(a => a.ProjectMemberId).HasColumnName("ProjectMemberId").IsRequired();
            projectActivity.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            projectActivity.Property(a => a.DeletedStatus).IsRequired();


            projectActivity.HasOne<Project>()
           .WithMany(x => x.ProjectActivities).IsRequired(true).HasForeignKey(c => c.ProjectId);



           // projectActivity.HasOne<ProjectMember>()
           //.WithMany(x => x.ProjectActivities)
           // .IsRequired(true).HasForeignKey(c => c.ProjectMemberId);


            projectActivity.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
