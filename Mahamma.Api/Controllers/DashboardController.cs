using Mahamma.Api.Infrastructure.Base;
using Mahamma.AppService.MemberSearch.GetProjectStatistics;
using Mahamma.Base.Domain.ApiActions.Enum;
using Mahamma.Base.Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mahamma.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route(Route.API)]
    public class DashboardController : BaseController
    {

        #region Ctor
        public DashboardController(IMediator mediator) : base(mediator)
        { }
        #endregion
        #region Methods
        [HttpPost]
        //[Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.DashboardProfile, PermissionId = (int)Permission.ViewCharts)]
        public async Task<IActionResult> GetProjectStatistics(GetProjectStatisticsCommand command)
        {
            return Ok(await Mediator.Send(command));
        }
        #endregion
    }
}
