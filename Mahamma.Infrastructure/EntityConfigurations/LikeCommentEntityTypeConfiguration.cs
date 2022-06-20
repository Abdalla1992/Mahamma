using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Task.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class LikeCommentEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Task.Entity.LikeComment>
    {
        public void Configure(EntityTypeBuilder<Domain.Task.Entity.LikeComment> LikeComment)
        {
            LikeComment.ToTable("LikeComment");
            LikeComment.HasKey(a => a.Id);
            LikeComment.Property(a => a.TaskCommentId).HasColumnName("TaskCommentId").IsRequired();
            LikeComment.Property(a => a.TaskMemberId).HasColumnName("TaskMemberId").IsRequired();
            LikeComment.Property(a => a.DeletedStatus).IsRequired();
            LikeComment.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();

            LikeComment.HasOne<TaskComment>()
            .WithMany(m => m.Likes).IsRequired(true).HasForeignKey(c => c.TaskCommentId);

            LikeComment.HasOne<TaskMember>()
            .WithMany(m => m.LikedComments).IsRequired(false).HasForeignKey(c => c.TaskMemberId);


            LikeComment.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
