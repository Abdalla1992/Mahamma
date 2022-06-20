using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.ApiClient.Setting
{
    public class MahammaIdentityClientApiSetting
    {
        public string IdentityUrl { get; set; }
        public string GetUserUrl { get; set; } = "api/Account/Get";
        public string GetUserUrlForBackgroundService { get; set; } = "api/Account/GetUserForBackgroundService";
        public string SetUserCompanyUrl { get; set; } = "api/Account/SetUserCompany";
        public string SetCompanyBasicRolesUrl { get; set; } = "api/Role/SetCompanyBasicRoles";
        public string UpdateUserProfileStatusUrl { get; set; } = "api/Account/UpdateUserProfileStatus";
        public string AuthorizeUserUrl { get; set; } = "api/Role/AuthorizeUserPermission";
    }
}
