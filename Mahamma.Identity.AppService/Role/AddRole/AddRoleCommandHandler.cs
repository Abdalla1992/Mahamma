using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Identity.Domain.Role.Repository;
using Mahamma.Identity.Domain.User.Dto;
using Mahamma.Identity.Domain.User.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.AddRole
{
    class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        private readonly IRoleRepository _roleRepository;
        private readonly RoleManager<Domain.Role.Entity.Role> _roleManager;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserRepository _userRepository;
        #endregion

        #region Ctor
        public AddRoleCommandHandler(IRoleRepository roleRepository, RoleManager<Domain.Role.Entity.Role> roleManager,
            IHttpContextAccessor httpContext, IUserRepository userRepository)
        {
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _httpContext = httpContext;
            _userRepository = userRepository;
        }
        #endregion

        #region Method
        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            long currentUserId = (long)_httpContext.HttpContext.Items["UserId"];
            UserDto userDto = await _userRepository.GetById(currentUserId);
            Domain.Role.Entity.Role role = new();
            if (userDto != null)
            {
                role.CreateRole(request.Name, (int)userDto.CompanyId);

                if ((await _roleManager.CreateAsync(role)).Succeeded)
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Role Saved Succefuly";
                }
                else
                {
                    response.Result.CommandMessage = "Failed to add the new Role. Try again shortly.";
                }
            }
            return response;
        }
        #endregion
    }
}
