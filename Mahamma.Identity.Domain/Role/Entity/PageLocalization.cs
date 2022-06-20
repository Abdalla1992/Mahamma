using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.Role.Entity
{
    public class PageLocalization : Entity<int>, IAggregateRoot
    {
        public string DisplayName { get; set; }
        public int LanguageId { get; set; }
        public int PageId { get; set; }


        public PageLocalization(int id, string displayName,int languageId,int pageId)
        {
            Id = id;
            DisplayName=displayName;
            LanguageId = languageId;
            PageId = pageId;
            CreationDate = new DateTime(2021, 10, 13);
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }
    }
}
