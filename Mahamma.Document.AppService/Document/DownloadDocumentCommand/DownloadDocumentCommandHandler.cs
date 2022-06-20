using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Document.AppService.Document.Helper;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Mahamma.Document.Domain.Document.Dto;
using Mahamma.Identity.ApiClient.Dto.User;
using Microsoft.AspNetCore.Http;
using Mahamma.ApiClient.Interface;

namespace Mahamma.Document.AppService.Document.DownloadDocumentCommand
{
    public class DownloadDocumentCommandHandler : IRequestHandler<DownloadDocumentCommand, ValidateableResponse<ApiResponse<DocumentDto>>>
    {
        #region Props
        private readonly ICompanyService _companyService;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IFileHelper _fileHelper;
        private readonly IHostEnvironment _hostEnvironment;
        #endregion

        #region ctor
        public DownloadDocumentCommandHandler(IFileHelper fileHelper, IHostEnvironment hostEnvironment, IHttpContextAccessor httpContext, ICompanyService companyService)
        {
            _fileHelper = fileHelper;
            _hostEnvironment = hostEnvironment;
            _httpContext = httpContext;
            _companyService = companyService;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<DocumentDto>>> Handle(DownloadDocumentCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<DocumentDto>> response = new(new ApiResponse<DocumentDto>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            string authToken = _httpContext.HttpContext.Request.Headers["Authorization"];
            var company = await _companyService.GetCompanyById(currentUser.CompanyId, authToken);
            if (company != null)
            {
                string contentRootPath = _hostEnvironment.ContentRootPath;
                string filePath = Path.Combine(contentRootPath, "wwwroot", company.FolderPath, request.FileName);

                if (File.Exists(filePath))
                {
                    response.Result.ResponseData = new DocumentDto
                    {
                        ContentType = _fileHelper.GetContentType(filePath),
                        FileDownloadName = Path.GetFileName(filePath),
                        MemoryStream = await _fileHelper.GetFileStream(filePath),
                    };
                    response.Result.CommandMessage = "file is downloading";
                }
                else
                {
                    response.Result.CommandMessage = "Failed to download the file. Try again shortly.";
                }
            }
            else
            {
                response.Result.CommandMessage = "Failed to download the file. Try again shortly.";
            }
            return response;
        }
    }
}
