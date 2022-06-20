using Mahamma.Notification.Api.Infrastructure.Base;
using Mahamma.Notification.AppService.Notification.AddNotification;
using Mahamma.Notification.AppService.Notification.AddNotificationList;
using Mahamma.Notification.AppService.Notification.AddNotificationShedule;
using Mahamma.Notification.AppService.Notification.DeleteNotificationSchedule;
using Mahamma.Notification.AppService.Notification.GetNotifications;
using Mahamma.Notification.AppService.Notification.GetNotificationsCount;
using Mahamma.Notification.AppService.Notification.GetUserNotificationSchedule;
using Mahamma.Notification.AppService.Notification.UpdateNotiicationSchedule;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Mahamma.Notification.Api.Controllers
{
    [ApiController]
    [Route(Route.API)]
    public class NotificationController : BaseController
    {
        #region Ctor
        public NotificationController(IMediator mediator) : base(mediator)
        { }
        #endregion

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetNotificationsCommand()));
        }

        [HttpGet]
        public async Task<IActionResult> GetCount()
        {
            return Ok(await Mediator.Send(new GetNotificationsCountCommand()));
        }

        [HttpGet]
        public async Task<IActionResult> AllNotificationsSeen()
        {
            return Ok(await Mediator.Send(new AllNotificationsSeenCommand()));
        }

        [HttpPost]
        public async Task<IActionResult> AddList([FromBody] AddNotificationListCommand addNotificationListCommand)
        {
            return Ok(await Mediator.Send(addNotificationListCommand));
        }
        [HttpPost]
        public async Task<IActionResult> AddNotificationSchedule([FromBody] AddNotificationScheduleCommand addNotificationScheduleCommand)
        {
            return Ok(await Mediator.Send(addNotificationScheduleCommand));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateNotificationSchedule([FromBody] UpdateNotiicationScheduleCommand updateNotiicationScheduleCommand)
        {
            return Ok(await Mediator.Send(updateNotiicationScheduleCommand));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteNotificationSchedule(int id)
        {
            return Ok(await Mediator.Send(new DeleteNotificationScheduleCommand(id)));
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotificationSchedule(long userId)
        {
            return Ok(await Mediator.Send(new GetUserNotificationScheduleQuery(userId)));
        }
    }
}
