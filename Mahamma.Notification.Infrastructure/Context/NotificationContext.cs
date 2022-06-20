using Mahamma.Notification.Domain._SharedKernel;
using Mahamma.Notification.Domain.Notification.Entity;
using Mahamma.Notification.Domain.UserPushNotificationTokens.Entity;
using Mahamma.Notification.Infrastructure.EntityConfiguration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Notification.Infrastructure.Context
{
   public class NotificationContext : DbContext, IUnitOfWork
    {
        #region DBSet

        public DbSet<Domain.Notification.Entity.Notification> Notification { get; set; }
        public DbSet<NotificationContent> NotificationContent { get; set; }
        public DbSet<FirebaseNotificationTokens> FirebaseNotificationTokens { get; set; }
        public DbSet<Domain.Notification.Entity.NotificationShedule> NotificationShedule { get; set; }

        #endregion

        #region CTRS
        public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
        {
        }
        #endregion

        #region Model Creation
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new NotificationEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationContentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new FirebaseNotificationTokensEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new NotificationSheduleEntityTypeConfiguration());

            modelBuilder.BuildEnums();
        }
        #endregion

        #region Save Object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await base.SaveChangesAsync(cancellationToken) > default(int);
            return result;
        }
        #endregion

    }
}
