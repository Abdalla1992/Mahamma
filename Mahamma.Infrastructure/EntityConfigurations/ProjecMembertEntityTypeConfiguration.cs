using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Project.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    class ProjecMembertEntityTypeConfiguration : IEntityTypeConfiguration<ProjectMember>
    {
        public void Configure(EntityTypeBuilder<ProjectMember> projectMember)
        {
            projectMember.ToTable("ProjectMember");
            projectMember.HasKey(a => a.Id);
            projectMember.Property(a => a.ProjectId).HasColumnName("ProjectId").IsRequired();
            projectMember.Property(a => a.UserId).HasColumnName("UserId").IsRequired();
            projectMember.Property(a => a.Rating).HasColumnName("Rating");
            projectMember.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            projectMember.Property(a => a.DeletedStatus).IsRequired();


            projectMember.HasOne<Project>()
           .WithMany(x => x.ProjectMembers).IsRequired(true).HasForeignKey(c => c.ProjectId);


            projectMember.HasMany(e => e.ProjectLikeComments).WithOne(cr => cr.ProjectMember).OnDelete(DeleteBehavior.NoAction).HasForeignKey(e => e.ProjectMemberId);


            projectMember.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);

        }
    }
}
