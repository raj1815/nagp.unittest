using System;
using System.Collections.Generic;
using System.Text;

namespace Nagp.UnitTest.Business.Common
{
    public class Helper
    {
        public static bool isTradingTime()
        {
            TimeSpan start = new TimeSpan(9, 0, 0); //10 o'clock
            TimeSpan end = new TimeSpan(15, 0, 0); //12 o'clock
            TimeSpan now = DateTime.Now.TimeOfDay;
            DayOfWeek dayOfWeek = DateTime.Now.DayOfWeek;
            return ((now > start) && (now < end)) && dayOfWeek > DayOfWeek.Sunday && dayOfWeek < DayOfWeek.Saturday;
        }
    }
}
