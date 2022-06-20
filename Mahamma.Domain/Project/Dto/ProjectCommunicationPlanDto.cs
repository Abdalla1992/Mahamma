namespace Mahamma.Domain.Project.Dto
{
    public class ProjectCommunicationPlanDto
    {
        public int Id { get; set; }
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
    }
}
