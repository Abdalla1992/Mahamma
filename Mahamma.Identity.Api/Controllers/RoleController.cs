using Mahamma.Identity.Api.Infrastructure.Base;
using Mahamma.Identity.AppService.Role.AddRole;
using Mahamma.Identity.AppService.Role.DeleteRole;
using Mahamma.Identity.AppService.Role.UpdateRole;
using Mahamma.Identity.AppService.UserRole.AuthorizeUser;
using Mahamma.Identity.AppService.UserRole.GetUserPermission;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mahamma.Identity.AppService.PagePermissionByRole.GetPagePermissionByRole;
using Mahamma.Identity.AppService.Role.SetCompanyBasicRoles;
using Mahamma.Identity.AppService.Role.GetAllRoles;
using Mahamma.Identity.AppService.Role.GetAllPagePermission;
using Mahamma.Identity.AppService.Role.GetRole;

namespace Mahamma.Identity.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route(Route.API)]
    public class RoleController : BaseController
    {
        #region CTRS
        public RoleController(IMediator mediator) : base(mediator)
        { }
        #endregion

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AuthorizeUserPermission([FromBody] AuthorizeUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        public async Task<IActionResult> GetUserPagePermissions([FromBody] GetUserPermissionQuery command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SetCompanyBasicRoles([FromBody] SetCompanyBasicRolesCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanyRoles()
        {
            return Ok(await Mediator.Send(new GetAllRolesCommand()));
        }

        [HttpGet]
        public async Task<IActionResult> GetRole(int id)
        {
            return Ok(await Mediator.Send(new GetRoleCommand(id)));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add([FromBody] AddRoleCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromBody] UpdateRoleCommand updateRoleCommand)
        {
            return Ok(await Mediator.Send(updateRoleCommand));
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteRoleCommand(id)));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPagePermission()
        {
            return Ok(await Mediator.Send(new GetAllPagePermissionCommand()));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetPagePermissionByRoleId([FromBody] GetPagePermissionByRoleCommand getPagePermissionByRoleCommand)
        {
            return Ok(await Mediator.Send(getPagePermissionByRoleCommand));
        }
    }
}
