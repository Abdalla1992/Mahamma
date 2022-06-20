using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.DeleteProjectRiskPlan
{
    public class DeleteProjectRiskPlanCommandHandler : IRequestHandler<DeleteProjectRiskPlanCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IProjectRiskPlanRepository _projectRiskPlanRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region Ctor
        public DeleteProjectRiskPlanCommandHandler(IProjectRiskPlanRepository projectRiskPlanRepository, IMessageResourceReader messageResourceReader
                                                    ,IHttpContextAccessor httpContext)
        {
            _projectRiskPlanRepository = projectRiskPlanRepository;
            _messageResourceReader = messageResourceReader;
            _httpContext = httpContext;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(DeleteProjectRiskPlanCommand request, CancellationToken cancellationToken)
        {
            UserDto currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());

            _projectRiskPlanRepository.DeleteProjectRiskPlan(request.Id);

            if (await _projectRiskPlanRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
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
