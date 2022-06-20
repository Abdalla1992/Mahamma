using Mahamma.Api.Infrastructure.Base;
using Mahamma.AppService.Folder.AddFolder;
using Mahamma.AppService.Folder.DeleteFolder;
using Mahamma.AppService.Folder.GetFolder;
using Mahamma.AppService.Folder.MoveFile;
using Mahamma.AppService.Folder.RenameFolder;
using Mahamma.Domain.ProjectAttachment.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mahamma.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route(Route.API)]
    public class FolderController : BaseController
    {
        #region Ctor
        public FolderController(IMediator mediator) : base(mediator)
        { }
        #endregion

        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] SearchFolderDto listFolderCommand)
        {
            return Ok(await Mediator.Send(listFolderCommand));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetFolderCommand(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddFolderCommand addFolderCommand)
        {
            return Ok(await Mediator.Send(addFolderCommand));
        }

        [HttpPut]
        public async Task<IActionResult> Rename([FromBody] RenameFolderCommand renameFolderCommand )
        {
            return Ok(await Mediator.Send(renameFolderCommand));
        }

        [HttpPut]
        public async Task<IActionResult> MoveFile([FromBody] MoveFileCommand moveFileCommand)
        {
            return Ok(await Mediator.Send(moveFileCommand));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteFolderCommand(id)));
        }


    }
}
