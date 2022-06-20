using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.MyWork.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class NoteEntityTypeConfiguration : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> note)
        {
            note.ToTable("Note");
            note.HasKey(a => a.Id);
            note.Property(a => a.Title).HasColumnName("Title").IsRequired();
            note.Property(a => a.OwnerId).HasColumnName("OwnerId").IsRequired();
            note.Property(a => a.OwnerId).HasColumnName("OwnerId");
            note.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            note.Property(a => a.DeletedStatus).IsRequired();

            note.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
