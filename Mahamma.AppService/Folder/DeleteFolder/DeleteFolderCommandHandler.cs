using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Folder.DeleteFolder
{
    public class DeleteFolderCommandHandler : IRequestHandler<DeleteFolderCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        private readonly IFolderRepository _folderRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IProjectRepository _projectRepository;


        public DeleteFolderCommandHandler(IFolderRepository folderRepository, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            IMessageResourceReader messageResourceReader,IProjectRepository projectRepository)
        {
            _folderRepository = folderRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            _projectRepository = projectRepository;
        }
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteFolderCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            Domain.ProjectAttachment.Entity.Folder folder = await _folderRepository.GetFolderById(request.FolderId, "FolderFiles,FolderFiles.ProjectAttachment");
            if (folder != null)
            {
                folder.DeleteFolder();
                if (await _folderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Data Deleted Successfully";
                }
                else
                {
                    response.Result.CommandMessage = "Failed To Delete Data";
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
