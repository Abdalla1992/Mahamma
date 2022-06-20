using Mahamma.Identity.Domain._SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Role.Repository
{
    public interface IPageLocalizationRepository : IRepository<Entity.PageLocalization>
    {
        Task<Entity.PageLocalization> GetByPageIdAndLanguageId(int pageId, int languageId);
    }
}
