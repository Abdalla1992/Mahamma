using Mahamma.Api.Infrastructure.Base;
using Mahamma.AppService.Project.AddProject;
using Mahamma.AppService.Project.AddProjectFile;
using Mahamma.AppService.Project.ArchiveProject;
using Mahamma.AppService.Project.AssignMember;
using Mahamma.AppService.Project.DeleteProject;
using Mahamma.AppService.Project.DeleteProjectFile;
using Mahamma.AppService.Project.GetProject;
using Mahamma.AppService.Project.GetProjectData;
using Mahamma.AppService.Project.ListProjectActivity;
using Mahamma.AppService.Project.ListProjectFile;
using Mahamma.AppService.Project.UpdateProject;
using Mahamma.Base.Domain.Enum;
using Mahamma.Base.Domain.ApiActions.Enum;
using Mahamma.Domain.Project.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mahamma.AppService.Project.GetProjectTaskSubtaskNames;
using Mahamma.Domain.ProjectAttachment.Dto;
using Mahamma.AppService.Project.AddProjectComment;
using Mahamma.AppService.Project.LikeProjectComment;
using Mahamma.AppService.Project.DeleteProjectComment;
using Mahamma.AppService.Project.GetProjectCharter;
using Mahamma.AppService.Project.UpdateProjectCharterCommand;
using Mahamma.AppService.Project.AddProjectRiskPlan;
using Mahamma.AppService.Project.UpdateProjectRiskPlan;
using Mahamma.AppService.Project.GetProjectRiskPlans;
using Mahamma.AppService.Project.DeleteProjectRiskPlan;
using Mahamma.AppService.Project.GetProjectCommunicationPlans;
using Mahamma.AppService.Project.AddProjectCommunicationPlan;
using Mahamma.AppService.Project.UpdateProjectCommunicationPlan;
using Mahamma.AppService.Project.DeleteProjectCommunicationPlan;

