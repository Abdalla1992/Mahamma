using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Role.Entity
{
    public class PermissionLocalization : Entity<int>, IAggregateRoot
    {
        public string DisplayName { get; set; }
        public int LanguageId { get; set; }
        public int PermissionId { get; set; }

        public PermissionLocalization(int id , string displayName,int permissionId, int languageId)
        {
            Id = id;
            DisplayName = displayName;
            PermissionId = permissionId;
            LanguageId = languageId;
            CreationDate = new DateTime(2021, 11, 4);
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }

    }
}
