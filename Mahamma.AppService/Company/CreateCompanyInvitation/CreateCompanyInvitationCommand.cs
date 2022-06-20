using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Company.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.CreateCompanyInvitation
{
    public class CreateCompanyInvitationCommand : IRequest<ValidateableResponse<ApiResponse<CompanyInvitationDto>>>
    {
        public CreateCompanyInvitationCommand()
        {

        }
    }
}
