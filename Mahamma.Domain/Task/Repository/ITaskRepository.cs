using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Task.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mahamma.Domain.Task.Repository
{
    public interface ITaskRepository : IRepository<Entity.Task>
    {
        void AddTask(Entity.Task task);
        Task<bool> CheckTaskExistence(string name, int projectId, int id = default);
        Task<bool> CheckUpdateTaskExistence(string name, int id);
        Task<bool> CheckSubtaskExistence(string name, int? parentTaskId, int id = default);
        Task<TaskDto> GetById(int id);
        Task<PageList<Entity.Task>> GetTaskList(int skipCount, int takeCount, Expression<Func<Entity.Task, bool>> filter = null);
        Task<PageList<TaskDto>> GetTaskList(SearchTaskDto searchTaskDto, string role, string superAdminRole, long cuurentUserId, int companyId);
        Task<TaskDto> GetTaskData(int id, long userId, string currentUserRole, string superAdminRoleName, int companyId, bool requestedFromMeeting = false);
        Task<Entity.Task> GetTaskById(int id, string includeProperties = "");
        Task<Mahamma.Domain.Task.Entity.Task> GetTask(int id, string includeProperties = "");
        void UpdateTask(Entity.Task task);
        System.Threading.Tasks.Task DeleteTaskList(List<int> taskIdList);
        System.Threading.Tasks.Task ArchiveTaskList(List<int> taskIdList);
        Task<List<Entity.Task>> GetTasksByProjectsList(int companyId, List<int> projectId, long userId, bool isSuperAdmin);
        List<TaskDto> MapTaskList(List<Task.Entity.Task> tasks);
        Task<List<Entity.Task>> GetSubtaskByTaskId(int taskId);
        Task<List<Domain.Task.Entity.Task>> GetTasksByProjectId(int projectId);
        Task<List<Entity.Task>> GetTasksThatDependOn(int taskId);
        Task<List<DropDownItem<int>>> GetProjectTasksDDL(int projectId);
        Task<List<DropDownItem<int>>> GetSubasksDDL(int taskId);
        bool CheckCommentLikedByCurrentUser(int commentId, long userId);
        Task<List<Domain.Task.Entity.Task>> GetTasksByProjectIdAndMemberId(int projectId, long userId);
        Task<List<Domain.Task.Entity.TaskMember>> GetMembersList(long userId, int projectId);

    }
}
