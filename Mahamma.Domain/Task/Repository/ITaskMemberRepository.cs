using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mahamma.Domain.Task.Repository
{
    public interface ITaskMemberRepository : IRepository<Entity.TaskMember>
    {
        Task<TaskMemberDto> GetMember(int taskId, long userId);
        Task<List<long>> GetMembersUserIdList(int taskId);
        Task<List<TaskMember>> GetMembersList(int taskId, List<long> removedMembersUserIdList);
        void AssingMemberListToTask(List<TaskMember> newMembers);
        Task<IQueryable<TaskDto>> GetHistoryTasks(long userId);
        public Task<List<TaskMember>> GetTaskMemberByTaskId(int taskId);
        Task<TaskMember> GetById(int taskMemberId);
        Task<List<TaskMember>> GetTaskMemberByTaskIdExceptCurrentUser(int taskId, long currentUser);






    }
}
