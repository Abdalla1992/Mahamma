using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Company.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class CompanyEntityTypeConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> company)
        {
            company.ToTable("Company");
            company.HasKey(a => a.Id);
            company.Property(a => a.Name).HasColumnName("Name").IsRequired();
            company.Property(a => a.CompanySize).HasColumnName("CompanySize").IsRequired();
            company.Property(a => a.CreatorUserId).HasColumnName("CreatorUserId");
            company.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            company.Property(a => a.DeletedStatus).IsRequired();

            company.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);

        }
    }
}
