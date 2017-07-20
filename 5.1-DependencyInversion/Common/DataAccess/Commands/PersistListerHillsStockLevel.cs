using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using _5._1_DependencyInversion.Common.DataAccess.DTO;

namespace _5._1_DependencyInversion.Common.DataAccess.Commands
{
    public class PersistListerHillsStockLevel : ICommand
    {
        private readonly IList<StockItem> stockItems;
        private readonly DateTime dateExportedUtc;
        private int timeoutInSeconds = 300;

        public PersistListerHillsStockLevel(IList<StockItem> stockItems, DateTime dateExportedUtc)
        {
            this.stockItems = stockItems;
            this.dateExportedUtc = dateExportedUtc;
        }

        public int TimeoutInSeconds
        {
            get { return timeoutInSeconds; }
            set { timeoutInSeconds = value; }
        }

        public void Execute()
        {
            using (var connection = DbConnection.GetOpenConnection(DbInstance.InterWarehouseTransfer))
            {
                using (var tran = connection.BeginTransaction(IsolationLevel.ReadUncommitted))
                {
                    var listerHillsFileId = connection.Query<int>("dbo.uspListerHillsFileMerge", new { DateExportedUtc = dateExportedUtc }, tran, commandTimeout: TimeoutInSeconds, commandType: CommandType.StoredProcedure).First();

                    var table = new DataTable("InputSkus");
                    table.Columns.Add("Sku", typeof(string));
                    table.Columns.Add("InStock", typeof(int));
                    table.Columns.Add("DateCreatedUtc", typeof(DateTime));
                    table.Columns.Add("ListerHillsFileId", typeof(int));

                    foreach (var item in stockItems)
                    {
                        table.Rows.Add(item.Sku, item.Available, DateTime.UtcNow, listerHillsFileId);
                    }

                    using (var bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, (SqlTransaction)tran))
                    {
                        bulkCopy.DestinationTableName = "dbo.ListerHillsStockLevel_Staging";
                        bulkCopy.BulkCopyTimeout = TimeoutInSeconds;
                        bulkCopy.WriteToServer(table);
                    }

                    connection.Execute("dbo.uspUpdateListerHillsStockLevelMerge", transaction: tran, commandTimeout: DbSettings.CommandTimeout, commandType: CommandType.StoredProcedure);

                    connection.Execute("dbo.uspTruncate", new { TableName = "ListerHillsStockLevel_Staging" }, tran, commandType: CommandType.StoredProcedure, commandTimeout: DbSettings.CommandTimeout);

                    tran.Commit();
                }
            }
        }
    }
}
