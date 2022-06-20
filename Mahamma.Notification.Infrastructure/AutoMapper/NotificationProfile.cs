using AutoMapper;
using Mahamma.Helper.EmailSending.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Notification.Infrastructure.AutoMapper
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            NotificationMapping();
            NotificationScheduleMapping();


        }

      

        private void NotificationMapping()
        {
            CreateMap<Domain.Notification.Entity.Notification, Domain.Notification.Dto.NotificationDto>();
            CreateMap<Domain.Notification.Entity.NotificationContent, Domain.Notification.Dto.NotificationContentDto>()
                .BeforeMap((src, dest) => dest.CreationDuration = DateHelperService.GetDurationInDaysMonthsYears(src.CreationDate, DateTime.Now))
                .ForMember(dto => dto.Notification, act => act.MapFrom(src => src.Notification));
        }

        private void NotificationScheduleMapping()
        {
            CreateMap<Domain.Notification.Entity.NotificationShedule, Domain.Notification.Dto.NotificationScheduleDto>()
                .ForMember(edto => edto.From, dto => dto.MapFrom(e => e.From))
                .ForMember(edto => edto.To, dto => dto.MapFrom(e => e.To));
        }
    }
}
