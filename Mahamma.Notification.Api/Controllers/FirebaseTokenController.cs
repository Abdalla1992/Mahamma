using Mahamma.Base.Domain.ApiActions.Enum;
using Mahamma.Base.Domain.Enum;
using Mahamma.Notification.Api.Infrastructure.Base;
using Mahamma.Notification.Api.Infrastructure.Filter;
using Mahamma.Notification.AppService.FirebaseTokens.AddFirebaseToken;
using Mahamma.Notification.AppService.FirebaseTokens.GetFirebaseToken;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mahamma.Notification.Api.Controllers
{
    [ApiController]
    [Route(Route.API)]
    public class FirebaseTokenController : BaseController
    {
        #region Ctor
        public FirebaseTokenController(IMediator mediator) : base(mediator)
        { }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetFirebaseToken()
        {
            var result = await Mediator.Send(new GetFirebaseTokenCommand());
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddFirebaseToken(AddFirebaseTokenCommand request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFirebaseToken(RemoveFirebaseTokenCommand request)
        {
            var result = await Mediator.Send(request);
            return Ok(result);
        }
    }
}
