using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Project.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class ProjectLikeCommentEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Project.Entity.ProjectLikeComment>
    {
        public void Configure(EntityTypeBuilder<ProjectLikeComment> ProjectLikeComment)
        {
            ProjectLikeComment.ToTable("ProjectLikeComment");
            ProjectLikeComment.HasKey(a => a.Id);
            ProjectLikeComment.Property(a => a.ProjectCommentId).HasColumnName("ProjectCommentId").IsRequired();
            ProjectLikeComment.Property(a => a.ProjectMemberId).HasColumnName("ProjectMemberId").IsRequired();
            ProjectLikeComment.Property(a => a.DeletedStatus).IsRequired();
            ProjectLikeComment.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();

            //ProjectLikeComment.HasOne<ProjectComment>()
            //.WithMany(m => m.Likes).IsRequired(true).HasForeignKey(c => c.ProjectCommentId);

            //ProjectLikeComment.HasOne<ProjectMember>()
            //.WithMany().IsRequired(true).HasForeignKey(c => c.ProjectMemberId);


            ProjectLikeComment.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
