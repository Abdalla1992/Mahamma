using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Company.Entity;
using Mahamma.Domain.Company.Repositroy;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.SetEmailToCompanyInvitation
{
    public class SetEmailToCompanyInvitationCommandHandler : IRequestHandler<SetEmailToCompanyInvitationCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly ICompanyInvitationRepository _companyInvitationRepository;
        #endregion

        #region Ctor
        public SetEmailToCompanyInvitationCommandHandler(ICompanyInvitationRepository companyInvitationRepository)
        {
            _companyInvitationRepository = companyInvitationRepository;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(SetEmailToCompanyInvitationCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            CompanyInvitation companyInvitation = await _companyInvitationRepository.GetEntityById(request.Id);
            if (companyInvitation != null)
            {
                companyInvitation.Email = request.Email;
                _companyInvitationRepository.UpdateCompanyInvitation(companyInvitation);
                if (await _companyInvitationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Data Updated Successfully";
                }
            }
            return response;
        }
    }
}
