using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Folder.AddFolder
{
    public class AddFolderCommand : IRequest<ValidateableResponse<ApiResponse<int>>>
    {
        #region Prop
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public int? TaskId { get; set; }
        #endregion

    }
}
