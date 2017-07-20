using System;
using System.IO;

namespace _5._1_DependencyInversion.Common
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2237:MarkISerializableTypesWithSerializable")]
    public class InvalidFileNameException : Exception
    {
        public InvalidFileNameException(FileSystemInfo file) : base(string.Format("The file name \"{0}\" (file \"{1}\") is not a recognised file name for IWT.", file.Name, file.FullName))
        {
        }

        public InvalidFileNameException(string filename) : base(string.Format("The file name \"{0}\" is not a recognised file name for IWT.", filename))
        {
        }
    }
}
