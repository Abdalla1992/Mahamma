using Mahamma.AppService.Settings;
using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain.Company.Entity;
using Mahamma.Domain.Company.Enum;
using Mahamma.Domain.Company.Repositroy;
using Mahamma.Helper.EmailSending.IService;
using Mahamma.Identity.ApiClient.Dto.User;
using Mahamma.Identity.ApiClient.Interface;
using Mahamma.Notification.ApiClient.Enum;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.SendInvitation
{
    public class SendInvitationCommandHandler : IRequestHandler<SendInvitationCommand, int>
    {
        #region Props
        private readonly ILogger<SendInvitationCommandHandler> _Logger;
        private readonly ICompanyInvitationRepository _companyInvitationRepository;
        private readonly BackGroundServiceSettings _backGroundServiceSettings;
        private readonly IEmailSenderService _emailSenderService;
        private readonly AppSetting _appSetting;
        private readonly IAccountService _accountService;
        #endregion

        #region CTRS
        public SendInvitationCommandHandler(IServiceProvider serviceProvider, BackGroundServiceSettings backGroundServiceSettings, AppSetting appSetting, ILogger<SendInvitationCommandHandler> logger, IEmailSenderService emailSenderService)
        {
            _companyInvitationRepository = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ICompanyInvitationRepository>();
            _backGroundServiceSettings = backGroundServiceSettings;
            _emailSenderService = emailSenderService;
            _appSetting = appSetting;
            _accountService = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IAccountService>();
            _Logger = logger;
        }
        #endregion

        public async Task<int> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
        {   
            PageList<CompanyInvitation> invitationList = await _companyInvitationRepository.GetInvitationList(request.SkipCount, _backGroundServiceSettings.TakeCount,
                c => c.InvitationStatusId == InvitationStatus.New.Id && !string.IsNullOrEmpty(c.Email));
            try
            {
                if (invitationList != null && invitationList.DataList.Any())
                {
                    foreach (var item in invitationList.DataList)
                    {
                        UserDto userDto = _accountService.GetUserByIdForBackgroundService(item.UserId);
                        string invitationLink = $"{_appSetting.InvitationLink}{item.InvitationId}";
                        if (userDto != null && File.Exists("EmailHTML/MahammaInvitationArabic.html") && File.Exists("EmailHTML/MahammaInvitationEnglish.html"))
                        {
                            StreamReader reader = userDto.LanguageId == Language.Arabic.Id ?
                                new StreamReader("EmailHTML/MahammaInvitationArabic.html") :
                                new StreamReader("EmailHTML/MahammaInvitationEnglish.html");
                            string body = reader.ReadToEnd();
                            string finalEmailBody = body.Replace("$$InvitationLink$$", invitationLink);
                            bool sent = await _emailSenderService.SendEmail("Member Invitation", finalEmailBody, item.Email);
                            if (sent)
                            {
                                item.UpdateInvitationStatus(InvitationStatus.Sent.Id);
                                _companyInvitationRepository.UpdateCompanyInvitation(item);
                            }
                            else
                            {
                                invitationList.SetResult(0, invitationList.DataList);
                            }
                        }
                        else
                        {
                            _Logger.LogError($"iInvitation is not sent Id: {item.Id}");
                        }
                    }
                }
            }
            finally
            {
                _companyInvitationRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken).Wait();
            }
            return invitationList.TotalCount;
        }


    }
}
