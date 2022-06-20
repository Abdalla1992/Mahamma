using AutoMapper;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class ProjectRiskPlanRepository : Base.EntityRepository<ProjectRiskPlan>, IProjectRiskPlanRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;

        public ProjectRiskPlanRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }

        public async Task<List<ProjectRiskPlanDto>> GetAllProjectRiskPlans(int projectId)
        {
            List<ProjectRiskPlan> projectRiskPlans = await GetWhereAsync(p => p.ProjectId == projectId);
            return Mapper.Map<List<ProjectRiskPlanDto>>(projectRiskPlans);
        }
        public void AddProjectRiskPlan(ProjectRiskPlanDto projectRiskPlanDto)
        {
            ProjectRiskPlan projectRiskPlan = Mapper.Map<ProjectRiskPlan>(projectRiskPlanDto);
            projectRiskPlan.PrepareProjectRiskPlanCreation();
            CreateAsyn(projectRiskPlan);
        }
        public void UpdateProjectRiskPlan(ProjectRiskPlanDto projectRiskPlanDto)
        {
            ProjectRiskPlan projectRiskPlan = AppDbContext.Set<ProjectRiskPlan>()
                .SingleOrDefault(p => p.Id == projectRiskPlanDto.Id);

            projectRiskPlan.UpdateProjectRiskPlan(projectRiskPlanDto.Issue, projectRiskPlanDto.Impact, projectRiskPlanDto.Action, projectRiskPlanDto.Owner);

            AppDbContext.Entry(projectRiskPlan).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public void DeleteProjectRiskPlan(int planId)
        {
            ProjectRiskPlan projectRiskPlan = AppDbContext.Set<ProjectRiskPlan>().SingleOrDefault(p => p.Id == planId);
            Delete(projectRiskPlan);
        }
    }
}
