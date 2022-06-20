using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.UpdateCompanyInvitationStatus
{
    public class UpdateCompanyInvitationStatusCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        [DataMember]
        public string InvitationId { get; set; }
        [DataMember]
        public int InvitationStatusId { get; set; }
    }
}
