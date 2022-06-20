using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Task.UpdateTask
{
    public class UpdateTaskCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime StartDate { get; set; }
        [DataMember]
        public DateTime DueDate { get; set; }
        [DataMember]
        public int TaskPriorityId { get; set; }
        [DataMember]
        public bool ReviewRequest { get; set; }
        #endregion
    }
}
