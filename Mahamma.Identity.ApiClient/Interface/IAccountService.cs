using Mahamma.Identity.ApiClient.Dto.Base;
using Mahamma.Identity.ApiClient.Dto.User;
using System.Threading.Tasks;

namespace Mahamma.Identity.ApiClient.Interface
{
    public interface IAccountService
    {
        UserDto GetUserById(BaseRequestDto baseRequest, long id);
        UserDto GetUserByIdForBackgroundService(long id);
        Task<bool> SetUserCompany(BaseRequestDto baseRequest, int companyId);
        Task<bool> UpdateUserProfileStatus(BaseRequestDto baseRequest, int userProfileStatusId);
    }
}
