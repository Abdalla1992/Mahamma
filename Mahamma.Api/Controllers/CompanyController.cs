using Mahamma.Api.Infrastructure.Base;
using Mahamma.AppService.Company.AddCompany;
using Mahamma.AppService.Company.CreateCompanyInvitation;
using Mahamma.AppService.Company.GetCompany;
using Mahamma.AppService.Company.GetCompanyInvitationQuery;
using Mahamma.AppService.Company.SendInvitationsFromFileCommand;
using Mahamma.AppService.Company.SetEmailToCompanyInvitation;
using Mahamma.AppService.Company.UpdateCompanyInvitationStatus;
using Mahamma.AppService.MemberSearch.SearchUserForProject;
using Mahamma.AppService.MemberSearch.SearchUserForTask;
using Mahamma.AppService.MemberSearch.SearchUserForWorkspace;
using Mahamma.Base.Domain.ApiActions.Enum;
using Mahamma.Base.Domain.Enum;
using Mahamma.Domain.Company.Dto;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Mahamma.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route(Route.API)]
    public class CompanyController : BaseController
    {
        #region Ctor
        public CompanyController(IMediator mediator) : base(mediator)
        { }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetCompanyQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddCompanyCommand addCompanyCommand)
        {
            return Ok(await Mediator.Send(addCompanyCommand));
        }

        [HttpPost]
        public async Task<IActionResult> SearchUserForWorkspace([FromBody] SearchUserForWorkspaceCommand searchCommand)
        {
            return Ok(await Mediator.Send(searchCommand));
        }
        [HttpPost]
        public async Task<IActionResult> SearchUserForProject([FromBody] SearchUserForProjectCommand searchCommand)
        {
            return Ok(await Mediator.Send(searchCommand));
        }
        [HttpPost]
        public async Task<IActionResult> SearchUserForTask([FromBody] SearchUserForTaskCommand searchCommand)
        {
            return Ok(await Mediator.Send(searchCommand));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanyInvitation(string invitationId)
        {
            return Ok(await Mediator.Send(new GetCompanyInvitationQueryCommand(invitationId)));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateInvitationStatus([FromBody] UpdateCompanyInvitationStatusCommand updateCompanyInvitationStatusCommand)
        {
            return Ok(await Mediator.Send(updateCompanyInvitationStatusCommand));
        }


        /// <summary>
        /// Get Workspace By Id
        /// </summary>
        /// <param name="GetCompnayWithWorkspaces"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompnayWithWorkspaces([FromBody] SearchCompanyDetailsDto searchCompanyDetailsDto)
        {
            return Ok(await Mediator.Send(searchCompanyDetailsDto));
        }
        [HttpGet]
        //CreateInvitationPermission
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.ViewWorkspace)]
        public async Task<IActionResult> CreateCompanyInvitation()
        {
            return Ok(await Mediator.Send(new CreateCompanyInvitationCommand()));
        }
        [HttpPost]
        //CreateInvitationPermission
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.ViewWorkspace)]
        public async Task<IActionResult> SetEmailToCompanyInvitation(SetEmailToCompanyInvitationCommand setEmailToCompanyInvitationCommand)
        {
            return Ok(await Mediator.Send(setEmailToCompanyInvitationCommand));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.ViewWorkspace)]
        public async Task<IActionResult> SendInvitationsFromFile([FromBody] SendInvitationsFromFileCommand sendInvitationsFromFileCommand)
        {
            return Ok(await Mediator.Send(sendInvitationsFromFileCommand));
        }
        #endregion
    }
}
