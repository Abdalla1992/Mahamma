using Mahamma.AppService.Dashboard.Dto;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.MemberSearch.Dto;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.Task.Dto;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.MemberSearch.GetProjectStatistics
{
    public class GetProjectStatisticsCommandHandler : IRequestHandler<GetProjectStatisticsCommand, ValidateableResponse<ApiResponse<DashboardDto>>>
    {
        #region Props
        private readonly ITaskRepository _taskRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly AppSetting _appSetting;
        private readonly IAccountService _accountService;
        private readonly ITaskMemberRepository _taskMemberRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        #endregion

        #region CTRS
        public GetProjectStatisticsCommandHandler(IHttpContextAccessor httpContext, ITaskRepository taskRepository,
            AppSetting _appSetting, ITaskMemberRepository taskMemberRepository, IAccountService accountService,
            IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _taskRepository = taskRepository;
            this._appSetting = _appSetting;
            _accountService = accountService;
            _taskMemberRepository = taskMemberRepository;
            _messageResourceReader = messageResourceReader;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<DashboardDto>>> Handle(GetProjectStatisticsCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<DashboardDto>> response = new(new ApiResponse<DashboardDto>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];

            List<Domain.Task.Entity.Task> tasks = await _taskRepository.GetTasksByProjectsList(currentUser.CompanyId, request.ProjectIdList, currentUser.Id, currentUser.RoleName == _appSetting.SuperAdminRole);
            if (tasks?.Count > default(int))
            {
                List<Tuple<string, int, string>> taskStatusTuple = Domain.Task.Enum.TaskStatus.List().Select(s => Tuple.Create(_messageResourceReader.GetKeyValue(s.Name, currentUser.LanguageId), 0, GetStatusColor(s.Id))).ToList();
                List<Tuple<string, int, string>> completedTaskStatusTuple = Domain.Task.Enum.TaskStatus.CompletedList().Select(s => Tuple.Create(_messageResourceReader.GetKeyValue(s.Name, currentUser.LanguageId), 0, GetStatusColor(s.Id))).ToList();
                List<Tuple<string, int, string>> notCompletedTaskStatusTuple = Domain.Task.Enum.TaskStatus.NotCompletedList().Select(s => Tuple.Create(_messageResourceReader.GetKeyValue(s.Name, currentUser.LanguageId), 0, GetStatusColor(s.Id))).ToList();
                List<Tuple<string, int, string>> subTaskStatusTuple = Domain.Task.Enum.TaskStatus.List().Select(s => Tuple.Create(_messageResourceReader.GetKeyValue(s.Name, currentUser.LanguageId), 0, GetStatusColor(s.Id))).ToList();
                List<Tuple<string, int, string>> completedSubtaskStatusTuple = Domain.Task.Enum.TaskStatus.CompletedList().Select(s => Tuple.Create(_messageResourceReader.GetKeyValue(s.Name, currentUser.LanguageId), 0, GetStatusColor(s.Id))).ToList();
                List<Tuple<string, int, string>> notCompletedSubtaskStatusTuple = Domain.Task.Enum.TaskStatus.NotCompletedList().Select(s => Tuple.Create(_messageResourceReader.GetKeyValue(s.Name, currentUser.LanguageId), 0, GetStatusColor(s.Id))).ToList();
                ProjectStatisticsDto result = new()
                {
                    PendingTasks = tasks.Count(t => t.TaskStatusId == Domain.Task.Enum.TaskStatus.New.Id),
                    CompletedTasks = tasks.Count(t => t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedEarly.Id ||
                                                      t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedOnTime.Id ||
                                                      t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedLate.Id),
                    InProgressTasks = tasks.Count(t => t.TaskStatusId == Domain.Task.Enum.TaskStatus.InProgress.Id ||
                                                       t.TaskStatusId == Domain.Task.Enum.TaskStatus.InProgressWithDelay.Id),
                    TotalTasks = tasks.Count(),
                    Tasks = taskStatusTuple,
                    SubTasks = subTaskStatusTuple,
                    CompletedTasksStatistics = completedTaskStatusTuple,
                    NotCompletedTasksStatistics = notCompletedTaskStatusTuple,
                    CompletedSubTasksStatistics = completedSubtaskStatusTuple,
                    NotCompletedSubTasksStatistics = notCompletedSubtaskStatusTuple
                };
                var taskStatistics = tasks.Where(t => !t.ParentTaskId.HasValue).GroupBy(t => t.TaskStatusId).Select(t => Tuple.Create(
                    _messageResourceReader.GetKeyValue(Domain.Task.Enum.TaskStatus.From(t.Key).Name, currentUser.LanguageId),
                    t.Count(), GetStatusColor(t.Key))).ToList();
                var completedTaskStatistics = tasks.Where(t => !t.ParentTaskId.HasValue &&
                                               (t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedEarly.Id ||
                                                t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedLate.Id ||
                                                t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedOnTime.Id)).GroupBy(t => t.TaskStatusId).Select(t => Tuple.Create(
                                                    _messageResourceReader.GetKeyValue(Domain.Task.Enum.TaskStatus.From(t.Key).Name, currentUser.LanguageId),
                                                    t.Count(), GetStatusColor(t.Key))).ToList();
                var notCompletedTaskStatistics = tasks.Where(t => !t.ParentTaskId.HasValue &&
                                               (t.TaskStatusId == Domain.Task.Enum.TaskStatus.New.Id ||
                                                t.TaskStatusId == Domain.Task.Enum.TaskStatus.InProgress.Id ||
                                                t.TaskStatusId == Domain.Task.Enum.TaskStatus.InProgressWithDelay.Id)).GroupBy(t => t.TaskStatusId).Select(t => Tuple.Create(
                                                    _messageResourceReader.GetKeyValue(Domain.Task.Enum.TaskStatus.From(t.Key).Name, currentUser.LanguageId),
                                                    t.Count(), GetStatusColor(t.Key))).ToList();
                var subTaskStatistics = tasks.Where(t => t.ParentTaskId.HasValue).GroupBy(t => t.TaskStatusId).Select(t => Tuple.Create(
                    _messageResourceReader.GetKeyValue(Domain.Task.Enum.TaskStatus.From(t.Key).Name, currentUser.LanguageId),
                    t.Count(), GetStatusColor(t.Key))).ToList();
                var completedSubTaskStatistics = tasks.Where(t => t.ParentTaskId.HasValue &&
                                               (t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedEarly.Id ||
                                                t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedLate.Id ||
                                                t.TaskStatusId == Domain.Task.Enum.TaskStatus.CompletedOnTime.Id)).GroupBy(t => t.TaskStatusId).Select(t => Tuple.Create(
                                                    _messageResourceReader.GetKeyValue(Domain.Task.Enum.TaskStatus.From(t.Key).Name, currentUser.LanguageId),
                                                    t.Count(), GetStatusColor(t.Key))).ToList();
                var notCompletedSubTaskStatistics = tasks.Where(t => t.ParentTaskId.HasValue &&
                                               (t.TaskStatusId == Domain.Task.Enum.TaskStatus.New.Id ||
                                                t.TaskStatusId == Domain.Task.Enum.TaskStatus.InProgress.Id ||
                                                t.TaskStatusId == Domain.Task.Enum.TaskStatus.InProgressWithDelay.Id)).GroupBy(t => t.TaskStatusId).Select(t => Tuple.Create(
                                                    _messageResourceReader.GetKeyValue(Domain.Task.Enum.TaskStatus.From(t.Key).Name, currentUser.LanguageId),
                                                    t.Count(), GetStatusColor(t.Key))).ToList();
                taskStatistics.ForEach(resultTask =>
                {
                    var index = result.Tasks.FindIndex(t => t.Item1 == resultTask.Item1);
                    result.Tasks[index] = resultTask;
                });
                completedTaskStatistics.ForEach(resultTask =>
                {
                    var index = result.CompletedTasksStatistics.FindIndex(t => t.Item1 == resultTask.Item1);
                    result.CompletedTasksStatistics[index] = resultTask;
                });
                notCompletedTaskStatistics.ForEach(resultTask =>
                {
                    var index = result.NotCompletedTasksStatistics.FindIndex(t => t.Item1 == resultTask.Item1);
                    result.NotCompletedTasksStatistics[index] = resultTask;
                });
                subTaskStatistics.ForEach(resultTask =>
                {
                    var index = result.SubTasks.FindIndex(t => t.Item1 == resultTask.Item1);
                    result.SubTasks[index] = resultTask;
                });
                completedSubTaskStatistics.ForEach(resultTask =>
                {
                    var index = result.CompletedSubTasksStatistics.FindIndex(t => t.Item1 == resultTask.Item1);
                    result.CompletedSubTasksStatistics[index] = resultTask;
                });
                notCompletedSubTaskStatistics.ForEach(resultTask =>
                {
                    var index = result.NotCompletedSubTasksStatistics.FindIndex(t => t.Item1 == resultTask.Item1);
                    result.NotCompletedSubTasksStatistics[index] = resultTask;
                });
                if (result != null)
                {
                    List<TaskDto> tasksList = _taskRepository.MapTaskList(tasks.Where(t => !t.ParentTaskId.HasValue).ToList());
                    List<TaskDto> subtasksList = _taskRepository.MapTaskList(tasks.Where(t => t.ParentTaskId.HasValue).ToList());
                    DashboardDto dashboardDto = new();
                    dashboardDto.ProjectStatistics = result;
                    dashboardDto.Tasks = tasksList;
                    dashboardDto.Subtasks = subtasksList;
                    foreach (var task in dashboardDto.Tasks)
                    {
                        task.Members = new List<MemberDto>();
                        List<long> membersIds = await _taskMemberRepository.GetMembersUserIdList(task.Id);
                        if (membersIds?.Count > 0)
                        {
                            foreach (var memberId in membersIds)
                            {
                                UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, memberId);
                                if (userDto != null)
                                {
                                    task.Members.Add(new MemberDto
                                    {
                                        UserId = userDto.Id,
                                        FullName = userDto.FullName,
                                        ProfileImage = userDto.ProfileImage
                                    });
                                }
                            }
                        }
                    }

                    foreach (var subtask in dashboardDto.Subtasks)
                    {
                        subtask.Members = new List<MemberDto>();
                        List<long> membersIds = await _taskMemberRepository.GetMembersUserIdList(subtask.Id);
                        if (membersIds?.Count > 0)
                        {
                            foreach (var memberId in membersIds)
                            {
                                UserDto userDto = _accountService.GetUserById(new BaseRequestDto() { AuthToken = GetAccessToken() }, memberId);
                                if (userDto != null)
                                {
                                    subtask.Members.Add(new MemberDto
                                    {
                                        UserId = userDto.Id,
                                        FullName = userDto.FullName,
                                        ProfileImage = userDto.ProfileImage
                                    });
                                }
                            }
                        }
                    }
                    response.Result.ResponseData = dashboardDto;
                    response.Result.CommandMessage = "Statistics";
                }
                else
                {
                    response.Result.CommandMessage = "There are no match for your Statistics";
                }
            }
            return response;
        }

        private string GetStatusColor(int statusId)
        {
            if (statusId == Domain.Task.Enum.TaskStatus.New.Id)
            {
                return "rgba(217,255,11,0.67)"; //yellow
            }
            else if (statusId == Domain.Task.Enum.TaskStatus.InProgress.Id)
            {
                return "rgba(0,255,0,0.3)"; //green
            }
            else if (statusId == Domain.Task.Enum.TaskStatus.InProgressWithDelay.Id)
            {
                return "rgba(255,0,0,0.8)"; //red
            }
            else if (statusId == Domain.Task.Enum.TaskStatus.CompletedOnTime.Id)
            {
                return "rgba(0,255,0,0.3)"; //green
            }
            else if (statusId == Domain.Task.Enum.TaskStatus.CompletedLate.Id)
            {
                return "rgba(255,0,0,0.3)"; //red
            }
            else if (statusId == Domain.Task.Enum.TaskStatus.CompletedEarly.Id)
            {
                return "rgba(217,255,11,0.67)"; //yellow
            }
            return "rgba(77,83,96,0.2)"; //gray
        }

        private string GetAccessToken()
        {
            string apiToken = string.Empty;
            if (_httpContext.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues value))
                apiToken = value;

            return apiToken;
        }
    }
}
