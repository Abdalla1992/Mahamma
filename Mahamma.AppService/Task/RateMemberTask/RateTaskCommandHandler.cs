using Mahamma.AppService.AtivityLogger.Task;
using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.Task.Entity;
using Mahamma.Domain.Task.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Notification.ApiClient.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.RateTask
{
    public class RateTaskCommandHandler : IRequestHandler<RateMemberTaskCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly ITaskRepository _taskRepository;
        private readonly ITaskActivityLogger _taskActivityLogger;
        private readonly ActivitesSettings _taskActivitesSettings;
        private readonly ITaskMemberRepository _taskMemberRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly INotificationService _notificationService;
        private readonly IProjectRepository _projectRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly INotificationResourceReader _notificationResourceReader;
        private readonly IAccountService _accountService;
        private readonly ITaskCommentRepository _taskCommentRepository;
        private readonly IProjectMemberRepository _projectMemberRepository;


        #endregion

        #region ctor
        public RateTaskCommandHandler(ITaskRepository taskRepository, ITaskActivityLogger taskActivityLogger,
            ActivitesSettings taskActivitesSettings, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext, ITaskMemberRepository taskMemberRepository,
           INotificationService notificationService, IProjectRepository projectRepository,
             IMessageResourceReader messageResourceReader, INotificationResourceReader notificationResourceReader,
             IAccountService accountService, ITaskCommentRepository taskCommentRepository, IProjectMemberRepository projectMemberRepository)
        {
            _taskRepository = taskRepository;
            _taskActivityLogger = taskActivityLogger;
            _taskActivitesSettings = taskActivitesSettings;
            _httpContext = httpContext;
            _taskMemberRepository = taskMemberRepository;
            _notificationService = notificationService;
            _projectRepository = projectRepository;
            _messageResourceReader = messageResourceReader;
            _notificationResourceReader = notificationResourceReader;
            _accountService = accountService;
            _taskCommentRepository = taskCommentRepository;
            _projectMemberRepository = projectMemberRepository;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(RateMemberTaskCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.Task.Entity.Task task = await _taskRepository.GetTaskById(request.TaskId, "TaskMembers,Project");
            double MaxValue = 5;
            if (task != null)
            {
                List<TaskMember> taskMemberList = task.TaskMembers.Where(tm => tm.UserId != task.CreatorUserId).ToList();
                foreach (var member in taskMemberList)
                {
                    member.Rating = request.TaskMembers?.FirstOrDefault(tm => tm.UserId == member.UserId).Rating;



                    List<ProjectMember> projectMembers = await _projectMemberRepository.GetProjectMemberByProjectId(task.ProjectId);
                    if (projectMembers != null)
                    {
                        ProjectMember oneMember = projectMembers.FirstOrDefault(pm => pm.UserId == member.UserId);
                        if(oneMember != null)
                        {

                            //List<Domain.Task.Entity.Task> tasks = await _taskRepository.GetTasksByProjectIdAndMemberId(task.ProjectId, member.UserId);
                            //int counttaskes = tasks.Count();
                            double? avergaeRate = await CalculateAveragePerMemberProject(task, member);
                            if (avergaeRate.Value > MaxValue)
                            {
                                avergaeRate = MaxValue;
                            }
                            oneMember.Rating = avergaeRate.HasValue && avergaeRate.Value > 0 ? Math.Round((double)avergaeRate, MidpointRounding.AwayFromZero) : 0;
                        }
                    }
                }
            }
            if (await _taskRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("SuccessToRateMemberTask", currentUser.LanguageId);
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToRateMemberTask", currentUser.LanguageId);
            }
            return response;
        }

        private async System.Threading.Tasks.Task<double?> CalculateAveragePerMemberProject(Domain.Task.Entity.Task task, TaskMember member)
        {
            double? averageRating = default;
            List<TaskMember> taskesMembers = await _taskRepository.GetMembersList(member.UserId, task.ProjectId);
            if (taskesMembers != null)
            {
                int counttaskes = taskesMembers.Count();
                double? ratingSumtion = taskesMembers.Sum(r => r.Rating);
                averageRating = ratingSumtion / counttaskes;
            }
            return averageRating;
        }
    }
}


