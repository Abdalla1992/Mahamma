using Mahamma.Document.Api.Infrastructure.Base;
using Mahamma.Document.AppService.Document.UploadDocumentCommand;
using Mahamma.Document.AppService.Document.DownloadDocumentCommand;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mahamma.Document.AppService.Document.Helper;
using Mahamma.Document.AppService.Document.DeleteDocumentCommand;
using Mahamma.Base.Domain.ApiActions.Enum;

namespace Mahamma.Document.Api.Controllers
{
    [ApiController]
    [Route(Route.API)]
    public class DocumentController : BaseController
    {
        public IFileHelper _fileHelper { get; set; }

        #region CTRS
        public DocumentController(IMediator mediator, IFileHelper fileHelper)
            : base(mediator)
        {
            _fileHelper = fileHelper;
        }
        #endregion

        [HttpPost]
        //[Infrastructure.Filter.AuthorizeAttributeFactory(PermissionId = (int)Permission.UploadDocument)]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            return Ok(await Mediator.Send(new UploadDocumentCommand { file = file }));
        }

        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PermissionId = (int)Permission.DownloadDocument)]
        public async Task<IActionResult> Download(string fileName)
        {
            var result = await Mediator.Send(new DownloadDocumentCommand { FileName = fileName });
            if (result.IsValidResponse)
            {
                return File(result.Result.ResponseData.MemoryStream, result.Result.ResponseData.ContentType, result.Result.ResponseData.FileDownloadName);
            }
            return NotFound();
        }
        [HttpDelete]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PermissionId = (int)Permission.DeleteDocument)]
        public async Task<IActionResult> Delete(string fileName)
        {
            return Ok(await Mediator.Send(new DeleteDocumentCommand { fileName = fileName }));
        }
    }
}
