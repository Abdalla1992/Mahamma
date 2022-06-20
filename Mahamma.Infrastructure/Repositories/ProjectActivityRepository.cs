using AutoMapper;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.ProjectActivity.Dto;
using Mahamma.Domain.ProjectActivity.Entity;
using Mahamma.Domain.ProjectActivity.Repository;
using Mahamma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class ProjectActivityRepository : Base.EntityRepository<ProjectActivity>, IProjectActivityRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public ProjectActivityRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }

        public void LogActivity(ProjectActivity projectActivity)
        {
            CreateAsyn(projectActivity);
        }

        public async Task<List<ProjectActivityDto>> GetProjectActivityList(int projectId)
        {
            var projectActivity =await GetWhereAsync(m => m.ProjectId == projectId);
            return Mapper.Map<List<ProjectActivityDto>>(projectActivity);
        }
    }
}
