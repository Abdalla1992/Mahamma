using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Task.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class TaskCommentsEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Task.Entity.TaskComment>
    {
        public void Configure(EntityTypeBuilder<Domain.Task.Entity.TaskComment> TaskComment)
        {
            TaskComment.ToTable("TaskComment");
            TaskComment.HasKey(a => a.Id);
            TaskComment.Property(a => a.Comment).HasColumnName("Comment").IsRequired();
            TaskComment.Property(a => a.TaskId).HasColumnName("TaskId");
            TaskComment.Property(a => a.TaskMemberId).HasColumnName("TaskMemberId").IsRequired();
            TaskComment.Property(a => a.DeletedStatus).IsRequired();
            TaskComment.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();

            TaskComment.HasOne<Task>()
            .WithMany(t => t.TaskComments).HasForeignKey(c => c.TaskId);

            TaskComment.HasOne<TaskMember>()
            .WithMany(m => m.TaskComments).IsRequired(true).HasForeignKey(c => c.TaskMemberId);

            TaskComment.HasMany(e => e.Replies)
            .WithOne().HasForeignKey(e => e.ParentCommentId);

            TaskComment.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
