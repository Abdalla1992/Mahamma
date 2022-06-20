using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.ProjectAttachment.Entity;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Folder.MoveFile
{
    public class MoveFileCommandHandler : IRequestHandler<MoveFileCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IFolderFileRepository _folderFileRepository;
        private readonly IProjectAttachmentRepository _projectAttachmentRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;

        public MoveFileCommandHandler(IFolderRepository folderRepository, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            IMessageResourceReader messageResourceReader, IProjectAttachmentRepository projectAttachmentRepository, IFolderFileRepository folderFileRepository)
        {
            _folderRepository = folderRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            _projectAttachmentRepository = projectAttachmentRepository;
            _folderFileRepository = folderFileRepository;
        }
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(MoveFileCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            var file = await _projectAttachmentRepository.GetEntityById(request.ProjectFileId);
            if (file != null)
            {
                if(request.OldFolderId != null)
                {
                    file.MoveFile((int)request.OldFolderId, request.NewFolderId);
                    _projectAttachmentRepository.UpdateProject(file);
                }
                else
                {
                    FolderFile folderFile = new();
                    folderFile.CreateFolderFile(request.NewFolderId, request.ProjectFileId);
                    _folderFileRepository.AddFolderFile(folderFile);
                }
                if (await _folderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "folder added successfuly.";
                }
                else
                {
                    response.Result.CommandMessage = "failed to add folder.";
                }
            }
            else
            {
                response.Result.CommandMessage = "No Project Found.";
            }

            return response;
        }
    }
}
