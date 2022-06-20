using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Folder.AddFolder
{
    public class AddFolderCommandHandler : IRequestHandler<AddFolderCommand, ValidateableResponse<ApiResponse<int>>>
    {
        private readonly IFolderRepository _folderRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IProjectRepository _projectRepository;


        public AddFolderCommandHandler(IFolderRepository folderRepository, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext,
            IMessageResourceReader messageResourceReader,IProjectRepository projectRepository)
        {
            _folderRepository = folderRepository;
            _httpContext = httpContext;
            _messageResourceReader = messageResourceReader;
            _projectRepository = projectRepository;
        }
        public async Task<ValidateableResponse<ApiResponse<int>>> Handle(AddFolderCommand request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<int>> response = new(new ApiResponse<int>());
            Domain.ProjectAttachment.Entity.Folder folder = new();
            Domain.Project.Entity.Project project=await _projectRepository.GetProjectById(request.ProjectId);
            if (project !=null)
            {
                folder.CreateFolder(request.Name, request.ProjectId, request.TaskId);
                _folderRepository.AddFolder(folder);
                if (await _folderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = folder.Id;
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataAddededSuccessfully", currentUser.LanguageId);
                }
                else
                {
                    response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToAddTheNewData", currentUser.LanguageId);
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
