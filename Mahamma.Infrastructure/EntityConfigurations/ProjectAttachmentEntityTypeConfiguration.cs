using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.ProjectAttachment.Entity;
using Mahamma.Domain.Task.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class ProjectAttachmentEntityTypeConfiguration : IEntityTypeConfiguration<ProjectAttachment>
    {
        public void Configure(EntityTypeBuilder<ProjectAttachment> projectAttachment)
        {
            projectAttachment.ToTable("ProjectAttachment");
            projectAttachment.HasKey(a => a.Id);
            projectAttachment.Property(a => a.FileUrl).HasColumnName("FileUrl").IsRequired();
            projectAttachment.Property(a => a.ProjectId).HasColumnName("ProjectId").IsRequired();
            projectAttachment.Property(a => a.TaskId).HasColumnName("TaskId");
            projectAttachment.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            projectAttachment.Property(a => a.DeletedStatus).IsRequired();

            projectAttachment.HasOne<Project>()
            .WithMany(m=>m.ProjectAttachments).IsRequired(true).HasForeignKey(c => c.ProjectId);

            projectAttachment.HasOne<Task>()
            .WithMany(m=>m.Attachments).IsRequired(false).HasForeignKey(c => c.TaskId);

            projectAttachment.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);

        }
    }
}
