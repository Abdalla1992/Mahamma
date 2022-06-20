using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Domain.ProjectAttachment.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.DeleteProjectFile
{
    public class DeleteProjectFileCommand : IRequest<ValidateableResponse<ApiResponse<List<ProjectAttachmentDto>>>>
    {
        #region Props
        [DataMember]
        public int Id { get; set; }
        #endregion

        public DeleteProjectFileCommand(int id)
        {
            Id = id;
        }
    }
}
