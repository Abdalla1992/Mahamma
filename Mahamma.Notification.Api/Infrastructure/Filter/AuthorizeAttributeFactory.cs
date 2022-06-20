using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Mahamma.Notification.Api.Infrastructure.Filter
{
    public class AuthorizeAttributeFactory : Attribute, IFilterFactory
    {
        public int PermissionId { get; set; }
        public int PageId { get; set; }
        public bool IsReusable => false;

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            AuthorizeAttribute filter = serviceProvider.GetService<AuthorizeAttribute>();

            filter.PermissionId = PermissionId;
            filter.PageId = PageId;

            return filter;
        }
    }
}
