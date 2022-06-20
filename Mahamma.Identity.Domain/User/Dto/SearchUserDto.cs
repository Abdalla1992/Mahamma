using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Request;
using MediatR;

namespace Mahamma.Identity.Domain.User.Dto
{
    public class SearchUserDto : SearchDto<UserDto>, IRequest<PageList<UserDto>>
    {
    }
}
