using Mahamma.Base.Domain;

namespace Mahamma.Domain.Project.Entity
{
    public class ProjectCharter : Entity<int>, IAggregateRoot
    {
        #region Prop
        public int ProjectId { get; set; }
        public string Summary { get; set; }
        public string Goals { get; set; }
        public string Deliverables { get; set; }
        public string Scope { get; set; }
        public string Benefits { get; set; }
        public string Costs { get; set; }
        public string Misalignments { get; set; }
        #endregion

        #region Methods
        public void PrepareProjectCharterCreation()
        {
            CreationDate = System.DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void UpdateProjectCharter(string summary, string goals, string deliverables, 
                                            string scope, string benefits, string costs, string misalignments)
        {
            Summary = summary;
            Goals = goals;
            Deliverables = deliverables;
            Scope = scope;
            Benefits = benefits;
            Costs = costs;
            Misalignments = misalignments;
        }
        #endregion
    }
}
