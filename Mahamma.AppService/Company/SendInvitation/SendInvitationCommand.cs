using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.Company.SendInvitation
{
    public class SendInvitationCommand : IRequest<int>
    {
        [DataMember]
        public int SkipCount { get; set; }
        public SendInvitationCommand(int skipCount)
        {
            SkipCount = skipCount;
        }
    }
}
