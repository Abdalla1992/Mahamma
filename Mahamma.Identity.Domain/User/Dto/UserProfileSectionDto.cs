using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.Domain.User.Dto
{
    public class UserProfileSectionDto
    {
        #region Props
        public long UserId { get; set; }
        public int SectionId { get; set; }
        public int OrderId { get; set; }

        #endregion
    }
}
