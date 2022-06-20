using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Task.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class TaskMemberEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Task.Entity.TaskMember>
    {
        public void Configure(EntityTypeBuilder<Domain.Task.Entity.TaskMember> TaskMember)
        {
            TaskMember.ToTable("TaskMember");
            TaskMember.HasKey(a => a.Id);
            TaskMember.Property(a => a.UserId).HasColumnName("UserId").IsRequired();
            TaskMember.Property(a => a.TaskId).HasColumnName("TaskId").IsRequired();
            TaskMember.Property(a => a.Rating).HasColumnName("Rating");
            TaskMember.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            TaskMember.Property(a => a.DeletedStatus).IsRequired();

            TaskMember.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
