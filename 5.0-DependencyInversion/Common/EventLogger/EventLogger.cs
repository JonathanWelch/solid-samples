namespace _5._0_DependencyInversion.Common.EventLogger
{
    public abstract class EventLogger
    {
        protected IEventWriter Writer { get; set; }

        public virtual void LogEvent(Event @event)
        {
            Writer.WriteEvent(@event);
        }
    }
}
