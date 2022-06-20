using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Task.AddSubTask
{
    public class AddSubTaskCommand : IRequest<ValidateableResponse<ApiResponse<int>>>
    {
        #region Props
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
        [DataMember]
        public int? ParentTaskId { get; set; }
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public List<long> UserIdList { get; set; }
        [DataMember]
        public List<string> Files { get; set; }
        [DataMember]
        public bool? IsCreatedFromMeeting { get; set; }
        [DataMember]
        public int? DependencyTaskId { get; set; }
        #endregion
    }
}
