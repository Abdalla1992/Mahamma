using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Company.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class CompanyInvitationFileEntityTypeConfiguration : IEntityTypeConfiguration<CompanyInvitationFile>
    {
        public void Configure(EntityTypeBuilder<CompanyInvitationFile> companyInvitationFile)
        {
            companyInvitationFile.ToTable("CompanyInvitationFile");
            companyInvitationFile.HasKey(a => a.Id);
            companyInvitationFile.Property(a => a.CompanyId).HasColumnName("CompanyId").IsRequired();
            companyInvitationFile.Property(a => a.Status).HasColumnName("Status").IsRequired();
            companyInvitationFile.Property(a => a.UserId).HasColumnName("UserId").IsRequired();
            companyInvitationFile.Property(a => a.DeletedStatus).IsRequired();

            companyInvitationFile.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);

        }
    }
}
