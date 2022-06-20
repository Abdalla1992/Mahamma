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

namespace Mahamma.AppService.Folder.GetFolder
{
    public class GetFolderCommand : IRequest<ValidateableResponse<ApiResponse<FolderDto>>>
    {
        #region Prop
        [DataMember]
        public int FolderId { get; set; }
        #endregion

        #region Ctor
        public GetFolderCommand(int id)
        {
            FolderId = id;
        }
        #endregion
    }
}
