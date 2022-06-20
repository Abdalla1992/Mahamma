using Mahamma.Api.Infrastructure.Base;
using Mahamma.AppService.GetUserProfile.GetUserProfileCommand;
using Mahamma.Base.Domain.ApiActions.Enum;
using Mahamma.Base.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mahamma.Api.Controllers
{
    //[Authorize]
    [ApiController]
    [Route(Route.API)]
    public class ProfileController : BaseController
    {
        #region Ctor
        public ProfileController(IMediator mediator) : base(mediator)
        { }
        #endregion

        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ProjectProfile, PermissionId = (int)Permission.ViewProject)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetUserProfileCommand(id)));
        }
    }
}
