using FluentValidation;
using Mahamma.Domain.Meeting.Repositroy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Meeting.CancelMeeting
{
    public class CancelMeetingValidator : AbstractValidator<CancelMeetingCommand>
    {
        private readonly IMeetingRepository _meetingRepository;
        public CancelMeetingValidator(IMeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
            RuleFor(command => command).Must(ValidDateForCancelMeeting).WithMessage("Invalid Date To Cancel Meeting");
        }

        private bool ValidDateForCancelMeeting(CancelMeetingCommand addTaskCommand)
        {
            return (_meetingRepository.ValidDate(addTaskCommand.MeetingId).Result);
        }

    }
}
