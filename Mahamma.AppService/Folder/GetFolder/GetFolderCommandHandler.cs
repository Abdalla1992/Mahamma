using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.ProjectAttachment.Dto;
using Mahamma.Domain.ProjectAttachment.Entity;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Folder.GetFolder
{
    public class GetFolderCommandHandler : IRequestHandler<GetFolderCommand, ValidateableResponse<ApiResponse<FolderDto>>>
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IFolderFileRepository _folderFileRepository;
        private readonly IProjectAttachmentRepository _projectAttachmentRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;

        public GetFolderCommandHandler(IFolderRepository folderRepository, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            IMessageResourceReader messageResourceReader, IProjectAttachmentRepository projectAttachmentRepository, IFolderFileRepository folderFileRepository)
        {
            _folderRepository = folderRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            _projectAttachmentRepository = projectAttachmentRepository;
            _folderFileRepository = folderFileRepository;
        }
        public async Task<ValidateableResponse<ApiResponse<FolderDto>>> Handle(GetFolderCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<FolderDto>> response = new(new ApiResponse<FolderDto>());
            FolderDto folder = await _folderRepository.GetById(request.FolderId);
            if (folder != null)
            {
                response.Result.ResponseData = folder;
                response.Result.CommandMessage = "Folder Found.";
            }
            else
            {
                response.Result.CommandMessage = "No Folder Found.";
            }

            return response;
        }
    }
}
