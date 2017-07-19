using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;

namespace _2_OpenClosed.DataAccess.Queries
{
    public class RetrieveExternalIdToSkuMap : IQuery<Dictionary<int, string>>
    {
        private readonly IEnumerable<int> _externalIds;

        public RetrieveExternalIdToSkuMap(IEnumerable<int> externalIds)
        {
            _externalIds = externalIds;
        }

        public Dictionary<int, string> Run()
        {
            const string query = @" SELECT ExternalId, RTrim(Sku) AS Sku FROM dbo.Inventory I WITH(NOLOCK) 
                                    JOIN @ExternalIds E on I.ExternalId = E.ParseNumeric
                                    WHERE ParentId <> -1";

            var externalIdTable = new DataTable();
            externalIdTable.Columns.Add("ParseNumeric").DataType = Type.GetType("System.Int32");

            foreach (var externalId in _externalIds.Distinct())
            {
                var row = externalIdTable.NewRow();
                row["ParseNumeric"] = externalId;

                externalIdTable.Rows.Add(row);
            }

            externalIdTable.EndLoadData();

            using (var conn = DbConnection.GetReliableOpenConnection(DbInstance.BackOffice))
            {
                return conn.Query(query, new { @ExternalIds = externalIdTable.AsTableValuedParameter("IntegerList") }, null, true, DbSettings.CommandTimeout).ToDictionary(
                    row => (int)row.ExternalId,
                    row => (string)row.Sku);
            }
        }
    }
}
