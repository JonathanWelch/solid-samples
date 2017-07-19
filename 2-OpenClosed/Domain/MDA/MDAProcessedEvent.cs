using System;

namespace _2_OpenClosed.Domain.MDA
{
    public class MDAProcessedEvent : IEvent
    {
        public MDAProcessedEvent(string mdaNumber, DateTime processedDate)
        {
            MDANumber = mdaNumber;
            ProcessedDate = processedDate;
        }

        public string MDANumber { get; private set; }

        public DateTime ProcessedDate { get; private set; }
    }
}
