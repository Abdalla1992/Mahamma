
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Identity.Domain.User.Dto;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.User.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.ListUsers
{
    public class ListUsersQueryHandler : IRequestHandler<SearchUserDto, PageList<UserDto>>
    {
        #region props
        private readonly IUserRepository _userRepository;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContext;
        #endregion

        #region ctor
        public ListUsersQueryHandler(IUserRepository userRepository, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContext)
        {
            _userRepository = userRepository;
            _httpContext = httpContext;
        }
        #endregion

        public async Task<PageList<UserDto>> Handle(SearchUserDto request, CancellationToken cancellationToken)
        {
            long userId = (long)_httpContext.HttpContext.Items["UserId"];
            PageList<UserDto> userList = new PageList<UserDto>();
            User user = await _userRepository.GetUserById(userId);
            if (user != null)
            {
                userList = await _userRepository.GetUserData(request, user.CompanyId.Value, user.Id);
            }
            return userList;
        }
    }
}
