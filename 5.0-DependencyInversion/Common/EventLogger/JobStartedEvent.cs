namespace _5._0_DependencyInversion.Common.EventLogger
{
    public class JobStartedEvent : Event
    {
        public JobStartedEvent(string jobName)
        {
            Level = Level.Info;
            EventType = EventType.Started;
            EventDetail = new EventDetail(jobName, "Job has started");
        }
    }
}
