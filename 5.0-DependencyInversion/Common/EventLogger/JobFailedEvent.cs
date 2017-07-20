namespace _5._0_DependencyInversion.Common.EventLogger
{
    public class JobFailedEvent : Event
    {
        public JobFailedEvent(string jobName, string errorMessage)
        {
            Level = Level.Error;
            EventType = EventType.Failed;
            EventDetail = new EventDetail(jobName, errorMessage);
        }
    }
}
