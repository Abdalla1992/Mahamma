using Mahamma.Base.Domain;

namespace Mahamma.Domain.Project.Entity
{
    public class ProjectCommunicationPlan : Entity<int>, IAggregateRoot
    {
        #region Prop
        public int ProjectId { get; set; }
        public string Recipient { get; set; }
        public string Frequency { get; set; }
        public string CommunicationType { get; set; }
        public string Owner { get; set; }
        public string KeyDates { get; set; }
        public string DeliveryMethod { get; set; }
        public string Goal { get; set; }
        public string ResourceLinks { get; set; }
        public string Notes { get; set; }
        #endregion

        #region Methods
        public void PrepareProjectCommunicationPlanCreation()
        {
            CreationDate = System.DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void UpdateProjectCommunicationPlan(string recipient, string frequency, string communicationType, string owner,
                                                    string keyDates, string deliveryMethod, string goal, string resourceLinks, string notes)
        {
            Recipient = recipient;
            Frequency = frequency;
            CommunicationType = communicationType;
            Owner = owner;
            KeyDates = keyDates;
            DeliveryMethod = deliveryMethod;
            Goal = goal;
            ResourceLinks = resourceLinks;
            Notes = notes;
        }
        #endregion
    }
}
