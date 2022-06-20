using Mahamma.Api.Infrastructure.Base;
using Mahamma.AppService.Task.AddComment;
using Mahamma.AppService.Task.AddSubTask;
using Mahamma.AppService.Task.AddTask;
using Mahamma.AppService.Task.AddTaskFile;
using Mahamma.AppService.Task.ArchiveTask;
using Mahamma.AppService.Task.ArchiveTaskList;
using Mahamma.AppService.Task.AssignMember;
using Mahamma.AppService.Task.DeleteComment;
using Mahamma.AppService.Task.DeleteTask;
using Mahamma.AppService.Task.DeleteTaskFile;
using Mahamma.AppService.Task.DeleteTaskList;
using Mahamma.AppService.Task.GetTask;
using Mahamma.AppService.Task.GetUserTaskRejected;
using Mahamma.AppService.Task.GetUserTasks;
using Mahamma.AppService.Task.LikeComment;
using Mahamma.AppService.Task.ListTaskFiles;
using Mahamma.AppService.Task.ListTaskLogs;
using Mahamma.AppService.Task.RateTask;
using Mahamma.AppService.Task.ReviewTask;
using Mahamma.AppService.Task.SubmitTask;
using Mahamma.AppService.Task.SubtaskDDL;
using Mahamma.AppService.Task.TaskDDL;
using Mahamma.AppService.Task.UpdateTask;
using Mahamma.AppService.Task.UpdateTaskProgressPercentage;
using Mahamma.Base.Domain.ApiActions.Enum;
using Mahamma.Base.Domain.Enum;
using Mahamma.Domain.Task.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mahamma.Api.Controllers
{
    [ApiController]
    [Route(Route.API)]
    public class TaskController : BaseController
    {
        #region CTRS
        public TaskController(IMediator mediator)
            : base(mediator)
        { }
        #endregion

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewTask)]
        public async Task<IActionResult> GetAll([FromBody] SearchTaskDto listTaskCommand)
        {
            return Ok(await Mediator.Send(listTaskCommand));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.AddTask)]
        public async Task<IActionResult> Add([FromBody] AddTaskCommand addTaskCommand)
        {
            return Ok(await Mediator.Send(addTaskCommand));
        }

        /// <summary>
        /// Get Task By Id
        /// </summary>
        /// <param name="getTaskQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.ViewTask)]
        public async Task<IActionResult> Get(int id, bool requestedFromMeeting = false)
        {
            return Ok(await Mediator.Send(new GetTaskQuery(id, requestedFromMeeting)));
        }


        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.AddSubTask)]
        public async Task<IActionResult> AddSub([FromBody] AddSubTaskCommand addSubTaskCommand)
        {
            return Ok(await Mediator.Send(addSubTaskCommand));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.SubmitTask)]
        public async Task<IActionResult> Submit([FromBody] SubmitTaskCommand submitTaskCommand)
        {
            return Ok(await Mediator.Send(submitTaskCommand));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.SubmitTask)]
        public async Task<IActionResult> Review([FromBody] ReviewTaskCommand reviewTaskCommand)
        {
            return Ok(await Mediator.Send(reviewTaskCommand));
        }

        [HttpPut]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.UpdateTask)]
        public async Task<IActionResult> Update([FromBody] UpdateTaskCommand updateTaskCommand)
        {
            return Ok(await Mediator.Send(updateTaskCommand));
        }

        [HttpDelete]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.DeleteTask)]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteTaskCommand(id)));
        }

        [HttpDelete]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.DeleteTask)]
        public async Task<IActionResult> DeleteList([FromBody] DeleteTaskListCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpDelete]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.ArchiveTask)]
        public async Task<IActionResult> Archive(int id)
        {
            return Ok(await Mediator.Send(new ArchiveTaskCommand(id)));
        }

        [HttpPut]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.ArchiveTask)]
        public async Task<IActionResult> ArchiveList([FromBody] ArchiveTaskListCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.ViewFile)]
        public async Task<IActionResult> GetFiles(int id)
        {
            return Ok(await Mediator.Send(new ListTaskFilesQuery(id)));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.AddFile)]
        public async Task<IActionResult> AddFile([FromBody] AddTaskFileCommand addTaskFileCommand)
        {
            return Ok(await Mediator.Send(addTaskFileCommand));
        }

        [HttpDelete]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.DeleteFile)]
        public async Task<IActionResult> DeleteFile([FromBody] DeleteTaskFileCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetActivities(int id)
        {
            return Ok(await Mediator.Send(new ListTaskLogsQuery(id)));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.AssignMember)]
        public async Task<IActionResult> AssignMember([FromBody] AssignMemberCommand assignMemberCommand)
        {
            return Ok(await Mediator.Send(assignMemberCommand));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.AddComment)]
        public async Task<IActionResult> Comment([FromBody] AddCommentCommand addCommentCommand)
        {
            return Ok(await Mediator.Send(addCommentCommand));
        }

        [HttpDelete]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.AddComment)]
        public async Task<IActionResult> DeleteComment([FromBody] DeleteCommentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.AddComment)]
        public async Task<IActionResult> LikeComment([FromBody] LikeCommentCommand likeCommentCommand)
        {
            return Ok(await Mediator.Send(likeCommentCommand));
        }
        [HttpGet]
        public async Task<IActionResult> GetUserTasks(long userId)
        {
            return Ok(await Mediator.Send(new GetUserTasksCommand(userId)));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.UpdateTask)]
        public async Task<IActionResult> UpdateProgressPercentage([FromBody] UpdateTaskProgressPercentageCommand updateTaskProgressPercentageCommand)
        {
            return Ok(await Mediator.Send(updateTaskProgressPercentageCommand));
        }

        [HttpGet]
        public async Task<IActionResult> GetTsaksDDL(int projectId)
        {
            return Ok(await Mediator.Send(new TaskDDLQuery(projectId)));
        }
        [HttpGet]
        public async Task<IActionResult> GetSubtsaksDDL(int taskId)
        {
            return Ok(await Mediator.Send(new SubtaskDDLQuery(taskId)));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTaskRejected(long userId)
        {
            return Ok(await Mediator.Send(new GetUserTaskRejectedCommand(userId)));
        }

        [HttpPost]
        public async Task<IActionResult> RateMemberTask([FromBody] RateMemberTaskCommand rateMemberTaskCommand)
        {
            return Ok(await Mediator.Send(rateMemberTaskCommand));
        }
    }
}
