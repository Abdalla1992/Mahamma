using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.AppService.MemberSearch.GetProjectStatistics
{
    public class GetProjectStatisticsCommandValidator : AbstractValidator<GetProjectStatisticsCommand>
    {
        public GetProjectStatisticsCommandValidator()
        {
        }
    }
}
