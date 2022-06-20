using Mahamma.Base.Dto.Enum;
using Mahamma.Domain.Company.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mahamma.Infrastructure.EntityConfigurations
{
    public class CompanyInvitationEntityTypeConfiguration : IEntityTypeConfiguration<CompanyInvitation>
    {
        public void Configure(EntityTypeBuilder<CompanyInvitation> companyInvitation)
        {
            companyInvitation.ToTable("CompanyInvitation");
            companyInvitation.HasKey(a => a.Id);
            companyInvitation.Property(a => a.CompanyId).HasColumnName("CompanyId").IsRequired();
            companyInvitation.Property(a => a.Email).HasColumnName("Email").IsRequired();
            companyInvitation.Property(a => a.UserId).HasColumnName("UserId").IsRequired();
            companyInvitation.Property(a => a.InvitationId).HasColumnName("InvitationId").IsRequired();
            companyInvitation.Property(a => a.InvitationStatusId).HasColumnName("InvitationStatusId").IsRequired();
            companyInvitation.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();
            companyInvitation.Property(a => a.DeletedStatus).IsRequired();

            companyInvitation.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);

        }
    }
}
