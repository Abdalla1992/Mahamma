using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Entity;
using System.Threading.Tasks;

namespace Mahamma.Domain.Project.Repositroy
{
    public interface IProjectCharterRepository : IRepository<ProjectCharter>
    {
        Task<ProjectCharterDto> GetProjectCharterByProjectId(int projectId);
        Task<bool> CheckProjectCharterExist(int projectId);
        void CreateProjectCharter(ProjectCharterDto projectCharterDto);
        void UpdateProjectCharter(ProjectCharterDto projectCharterDto);
    }
}
