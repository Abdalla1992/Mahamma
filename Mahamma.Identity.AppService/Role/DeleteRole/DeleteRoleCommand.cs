﻿using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Identity.AppService.Role.DeleteRole
{
   public class DeleteRoleCommand : IRequest<ValidateableResponse<ApiResponse<bool>>>
    {
        #region Props
        [DataMember]
        public int RoleId { get; set; }
        #endregion

        #region Method
        public DeleteRoleCommand(int id)
        {
            RoleId = id;
        }
        #endregion
    }
}
