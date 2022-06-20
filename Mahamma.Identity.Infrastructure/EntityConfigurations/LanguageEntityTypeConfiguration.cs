using Mahamma.Base.Dto.Enum;
using Mahamma.Identity.Domain.Language.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Identity.Infrastructure.EntityConfigurations
{
    public class LanguageEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Language.Entity.Language>
    {
        public void Configure(EntityTypeBuilder<Language> Language)
        {
            Language.ToTable("Language");
            Language.HasKey(a => a.Id);
            Language.Property(a => a.Name).HasColumnName("Name").IsRequired();
            Language.Property(a => a.Alias).HasColumnName("Alias").IsRequired();
            Language.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            Language.Property(a => a.DeletedStatus).HasColumnName("DeletedStatus").IsRequired();

            Language.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);

        }
    }
}
