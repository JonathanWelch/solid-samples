namespace _5._0_DependencyInversion.Common.EventLogger
{
    public class Event
    {
        public Level Level { get; set; }

        public EventType EventType { get; set; }

        public EventDetail EventDetail { get; set; }
    }
}
