using System.Collections.Generic;
using Dapper;
using _2_OpenClosed.DataAccess.DTO;

namespace _2_OpenClosed.DataAccess.Queries
{
    public class RetrieveMerretMDAItemsList : IQuery<IEnumerable<MerretMDAItemDTO>>
    {
        private readonly IEnumerable<string> _mdaList;

        public RetrieveMerretMDAItemsList(IEnumerable<string> mdaList)
        {
            _mdaList = mdaList;
        }

        public IEnumerable<MerretMDAItemDTO> Run()
        {
            const string query = @" SELECT MDANumber, SkuNumber AS SkuExternalId, SUM(MDAQuantity) AS MDAQuantity, SUM(MDAReceivedQuantity) AS MDAReceivedQuantity
                                    FROM Persistence.MDAItem
                                    WHERE MDANumber IN @MDAList
                                    GROUP BY MDANumber, SkuNumber
                                    ";

            using (var conn = DbConnection.GetReliableOpenConnection(DbInstance.MerretReport))
            {
                return conn.Query<MerretMDAItemDTO>(query, new { MDAList = _mdaList }, commandTimeout: DbSettings.CommandTimeout);
            }
        }
    }
}
