using System;

namespace _2_OpenClosed.Common.Utilities
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime GetDate()
        {
            return DateTime.Now;
        }
    }
}
