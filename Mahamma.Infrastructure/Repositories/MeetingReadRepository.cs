using Mahamma.Base.Dto.ApiResponse;
using Mahamma.Domain._SharedKernel;
using Mahamma.Domain.Company.Dto;
using Mahamma.Domain.Company.Repositroy;
using Mahamma.Domain.Meeting.Dto;
using Mahamma.Domain.Meeting.Repositroy;
using Mahamma.Infrastructure.Base;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mahamma.Infrastructure.Repositories
{
    public class MeetingReadRepository : DapperRepository, IMeetingReadRepository
    {
        #region Ctor
        public MeetingReadRepository(IConfiguration config) : base(config)
        {

        }
        #endregion

        public async Task<List<MinuteOfMeetingActionDto>> GetMinutesOfMeeting(int meetingId, long userId)
        {
            List<MinuteOfMeetingActionDto> minuteOfMeetingActionDto = new();
            var queryParams = new Dapper.DynamicParameters();
            queryParams.Add("@MeetingId", meetingId, System.Data.DbType.Int64, System.Data.ParameterDirection.Input);
            queryParams.Add("@UserId", userId, System.Data.DbType.Int64, System.Data.ParameterDirection.Input);

            minuteOfMeetingActionDto = (await GetListAsync<MinuteOfMeetingActionDto>("dbo.sp_GetMinutesOfMeeting", queryParams, System.Data.CommandType.StoredProcedure)).ToList();
            return minuteOfMeetingActionDto;
        }
    }
}
