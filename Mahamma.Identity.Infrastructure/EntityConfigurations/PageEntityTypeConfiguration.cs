using Mahamma.Base.Dto.Enum;
using Mahamma.Identity.Domain.Role.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Identity.Infrastructure.EntityConfigurations
{
    public class PageEntityTypeConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> Page)
        {
            Page.ToTable("Page");
            Page.HasKey(a => a.Id);
            Page.Property(a => a.Name).HasColumnName("Name").IsRequired();
            Page.Property(a => a.DeletedStatus).IsRequired();
            Page.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();

            Page.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
