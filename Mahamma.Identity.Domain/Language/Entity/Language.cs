using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Language.Entity
{
    public class Language : Entity<int>, IAggregateRoot
    {
        public string Name { get; set; }
        public string Alias { get; set; }
        public bool IsRtl { get; set; }
        public List<Domain.User.Entity.User> UserList { get; set; }

        public Language(int id ,string name,string alias,bool isRtl)
        {
            Id = id;
            Name = name;
            Alias = alias;
            IsRtl = isRtl;
            CreationDate = new DateTime(2021, 10, 31);
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }

        #region Methods
        public void CreateLanguage(string name , string alias)
        {
            Name = name;
            Alias = alias;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
        #endregion
    }
}
