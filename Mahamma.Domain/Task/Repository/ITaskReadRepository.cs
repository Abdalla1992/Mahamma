using Mahamma.Domain.Task.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahamma.Domain.Task.Repository
{
    public interface ITaskReadRepository
    {
        Task<TaskDto> GetTaskData(int TaskId);
        Task<List<UserTaskDto>> GetUserTask(long userId);
        Task<List<UserTaskAcceptedRejectedDto>> GetUserTaskAcceptedRejectedStatus(long userId);
    }
}
