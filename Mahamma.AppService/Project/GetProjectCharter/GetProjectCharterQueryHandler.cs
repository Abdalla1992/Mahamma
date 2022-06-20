using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.GetProjectCharter
{
    public class GetProjectCharterQueryHandler : IRequestHandler<GetProjectCharterQuery, ValidateableResponse<ApiResponse<ProjectCharterDto>>>
    {
        #region Prop
        private readonly IProjectCharterRepository _projectCharterRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        #region Ctor
        public GetProjectCharterQueryHandler(IProjectCharterRepository projectCharterRepository, IMessageResourceReader messageResourceReader
                                                ,IHttpContextAccessor httpContext)
        {
            _projectCharterRepository = projectCharterRepository;
            _messageResourceReader = messageResourceReader;
            _httpContext = httpContext;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<ProjectCharterDto>>> Handle(GetProjectCharterQuery request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            ValidateableResponse<ApiResponse<ProjectCharterDto>> response = new(new ApiResponse<ProjectCharterDto>());

            ProjectCharterDto projectCharterDto = await _projectCharterRepository.GetProjectCharterByProjectId(request.ProjectId);

            if (projectCharterDto != null)
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("ProcessCompletedSuccessfully", currentUser.LanguageId);
                response.Result.ResponseData = projectCharterDto;
            }
            else
            {
                response.Result.CommandMessage = _messageResourceReader.GetKeyValue("NoDataFound", currentUser.LanguageId);
            }

            return response;
        }
    }
}
