using AutoMapper;
using Mahamma.Identity.Domain._SharedKernel;
using Mahamma.Identity.Domain.Role.Entity;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Infrastructure.Base;
using Mahamma.Identity.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.Repositories
{
    public class PageLocalizationRepository : EntityRepository<PageLocalization>, IPageLocalizationRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public PageLocalizationRepository(IMapper mapper, IdentityContext context) : base(context, mapper)
        { }

        public async Task<PageLocalization> GetByPageIdAndLanguageId(int pageId, int languageId)
        {
            return await FirstOrDefaultAsync(p => p.PageId == pageId && p.LanguageId == languageId);
        }
    }
}
