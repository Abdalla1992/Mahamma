using AutoMapper;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Notification.Domain._SharedKernel;
using Mahamma.Notification.Domain.Notification.Dto;
using Mahamma.Notification.Domain.Notification.Entity;
using Mahamma.Notification.Domain.Notification.Repository;
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
    public class NotificationContentRepository : Base.EntityRepository<NotificationContent>, INotificationContentRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public NotificationContentRepository(IMapper mapper, NotificationContext context) : base(context, mapper)
        { }

        public void AddNotificationContent(NotificationContent notificationContent)
        {
            CreateAsyn(notificationContent);
        }

        public async Task<NotificationContent> GetNotificationContentByNotificationId(int notificationId , int languageId)
        {
            return await FirstOrDefaultAsync(s => s.NotificationId == notificationId && s.LanguageId == languageId);
        }

        public async Task<PageList<NotificationContent>> GetNotificationList(int skipCount, int takeCount, Expression<Func<NotificationContent, bool>> filter = null, string includeProperties = "")
        {
            PageList<NotificationContent> taskList = new();
            var notificationList = (await GetWhereAsync(filter, includeProperties)).Skip(skipCount);
            var Count = notificationList.Count();
            var resultList = notificationList.Take(takeCount);
            taskList.SetResult(Count, resultList.ToList());
            return taskList;
        }

        public async Task<PageList<NotificationContentDto>> GetMappedNotificationList(int skipCount, int takeCount, Expression<Func<NotificationContent, bool>> filter = null, string includeProperties = "")
        {
            PageList<NotificationContentDto> result = new();
            var notificationList = (await GetWhereAsync(filter, includeProperties)).Skip(skipCount);
            var Count = notificationList.Count();
            var resultList = notificationList.Take(takeCount).OrderByDescending(n => n.CreationDate);
            result.SetResult(Count, Mapper.Map<List<NotificationContentDto>>(resultList.ToList()));
            return result;
        }
    }
}
