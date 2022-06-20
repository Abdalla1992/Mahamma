using Mahamma.Api.Infrastructure.Base;
using Mahamma.AppService.MyWork.AddNoteCommand;
using Mahamma.AppService.MyWork.DeleteNoteCommand;
using Mahamma.AppService.MyWork.GetAllNotesQuery;
using Mahamma.AppService.MyWork.GetUserTasksInfoQuery;
using Mahamma.AppService.MyWork.UpdateColorCommand;
using Mahamma.Base.Domain.ApiActions.Enum;
using Mahamma.Base.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mahamma.Api.Controllers
{
    [ApiController]
    [Route(Route.API)]
    public class MyWorkController : BaseController
    {
        #region CTRS
        public MyWorkController(IMediator mediator) : base(mediator) { }
        #endregion

        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.ViewWorkspace)]
        public async Task<IActionResult> GetAllNotes()
        {
            return Ok(await Mediator.Send(new GetAllNotesQuery()));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.ViewWorkspace)]
        public async Task<IActionResult> AddNote([FromBody] AddNoteCommand addNoteCommand)
        {
            return Ok(await Mediator.Send(addNoteCommand));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.ViewWorkspace)]
        public async Task<IActionResult> UpdateNoteColor([FromBody] UpdateNoteColorCommand updateNoteColorCommand)
        {
            return Ok(await Mediator.Send(updateNoteColorCommand));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.ViewWorkspace)]
        public async Task<IActionResult> DeleteNote([FromBody] DeleteNoteCommand deleteNoteCommand)
        {
            return Ok(await Mediator.Send(deleteNoteCommand));
        }

        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.ViewWorkspace)]
        public async Task<IActionResult> GetUserTasksInfo()
        {
            return Ok(await Mediator.Send(new GetUserTasksInfoQuery()));
        }
    }
}
