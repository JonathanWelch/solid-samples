using System.Collections.Generic;
using Dapper;
using _2_OpenClosed.DataAccess.DTO;

namespace _2_OpenClosed.DataAccess.Queries
{
    public class RetrieveMerretMDAList : IQuery<IEnumerable<MerretMDADTO>>
    {
        private readonly IEnumerable<string> _mdaList;

        public RetrieveMerretMDAList(IEnumerable<string> mdaList)
        {
            _mdaList = mdaList;
        }

        public IEnumerable<MerretMDADTO> Run()
        {
            const string query = @" SELECT MDANumber, BookingDateTime
                                    FROM Persistence.MDA
                                    WHERE MDANumber IN @MDAList";

            using (var conn = DbConnection.GetReliableOpenConnection(DbInstance.MerretReport))
            {
                return conn.Query<MerretMDADTO>(query, new { MDAList = _mdaList }, commandTimeout: DbSettings.CommandTimeout);
            }
        }
    }
}
