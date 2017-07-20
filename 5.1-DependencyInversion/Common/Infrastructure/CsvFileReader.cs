using System.Collections.Generic;

namespace _5._1_DependencyInversion.Common.Infrastructure
{
    public class CsvFileReader
    {
        private readonly CsvReader _csvReader;
        private readonly ICsvLineValidator _csvLineValidator;

        public CsvFileReader(CsvReader csvReader, ICsvLineValidator csvLineValidator)
        {
            _csvReader = csvReader;
            _csvLineValidator = csvLineValidator;
        }

        public IEnumerable<ICsvLine> ReadFile()
        {
            var csvLines = new List<ICsvLine>();

            while (_csvReader.ReadNext())
            {
                var currentLine = _csvReader.Current;
                var csvLine = _csvLineValidator.Validate(currentLine);
                csvLines.Add(csvLine);
            }

            return csvLines;
        }
    }
}
