using Mahamma.Identity.ApiClient.Dto.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Api.Hub
{
    public class NameUserIdProvider : IUserIdProvider
    {
        private readonly IHttpContextAccessor _httpContext;

        public NameUserIdProvider(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        public string GetUserId(HubConnectionContext connection)
        {
            var currentUserId = (string)_httpContext.HttpContext.Items["UserId"];
            return currentUserId;
        }
    }
}
