using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Project.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class ProjectEntityTypeConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> project)
        {
            project.ToTable("Project");
            project.HasKey(a => a.Id);
            project.Property(a => a.Name).HasColumnName("Name").IsRequired();
            project.Property(a => a.Description).HasColumnName("Description").IsRequired();
            project.Property(a => a.DueDate).HasColumnName("DueDate").HasColumnType("datetime").IsRequired();
            project.Property(a => a.WorkSpaceId).HasColumnName("WorkSpaceId").IsRequired();
            project.Property(a => a.CreatorUserId).HasColumnName("CreatorUserId");
            project.Property(a => a.ProgressPercentage).HasColumnName("ProgressPercentage");
            project.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            project.Property(a => a.DeletedStatus).IsRequired();



            project.HasOne(p => p.Workspace)
            .WithMany().IsRequired(true).HasForeignKey(c => c.WorkSpaceId);

            //project.OwnsMany<ProjectMember>("ProjectMembers", m=>
            //{
            //    m.ToTable("ProjectMember");
            //    m.Property<int>("Id");
            //    m.HasKey("Id");
            //    m.Property(m => m.UserId).IsRequired();
            //    m.Property(m => m.CreationDate).HasColumnType("datetime").IsRequired();
            //    m.Property(m => m.DeletedStatus).IsRequired();
            //});

            //project.OwnsMany<ProjectAttachment>("ProjectAttachments", m =>
            //{
            //    m.ToTable("ProjectAttachment");
            //    m.Property<int>("Id");
            //    m.HasKey("Id");
            //    m.Property(m => m.FileUrl).IsRequired();
            //    m.Property(m => m.CreationDate).HasColumnType("datetime").IsRequired();
            //    m.Property(m => m.DeletedStatus).IsRequired();
            //});

            project.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);

        }
    }
}
