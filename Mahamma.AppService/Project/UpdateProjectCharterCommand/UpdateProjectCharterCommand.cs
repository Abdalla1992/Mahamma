using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Project.UpdateProjectCharterCommand
{
    public class UpdateProjectCharterCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public string Summary { get; set; }
        [DataMember]
        public string Goals { get; set; }
        [DataMember]
        public string Deliverables { get; set; }
        [DataMember]
        public string Scope { get; set; }
        [DataMember]
        public string Benefits { get; set; }
        [DataMember]
        public string Costs { get; set; }
        [DataMember]
        public string Misalignments { get; set; }
        #endregion
    }
}
