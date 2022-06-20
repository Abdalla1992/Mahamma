using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Task.DeleteTaskFile
{
    public class DeleteTaskFileCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public int FileId { get; set; }
        #endregion
    }
}
