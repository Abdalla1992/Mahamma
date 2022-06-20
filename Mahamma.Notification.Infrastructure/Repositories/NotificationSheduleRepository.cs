using AutoMapper;
using Mahamma.Base.Dto.Enum;
using Mahamma.Notification.Domain._SharedKernel;
using Mahamma.Notification.Domain.Notification.Dto;
using Mahamma.Notification.Domain.Notification.Entity;
using Mahamma.Notification.Domain.Notification.Repository;
using Mahamma.Notification.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Infrastructure.Repositories
{
    public class NotificationSheduleRepository : Base.EntityRepository<NotificationShedule>, INotificationSheduleRepository
    {
        public IUnitOfWork UnitOfWork => AppDbContext;
        public NotificationSheduleRepository(IMapper mapper, NotificationContext context) : base(context, mapper)
        { }

        public void CreateNotificationSchedule(NotificationShedule notificationShedule)
        {
            CreateAsyn(notificationShedule);
        }

        public async Task<NotificationShedule> GetnotificationSheduleByUserId(long userId)
        {
            //NotificationShedule notificationShedule = AppDbContext.NotificationShedule.SingleOrDefault(m => m.UserId == userId && m.DeletedStatus == DeletedStatus.NotDeleted.Id);
            //return notificationShedule;

            NotificationShedule notificationShedule = await FirstOrDefaultAsync(m => m.UserId == userId && m.DeletedStatus == DeletedStatus.NotDeleted.Id);
            return notificationShedule;


        }

        public async Task<NotificationShedule> GetEntityById(int id)
        {
            return await FirstOrDefaultAsync(m => m.Id == id);
        }

        public void UpdateNotificationShedule(NotificationShedule notiicationSchedule)
        {
            Update(notiicationSchedule);
        }

        public async Task<NotificationScheduleDto> GetUsernotificationShedule(long userId)
        {
            NotificationShedule notificationShedule =await FirstOrDefaultAsync(m => m.UserId == userId);
            return Mapper.Map<NotificationScheduleDto>(notificationShedule);
        }
    }
}
