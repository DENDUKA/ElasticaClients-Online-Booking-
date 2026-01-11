using System;
using System.Globalization;

namespace ElasticaClients.Models
{
    public static class ModelHelper
    {
        public static string GetTimeFormat(DateTime StartTime)
        {
            return string.Format("{0} {1} {2:00}:{3:00}", StartTime.Day, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(StartTime.Month), StartTime.Hour, StartTime.Minute);
        }

        public static string GetDateFormat(DateTime StartTime)
        {
            return string.Format("{0} {1}", StartTime.Day, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(StartTime.Month));
        }

        public static string GetDateWithYearFormat(DateTime StartTime)
        {
            return string.Format("{0} {1} {2}", StartTime.Day, CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(StartTime.Month), StartTime.Year);
        }
    }
}