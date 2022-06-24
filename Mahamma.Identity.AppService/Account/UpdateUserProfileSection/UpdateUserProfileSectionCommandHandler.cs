using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using Mahamma.Helper.EmailSending.IService;
using Mahamma.Identity.Domain.User.Entity;
using Mahamma.Identity.Domain.User.Repository;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.UpdateUserProfileSection
{
    public class UpdateUserProfileSectionCommandHandler : IRequestHandler<UpdateUserProfileSectionCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly IUserRepository _userRepository;
        public readonly UserManager<User> _userManager;
        private readonly IEmailSenderService _emailSenderService;
        #endregion

        #region CTRS
        public UpdateUserProfileSectionCommandHandler(IUserRepository userRepository, UserManager<User> userManager, IEmailSenderService emailSenderService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _emailSenderService = emailSenderService;
        }
        #endregion


        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(UpdateUserProfileSectionCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            User user = await _userRepository.GetUserById(request.UserId);
            if (user != null)
            {
                if (user.UserProfileSections != null)
                    user.UserProfileSections.Clear();
                else
                    user.UserProfileSections = new List<UserProfileSection>();


                request.UserProfileSections.ForEach(x => user.UserProfileSections.Add(new UserProfileSection().CreateUserSection(x.SectionId, x.OrderId)));

                if (await _userRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken))// && (await _roleManager.UpdateAsync(role)).Succeeded)
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "User section updated";
                }
                else
                {
                    response.Result.ResponseData = true;
                    response.Result.CommandMessage = "Failed to update user sections. Try again shortly.";
                }
            }
            else
            {
                response.Result.CommandMessage = "There is no user";
            }
            return response;
        }

        private string CreateRandomPassword(int length = 8)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string specialChars = "!@#$%^&*?_-";
            string capitalChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            string smallChars = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "0123456789";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < 3; i++)
            {
                chars[i] = smallChars[random.Next(0, smallChars.Length)];
            }
            for (int i = 3; i < 5; i++)
            {
                chars[i] = numbers[random.Next(0, numbers.Length)];
            }
            for (int i = 5; i < 7; i++)
            {
                chars[i] = capitalChars[random.Next(0, capitalChars.Length)];
            }
            for (int i = 7; i < 8; i++)
            {
                chars[i] = specialChars[random.Next(0, specialChars.Length)];
            }
            return new string(chars);
        }
    }
}
