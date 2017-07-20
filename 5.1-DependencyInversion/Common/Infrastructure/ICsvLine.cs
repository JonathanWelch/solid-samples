using System.Collections.Generic;

namespace _5._1_DependencyInversion.Common.Infrastructure
{
    public interface ICsvLine
    {
        IEnumerable<string> Values { get; }

        IEnumerable<ValidationError> Errors { get; }
    }
}
