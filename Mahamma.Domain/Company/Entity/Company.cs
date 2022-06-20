using Mahamma.Base.Domain;
using System;

using System.Collections.Generic;

namespace Mahamma.Domain.Company.Entity
{
    public class Company : Entity<int> , IAggregateRoot
    {
        #region Props
        public string Name { get; set; }
        public string CompanySize { get; set; }
        public string FolderPath { get; set; }
        public string Descreption { get; set; }
        public long CreatorUserId { get; set; }
        #endregion

        #region Navigation Prop
        public List<CompanyInvitation> CompanyInvitations { get; set; }
        public List<Domain.Workspace.Entity.Workspace> Workspaces { get; set; }
        #endregion

        #region Methods
        public void CreateCompany(string name, string companySize, string descreption,long creatorUserId, List<CompanyInvitation> companyInvitations)
        {
            Name = name;
            CompanySize = companySize;
            Descreption = descreption;
            FolderPath = $"CompaniesFolder\\{Name}_{Guid.NewGuid()}\\";
            CompanyInvitations = companyInvitations;
            CreatorUserId = creatorUserId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        public void UpdateCompany(string name, string companySize, string descreption)
        {
            Name = name; 
            CompanySize = companySize;
            Descreption = descreption;
        }

        public void DeleteCompany()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
        #endregion

    }
}
