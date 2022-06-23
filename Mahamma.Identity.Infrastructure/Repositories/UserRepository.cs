using AutoMapper;
using Mahamma.ApiClient.Dto.Company;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Identity.Domain._SharedKernel;
using Mahamma.Identity.Domain.User.Dto;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.User.Repository;
using Mahamma.Identity.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mahamma.Identity.Infrastructure.Repositories
{
    public class UserRepository : Base.EntityRepository<User>, IUserRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public UserRepository(IMapper mapper, IdentityContext context) : base(context, mapper)
        { }

        public async Task<User> GetUserById(long id)
        {
            return await FirstOrDefaultAsync(m => m.Id == id && m.DeletedStatus != DeletedStatus.Deleted.Id);
        }

        public async Task<UserDto> GetById(long id)
        {
            return Mapper.Map<UserDto>(await FirstOrDefaultAsync(m => m.Id == id && m.DeletedStatus != DeletedStatus.Deleted.Id, "UserProfileSections"));
        }

        public async Task<List<MemberDto>> GetUserList(List<long> idList)
        {
            return Mapper.Map<List<MemberDto>>(await GetWhereNoTrackingAsync(m => idList.Contains(m.Id) && m.DeletedStatus != DeletedStatus.Deleted.Id));
        }

        public void UpdateProject(User user)
        {
            Update(user);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            User user = await FirstOrDefaultAsync(u => u.Email == email && u.DeletedStatus != DeletedStatus.Deleted.Id);
            return user;
        }
        public UserDto MapUserToUserDto(User user)
        {
            return Mapper.Map<UserDto>(user);
        }
        public List<UserDto> MapUserListToUserDtoList(List<User> users)
        {
            return Mapper.Map<List<UserDto>>(users);
        }
        public async Task<bool> CheckEmailExistence(string email, long id = default)
        {
            return await GetAnyAsync(w => w.Email == email && (id == default || w.Id != id) && w.DeletedStatus == DeletedStatus.NotDeleted.Id);
        }
        public async Task<PageList<UserDto>> GetUserData(SearchUserDto searchUserDto, int currentCompanyId, long userId)
        {
            #region Declare Return Var with Intial Value

            PageList<UserDto> userListDto = new();
            #endregion

            #region Preparing Filter
            Expression<Func<User, bool>> filter;
                filter = t => t.DeletedStatus == DeletedStatus.NotDeleted.Id && t.CompanyId == currentCompanyId
                           //&& (t.Id != userId)
                           && (String.IsNullOrEmpty(searchUserDto.Filter.FullName) || t.FullName.ToLower().Contains(searchUserDto.Filter.FullName));
            #endregion

            List<User> userList = searchUserDto.Sorting.Column switch
            {
                "fullName" => await GetPageAsyncWithoutQueryFilter(searchUserDto.Paginator.Page, searchUserDto.Paginator.PageSize, filter, x => x.FullName, searchUserDto.Sorting.SortingDirection.Id),
                _ => await GetPageAsyncWithoutQueryFilter(searchUserDto.Paginator.Page, searchUserDto.Paginator.PageSize, filter, x => x.Id, SortDirection.Descending.Id),

            };
            if (userList?.Count > default(int))
            {
                int totalCount = await GetCountAsyncWithoutQueryFilter(filter);
                userListDto.SetResult(totalCount, Mapper.Map<List<User>, List<UserDto>>(userList));
            }

            return userListDto;
        }
    }
}
