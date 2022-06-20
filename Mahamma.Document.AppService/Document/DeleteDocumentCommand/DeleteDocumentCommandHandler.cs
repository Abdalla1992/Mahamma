using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Document.AppService.Document.Helper;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.ApiClient.Interface;

namespace Mahamma.Document.AppService.Document.DeleteDocumentCommand
{
    public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly ICompanyService _companyService;
        private readonly IFileHelper _fileHelper;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region ctor
        public DeleteDocumentCommandHandler(IFileHelper fileHelper, IHostEnvironment hostEnvironment, IHttpContextAccessor httpContext, ICompanyService companyService)
        {
            _fileHelper = fileHelper;
            _hostEnvironment = hostEnvironment;
            _httpContext = httpContext;
            _companyService = companyService;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            string authToken = _httpContext.HttpContext.Request.Headers["Authorization"];
            var company = await _companyService.GetCompanyById(currentUser.CompanyId, authToken);
            if (company != null)
            {
                string contentRootPath = _hostEnvironment.ContentRootPath;
                string path = Path.Combine(contentRootPath, "wwwroot", company.FolderPath, request.fileName);
                bool fileDeleted = _fileHelper.DeleteFile(path);
                if (fileDeleted)
                {
                    response.Result.ResponseData = fileDeleted;
                    response.Result.CommandMessage = "file Deleted Successfully";
                }
                else
                {
                    response.Result.CommandMessage = "Failed to delete the file. Try again shortly.";
                }
            }
            else
            {
                response.Result.CommandMessage = "Failed to delete the file. Try again shortly.";
            }
            return response;
        }
    }
}
