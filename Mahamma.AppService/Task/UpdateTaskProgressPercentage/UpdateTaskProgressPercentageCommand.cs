using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.UpdateTaskProgressPercentage
{
    public class UpdateTaskProgressPercentageCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        [DataMember]
        public int TaskId { get; set; }
        [DataMember]
        public double ProgressPercentage { get; set; }
    }
}
