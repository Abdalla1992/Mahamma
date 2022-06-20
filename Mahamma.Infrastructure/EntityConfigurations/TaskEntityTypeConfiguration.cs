using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Meeting.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class TaskEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Task.Entity.Task>
    {
        public void Configure(EntityTypeBuilder<Domain.Task.Entity.Task> Task)
        {
            Task.ToTable("Task");
            Task.HasKey(a => a.Id);
            Task.Property(a => a.Name).HasColumnName("Name").IsRequired();
            Task.Property(a => a.Description).HasColumnName("Description");
            Task.Property(a => a.StartDate).HasColumnType("datetime").HasColumnName("StartDate");
            Task.Property(a => a.DueDate).HasColumnType("datetime").HasColumnName("DueDate");
            Task.Property(a => a.TaskPriorityId).HasColumnName("TaskPriorityId");
            Task.Property(a => a.ReviewRequest).HasColumnName("ReviewRequest");
            Task.Property(a => a.ProjectId).HasColumnName("ProjectId").IsRequired();
            Task.Property(a => a.CreatorUserId).HasColumnName("CreatorUserId");
            Task.Property(a => a.TaskStatusId).HasColumnName("TaskStatusId");
            Task.Property(a => a.Rating).HasColumnName("Rating");
            Task.Property(a => a.ProgressPercentage).HasColumnName("ProgressPercentage");
            Task.Property(a => a.DependencyTaskId).HasColumnName("DependencyTaskId");
            Task.Property(a => a.DeletedStatus).IsRequired();
            Task.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();

            Task.HasOne(t => t.Project).WithMany(m=>m.Tasks).IsRequired(true).HasForeignKey(c => c.ProjectId);

            Task.HasMany(e => e.SubTask).WithOne().HasForeignKey(e => e.ParentTaskId);

            Task.HasMany(e => e.TaskMembers).WithOne(cr => cr.Task).HasForeignKey(e => e.TaskId);

            Task.HasOne<Domain.Task.Enum.TaskPriority>().WithMany().IsRequired(true).HasForeignKey(c => c.TaskPriorityId);

            Task.HasOne<Domain.Task.Enum.TaskStatus>().WithMany().IsRequired(true).HasForeignKey(c => c.TaskStatusId);        

            Task.HasQueryFilter(c => c.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
