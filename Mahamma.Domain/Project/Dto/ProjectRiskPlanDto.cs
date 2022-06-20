namespace Mahamma.Domain.Project.Dto
{
    public class ProjectRiskPlanDto
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public string Issue { get; set; }
        public string Impact { get; set; }
        public string Action { get; set; }
        public string Owner { get; set; }
    }
}
