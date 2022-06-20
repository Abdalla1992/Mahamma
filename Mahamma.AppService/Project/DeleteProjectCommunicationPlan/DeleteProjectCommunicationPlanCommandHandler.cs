using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.DeleteProjectCommunicationPlan
{
    public class DeleteProjectCommunicationPlanCommandHandler : IRequestHandler<DeleteProjectCommunicationPlanCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IProjectCommunicationPlanRepository _projectCommunicationPlanRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region Ctor
        public DeleteProjectCommunicationPlanCommandHandler(IProjectCommunicationPlanRepository projectCommunicationPlanRepository, IMessageResourceReader messageResourceReader
                                                    , IHttpContextAccessor httpContext)
        {
            _projectCommunicationPlanRepository = projectCommunicationPlanRepository;
            _messageResourceReader = messageResourceReader;
            _httpContext = httpContext;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteProjectCommunicationPlanCommand request, CancellationToken cancellationToken)
        {
            UserDto currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            _projectCommunicationPlanRepository.DeleteProjectCommunicationPlan(request.Id);

            if (await _projectCommunicationPlanRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
            {
                response.Result.ResponseData = true;
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("DataSavedSuccessfully", currentUser.LanguageId);
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("FailedToUpdateData", currentUser.LanguageId);
            }

            return response;
        }
    }
}
