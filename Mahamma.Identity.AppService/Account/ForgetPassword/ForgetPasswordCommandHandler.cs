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

namespace Mahamma.Identity.AppService.Account.ForgetPassword
{
    public class ForgetPasswordCommandHandler : IRequestHandler<ForgetPasswordCommand, ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        private readonly IUserRepository _userRepository;
        public readonly UserManager<User> _userManager;
        private readonly IEmailSenderService _emailSenderService;
        #endregion

        #region CTRS
        public ForgetPasswordCommandHandler(IUserRepository userRepository, UserManager<User> userManager, IEmailSenderService emailSenderService)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _emailSenderService = emailSenderService;
        }
        #endregion


        public async Task<ValidateableResponse<ApiResponse<bool>>> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            ValidateableResponse<ApiResponse<bool>> response = new(new ApiResponse<bool>());
            string newPassword = CreateRandomPassword(8);
            User user = await _userRepository.GetUserByEmail(request.Email);
            if (user != null)
            {
                string passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                if (!string.IsNullOrWhiteSpace(passwordToken))
                {
                    IdentityResult identityResult = await _userManager.ResetPasswordAsync(user, passwordToken, newPassword);
                    if (identityResult.Succeeded)
                    {
                        try
                        {
                            if (await _emailSenderService.SendEmail("Your new password", $"Kindly find your new password: {newPassword}", request.Email))
                            {
                                response.Result.ResponseData = true;
                                response.Result.CommandMessage = "Please check your email, an email with a new password sent to your email";
                            }
                            else
                            {
                                response.Result.CommandMessage = "Failed to send new password email, please try again later";
                            }
                        }
                        catch (Exception ex)
                        {
                            response.Result.CommandMessage = "Failed to send new password email, please try again later";
                            return response;
                        }
                    }
                    else
                    {
                        response.Result.CommandMessage = "Failed to send new password email, please try again later";
                    }
                }
            }
            else
            {
                response.Result.CommandMessage = "There is no user for this email";
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
