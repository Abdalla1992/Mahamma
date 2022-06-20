namespace Mahamma.Domain.Project.Dto
{
    public class ProjectCharterDto
    {
        #region Props
        public int ProjectId { get; set; }
        public string Summary { get; set; }
        public string Goals { get; set; }
        public string Deliverables { get; set; }
        public string Scope { get; set; }
        public string Benefits { get; set; }
        public string Costs { get; set; }
        public string Misalignments { get; set; }
        #endregion
    }
}
