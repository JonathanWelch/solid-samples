using System;
using System.Data;
using System.Linq;
using Dapper;

namespace _5._0_DependencyInversion.Common.DataAccess.Queries
{
    public class RetrieveListerHillsLastUpdate : IQuery<DateTime>
    {
        public DateTime Run()
        {
            using (var conn = DbConnection.GetReliableOpenConnection(DbInstance.InterWarehouseTransfer))
            {
                return conn.Query<DateTime>("dbo.uspGetListerHillsLastUpdate", commandType: CommandType.StoredProcedure, commandTimeout: DbSettings.CommandTimeout).SingleOrDefault();
            }
        }
    }
}
