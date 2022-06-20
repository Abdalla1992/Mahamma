
using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Project.Dto;
using Mahamma.Domain.Project.Entity;
using Mahamma.Domain.Project.Repositroy;
using Mahamma.Domain.ProjectAttachment.Dto;
using Mahamma.Domain.ProjectAttachment.Entity;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class ProjectAttachmentRepository : Base.EntityRepository<ProjectAttachment>, IProjectAttachmentRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public ProjectAttachmentRepository(IMapper mapper, MahammaContext context) : base(context, mapper)
        { }
        public async Task<ProjectAttachmentDto> GetById(int id)
        {
            return Mapper.Map<ProjectAttachmentDto>(await FirstOrDefaultNoTrackingAsync(t => t.Id == id));
        }

        public async Task<ProjectAttachment> GetEntityById(int id)
        {
            return await FirstOrDefaultAsync(t => t.Id == id);
        }
        public void AddAttachment(ProjectAttachment attachment)
        {
            CreateAsyn(attachment);
        }
        public void AddAttachmentList(List<ProjectAttachment> attachments)
        {
            CreateListAsyn(attachments);
        }
        public async Task<List<ProjectAttachmentDto>> GetTaskFilesList(int taskId)
        {
            var files = await GetWhereNoTrackingAsync(t => t.Id == taskId);
            return Mapper.Map<List<ProjectAttachmentDto>>(files.ToList());
        }

        public void UpdateProject(ProjectAttachment projectAttachment)
        {
            Update(projectAttachment);
        }

        public async Task<PageList<ProjectAttachmentDto>> GetProjectFileList(SearchProjectAttachmentDto searchProjectAttachmentDto)
        {
            #region Declare Return Var with Intial Value
            PageList<ProjectAttachmentDto> projectAttachmentListDto = new();
            #endregion
            #region Preparing Filter 
            Expression<Func<ProjectAttachment, bool>> filter = t => t.DeletedStatus == DeletedStatus.NotDeleted.Id
                                                           && t.ProjectId == searchProjectAttachmentDto.Filter.ProjectId
                                                           && (!searchProjectAttachmentDto.Filter.TaskId.HasValue || t.TaskId == searchProjectAttachmentDto.Filter.TaskId)
                                                           && ((!searchProjectAttachmentDto.Filter.FolderId.HasValue && (t.FolderFiles == null || t.FolderFiles.Count == 0)) || t.FolderFiles.Where(f => f.DeletedStatus == DeletedStatus.NotDeleted.Id).Any(f => f.FolderId == searchProjectAttachmentDto.Filter.FolderId))
                                                           && (string.IsNullOrEmpty(searchProjectAttachmentDto.Filter.FileName) || t.FileName.ToLower().Contains(searchProjectAttachmentDto.Filter.FileName));

            #endregion


            List<ProjectAttachment> projectAttachmentList = searchProjectAttachmentDto.Sorting.Column switch
            {
                _ => await GetPageAsyncWithoutQueryFilter(searchProjectAttachmentDto.Paginator.Page, searchProjectAttachmentDto.Paginator.PageSize, filter, x => x.Id, SortDirection.Descending.Id, "FolderFiles"),
            };
            if (projectAttachmentList?.Count > default(int))
            {
                int totalCount = await GetCountAsyncWithoutQueryFilter(filter);
                projectAttachmentListDto.SetResult(totalCount, Mapper.Map<List<ProjectAttachment>, List<ProjectAttachmentDto>>(projectAttachmentList));
            }
            return projectAttachmentListDto;
        }
        public async Task<PageList<ProjectAttachmentDto>> GetProjectLatestFileList(int projectId, int filesCount)
        {
            var project = await GetPageAsync(default(int), filesCount, x => x.ProjectId == projectId && x.DeletedStatus == DeletedStatus.NotDeleted.Id,
                f => f.CreationDate, SortDirection.Descending.Id);
            List<ProjectAttachmentDto> projectAttachments = Mapper.Map<List<ProjectAttachmentDto>>(project);
            int totalCount = await GetCountAsync(x => x.ProjectId == projectId && x.DeletedStatus == DeletedStatus.NotDeleted.Id);
            PageList<ProjectAttachmentDto> pageList = new PageList<ProjectAttachmentDto>();
            pageList.SetResult(totalCount, projectAttachments);
            return pageList;
        }
        public async Task<PageList<ProjectAttachmentDto>> GetTaskLatestFileList(int? taskId, int filesCount)
        {
            var project = await GetPageAsync(default(int), filesCount, x => x.TaskId == taskId && x.DeletedStatus == DeletedStatus.NotDeleted.Id,
                f => f.CreationDate, SortDirection.Descending.Id);
            List<ProjectAttachmentDto> projectAttachments = Mapper.Map<List<ProjectAttachmentDto>>(project);
            int totalCount = await GetCountAsync(x => x.TaskId == taskId && x.DeletedStatus == DeletedStatus.NotDeleted.Id);
            PageList<ProjectAttachmentDto> pageList = new PageList<ProjectAttachmentDto>();
            pageList.SetResult(totalCount, projectAttachments);
            return pageList;
        }
    }
}
