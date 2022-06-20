using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.ProjectAttachment.Dto;
using Mahamma.Domain.ProjectAttachment.Entity;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class FolderRepository :Base.EntityRepository<Folder> ,IFolderRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public FolderRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }

        public void AddFolder(Folder folder)
        {
            CreateAsyn(folder);
        }

        public async Task<Folder> GetFolderById(int id, string includeProperties = "")
        {
            return await FirstOrDefaultAsync(ll => ll.Id == id, includeProperties);
        }

        public async Task<FolderDto> GetById(int id)
        {
            return Mapper.Map<Folder, FolderDto>(await FirstOrDefaultAsync(ll => ll.Id == id));
        }

        public void UpdateFolder(Folder folder)
        {
            Update(folder);
        }

        public async Task<PageList<FolderDto>> GetFolderList(SearchFolderDto searchFolderDto, int companyId)
        {
            #region Declare Return Var with Intial Value
            PageList<FolderDto> folderListDto = new();
            #endregion
            #region Preparing Filter 
            Expression<Func<Folder, bool>> filter = t => t.DeletedStatus == DeletedStatus.NotDeleted.Id
                                                           && (t.ProjectId == searchFolderDto.Filter.ProjectId || (searchFolderDto.Filter.ProjectId == 0 && t.Project.Workspace.CompanyId == companyId))
                                                           && (t.TaskId == searchFolderDto.Filter.TaskId || searchFolderDto.Filter.TaskId == 0)
                                                           && (string.IsNullOrEmpty(searchFolderDto.Filter.Name) || t.Name.ToLower().Contains(searchFolderDto.Filter.Name));
            #endregion


            List<Folder> folderList = searchFolderDto.Sorting.Column switch
            {
                _ => await GetPageAsyncWithoutQueryFilter(searchFolderDto.Paginator.Page, searchFolderDto.Paginator.PageSize, filter, x => x.Id, SortDirection.Descending.Id),
            };
            if (folderList?.Count > default(int))
            {
                int totalCount = await GetCountAsyncWithoutQueryFilter(filter);
                folderListDto.SetResult(totalCount, Mapper.Map<List<Folder>, List<FolderDto>>(folderList));
            }
            return folderListDto;
        }
    }
}
