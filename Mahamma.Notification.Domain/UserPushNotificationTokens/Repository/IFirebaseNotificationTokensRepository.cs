using Mahamma.Notification.Domain._SharedKernel;
using Mahamma.Notification.Domain.UserPushNotificationTokens.Entity;
using System.Threading.Tasks;

namespace Mahamma.Notification.Domain.UserPushNotificationTokens.Repository
{
    public interface IFirebaseNotificationTokensRepository : IRepository<Entity.FirebaseNotificationTokens>
    {
        void AddFirebaseToken(FirebaseNotificationTokens firebaseNotificationToken);
        void UpdateFirebaseToken(FirebaseNotificationTokens firebaseToken);
        Task<FirebaseNotificationTokens> GetFirebaseNotificationToken(string firebaseToken, long userId);
        Task<FirebaseNotificationTokens> GetFirebaseNotificationToken(long userId);
    }
}
