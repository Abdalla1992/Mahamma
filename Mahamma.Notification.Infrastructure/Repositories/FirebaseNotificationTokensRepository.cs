using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Notification.Domain._SharedKernel;
using Mahamma.Notification.Domain.Notification.Repository;
using Mahamma.Notification.Domain.UserPushNotificationTokens.Entity;
using Mahamma.Notification.Domain.UserPushNotificationTokens.Repository;
using Mahamma.Notification.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Infrastructure.Repositories
{
    public class FirebaseNotificationTokensRepository : Base.EntityRepository<FirebaseNotificationTokens>, IFirebaseNotificationTokensRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public FirebaseNotificationTokensRepository(IMapper mapper, NotificationContext context) : base(context, mapper)
        { }
        public void AddFirebaseToken(FirebaseNotificationTokens firebaseToken)
        {
            CreateAsyn(firebaseToken);
        }

        public void UpdateFirebaseToken(FirebaseNotificationTokens firebaseToken)
        {
            Update(firebaseToken);
        }

        public async Task<FirebaseNotificationTokens> GetFirebaseNotificationToken(string firebaseToken, long userId)
        {
            return await FirstOrDefaultAsync(s => s.FirebaseToken == firebaseToken && s.UserId == userId);
        }

        public async Task<FirebaseNotificationTokens> GetFirebaseNotificationToken(long userId)
        {
            return await FirstOrDefaultAsync(s => s.DeletedStatus == DeletedStatus.NotDeleted.Id && s.UserId == userId);
        }
    }
}
