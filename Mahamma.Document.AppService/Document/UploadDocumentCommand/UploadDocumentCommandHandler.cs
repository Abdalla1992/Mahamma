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
using Mahamma.Document.AppService.Document.Settings;
using Mahamma.Document.Domain.Document.Dto;

namespace Mahamma.Document.AppService.Document.UploadDocumentCommand
{
    public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, UploadResponse>
    {
        #region Props
        private readonly ICompanyService _companyService;
        private readonly IFileHelper _fileHelper;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UploadSetting _uploadSetting;
        private readonly UserProfileImageSetting _userProfileImageSetting;
        #endregion

        #region ctor
        public UploadDocumentCommandHandler(IFileHelper fileHelper, IHostEnvironment hostEnvironment, IHttpContextAccessor httpContext,
            ICompanyService companyService, UploadSetting uploadSetting, UserProfileImageSetting userProfileImageSetting)
        {
            _fileHelper = fileHelper;
            _hostEnvironment = hostEnvironment;
            _httpContext = httpContext;
            _companyService = companyService;
            _uploadSetting = uploadSetting;
            _userProfileImageSetting = userProfileImageSetting;
        }
        #endregion
        public async Task<UploadResponse> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
        {            
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            string authToken = _httpContext.HttpContext.Request.Headers["Authorization"];
            var company = await _companyService.GetCompanyById(currentUser.CompanyId, authToken);
            string contentRootPath = _hostEnvironment.ContentRootPath;            
            string path = string.Empty;
            string directoryName = string.Empty;
            if (company != null)
            {
                directoryName = company.FolderPath;
                path = Path.Combine(contentRootPath, "wwwroot", company.FolderPath);
            }
            else
            {
                directoryName = _userProfileImageSetting.ProfileImagePath;
                path = Path.Combine(contentRootPath, "wwwroot", _userProfileImageSetting.ProfileImagePath);
            }
            string fileSaved = await _fileHelper.SaveFileAsync(request.file, path);
            var url = new System.Uri(Path.Combine(path, fileSaved));
            UploadResponse uploadResponse = new UploadResponse
            {
                FileName = fileSaved,
                FileUrl = $"{_uploadSetting.ContentServerUrl}{directoryName}/{fileSaved}",
                ActualFileName = request.file.FileName
            };
            //if (!string.IsNullOrWhiteSpace(fileSaved))
            //{
            //    response.Result.ResponseData = fileSaved;
            //    response.Result.CommandMessage = "file Added Successfully";
            //}
            //else
            //{
            //    response.Result.CommandMessage = "Failed to add the new file. Try again shortly.";
            //}

            return uploadResponse;
        }
    }
}
