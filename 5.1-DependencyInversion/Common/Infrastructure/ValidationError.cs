namespace _5._1_DependencyInversion.Common.Infrastructure
{
    public class ValidationError
    {
        public ValidationError(Severity severity, string description)
        {
            Severity = severity;
            Description = description;
        }

        public Severity Severity { get; private set; }

        public string Description { get; private set; }
    }
}
