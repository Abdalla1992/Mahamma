using System;

namespace Mahamma.Helper.EmailSending.Service
{
    public static class DateHelperService
    {
        public static string GetDurationInDaysMonthsYears(DateTime dateFrom, DateTime dateTo)
        {
            int totalHrs = Convert.ToInt32((dateTo - dateFrom).TotalHours);

            if(totalHrs >= 24)
            {
                int totalDays = Convert.ToInt32((dateTo - dateFrom).TotalDays);
                return $"{totalDays}d";
            }
            else
            {
                return $"{totalHrs}h";
            }
        }
    }
}
