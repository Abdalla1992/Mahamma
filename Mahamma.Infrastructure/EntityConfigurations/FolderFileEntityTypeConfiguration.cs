using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.ProjectAttachment.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class FolderFileEntityTypeConfiguration : IEntityTypeConfiguration<FolderFile>
    {
        public void Configure(EntityTypeBuilder<FolderFile> folderFile)
        {
            folderFile.ToTable("FolderFile");
            folderFile.HasKey(a => a.Id);
            folderFile.Property(a => a.FolderId).HasColumnName("FolderId").IsRequired();
            folderFile.Property(a => a.ProjectAttachmentId).HasColumnName("ProjectAttachmentId").IsRequired();
            folderFile.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            folderFile.Property(a => a.DeletedStatus).IsRequired();

            folderFile.HasOne(f => f.Folder).WithMany(x =>x.FolderFiles).HasForeignKey(c => c.FolderId);

            folderFile.HasOne(f => f.ProjectAttachment)
                .WithMany(x => x.FolderFiles).OnDelete(DeleteBehavior.NoAction)
                .IsRequired(true).HasForeignKey(c => c.ProjectAttachmentId);

            folderFile.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
