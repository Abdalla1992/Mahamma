using Mahamma.Identity.Domain._SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Role.Repository
{
    public interface IPermissionLocalizationRepository : IRepository<Entity.PermissionLocalization>
    {
        Task<Entity.PermissionLocalization> GetByPermissionIdAndLanguageId(int permissionId, int languageId);
    }
}
