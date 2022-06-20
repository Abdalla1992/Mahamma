using AutoMapper;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class ProjectCharterRepository : Base.EntityRepository<ProjectCharter>, IProjectCharterRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;

        public ProjectCharterRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }

        public async Task<ProjectCharterDto> GetProjectCharterByProjectId(int projectId)
        {
            ProjectCharter projectCharter = await FirstOrDefaultNoTrackingAsync(p => p.ProjectId == projectId);
            return Mapper.Map<ProjectCharterDto>(projectCharter);
        }

        public async Task<bool> CheckProjectCharterExist(int projectId)
        {
            return await GetAnyAsync(p => p.ProjectId == projectId);
        }

        public void CreateProjectCharter(ProjectCharterDto projectCharterDto)
        {
            ProjectCharter projectCharter = Mapper.Map<ProjectCharter>(projectCharterDto);
            projectCharter.PrepareProjectCharterCreation();
            CreateAsyn(projectCharter);
        }

        public void UpdateProjectCharter(ProjectCharterDto projectCharterDto)
        {
            ProjectCharter projectCharter = AppDbContext.Set<ProjectCharter>()
                .SingleOrDefault(p => p.ProjectId == projectCharterDto.ProjectId);

            projectCharter.UpdateProjectCharter(projectCharterDto.Summary, projectCharterDto.Goals, projectCharterDto.Deliverables,
                                                projectCharterDto.Scope, projectCharterDto.Benefits, projectCharterDto.Costs, projectCharterDto.Misalignments);

            AppDbContext.Entry(projectCharter).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
    }
}
