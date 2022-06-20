using Mahamma.Identity.ApiClient.Dto.Role;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Notification.AppService.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Mahamma.Notification.Api.Infrastructure.Filter
{
    public class AuthorizeAttribute : IAsyncActionFilter
    {
        public int PermissionId { get; set; }
        public int PageId { get; set; }

        private readonly IRoleService _roleService;
        private readonly AppSetting _appSetting;
        public AuthorizeAttribute(IRoleService roleService, AppSetting appSetting)
        {
            _roleService = roleService;
            _appSetting = appSetting;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = (UserDto)context.HttpContext.Items["User"];
            if (user == null)
            {
                //context.Result = new ForbidResult(); //UnauthorizedResult();
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else if (user.RoleId == _appSetting.SuperAdminRole)
            {
                // logic before action goes here
                await next(); // the actual action
                // logic after the action goes here
            }
            else
            {
                UserPermissionDto userPermissionDto = new UserPermissionDto() { PermissionId = PermissionId, PageId = PageId, UserId = user.Id };
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
