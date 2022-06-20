using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.ApiClient.Dto.Company
{
    public class CompanyInvitationDto
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Email { get; set; }
        public long UserId { get; set; }
        public string InvitationId { get; set; }
        public int InvitationStatusId { get; set; }
    }
}
