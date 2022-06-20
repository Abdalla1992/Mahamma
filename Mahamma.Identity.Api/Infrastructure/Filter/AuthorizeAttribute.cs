using Mahamma.Identity.AppService.UserRole.AuthorizeUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace Mahamma.Identity.Api.Infrastructure.Filter
{
    public class AuthorizeAttribute : IAsyncActionFilter
    {
        public int PermissionId { get; set; }
        public int PageId { get; set; }
        public IMediator Mediator { get; set; }
        public AuthorizeAttribute(IMediator mediator)
        {
            Mediator = mediator;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = (long)context.HttpContext.Items["UserId"];
            if (userId > default(long))
            {
                //context.Result = new ForbidResult(); //UnauthorizedResult();
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                AuthorizeUserCommand authorizeUserCommand = new AuthorizeUserCommand(userId, PermissionId, PageId);

                var authorized = await Mediator.Send(authorizeUserCommand);
                if (authorized != null && authorized.Result.ResponseData)
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
