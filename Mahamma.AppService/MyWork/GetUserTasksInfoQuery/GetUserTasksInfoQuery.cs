using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.MyWork.Dto;
using MediatR;

namespace Mahamma.AppService.MyWork.GetUserTasksInfoQuery
{
    public class GetUserTasksInfoQuery : IRequest<ValidateableResponse<ApiResponse<UserTasksInfoDto>>>
    {
    }
}
