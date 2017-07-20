namespace _5._0_DependencyInversion.Common.EventLogger
{
    public class EventDetail
    {
        public EventDetail(string subComponent, string message)
        {
            Message = message;
            SubComponent = subComponent;
        }

        public EventDetail(string message)
        {
            Message = message;
            SubComponent = string.Empty;
        }

        public string Message { get; set; }

        public string SubComponent { get; set; }
    }
}
