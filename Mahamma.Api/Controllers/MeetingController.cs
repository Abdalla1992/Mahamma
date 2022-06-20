using Mahamma.Api.Infrastructure.Base;
using Mahamma.Base.Domain.Enum;
using Mahamma.Base.Domain.ApiActions.Enum;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mahamma.AppService.Meeting.AddMeeting;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.AppService.Meeting.CancelMeeting;
using Mahamma.AppService.Meeting.UpdateMeeting;
using Mahamma.AppService.Meeting.GetMeeting;
using Mahamma.AppService.Meeting.AddMinuteOfMeeting;
using Mahamma.AppService.Meeting.GetMinutesMeeting;
using Mahamma.AppService.Meeting.UpdateMinuteOfMeeting;
using Mahamma.AppService.Meeting.DeleteMinuteOfMeeting;
using Mahamma.AppService.Meeting.PublishMinuteOfMeeting;

namespace Mahamma.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route(Route.API)]
    public class MeetingController : BaseController
    {
        #region Ctor
        public MeetingController(IMediator mediator) : base(mediator)
        { }
        #endregion

        #region Methods
        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ManageMeetings, PermissionId = (int)Permission.ViewMeeting)]
        public async Task<IActionResult> GetAll([FromBody] SearchMeetingDto listMeetingCommand)
        {
            return Ok(await Mediator.Send(listMeetingCommand));
        }

        [HttpGet]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ManageMeetings, PermissionId = (int)Permission.ViewMeeting)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetMeetingCommand(id)));
        }

        [HttpPost]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ManageMeetings, PermissionId = (int)Permission.AddMeeting)]
        public async Task<IActionResult> Add([FromBody] AddMeetingCommand addMeetingCommand)
        {
            return Ok(await Mediator.Send(addMeetingCommand));
        }

        [HttpPut]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ManageMeetings, PermissionId = (int)Permission.UpdateMeeting)]
        public async Task<IActionResult> Update([FromBody] UpdateMeetingCommand updateMeetingCommand)
        {
            return Ok(await Mediator.Send(updateMeetingCommand));
        }

        [HttpDelete]
        [Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.ManageMeetings, PermissionId = (int)Permission.DeleteMeeting)]
        public async Task<IActionResult> Cancel(int id)
        {
            return Ok(await Mediator.Send(new CancelMeetingCommand(id)));
        }

        [HttpGet]
        //[Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.MeetingProfile, PermissionId = (int)Permission.ViewMeeting)]
        public async Task<IActionResult> GetMinutesMeeting(int id)
        {
            return Ok(await Mediator.Send(new GetMinutesMeetingCommand(id)));
        }

        [HttpPost]
        //[Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.MeetingProfile, PermissionId = (int)Permission.AddMeeting)]
        public async Task<IActionResult> AddMinuteOfMeeting([FromBody] AddMinuteOfMeetingCommand addMinuteOfMeetingCommand)
        {
            return Ok(await Mediator.Send(addMinuteOfMeetingCommand));
        }

        [HttpPost]
        //[Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.MeetingProfile, PermissionId = (int)Permission.AddMeeting)]
        public async Task<IActionResult> UpdateMinuteOfMeeting([FromBody] UpdateMinuteOfMeetingCommand updateMinuteOfMeetingCommand)
        {
            return Ok(await Mediator.Send(updateMinuteOfMeetingCommand));
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteMinuteOfMeeting(int id, int meetingId)
        {
            return Ok(await Mediator.Send(new DeleteMinuteOfMeetingCommand(id, meetingId)));
        }

        [HttpPost]
        //[Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.MeetingProfile, PermissionId = (int)Permission.)]
        public async Task<IActionResult> PublishMinuteOfMeeting([FromBody] PublishMinuteOfMeetingCommand publishMinuteOfMeetingCommand)
        {
            return Ok(await Mediator.Send(publishMinuteOfMeetingCommand));
        }

        ///// <summary>
        ///// Get Workspace By Id
        ///// </summary>
        ///// <param name="getMeetingQuery"></param>
        ///// <returns></returns>
        //[HttpGet]

        //[Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.MeetingProfile, PermissionId = (int)Permission.ViewMeeting)]
        //public async Task<IActionResult> Get(int id)
        //{
        //    return Ok(await Mediator.Send(new GetMeetingQuery(id)));
        //}

        //[HttpPut]
        //[Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.MeetingProfile, PermissionId = (int)Permission.UpdateMeeting)]
        //public async Task<IActionResult> Update([FromBody] UpdateMeetingCommand updateMeetingCommand)
        //{
        //    return Ok(await Mediator.Send(updateMeetingCommand));
        //}

        //[HttpDelete]
        //[Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.MeetingProfile, PermissionId = (int)Permission.DeleteMeeting)]
        //[AllowAnonymous]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    return Ok(await Mediator.Send(new DeleteMeetingCommand(id)));
        //}

        ///// <summary>
        ///// Assign Member
        ///// </summary>
        ///// <param name="AssignMemberCommand"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Infrastructure.Filter.AuthorizeAttributeFactory(PageId = (int)PageEnum.MeetingProfile, PermissionId = (int)Permission.AssignMember)]
        //public async Task<IActionResult> AssignMember([FromBody] AssignMemberMeetingCommand assignMemberCommand)
        //{
        //    return Ok(await Mediator.Send(assignMemberCommand));
        //}
        #endregion
    }
}
