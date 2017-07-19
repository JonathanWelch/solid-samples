using System.Data;
using System.Linq;
using Dapper;
using _1_SingleResponsibility_WebApi.Common.DataAccess.DTO;

namespace _1_SingleResponsibility_WebApi.Common.DataAccess.Queries
{
    public class RetrieveStockRequest : IQuery<StockRequestMessage>
    {
        private readonly int _messageNo;

        public RetrieveStockRequest(int messageNo)
        {
            _messageNo = messageNo;
        }

        public StockRequestMessage Run()
        {
            StockRequestMessage message;
            using (var conn = DbConnection.GetOpenConnection(DbInstance.InterWarehouseTransfer))
            {
                message = conn.Query<StockRequestMessage>(@"dbo.uspGetStockRequest", new { MessageNo = _messageNo }, commandType: CommandType.StoredProcedure, commandTimeout: DbSettings.CommandTimeout).FirstOrDefault();
            }

            return message;
        }
    }
}