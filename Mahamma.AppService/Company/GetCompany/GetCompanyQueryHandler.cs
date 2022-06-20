using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.Company.Dto;
using Mahamma.Domain.Company.Repositroy;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.GetCompany
{
    public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, ValidateableResponse<ApiResponse<CompanyDto>>>
    {
        #region Props
        private readonly ICompanyRepository _companyRepository;
        private readonly IMessageResourceReader _messageResourceReader;

        #endregion

        #region CTRS
        public GetCompanyQueryHandler(ICompanyRepository companyRepository,IMessageResourceReader messageResourceReader)
        {
            _companyRepository = companyRepository;
            _messageResourceReader = messageResourceReader;
        }
        #endregion

        public async Task<ValidateableResponse<ApiResponse<CompanyDto>>> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<CompanyDto>> response = new(new ApiResponse<CompanyDto>());
            CompanyDto companyDto = await _companyRepository.GetCompanyData(request.Id);

            if (companyDto != null)
            {
                response.Result.CommandMessage = "Process completed successfully.";
                response.Result.ResponseData = companyDto;
            }
            else
            {
                response.Result.CommandMessage = "No data found.";
            }
            return response;
        }
    }
}
