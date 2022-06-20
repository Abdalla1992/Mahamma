using Mahamma.Api.Infrastructure.Base;
using Mahamma.AppService.Workspace.AssignMember;
using Mahamma.AppService.Workspace.AddWorkspace;
using Mahamma.AppService.Workspace.DeleteWorkspace;
using Mahamma.AppService.Workspace.GetWorkspace;
using Mahamma.AppService.Workspace.UpdateWorkspace;
using Mahamma.Base.Domain.ApiActions.Enum;
using Mahamma.Base.Domain.Enum;
using Mahamma.Domain.Workspace.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mahamma.AppService.Workspace.DeleteWorkspaceMember;

namespace Mahamma.Api.Controllers
{
    [ApiController]
    [Route(Route.API)]
    public class WorkspaceController : BaseController
    {
        #region CTRS
        public WorkspaceController(IMediator mediator)
            : base(mediator)
        { }
        #endregion

        /// <summary>
        /// Get Workspace list data
        /// </summary>
        /// <param name="listWorkspaceCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.ViewWorkspace)]
        public async Task<IActionResult> GetAll([FromBody] SearchWorkspaceDto listWorkspaceCommand)
        {
            return Ok(await Mediator.Send(listWorkspaceCommand));
        }

        /// <summary>
        /// Add Workspace data
        /// </summary>
        /// <param name="addWorkspaceCommand"></param>
        /// <returns></returns>
        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.AddWorkspace)]
        public async Task<IActionResult> Add([FromBody] AddWorkspaceCommand addWorkspaceCommand)
        {
            return Ok(await Mediator.Send(addWorkspaceCommand));
        }

        /// <summary>
        /// Get Workspace By Id
        /// </summary>
        /// <param name="getWorkspaceQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.ViewWorkspace)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetWorkspaceQuery(id)));
        }

        /// <summary>
        /// Update Workspace data
        /// </summary>
        /// <param name="updateWorkspaceCommand"></param>
        /// <returns></returns>
        [HttpPut]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.UpdateWorkspace)]
        public async Task<IActionResult> Update([FromBody] UpdateWorkspaceCommand updateWorkspaceCommand)
        {
            return Ok(await Mediator.Send(updateWorkspaceCommand));
        }

        /// <summary>
        /// Delete Workspace
        /// </summary>
        /// <param name="deleteWorkspaceCommand"></param>
        /// <returns></returns>
        [HttpDelete]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.DeleteWorkspace)]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteWorkspaceCommand(id)));
        }


        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.AssignMember)]
        public async Task<IActionResult> AssigWorkSpacenMember([FromBody] AssignWorkSpaceMemberCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        /// <summary>
        /// Remove workspace member
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="workspaceId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.DeleteWorkspace)]
        public async Task<IActionResult> RemoveWSMember(long userId, int workspaceId)
        {
            return Ok(await Mediator.Send(new DeleteWorkspaceMemberCommand(userId, workspaceId)));
        }
    }
}
