using Mahamma.Identity.ApiClient.Dto.Role;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Mahamma.Document.Api.Infrastructure.Filter
{
    public class AuthorizeAttribute : IAsyncActionFilter
    {
        public int PermissionId { get; set; }
        private readonly IRoleService _roleService;
        public AuthorizeAttribute(IRoleService roleService)
        {
            _roleService = roleService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = (UserDto)context.HttpContext.Items["User"];
            if (user == null)
            {
                //context.Result = new ForbidResult(); //UnauthorizedResult();
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                UserPermissionDto userPermissionDto = new UserPermissionDto() { PermissionId = PermissionId, UserId = user.Id };
                bool authorized = await _roleService.AuthorizeUser(userPermissionDto);
                if (authorized)
                {
                    // logic before action goes here
                    await next(); // the actual action
                    // logic after the action goes here
                }
                else
                {
                    //context.Result = new ForbidResult(); //UnauthorizedResult();
                    context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
        }
    }
}
