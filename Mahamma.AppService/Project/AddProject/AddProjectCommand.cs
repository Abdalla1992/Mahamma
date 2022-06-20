using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Project.AddProject
{
   public  class AddProjectCommand : IRequest<ValidateableResponse<ApiResponse<int>>>
    {
        #region Prop
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime DueDate { get; set; }
        [DataMember]
        public int WorkSpaceId { get; set; }
        [DataMember]
        public List<long> UserIdList { get; set; }
        [DataMember]
        public bool? IsCreatedFromMeeting { get; set; }
        #endregion
    }
}
