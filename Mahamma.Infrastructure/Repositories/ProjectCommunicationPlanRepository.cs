using AutoMapper;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Infrastructure.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class ProjectCommunicationPlanRepository : Base.EntityRepository<ProjectCommunicationPlan>, IProjectCommunicationPlanRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;

        public ProjectCommunicationPlanRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }

        public async Task<List<ProjectCommunicationPlanDto>> GetAllProjectCommunicationPlans(int projectId)
        {
            List<ProjectCommunicationPlan> projectCommunicationPlans = await GetWhereAsync(p => p.ProjectId == projectId);
            return Mapper.Map<List<ProjectCommunicationPlanDto>>(projectCommunicationPlans);
        }
        public void AddProjectCommunicationPlan(ProjectCommunicationPlanDto projectCommunicationPlanDto)
        {
            ProjectCommunicationPlan projectCommunicationPlan = Mapper.Map<ProjectCommunicationPlan>(projectCommunicationPlanDto);
            projectCommunicationPlan.PrepareProjectCommunicationPlanCreation();
            CreateAsyn(projectCommunicationPlan);
        }
        public void UpdateProjectCommunicationPlan(ProjectCommunicationPlanDto projectCommunicationPlanDto)
        {
            ProjectCommunicationPlan projectCommunicationPlan = AppDbContext.Set<ProjectCommunicationPlan>()
                .SingleOrDefault(p => p.Id == projectCommunicationPlanDto.Id);

            projectCommunicationPlan.UpdateProjectCommunicationPlan(projectCommunicationPlanDto.Recipient, projectCommunicationPlanDto.Frequency,
                                                                    projectCommunicationPlanDto.CommunicationType, projectCommunicationPlanDto.Owner,
                                                                    projectCommunicationPlanDto.KeyDates, projectCommunicationPlanDto.DeliveryMethod,
                                                                    projectCommunicationPlanDto.Goal, projectCommunicationPlanDto.ResourceLinks,
                                                                    projectCommunicationPlanDto.Notes);

            AppDbContext.Entry(projectCommunicationPlan).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public void DeleteProjectCommunicationPlan(int planId)
        {
            ProjectCommunicationPlan projectCommunicationPlan = AppDbContext.Set<ProjectCommunicationPlan>().SingleOrDefault(p => p.Id == planId);
            Delete(projectCommunicationPlan);
        }
    }
}
