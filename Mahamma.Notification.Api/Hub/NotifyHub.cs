using Mahamma.Base.Domain.ApiActions.Enum;
using Mahamma.Base.Domain.Enum;
using Mahamma.Notification.Api.Infrastructure.Filter;
//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mahamma.Notification.Api.Hub
{
    [AuthorizeAttributeFactory]
    public class NotifyHub : Hub<IHubClient>
    {
    }
}
