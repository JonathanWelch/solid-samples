using System;
using System.Data;
using System.Linq;
using Dapper;

namespace _5._1_DependencyInversion.Common.DataAccess.Queries
{
    public class MostRecentlyExportedListerHillsFile : IQuery<DateTime>
    {
        public DateTime Run()
        {
            using (var conn = DbConnection.GetReliableOpenConnection(DbInstance.InterWarehouseTransfer))
            {
                return conn.Query<DateTime>("uspGetListerHillDateExported", commandType: CommandType.StoredProcedure, commandTimeout: DbSettings.CommandTimeout).FirstOrDefault();
            }
        }
    }
}
