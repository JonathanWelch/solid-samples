using System;

namespace _5._0_DependencyInversion.Common.EventLogger
{
    public interface IEventWriter : IDisposable
    {
        void WriteEvent(Event @event);
    }
}
