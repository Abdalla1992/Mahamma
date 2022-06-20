using Mahamma.Identity.Api.Infrastructure.Base;
using Mahamma.Identity.AppService.Language.GetAll;
using Mahamma.Identity.AppService.Language.GetLanguage;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mahamma.Identity.Api.Controllers
{
    [ApiController]
    [Route(Route.API)]
    public class LanguageController : BaseController
    {
        #region CTRS
        public LanguageController(IMediator mediator) : base(mediator)
        { }
        #endregion
        [HttpGet]
        public async Task<IActionResult> GetAllLanguages()
        {
            return Ok(await Mediator.Send(new GetAllQuery()));
        }
        [HttpGet]
        public async Task<IActionResult> GetLanguage(int id)
        {
            return Ok(await Mediator.Send(new GetLanguageQuery(id)));
        }
    }
}
