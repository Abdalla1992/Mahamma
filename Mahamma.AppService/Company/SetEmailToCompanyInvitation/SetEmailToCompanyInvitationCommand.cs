using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.SetEmailToCompanyInvitation
{
    public class SetEmailToCompanyInvitationCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
