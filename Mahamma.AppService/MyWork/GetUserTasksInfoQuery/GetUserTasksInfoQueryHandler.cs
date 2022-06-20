using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.MyWork.Dto;
using Mahamma.Domain.MyWork.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.MyWork.GetUserTasksInfoQuery
{
    public class GetUserTasksInfoQueryHandler : IRequestHandler<GetUserTasksInfoQuery, ValidateableResponse<ApiResponse<UserTasksInfoDto>>>
    {
        #region Props
        private readonly IHttpContextAccessor _httpContext;
        private readonly INoteRepository _noteRepository;
        private readonly IMessageResourceReader _messageResourceReader;
        #endregion
        public GetUserTasksInfoQueryHandler(IHttpContextAccessor httpContext, INoteRepository noteRepository,
                                        IMessageResourceReader messageResourceReader)
        {
            _httpContext = httpContext;
            _noteRepository = noteRepository;
            _messageResourceReader = messageResourceReader;
        }
        public async Task<ValidateableResponse<ApiResponse<UserTasksInfoDto>>> Handle(GetUserTasksInfoQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<UserTasksInfoDto>> response = new(new ApiResponse<UserTasksInfoDto>());
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];

            try
            {
                response.Result.ResponseData = _noteRepository.GetUserTasksInfo(currentUser.Id);
            }
            catch (System.Exception ex)
            {

            }

            return response;
        }
    }
}
