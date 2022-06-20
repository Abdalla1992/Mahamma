using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.Company.Dto;
using Mahamma.Domain.Task.Dto;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Company.GetCompany
{
    public class GetCompanyQuery : IRequest<ValidateableResponse<ApiResponse<CompanyDto>>>
    {
        #region Props
        [DataMember]
        public int Id { get; set; }
        #endregion

        #region CTRS
        public GetCompanyQuery(int id)
        {
            Id = id;
        }
        #endregion
    }
}
