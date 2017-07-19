using System.Data;
using System.Linq;
using Dapper;

namespace _1_SingleResponsibility_WebApi.Common.DataAccess.Queries
{
    public class RetrieveIwtByAdviceId : IQuery<DTO.IwtRequest>
    {
        private readonly string _adviceId;

        public RetrieveIwtByAdviceId(string adviceId)
        {
            _adviceId = adviceId;
        }

        public DTO.IwtRequest Run()
        {
            using (var conn = DbConnection.GetReliableOpenConnection(DbInstance.InterWarehouseTransfer))
            {
                return conn.Query<DTO.IwtRequest>(@"dbo.uspGetIwtAdviceId", new { IwtAdviceId = _adviceId }, commandType: CommandType.StoredProcedure, commandTimeout: DbSettings.CommandTimeout).FirstOrDefault();
            }
        }
    }
}