using System.Collections.Generic;
using System.IO;

namespace _5._1_DependencyInversion.Common.Infrastructure.FileProcessing
{
    public interface IFileProcessor
    {
        IEnumerable<DirectoryInfo> OutputDirectories { get; set; }

        void Process(FileInfo inputFile);
    }
}
