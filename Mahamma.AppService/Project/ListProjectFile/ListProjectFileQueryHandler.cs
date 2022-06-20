using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Base.Resources.IResourceReader;
using Mahamma.Domain.ProjectAttachment.Dto;
using Mahamma.Domain.ProjectAttachment.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.ListProjectFile
{
    public class ListProjectFileQueryHandler : IRequestHandler<SearchProjectAttachmentDto, PageList<ProjectAttachmentDto>>
    {
        #region Prop
        private readonly IProjectAttachmentRepository _projectAttachmentRepository;
        private readonly IMessageResourceReader _messageResourceReader;

        #endregion

        #region Ctor
        public ListProjectFileQueryHandler(IProjectAttachmentRepository projectAttachmentRepository, IMessageResourceReader messageResourceReader)
        {
            _projectAttachmentRepository = projectAttachmentRepository;
            _messageResourceReader = messageResourceReader;

        }
        #endregion

        #region Methods
        public async Task<PageList<ProjectAttachmentDto>> Handle(SearchProjectAttachmentDto request, CancellationToken cancellationToken)
        {
            PageList<ProjectAttachmentDto> response = new PageList<ProjectAttachmentDto>();
            return await _projectAttachmentRepository.GetProjectFileList(request);
        }
        #endregion
    }
}
