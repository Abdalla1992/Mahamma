using Mahamma.Domain._SharedKernel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahamma.Domain.TaskActivity.Repository
{
    public interface ITaskActivityRepository : IRepository<Entity.TaskActivity>
    {
        Task<List<Dto.TaskActivityDto>> GetLogs(int taskId);
        void LogActivity(Entity.TaskActivity activity);
    }
}
