using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Request;
using MediatR;

namespace Mahamma.Domain.Task.Dto
{
    public class SearchTaskDto : SearchDto<TaskDto>, IRequest<PageList<TaskDto>>
    {
    }
}
