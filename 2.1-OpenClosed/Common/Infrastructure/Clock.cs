using System;

namespace _2._1_OpenClosed.Common.Infrastructure
{
    public class Clock
    {
        public virtual DateTime Now()
        {
            return DateTime.Now;
        }

        public virtual DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
