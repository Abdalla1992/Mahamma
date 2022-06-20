using AutoMapper;
using Mahamma.Identity.Domain._SharedKernel;
using Mahamma.Identity.Domain.Language.Dto;
using Mahamma.Identity.Domain.Language.Entity;
using Mahamma.Identity.Domain.Language.Repository;
using Mahamma.Identity.Infrastructure.Base;
using Mahamma.Identity.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.Repositories
{
    public class LanguageRepository : EntityRepository<Language>, ILanguageRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public LanguageRepository(IMapper mapper, IdentityContext context) : base(context, mapper)
        { }

        public async Task<List<LanguageDto>> GetAllLanguages()
        {
            return Mapper.Map<List<LanguageDto>>(await GetAllAsync());
        }

        public async Task<LanguageDto> GetLanguageById(int id)
        {
            return Mapper.Map<LanguageDto>(await FirstOrDefaultAsync(l => l.Id == id));
        }
    }
}
