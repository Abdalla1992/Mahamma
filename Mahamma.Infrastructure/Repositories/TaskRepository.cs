using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Entity;
using Mahamma.Domain.Task.Repository;
using Mahamma.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class TaskRepository : Base.EntityRepository<Domain.Task.Entity.Task>, ITaskRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public TaskRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }
        public void AddTask(Mahamma.Domain.Task.Entity.Task task)
        {
            CreateAsyn(task);
        }
        public async Task<bool> CheckTaskExistence(string name, int projectId, int id = default)
        {
            return await GetAnyAsync(t => t.Name == name && t.ProjectId == projectId && (id == default || t.Id != id) && t.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
        public async Task<bool> CheckUpdateTaskExistence(string name, int id)
        {
            Domain.Task.Entity.Task task = await FirstOrDefaultAsync(t => t.Id == id);
            return await GetAnyAsync(t => t.Name == name && t.ProjectId == task.ProjectId && t.Id != id
            && (!task.ParentTaskId.HasValue || t.ParentTaskId == task.ParentTaskId) && t.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
        public async Task<bool> CheckSubtaskExistence(string name, int? parentTaskId, int id = default)
        {
            return await GetAnyAsync(t => t.Name == name && t.ParentTaskId == parentTaskId && (id == default || t.Id != id) && t.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
        public async Task<TaskDto> GetById(int id)
        {
            return Mapper.Map<TaskDto>(await FirstOrDefaultNoTrackingAsync(t => t.Id == id));
        }
        public async Task<PageList<Domain.Task.Entity.Task>> GetTaskList(int skipCount, int takeCount, Expression<Func<Domain.Task.Entity.Task, bool>> filter)
        {
            PageList<Domain.Task.Entity.Task> taskList = new();
            var remainingTaskList = GetWhere(filter, "TaskMembers").Skip(skipCount);
            var Count = await remainingTaskList.CountAsync();
            var resultList = remainingTaskList.Take(takeCount);
            taskList.SetResult(Count, resultList.ToList());
            return taskList;
        }
        public async Task<PageList<TaskDto>> GetTaskList(SearchTaskDto searchTaskeDto, string role, string superAdminRole, long cuurentUserId, int companyId)
        {
            #region Declare Return Var with Intial Value
            PageList<TaskDto> taskListDto = new();
            #endregion
            #region Preparing Filter 
            Expression<Func<Domain.Task.Entity.Task, bool>> filter;
            if (role == superAdminRole)
            {
                filter = t => t.DeletedStatus == DeletedStatus.NotDeleted.Id
                                                         && (searchTaskeDto.Filter.Id == default || t.Id == searchTaskeDto.Filter.Id)
                                                         && (searchTaskeDto.Filter.ParentTaskId == default(int) || t.ParentTaskId == searchTaskeDto.Filter.ParentTaskId)
                                                         && (searchTaskeDto.Filter.TaskPriorityId == default || t.TaskPriorityId == searchTaskeDto.Filter.TaskPriorityId)
                                                         && (searchTaskeDto.Filter.ProjectId > 0 ? t.ProjectId == searchTaskeDto.Filter.ProjectId : t.Project.Workspace.CompanyId == companyId)
                                                         && (string.IsNullOrEmpty(searchTaskeDto.Filter.Name) || t.Name.ToLower().Contains(searchTaskeDto.Filter.CleanName))
                                                         && (string.IsNullOrEmpty(searchTaskeDto.Filter.Description) || t.Description.Contains(searchTaskeDto.Filter.Description, StringComparison.OrdinalIgnoreCase))
                                                         && (!searchTaskeDto.Filter.StartDate.HasValue || t.StartDate >= searchTaskeDto.Filter.StartDate)
                                                         && (!searchTaskeDto.Filter.DueDate.HasValue || t.DueDate <= searchTaskeDto.Filter.DueDate);
            }
            else
            {
                filter = t => t.DeletedStatus == DeletedStatus.NotDeleted.Id
                                                         && (searchTaskeDto.Filter.Id == default || t.Id == searchTaskeDto.Filter.Id)
                                                         && (searchTaskeDto.Filter.ParentTaskId == default(int) || t.ParentTaskId == searchTaskeDto.Filter.ParentTaskId)
                                                         && (searchTaskeDto.Filter.TaskPriorityId == default || t.TaskPriorityId == searchTaskeDto.Filter.TaskPriorityId)
                                                         && (searchTaskeDto.Filter.ProjectId > 0 ? t.ProjectId == searchTaskeDto.Filter.ProjectId : t.TaskMembers.Any(tm => tm.UserId == cuurentUserId))
                                                         && t.TaskMembers.Any(m => m.UserId == cuurentUserId && m.DeletedStatus != DeletedStatus.Deleted.Id)
                                                         && (string.IsNullOrEmpty(searchTaskeDto.Filter.Name) || t.Name.ToLower().Contains(searchTaskeDto.Filter.CleanName))
                                                         && (string.IsNullOrEmpty(searchTaskeDto.Filter.Description) || t.Description.ToLower().Contains(searchTaskeDto.Filter.Description.ToLower()))
                                                         && (!searchTaskeDto.Filter.StartDate.HasValue || t.StartDate >= searchTaskeDto.Filter.StartDate)
                                                         && (!searchTaskeDto.Filter.DueDate.HasValue || t.DueDate <= searchTaskeDto.Filter.DueDate);
            }
            #endregion

            List<Mahamma.Domain.Task.Entity.Task> taskList = searchTaskeDto.Sorting.Column switch
            {
                "name" => await GetPageAsyncWithoutQueryFilter(searchTaskeDto.Paginator.Page, searchTaskeDto.Paginator.PageSize, filter, x => x.Name, searchTaskeDto.Sorting.SortingDirection.Id, "SubTask"),
                _ => await GetPageAsyncWithoutQueryFilter(searchTaskeDto.Paginator.Page, searchTaskeDto.Paginator.PageSize, filter, x => x.Id, SortDirection.Descending.Id, "SubTask"),
            };
            if (taskList?.Count > default(int))
            {
                int totalCount = await GetCountAsyncWithoutQueryFilter(filter);
                taskListDto.SetResult(totalCount, Mapper.Map<List<Domain.Task.Entity.Task>, List<TaskDto>>(taskList));
            }
            return taskListDto;
        }
        public async Task<TaskDto> GetTaskData(int id, long userId, string currentUserRole, string superAdminRoleName, int companyId, bool requestedFromMeeting = false)
        {
            Domain.Task.Entity.Task task = null;
            if (currentUserRole == superAdminRoleName || requestedFromMeeting)
            {
                task = await FirstOrDefaultAsyncWithoutQueryFilter(t => t.Id == id && t.Project.Workspace.CompanyId == companyId,
                    "TaskMembers,TaskMembers.TaskComments,TaskMembers.TaskComments.Replies,TaskMembers.TaskComments.Likes,SubTask,Project");
            }
            else
            {
                task = await FirstOrDefaultAsyncWithoutQueryFilter(t => t.Id == id && t.TaskMembers.Any(m => m.UserId == userId && m.DeletedStatus == DeletedStatus.NotDeleted.Id),
                   "TaskMembers,TaskMembers.TaskComments,TaskMembers.TaskComments.Replies,TaskMembers.TaskComments.Likes,SubTask,Project");
            }
            return Mapper.Map<TaskDto>(task);
        }
        public async Task<Mahamma.Domain.Task.Entity.Task> GetTaskById(int id, string includeProperties = "")
        {
            return await FirstOrDefaultAsync(t => t.Id == id, includeProperties);
        }
        public async Task<Mahamma.Domain.Task.Entity.Task> GetTask(int id, string includeProperties = "")
        {
            return await FirstOrDefaultAsyncWithoutQueryFilter(t => t.Id == id, includeProperties);
        }
        public void UpdateTask(Mahamma.Domain.Task.Entity.Task task)
        {
            Update(task);
        }
        public async System.Threading.Tasks.Task LikeComment(int currentUserId, int taskId, int commentId)
        {
            var task = await FirstOrDefaultAsync(t => t.Id == taskId, "TaskMembers,TaskMembers.TaskComments,TaskMembers.TaskComments.Likes");
            var member = task.TaskMembers.FirstOrDefault(m => m.UserId == currentUserId);
            var comment = member.TaskComments.FirstOrDefault(c => c.Id == commentId);
            LikeComment like = comment.Likes.FirstOrDefault(c => c.TaskCommentId == commentId && c.TaskMemberId == member.Id);
            if (like == null)
            {
                like = new();
                like.LikeAComment(commentId, member.Id);
            }
            else
                like.DisLikeComment();
            comment.Likes.Add(like);
        }
        public async System.Threading.Tasks.Task DeleteTaskList(List<int> taskIdList)
        {
            var tasks = await GetWhere(t => taskIdList.Contains(t.Id)).ToListAsync();
            tasks.ForEach(t => t.DeleteTask());
        }
        public async System.Threading.Tasks.Task ArchiveTaskList(List<int> taskIdList)
        {
            var tasks = await GetWhere(t => taskIdList.Contains(t.Id)).ToListAsync();
            tasks.ForEach(t => t.ArchiveTask());
        }

        public Task<List<Domain.Task.Entity.Task>> GetTasksByProjectsList(int companyId, List<int> projectList, long userId, bool isSuperAdmin)
        {
            if (isSuperAdmin && projectList != null && projectList?.Count <= 0)
                return GetWhereAsync(t => t.Project.Workspace.CompanyId == companyId, "TaskMembers,TaskMembers.TaskComments,TaskMembers.TaskComments.Replies,TaskMembers.TaskComments.Likes,SubTask,Project,Project.Workspace");
            else
                return GetWhereAsync(t =>
                (projectList.Contains(t.ProjectId) || projectList == null || projectList.Count == 0)
                && t.TaskMembers.Select(m => m.UserId).Contains(userId)
                && t.Project.Workspace.CompanyId == companyId,
                "TaskMembers,TaskMembers.TaskComments,TaskMembers.TaskComments.Replies,TaskMembers.TaskComments.Likes,SubTask,Project,Project.Workspace");
        }
        public List<TaskDto> MapTaskList(List<Domain.Task.Entity.Task> tasks)
        {
            return Mapper.Map<List<TaskDto>>(tasks);
        }

        public async Task<List<Domain.Task.Entity.Task>> GetSubtaskByTaskId(int taskId)
        {
            return await GetWhereAsync(t => t.ParentTaskId == taskId && t.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }

        public async Task<List<Domain.Task.Entity.Task>> GetTasksByProjectId(int projectId)
        {
            return await GetWhereAsync(t => t.ProjectId == projectId
                    && !t.ParentTaskId.HasValue
                    && t.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }



        public async Task<List<Domain.Task.Entity.Task>> GetTasksByProjectIdAndMemberId(int projectId,long userId)
        {
            return await GetWhereAsync(t => t.ProjectId == projectId
                    && t.TaskMembers.Any(t => t.UserId == userId)
                    && t.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }





        public async Task<List<Domain.Task.Entity.TaskMember>> GetMembersList(long userId , int projectId)
        {
            var task =await GetWhereAsync(t => t.TaskMembers.Any(r => r.UserId == userId) && t.ProjectId==projectId, "TaskMembers");
            var taskesMember = task.SelectMany(t => t.TaskMembers).Where(f =>f.Rating !=null && f.UserId == userId).ToList();
            return taskesMember;
        }






        public async Task<List<Domain.Task.Entity.Task>> GetTasksThatDependOn(int taskId)
        {
            return await GetWhereAsync(t => t.DependencyTaskId == taskId);
        }

        public async Task<List<DropDownItem<int>>> GetProjectTasksDDL(int projectId)
        {
            return Mapper.Map<List<DropDownItem<int>>>(await GetWhereAsync(t => t.ProjectId == projectId));
        }

        public async Task<List<DropDownItem<int>>> GetSubasksDDL(int taskId)
        {
            return Mapper.Map<List<DropDownItem<int>>>(await GetWhereAsync(t => t.ParentTaskId.HasValue && t.ParentTaskId.Value == taskId));
        }

        public bool CheckCommentLikedByCurrentUser(int commentId, long userId)
        {
            return AppDbContext.Set<LikeComment>().Any(c => c.TaskCommentId == commentId && c.TaskMember.UserId == userId);
        }
    }
}
