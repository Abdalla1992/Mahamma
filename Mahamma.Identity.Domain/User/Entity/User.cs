using Mahamma.Base.Domain;
using Mahamma.Identity.Domain.Language.Enum;
using Microsoft.AspNetCore.Identity;
using System;

namespace Mahamma.Identity.Domain.User.Entity
{
    public class User : IdentityUser<long>, IAggregateRoot
    {
        #region Props
        public string ProfileImage { get; private set; }
        public string FullName { get; private set; }
        public string JobTitle { get; private set; }
        public int WorkingDays { get; private set; }
        public int WorkingHours { get; private set; }
        public int? CompanyId { get; set; }
        public int UserProfileStatusId { get; set; }
        public int LanguageId { get; set; }
        public string Bio { get; set; }
        public string Skills { get; set; }
        public DateTime CreationDate { get; set; }
        public int DeletedStatus { get; set; }
        #endregion

        #region Merhods

        public void CreateUser(string email, int userProfileStatusId, int? companyId = null)
        {
            UserName = email;
            Email = email;
            UserProfileStatusId = userProfileStatusId;
            CompanyId = companyId;
            CreationDate = DateTime.Now;
            DeletedStatus = Base.Dto.Enum.DeletedStatus.NotDeleted.Id;
            LanguageId = LanguageEnum.English.Id;
        }

        public void UpdateUser(string profileImage, string fullName, string jobTitle, int workingDays, int workingHours, int userProfileStatusId)
        {
            ProfileImage = profileImage;
            FullName = fullName;
            JobTitle = jobTitle;
            WorkingDays = workingDays;
            WorkingHours = workingHours;
            UserProfileStatusId = userProfileStatusId;
        }
        public void SetCompany(int companyId, int userProfileStatusId)
        {
            CompanyId = companyId;
            UserProfileStatusId = userProfileStatusId;
        }
        public void UpdateUserProfileStatus(int userProfileStatusId)
        {
            UserProfileStatusId = userProfileStatusId;
        }

        public void UpdateUserProfileSetting(string profileImage, string fullName, string jobTitle
            , string email, int languageId, string bio, string skills)
        {
            ProfileImage = profileImage;
            FullName = fullName;
            JobTitle = jobTitle;
            UserName = email;
            Email = email;
            UserName = email;
            LanguageId = languageId;
            Bio = bio;
            Skills = skills;
        }

        #endregion
    }
}
