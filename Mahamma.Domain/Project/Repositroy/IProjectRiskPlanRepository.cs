using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahamma.Domain.Project.Repositroy
{
    public interface IProjectRiskPlanRepository : IRepository<ProjectRiskPlan>
    {
        Task<List<ProjectRiskPlanDto>> GetAllProjectRiskPlans(int projectId);
        void AddProjectRiskPlan(ProjectRiskPlanDto projectRiskPlanDto);
        void UpdateProjectRiskPlan(ProjectRiskPlanDto projectRiskPlanDto);
        void DeleteProjectRiskPlan(int planId);
    }
}
