using Mahamma.ApiClient.Dto.Base;
using Mahamma.ApiClient.Dto.Company;
using System.Threading.Tasks;

namespace Mahamma.ApiClient.Interface
{
    public interface ICompanyService
    {
        Task<CompanyDto> GetCompanyById(int id, string authToken);
        Task<CompanyInvitationDto> GetCompanyInvitation(string invitationId);
        Task<bool> UpdateInvitationStatus(BaseRequestDto baseRequest, string invitationId, int invitationStatusId);
    }
}
