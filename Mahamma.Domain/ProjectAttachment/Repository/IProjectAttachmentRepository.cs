using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.ProjectAttachment.Dto;
using Mahamma.Domain.ProjectAttachment;
using System.Threading.Tasks;
using System.Collections.Generic;
using Mahamma.Base.Dto.ApiResponse;

namespace Mahamma.Domain.ProjectAttachment.Repository
{
    public interface IProjectAttachmentRepository : IRepository<Entity.ProjectAttachment>
    {
        void AddAttachment(Entity.ProjectAttachment attachment);
        void AddAttachmentList(List<Entity.ProjectAttachment> attachments);
        Task<ProjectAttachmentDto> GetById(int id);
        Task<Entity.ProjectAttachment> GetEntityById(int id);
        Task<List<ProjectAttachmentDto>> GetTaskFilesList(int taskId);
        void UpdateProject(Entity.ProjectAttachment projectAttachment);
        Task<PageList<ProjectAttachmentDto>> GetProjectFileList(SearchProjectAttachmentDto searchProjectAttachmentDto);
        Task<PageList<ProjectAttachmentDto>> GetProjectLatestFileList(int projectId, int filesCount);
        Task<PageList<ProjectAttachmentDto>> GetTaskLatestFileList(int? taskId, int filesCount);
    }
}
