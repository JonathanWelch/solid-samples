using System;
using System.Data;
using Dapper;

namespace _2_OpenClosed.DataAccess.Command
{
    public class MarkMdaAsProcessedCommand : ICommand
    {
        private readonly string mdaNumber;

        private readonly DateTime date;

        public MarkMdaAsProcessedCommand(string mdaNumber, DateTime date)
        {
            this.mdaNumber = mdaNumber;
            this.date = date;
        }

        public void Execute()
        {
            using (var conn = DbConnection.GetReliableOpenConnection(DbInstance.InterWarehouseTransfer))
            {
                conn.Execute("dbo.uspMarkMdaAsProcessed", new { MDANumber = mdaNumber, Date = date }, commandType: CommandType.StoredProcedure, commandTimeout: DbSettings.CommandTimeout);
            }
        }
    }
}
