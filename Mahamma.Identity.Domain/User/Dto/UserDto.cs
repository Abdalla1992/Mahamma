﻿using Mahamma.Base.Dto.Dto;
using System;

namespace Mahamma.Identity.Domain.User.Dto
{
    public class UserDto : BaseDto<long>
    {
        #region Props
        public string ProfileImage { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int WorkingDays { get; set; }
        public int WorkingHours { get; set; }
        public int CompanyId { get; set; }
        public string AuthToken { get; set; }
        public int UserProfileStatusId { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public int LanguageId { get; set; }
        public string Bio { get; set; }
        public string Skills { get; set; }
        #endregion
    }
}
