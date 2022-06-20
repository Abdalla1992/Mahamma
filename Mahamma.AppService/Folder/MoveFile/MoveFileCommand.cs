using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Folder.MoveFile
{
    public class MoveFileCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        [DataMember]
        public int? OldFolderId { get; set; }
        [DataMember]
        public int NewFolderId { get; set; }
        [DataMember]
        public int ProjectFileId { get; set; }
        #endregion
    }
}
