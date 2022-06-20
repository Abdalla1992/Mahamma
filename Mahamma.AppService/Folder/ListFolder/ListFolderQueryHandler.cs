using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain.ProjectAttachment.Dto;
using Mahamma.Domain.ProjectAttachment.Repository;
using Mahamma.Identity.ApiClient.Dto.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Folder.ListFolder
{
    public class ListFolderQueryHandler : IRequestHandler<SearchFolderDto, PageList<FolderDto>>
    {
        #region Prop
        private readonly IHttpContextAccessor _httpContext;
        private readonly IFolderRepository _folderRepository;
        #endregion

        #region Ctor
        public ListFolderQueryHandler(IFolderRepository folderRepository, IHttpContextAccessor httpContext)
        {
            _folderRepository = folderRepository;
            _httpContext = httpContext;
        }
        #endregion
        public async Task<PageList<FolderDto>> Handle(SearchFolderDto request, CancellationToken cancellationToken)
        {
            var currentUser = (UserDto)_httpContext.HttpContext.Items["User"];
            PageList<FolderDto> response = new PageList<FolderDto>();
            return await _folderRepository.GetFolderList(request, currentUser.CompanyId);
        }
    }
}
