using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Task.AddTaskFile
{
    public class AddTaskFileCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props

        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public int? TaskId { get; set; }
        [DataMember]
        public string FileURL { get; set; }
        #endregion
    }
}
