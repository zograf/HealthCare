using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthCare.Domain.Services
{
    public class UtilityService
    {
        public static DateTime RemoveSeconds(DateTime dateTime)
        {
            int year = dateTime.Year;
            int month = dateTime.Month;
            int day = dateTime.Day;
            int hour = dateTime.Hour;
            int minute = dateTime.Minute;
            int second = 0;
            return new DateTime(year, month, day, hour, minute, second);
        }
        
        public static bool IsDateTimeOverlap(KeyValuePair<DateTime, DateTime> first, KeyValuePair<DateTime, DateTime> second)
        {
            return MaxDate(first.Key, second.Key) < MinDate(first.Value, second.Value);

        }
        
        public static bool IsDateTimeOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            return MaxDate(start1, start2) < MinDate(end1, end2);

        }

        public static DateTime MaxDate(DateTime time1, DateTime time2)
        {
            return (time1 > time2 ? time1 : time2);
        }

        public static DateTime MinDate(DateTime time1, DateTime time2)
        {
            return (time1 < time2 ? time1 : time2);
        }
    }
}
