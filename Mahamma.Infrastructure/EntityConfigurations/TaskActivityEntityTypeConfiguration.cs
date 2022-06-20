using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Task.Entity;
using Mahamma.Domain.TaskActivity.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class TaskActivityEntityTypeConfiguration : IEntityTypeConfiguration<TaskActivity>
    {
        public void Configure(EntityTypeBuilder<TaskActivity> taskActivity)
        {
            taskActivity.ToTable("TaskActivity");
            taskActivity.HasKey(a => a.Id);
            taskActivity.Property(a => a.TaskId).HasColumnName("TaskId").IsRequired();
            taskActivity.Property(a => a.Action).HasColumnName("Action").IsRequired();
            taskActivity.Property(a => a.TaskMemberId).HasColumnName("TaskMemberId").IsRequired();
            taskActivity.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            taskActivity.Property(a => a.DeletedStatus).IsRequired();


            taskActivity.HasOne<Task>()
           .WithMany(x => x.TaskActivities).IsRequired(true).HasForeignKey(c => c.TaskId);

            taskActivity.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
