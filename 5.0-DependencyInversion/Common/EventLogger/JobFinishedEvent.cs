namespace _5._0_DependencyInversion.Common.EventLogger
{
    public class JobFinishedEvent : Event
    {
        public JobFinishedEvent(string jobName)
        {
            Level = Level.Info;
            EventType = EventType.Finished;
            EventDetail = new EventDetail(jobName, "Job has finished");
        }
    }
}
