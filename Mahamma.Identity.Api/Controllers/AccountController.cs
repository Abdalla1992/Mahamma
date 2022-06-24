using Mahamma.Identity.AppService.Account.Login.LoginCommand;
using Mahamma.AppService.Task.AddComment;
using Mahamma.Identity.Api.Infrastructure.Base;
using Mahamma.Identity.AppService.Account.GoogleExternalLogin;
using Mahamma.Identity.AppService.Account.GetAccount;
using Mahamma.Identity.AppService.Account.SetCompaany;
using Mahamma.Identity.AppService.Account.CompleteUserProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mahamma.Identity.AppService.Account.UpdateUserProfileStatus;
using Mahamma.Identity.AppService.Account.UpdateUserProfileSetting;
using Mahamma.Identity.AppService.Account.GetAccountList;
using Mahamma.Identity.Domain.User.Dto;
using Mahamma.Identity.AppService.Account.ForgetPassword;
using Serilog;
using Mahamma.Identity.AppService.Account.UpdateUserProfileSection;

namespace Mahamma.Identity.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route(Route.API)]
    public class AccountController : BaseController
    {

        #region CTRS
        public AccountController(IMediator mediator) : base(mediator)
        { }
        #endregion

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetMemberList(GetAccountListQuery comamnd)
        {
            return Ok(await Mediator.Send(comamnd));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleExternalLogin([FromBody] GoogleExternalLoginCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] RegistrationCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CompleteUserProfileCommand completeUserProfileCommand)
        {
            return Ok(await Mediator.Send(completeUserProfileCommand));
        }

        /// <summary>
        /// Get Workspace By Id
        /// </summary>
        /// <param name="getAccountQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            Log.Warning($"GetUserRequest: {id}");
            return Ok(await Mediator.Send(new GetAccountQuery(id)));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserForBackgroundService(int id)
        {
            return Ok(await Mediator.Send(new GetAccountQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> SetUserCompany([FromBody] SetCompanyCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUserProfileStatus([FromBody] UpdateUserProfileStatusCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpPost]
        public async Task<IActionResult> GetAll([FromBody] SearchUserDto command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateUserProfileSetting([FromBody] UpdateUserProfileSettingCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            return Ok(await Mediator.Send(new ForgetPasswordCommand(email)));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateProfileSection([FromBody] UpdateUserProfileSectionCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
