using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.ProjectActivity.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Domain.ProjectActivity.Repository
{
   public interface IProjectActivityRepository : IRepository<Entity.ProjectActivity>
    {
        void LogActivity(Entity.ProjectActivity projectActivity);
        Task<List<ProjectActivityDto>> GetProjectActivityList(int projectId);
    }
}
