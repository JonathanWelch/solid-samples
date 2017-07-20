using System;
using System.Linq;
using System.Text.RegularExpressions;
using _5._1_DependencyInversion.Common;

namespace _5._1_DependencyInversion.Domain
{
    public class ListerHillsStockFileName
    {
        private const string FileNamePattern = @"ListerHills_(?<DateTime>[0-9]{14})\.(?<Extension>csv)";

        public ListerHillsStockFileName(DateTime date, string extension)
        {
            Date = date;
            FileExtension = extension;
        }

        public string FileExtension { get; private set; }

        public DateTime Date { get; private set; }

        public static ListerHillsStockFileName Parse(string filename)
        {
            var regex = new Regex(FileNamePattern, RegexOptions.IgnoreCase);
            var match = regex.Match(filename);

            if (!match.Success)
            {
                throw new InvalidFileNameException(filename);
            }

            var fileNameProperties = regex.GetGroupNames().ToDictionary(s => s, s => match.Groups[s].Value);
            DateTime fileNameDateTime;

            try
            {
                fileNameDateTime = DateTime.ParseExact(fileNameProperties["DateTime"], "yyyyMMddHHmmss", null);
            }
            catch (FormatException)
            {
                throw new FormatException(string.Format("DateTime format in file name is incorrect {0}", filename));
            }

            var fn = new ListerHillsStockFileName(fileNameDateTime, fileNameProperties["Extension"]);

            return fn;
        }
    }
}
