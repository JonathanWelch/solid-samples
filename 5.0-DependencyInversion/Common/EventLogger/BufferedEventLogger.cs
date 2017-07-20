using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _5._0_DependencyInversion.Common.Extensions;

namespace _5._0_DependencyInversion.Common.EventLogger
{
    public class BufferedEventLogger : EventLogger, IDisposable
    {
        private readonly int _bufferSize;
        private readonly object _lock;
        private readonly Timer _timer;
        private readonly int _minimumIntervalToLogInSeconds = 10;
        private readonly DateTime _lastProcessedTime = DateTime.MinValue;
        private ConcurrentQueue<Event> _events;

        public BufferedEventLogger(string source)
            : this(new EventLogWriter(source))
        {
        }

        public BufferedEventLogger(IEventWriter eventWriter)
        {
            Writer = eventWriter;
            this._bufferSize = 10; // Size TBC
            this._events = new ConcurrentQueue<Event>();
            this._lock = new object();
            this._timer = new Timer(x => Process(), null, TimeSpan.Zero, new TimeSpan(0, 0, this._minimumIntervalToLogInSeconds));
        }

        public override void LogEvent(Event @event)
        {
            lock (this._lock)
            {
                this._events.Enqueue(@event);
            }

            if (this._events.Count >= this._bufferSize)
            {
                Process();
            }
        }

        public void Dispose()
        {
            if (this._timer != null)
            {
                this._timer.Dispose();
            }

            if (Writer != null)
            {
                Writer.Dispose();
            }
        }

        private void Process()
        {
            var diff = DateTime.Now - this._lastProcessedTime;

            if (!(diff.TotalSeconds >= this._minimumIntervalToLogInSeconds))
            {
                return;
            }

            var processList = this.GetEventsToBeProcessed();
            foreach (var @event in processList)
            {
                base.LogEvent(@event);
            }
        }

        private IEnumerable<Event> GetEventsToBeProcessed()
        {
            var processList = new List<Event>();

            if (!this._events.Any())
            {
                return processList;
            }

            lock (this._lock)
            {
                processList = this._events.DistinctBy(x => new { EventId = x.EventType, x.EventDetail.Message }).ToList();
                this._events = new ConcurrentQueue<Event>();
            }

            return processList;
        }
    }
}
