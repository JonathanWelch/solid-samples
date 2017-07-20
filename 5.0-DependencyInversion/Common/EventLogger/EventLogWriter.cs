using System.Diagnostics;
using Newtonsoft.Json;

namespace _5._0_DependencyInversion.Common.EventLogger
{
    public class EventLogWriter : IEventWriter
    {
        private static EventLog eventLog;

        public EventLogWriter(string source)
        {
            eventLog = new EventLog { Source = source };
        }

        public virtual void WriteEvent(Event @event)
        {
            var logEntryType = MapLevelToEventLogEntryType(@event.Level);

            var message = JsonConvert.SerializeObject(@event.EventDetail);

            eventLog.WriteEntry(message, logEntryType, (int)@event.EventType);
        }

        private static EventLogEntryType MapLevelToEventLogEntryType(Level level)
        {
            switch (level)
            {
                case Level.Error:
                    return EventLogEntryType.Error;
                case Level.Warning:
                    return EventLogEntryType.Warning;
            }

            return EventLogEntryType.Information;
        }

        public void Dispose()
        {
            if (eventLog != null)
            {
                eventLog.Dispose();
            }
        }
    }
}
