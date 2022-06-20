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
    public class ProjectCommentEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Project.Entity.ProjectComment>
    {
        public void Configure(EntityTypeBuilder<ProjectComment> ProjectComment)
        {
            ProjectComment.ToTable("ProjectComment");
            ProjectComment.HasKey(a => a.Id);
            ProjectComment.Property(a => a.Comment).HasColumnName("Comment").IsRequired();
            ProjectComment.Property(a => a.ProjectId).HasColumnName("ProjectId");
            ProjectComment.Property(a => a.ProjectMemberId).HasColumnName("ProjectMemberId").IsRequired();
            ProjectComment.Property(a => a.DeletedStatus).IsRequired();
            ProjectComment.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();

            ProjectComment.HasOne<Project>()
            .WithMany(t => t.ProjectComments).HasForeignKey(c => c.ProjectId);

            ProjectComment.HasOne<ProjectMember>()
            .WithMany(m => m.ProjectComments).IsRequired(true).HasForeignKey(c => c.ProjectMemberId);

            ProjectComment.HasMany(e => e.Replies)
            .WithOne().HasForeignKey(e => e.ParentCommentId);


            ProjectComment.HasMany(e => e.Likes).WithOne(cr => cr.ProjectComment).HasForeignKey(e => e.ProjectCommentId);

            ProjectComment.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
