using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.ProjectAttachment.Dto;
using System.Threading.Tasks;

namespace Mahamma.Domain.ProjectAttachment.Repository
{
    public interface IFolderRepository : IRepository<Entity.Folder>
    {
        void AddFolder(Entity.Folder folder);
        Task<Entity.Folder> GetFolderById(int id, string includeProperties = "");
        Task<FolderDto> GetById(int id);
        void UpdateFolder(Entity.Folder folder);
        Task<PageList<FolderDto>> GetFolderList(SearchFolderDto request, int companyId);
    }
}
