using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahamma.Domain.Task.Repository
{
    public interface ITaskCommentRepository : IRepository<Entity.TaskComment>
    {
        Task<TaskComment> GetEntityById(int commentId);
        void AddComment(TaskComment comment);
        Task<bool> ValidToLikeComment(int taskId, int commentId);
        Task<List<TaskCommentDto>> GetTaskComment(int taskId);
    }
}
