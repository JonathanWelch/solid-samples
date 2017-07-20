using System.Collections.Generic;

namespace _5._1_DependencyInversion.Common.Infrastructure
{
    public class CsvLine : ICsvLine
    {
        public CsvLine(IEnumerable<string> values, IEnumerable<ValidationError> errors)
        {
            Values = values;
            Errors = errors;
        }

        public IEnumerable<string> Values { get; private set; }

        public IEnumerable<ValidationError> Errors { get; private set; }
    }
}
