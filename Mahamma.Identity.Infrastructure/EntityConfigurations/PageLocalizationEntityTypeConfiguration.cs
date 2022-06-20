using Mahamma.Base.Dto.Enum;
using Mahamma.Identity.Domain.Language.Entity;
using Mahamma.Identity.Domain.Language.Enum;
using Mahamma.Identity.Domain.Role.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.EntityConfigurations
{
    public class PageLocalizationEntityTypeConfiguration : IEntityTypeConfiguration<PageLocalization>
    {
        public void Configure(EntityTypeBuilder<PageLocalization> PageLocalization)
        {
            PageLocalization.ToTable("PageLocalization");
            PageLocalization.HasKey(a => a.Id);
            PageLocalization.Property(a => a.DisplayName).HasColumnName("DisplayName").IsRequired();
            PageLocalization.Property(a => a.LanguageId).HasColumnName("LanguageId").IsRequired();
            PageLocalization.Property(a => a.PageId).HasColumnName("PageId").IsRequired();
            PageLocalization.Property(a => a.DeletedStatus).IsRequired();
            PageLocalization.Property(a => a.CreationDate).HasColumnType("datetime").IsRequired();

            PageLocalization.HasOne<Language>().WithMany().IsRequired(true).HasForeignKey(c => c.LanguageId);
            PageLocalization.HasOne<Page>().WithMany().IsRequired(true).HasForeignKey(c => c.PageId);


            PageLocalization.HasQueryFilter(a => a.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
    }
}
