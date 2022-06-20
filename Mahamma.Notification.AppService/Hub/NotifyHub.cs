using Mahamma.Base.Domain.ApiActions.Enum;
using Mahamma.Base.Domain.Enum;
using Mahamma.Notification.AppService.Hub;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mahamma.Notification.AppService.Hub
{
    [AuthorizeAttributeFactory(PageId = (int)PageEnum.WorkspaceProfile, PermissionId = (int)Permission.ViewWorkspace)]
    public class NotifyHub : Hub<IHubClient>
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
