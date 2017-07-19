using System;

namespace _2_OpenClosed.Domain.MDA
{
    public class MDABookedDateEvent : IEvent
    {
        public MDABookedDateEvent(string mdaNumber, DateTime bookedDate)
        {
            MDANumber = mdaNumber;
            BookedDate = bookedDate;
        }

        public string MDANumber { get; private set; }

        public DateTime BookedDate { get; private set; }
    }
}
