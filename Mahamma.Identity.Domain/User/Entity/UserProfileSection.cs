using Mahamma.Base.Domain;
using System;

namespace Mahamma.Identity.Domain.User.Entity
{
    public class UserProfileSection : Entity<int>, IAggregateRoot
    {
        #region Props
        public long UserId { get; set; }
        public int SectionId { get; set; }
        public int OrderId { get; set; }

        #endregion

        #region Methods
        public UserProfileSection CreateUserSection(int sectionId, int orderId)
        {
            OrderId = orderId;
            SectionId = sectionId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
            return this;
        }
        #endregion
    }
}
