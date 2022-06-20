using Mahamma.Identity.Domain._SharedKernel;
using System.Threading.Tasks;
using Mahamma.Identity.Domain.User.Dto;
using System.Collections.Generic;
using Mahamma.ApiClient.Dto.Company;
using Mahamma.Base.Dto.ApiResponse;

namespace Mahamma.Identity.Domain.User.Repository
{
    public interface IUserRepository : IRepository<Entity.User>
    {
        Task<Entity.User> GetUserById(long id);
        void UpdateProject(Entity.User user);
        Task<Entity.User> GetUserByEmail(string email);
        Task<UserDto> GetById(long id);
        Task<List<MemberDto>> GetUserList(List<long> idList);
        UserDto MapUserToUserDto(Entity.User user);
        List<UserDto> MapUserListToUserDtoList(List<Entity.User> users);
        Task<bool> CheckEmailExistence(string email, long id = default);
        Task<PageList<UserDto>> GetUserData(SearchUserDto searchUserDto, int currentCompanyId, long userId);
    }
}
