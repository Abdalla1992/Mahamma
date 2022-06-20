using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.ApiClient.Setting
{
    public class MahammaClientApiSetting
    {
        public string MahammaApiUrl { get; set; }
        public string GetCompanyUrl { get; set; } = "api/Company/Get";
        public string GetCompanyInvitationUrl { get; set; } = "api/Company/GetCompanyInvitation";
        public string UpdateInvitationStatusUrl { get; set; } = "api/Company/UpdateInvitationStatus";
    }
}
