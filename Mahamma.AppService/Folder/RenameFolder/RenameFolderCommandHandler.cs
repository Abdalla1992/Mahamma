using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Folder.RenameFolder
{
    public class RenameFolderCommandHandler : IRequestHandler<RenameFolderCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IFolderRepository _folderRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        #endregion

        #region Ctor
        public RenameFolderCommandHandler(IFolderRepository folderRepository, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            IMessageResourceReader messageResourceReader)
        {
            _folderRepository = folderRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
        }
        #endregion
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(RenameFolderCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            Domain.ProjectAttachment.Entity.Folder folder= await _folderRepository.GetFolderById(request.Id);
            if (folder !=null)
            {
                folder.RenameFolder(request.Name);
                _folderRepository.UpdateFolder(folder);
                if (await _folderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataUpdatedSuccessfully", currentUser.LanguageId);
                }
                else
                {
                    response.Result.CommandMessage = $"{request.Name}" + _messageResourceReader.GetKeyValue("FailedTmodify", currentUser.LanguageId);
                }
            }
            else
            {
                response.Result.CommandMessage =_messageResourceReader.GetKeyValue("NoDataFound", currentUser.LanguageId);
            }
            return response;
        }
    }
}
