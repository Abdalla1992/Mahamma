using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.Enum;
using Mahamma.Notification.Domain._SharedKernel;
using Mahamma.Notification.Domain.Notification.Entity;
using Mahamma.Notification.Domain.Notification.Repository;
using Mahamma.Notification.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mahamma.Notification.Infrastructure.Repositories
{
    public class NotificationRepository : Base.EntityRepository<Domain.Notification.Entity.Notification>,INotificationRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public NotificationRepository(IMapper mapper, NotificationContext context) : base(context, mapper)
        { }
        public void AddNotification(Domain.Notification.Entity.Notification notification)
        {
            CreateAsyn(notification);
        }

        public async Task<PageList<Domain.Notification.Entity.Notification>> GetNotificationList(int skipCount, int takeCount, Expression<Func<Domain.Notification.Entity.Notification, bool>> filter = null)
        {
            PageList<Domain.Notification.Entity.Notification> NotificationList = new();
            var remainingNotificationList = GetWhere(filter).Skip(skipCount);
            var Count = await remainingNotificationList.CountAsync();
            var resultList = remainingNotificationList.Take(takeCount);
            NotificationList.SetResult(Count, resultList.ToList());
            return NotificationList;
        }

        public async Task<PageList<Domain.Notification.Entity.Notification>> GetNotificationListForFirebaseUsers(int skipCount, int takeCount, Expression<Func<Domain.Notification.Entity.Notification, bool>> filter = null)
        {
            PageList<Domain.Notification.Entity.Notification> NotificationList = new();
            var notificationForFirebase = AppDbContext.Set<Domain.Notification.Entity.Notification>().Where(n =>
            AppDbContext.Set<Domain.UserPushNotificationTokens.Entity.FirebaseNotificationTokens>().Any(f => f.UserId == n.ReceiverUserId && f.DeletedStatus == DeletedStatus.NotDeleted.Id)).Where(filter);
            var remainingNotificationList = notificationForFirebase.Skip(skipCount);
            var Count = await remainingNotificationList.CountAsync();
            var resultList = remainingNotificationList.Take(takeCount);
            NotificationList.SetResult(Count, resultList.ToList());
            return NotificationList;
         }

        public async Task<PageList<Domain.Notification.Entity.Notification>> GetReadyToSendNotificationList(int skipCount, int takeCount, Expression<Func<Domain.Notification.Entity.Notification, bool>> filter = null)
        {
            PageList<Domain.Notification.Entity.Notification> NotificationList = new();
            int currentDayInMonth = DateTime.Now.Date.Day;
            int currentDayInWeek = (int)DateTime.Now.DayOfWeek;
            var notificationForFirebase = (from n in AppDbContext.Set<Domain.Notification.Entity.Notification>()
                                            from s in AppDbContext.Set<NotificationShedule>()
                                            where (s.UserId == n.ReceiverUserId
                                                && ((s.MonthDayId != null && s.MonthDayId == currentDayInMonth) || (s.WeekDayId > 0 && s.WeekDayId == currentDayInWeek))
                                                && (s.From.TimeOfDay <= DateTime.Now.TimeOfDay) && (s.To.TimeOfDay > DateTime.Now.TimeOfDay))
                                                || (AppDbContext.Set<NotificationShedule>().Count(s => s.UserId == n.ReceiverUserId) == 0)
                                            select n);

            notificationForFirebase = notificationForFirebase.Where(filter);
            var remainingNotificationList = notificationForFirebase.Skip(skipCount);
            var Count = await remainingNotificationList.CountAsync();
            var resultList = remainingNotificationList.Take(takeCount);
            NotificationList.SetResult(Count, resultList.ToList());
            return NotificationList;
        }

        private bool CheckForSchedule(Domain.Notification.Entity.Notification notification, NotificationShedule schedule)
        {
            int currentDayInMonth = 1;//DateTime.Now.Date.Day;
            int currentDayInWeek = 1;//(int)DateTime.Now.DayOfWeek;
            return schedule.UserId == notification.ReceiverUserId
                    && ((schedule.MonthDayId != null && schedule.MonthDayId == currentDayInMonth) || (schedule.WeekDayId > 0 && schedule.WeekDayId == currentDayInWeek))
                    && ((schedule.From.TimeOfDay <= DateTime.Now.TimeOfDay) && (schedule.To.TimeOfDay > DateTime.Now.TimeOfDay));
        }

        public void UpdateNotification(Domain.Notification.Entity.Notification notification)
        {
            Update(notification);
        }

        public async Task<int> GetNotificationCount(Expression<Func<Domain.Notification.Entity.Notification, bool>> filter = null)
        {
            return await GetCountAsync(filter);
        }

        private bool ScheduleIsNotFound(Domain.Notification.Entity.Notification notification)
        {
            return !(AppDbContext.NotificationShedule.Any(s => s.UserId == notification.ReceiverUserId));
        }
    }
}
