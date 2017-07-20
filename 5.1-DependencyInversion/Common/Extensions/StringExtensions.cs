using System;
using System.Text;

namespace _5._1_DependencyInversion.Common.Extensions
{
    public static class StringExtensions
    {
        public static string AppendTimeStamp(this string str)
        {
            var fileNameParts = str.Split('.');

            var fileNameBuilder = new StringBuilder();

            for (var i = 0; i < fileNameParts.Length - 1; i++)
            {
                fileNameBuilder.Append(fileNameParts[i]);
                fileNameBuilder.Append(".");
            }

            fileNameBuilder.Append(DateTime.UtcNow.ToString("yyyyMMddHHmmssff"));
            fileNameBuilder.Append(".");
            fileNameBuilder.Append(fileNameParts[fileNameParts.Length - 1]);

            return fileNameBuilder.ToString();
        }

        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }
    }
}
