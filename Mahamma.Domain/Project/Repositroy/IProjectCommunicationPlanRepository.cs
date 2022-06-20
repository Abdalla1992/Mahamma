using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahamma.Domain.Project.Repositroy
{
    public interface IProjectCommunicationPlanRepository : IRepository<ProjectCommunicationPlan>
    {
        Task<List<ProjectCommunicationPlanDto>> GetAllProjectCommunicationPlans(int projectId);
        void AddProjectCommunicationPlan(ProjectCommunicationPlanDto projectCommunicationPlanDto);
        void UpdateProjectCommunicationPlan(ProjectCommunicationPlanDto projectCommunicationPlanDto);
        void DeleteProjectCommunicationPlan(int planId);
    }
}
