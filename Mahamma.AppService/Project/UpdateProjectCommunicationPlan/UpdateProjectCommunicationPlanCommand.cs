using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Project.UpdateProjectCommunicationPlan
{
    public class UpdateProjectCommunicationPlanCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Prop
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ProjectId { get; set; }
        [DataMember]
        public string Recipient { get; set; }
        [DataMember]
        public string Frequency { get; set; }
        [DataMember]
        public string CommunicationType { get; set; }
        [DataMember]
        public string Owner { get; set; }
        [DataMember]
        public string KeyDates { get; set; }
        [DataMember]
        public string DeliveryMethod { get; set; }
        [DataMember]
        public string Goal { get; set; }
        [DataMember]
        public string ResourceLinks { get; set; }
        [DataMember]
        public string Notes { get; set; }
        #endregion
    }
}