namespace Mahamma.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route(Route.API)]
    public class ProjectController : BaseController
    {
        #region Ctor
        public ProjectController(IMediator mediator) : base(mediator)
        { }
        #endregion

        #region Methods

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.AddProject)]
        public async Task<IActionResult> Add([FromBody] AddProjectCommand addProjectCommand)
        {
            return Ok(await Mediator.Send(addProjectCommand));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> GetAll([FromBody] SearchProjectDto listProjectCommand)
        {
            return Ok(await Mediator.Send(listProjectCommand));
        }

        [HttpGet]

        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> Get(int id, bool requestedFromMeeting = false)
        {
            return Ok(await Mediator.Send(new GetProjectQuery(id, requestedFromMeeting)));
        }

        [HttpPut]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.UpdateProject)]
        public async Task<IActionResult> Update([FromBody] UpdateProjectCommand updateProjectCommand)
        {
            return Ok(await Mediator.Send(updateProjectCommand));
        }

        [HttpDelete]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.DeleteProject)]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteProjectCommand(id)));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.AssignMember)]
        public async Task<IActionResult> AssignMember([FromBody] AssignMemberProjectCommand assignMemberCommand)
        {
            return Ok(await Mediator.Send(assignMemberCommand));
        }


        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> GetProjectData(int id)
        {
            return Ok(await Mediator.Send(new GetProjectDataQuery(id)));
        }


        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.AddFile)]
        public async Task<IActionResult> AddProjectFile([FromBody] AddProjectFileCommand addProjectFileCommand)
        {
            return Ok(await Mediator.Send(addProjectFileCommand));
        }


        [HttpDelete]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.DeleteFile)]
        public async Task<IActionResult> DeleteProjectFile(int id)
        {
            return Ok(await Mediator.Send(new DeleteProjectFileCommand(id)));
        }

        [HttpDelete]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ArchiveProject)]
        public async Task<IActionResult> Archive(int id)
        {
            return Ok(await Mediator.Send(new ArchiveProjectCommand(id)));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewFile)]
        public async Task<IActionResult> GetProjectFiles(SearchProjectAttachmentDto searchProjectAttachmentDto)
        {
            return Ok(await Mediator.Send(searchProjectAttachmentDto));
        }


        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> GetProjectActivities(int id)
        {
            return Ok(await Mediator.Send(new ListProjectActivityQuery(id)));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewFile)]
        public async Task<IActionResult> GetProjectTaskSubtaskName([FromBody] GetProjectTaskSubtaskNamesQuery getProjectTaskSubtaskNamesQuery)
        {
            return Ok(await Mediator.Send(getProjectTaskSubtaskNamesQuery));
        }

        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.DashboardProfile, PermissionId = (int)Permission.ViewCharts)]
        public async Task<IActionResult> GetUserProjectList()
        {
            return Ok(await Mediator.Send(new GetUserProjectsQuery()));
        }

        [HttpPost]
        //[Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.AddComment)]
        public async Task<IActionResult> AddComment([FromBody] AddProjectCommentCommand addProjectCommentCommand)
        {
            return Ok(await Mediator.Send(addProjectCommentCommand));
        }

        [HttpDelete]
        //[Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.AddComment)]
        public async Task<IActionResult> DeleteComment([FromBody] DeleteProjectCommentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        //[Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.TaskProfile, PermissionId = (int)Permission.AddComment)]
        public async Task<IActionResult> LikeComment([FromBody] LikeProjectCommentCommand likeProjectCommentCommand)
        {
            return Ok(await Mediator.Send(likeProjectCommentCommand));
        }

        /// <summary>
        /// Get Project Charter
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> GetProjectCharter(int projectId)
        {
            return Ok(await Mediator.Send(new GetProjectCharterQuery(projectId)));
        }

        /// <summary>
        /// Update Project Charter
        /// </summary>
        /// <param name="UpdateProjectCharterCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> UpdateProjectCharter([FromBody] UpdateProjectCharterCommand updateProjectCharterCommand)
        {
            return Ok(await Mediator.Send(updateProjectCharterCommand));
        }

        /// <summary>
        /// Get Project Risk Plans
        /// </summary>
        /// <param name="GetAllProjectRiskPlans"></param>
        /// <returns></returns>
        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> GetAllProjectRiskPlans(int projectId)
        {
            return Ok(await Mediator.Send(new GetProjectRiskPlansQuery(projectId)));
        }
        /// <summary>
        /// Add Project Risk Plan
        /// </summary>
        /// <param name="AddProjectRiskPlan"></param>
        /// <returns></returns>
        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> AddProjectRiskPlan([FromBody] AddProjectRiskPlanCommand addProjectRiskPlanCommand)
        {
            return Ok(await Mediator.Send(addProjectRiskPlanCommand));
        }
        /// <summary>
        /// Update Project Risk Plan
        /// </summary>
        /// <param name="UpdateProjectRiskPlan"></param>
        /// <returns></returns>
        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> UpdateProjectRiskPlan([FromBody] UpdateProjectRiskPlanCommand updateProjectRiskPlanCommand)
        {
            return Ok(await Mediator.Send(updateProjectRiskPlanCommand));
        }
        /// <summary>
        /// Delete Project Risk Plan
        /// </summary>
        /// <param name="DeleteProjectRiskPlan"></param>
        /// <returns></returns>
        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> DeleteProjectRiskPlan(int planId)
        {
            return Ok(await Mediator.Send(new DeleteProjectRiskPlanCommand(planId)));
        }

        /// <summary>
        /// Get Project Communication Plans
        /// </summary>
        /// <param name="GetAllProjectCommunicationPlans"></param>
        /// <returns></returns>
        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> GetAllProjectCommunicationPlans(int projectId)
        {
            return Ok(await Mediator.Send(new GetProjectCommunicationPlansQuery(projectId)));
        }
        /// <summary>
        /// Add Project Communication Plan
        /// </summary>
        /// <param name="AddProjectCommunicationPlan"></param>
        /// <returns></returns>
        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> AddProjectCommunicationPlan([FromBody] AddProjectCommunicationPlanCommand addProjectCommunicationPlanCommand)
        {
            return Ok(await Mediator.Send(addProjectCommunicationPlanCommand));
        }
        /// <summary>
        /// Update Project Communication Plan
        /// </summary>
        /// <param name="UpdateProjectCommunicationPlan"></param>
        /// <returns></returns>
        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> UpdateProjectCommunicationPlan([FromBody] UpdateProjectCommunicationPlanCommand updateProjectCommunicationPlanCommand)
        {
            return Ok(await Mediator.Send(updateProjectCommunicationPlanCommand));
        }
        /// <summary>
        /// Delete Project Communication Plan
        /// </summary>
        /// <param name="DeleteProjectCommunicationPlan"></param>
        /// <returns></returns>
        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> DeleteProjectCommunicationPlan(int planId)
        {
            return Ok(await Mediator.Send(new DeleteProjectCommunicationPlanCommand(planId)));
        }
        #endregion
    }
}
