using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Account.ForgetPassword
{
    public class ForgetPasswordCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public string Email { get; set; }
        #endregion

        #region CTRS
        public ForgetPasswordCommand(string email)
        {
            Email = email;
        } 
        #endregion
    }
}
