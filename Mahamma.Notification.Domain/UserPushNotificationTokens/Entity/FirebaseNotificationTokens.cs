using Mahamma.Base.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.UserPushNotificationTokens.Entity
{
    public class FirebaseNotificationTokens : Entity<int>, IAggregateRoot
    {
        public string FirebaseToken { get; private set; }
        public long UserId { get; private set; }

        public void CreateuserFirebaseToken(string firebaseToken, long userId)
        {
            FirebaseToken = firebaseToken;
            UserId = userId;

            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
        }

        public void DeleteFirebaseToken()
        {
            DeletedStatus = Base.Dto.Enum.DeletedStatus.Deleted.Id;
        }
    }
}
