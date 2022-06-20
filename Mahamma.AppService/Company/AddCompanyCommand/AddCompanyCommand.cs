using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Base.Dto.MediatRExt;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Mahamma.AppService.Company.AddCompany
{
    public  class AddCompanyCommand : IRequest<ValidateableResponse<ApiResponse<int>>>
    {
        #region Prop
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string CompanySize { get; set; }
        [DataMember]
        public string Descreption { get; set; }
        [DataMember]
        public List<string> InvitationsEmails { get; set; }
        #endregion
    }
}
