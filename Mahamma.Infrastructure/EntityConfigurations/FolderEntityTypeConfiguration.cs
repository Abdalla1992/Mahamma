using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.ProjectAttachment.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class FolderEntityTypeConfiguration : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> folder)
        {
            folder.ToTable("Folder");
            folder.HasKey(a => a.Id);
            folder.Property(a => a.Name).HasColumnName("Name").IsRequired();
            folder.Property(a => a.ProjectId).HasColumnName("ProjectId").IsRequired();
            folder.Property(a => a.TaskId).HasColumnName("TaskId");
            folder.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            folder.Property(a => a.DeletedStatus).IsRequired();

            folder.HasOne(m => m.Project).WithMany().IsRequired(true).HasForeignKey(m => m.ProjectId);
            folder.HasOne(m => m.Task).WithMany().HasForeignKey(m => m.TaskId);

            folder.HasMany(m => m.FolderFiles).WithOne().IsRequired(true).HasForeignKey(m => m.FolderId);


            folder.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);

        }
    }
}
