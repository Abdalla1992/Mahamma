using Mahamma.Identity.Domain._SharedKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Language.Repository
{
    public interface ILanguageRepository : IRepository<Entity.Language>
    {
        Task<List<Dto.LanguageDto>> GetAllLanguages();
        Task<Dto.LanguageDto> GetLanguageById(int id);
    }
}
