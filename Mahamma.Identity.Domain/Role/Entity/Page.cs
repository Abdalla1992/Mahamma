using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Role.Entity
{
    public class Page : Entity<int>, IAggregateRoot
    {
        public string Name { get; set; }
        public List<PagePermission> PagePermissions { get; set; }

        public Page(int id, string name)
        {
            Id = id;
            Name = name;
            CreationDate = new DateTime(2021, 10, 13);
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
    }
}
