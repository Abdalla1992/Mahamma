using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mahamma.Document.Api.Infrastructure.Base
{
    public class BaseController : ControllerBase
    {
        protected IMediator Mediator { get; }
        public BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}
