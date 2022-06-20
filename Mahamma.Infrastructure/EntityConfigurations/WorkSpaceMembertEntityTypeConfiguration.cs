using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Workspace.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    class WorkspaceMembertEntityTypeConfiguration : IEntityTypeConfiguration<WorkspaceMember>
    {
        public void Configure(EntityTypeBuilder<WorkspaceMember> Workspacemember)
        {
            Workspacemember.ToTable("WorkspaceMember");
            Workspacemember.HasKey(a => a.Id);
            Workspacemember.Property(a => a.WorkspaceId).HasColumnName("WorkspaceId").IsRequired();
            Workspacemember.Property(a => a.UserId).HasColumnName("UserId").IsRequired();
            Workspacemember.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            Workspacemember.Property(a => a.DeletedStatus).IsRequired();

             Workspacemember.HasOne<Workspace>()
            .WithMany(x =>x.WorkspaceMembers).IsRequired(true).HasForeignKey(c => c.WorkspaceId);

            Workspacemember.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
