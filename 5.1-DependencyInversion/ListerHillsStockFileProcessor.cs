using System.Collections.Generic;
using System.IO;
using System.Linq;
using _5._1_DependencyInversion.Common.DataAccess;
using _5._1_DependencyInversion.Common.DataAccess.Commands;
using _5._1_DependencyInversion.Common.DataAccess.DTO;
using _5._1_DependencyInversion.Common.DataAccess.Queries;
using _5._1_DependencyInversion.Common.Infrastructure;
using _5._1_DependencyInversion.Common.Infrastructure.FileProcessing;
using _5._1_DependencyInversion.Domain;

namespace _5._1_DependencyInversion
{
    public class ListerHillsStockFileProcessor : IFileProcessor
    {
        private readonly ICsvLineValidator csvLineValidator;

        private readonly ICommandAndQueryExecutor executor;

        public ListerHillsStockFileProcessor()
            : this(new ListerHillsStockLineValidator(), new CommandAndQueryExecutor())
        {
        }

        public ListerHillsStockFileProcessor(ICsvLineValidator csvLineValidator, ICommandAndQueryExecutor executor)
        {
            this.csvLineValidator = csvLineValidator;
            this.executor = executor;
        }

        public IEnumerable<DirectoryInfo> OutputDirectories { get; set; }

        public void Process(FileInfo inputFile)
        {
            using (var fileStream = new FileStream(inputFile.FullName, FileMode.Open))
            {
                var filename = ListerHillsStockFileName.Parse(inputFile.Name);

                var dateExportedUtc = filename.Date;

                var lastUpdate = this.executor.Query(new MostRecentlyExportedListerHillsFile());

                if (dateExportedUtc < lastUpdate)
                {
                    return;
                }

                var listerHillsStockReader = new CsvFileReader(new CsvReader(fileStream), this.csvLineValidator);
                var lines = listerHillsStockReader.ReadFile();

                var listerHillsStock = MapToStockItem(lines).ToList();

                this.executor.Execute(new PersistListerHillsStockLevel(listerHillsStock, dateExportedUtc));
            }
        }

        private static IEnumerable<StockItem> MapToStockItem(IEnumerable<ICsvLine> lines)
        {
            return from l in lines
                   where l.Errors.Any() == false
                   select new StockItem(l.Values.ElementAt(0), int.Parse(l.Values.ElementAt(1)));
        }
    }
}
