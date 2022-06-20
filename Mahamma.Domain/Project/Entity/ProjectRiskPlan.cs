using Mahamma.Base.Domain;

namespace Mahamma.Domain.Project.Entity
{
    public class ProjectRiskPlan : Entity<int>, IAggregateRoot
    {
        #region Prop
        public int ProjectId { get; set; }
        public string Issue { get; set; }
        public string Impact { get; set; }
        public string Action { get; set; }
        public string Owner { get; set; }
        #endregion

        #region Methods
        public void PrepareProjectRiskPlanCreation()
        {
            CreationDate = System.DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void UpdateProjectRiskPlan(string issue, string impact, string action, string owner)
        {
            Issue = issue;
            Impact = impact;
            Action = action;
            Owner = owner;
        }
        #endregion
    }
}
