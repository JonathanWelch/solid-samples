using System;
using System.Data;
using System.Linq;
using Dapper;
using _1_SingleResponsibility_WebApi.Common.ApiModels;

namespace _1_SingleResponsibility_WebApi.Common.DataAccess.Commands
{
    public class PersistIwtStockRequest : ICommand
    {
        private readonly StockRequestModel allocationRequest;

        public PersistIwtStockRequest(StockRequestModel stockRequest)
        {
            if (stockRequest.Requests != null)
            {
                stockRequest.Requests = stockRequest.Requests.GroupBy(x => new { x.Sku, x.TmId })
                    .Select(x => new ApiModels.Stock
                    {
                        Quantity = x.Sum(y => y.Quantity),
                        Sku = x.Key.Sku,
                        TmId = x.Key.TmId
                    }).ToList();
            }

            allocationRequest = stockRequest;
        }

        public void Execute()
        {
            using (var conn = DbConnection.GetOpenConnection(DbInstance.InterWarehouseTransfer))
            {
                var stockTable = new DataTable();
                stockTable.Columns.Add("Sku");
                stockTable.Columns.Add("Quantity").DataType = Type.GetType("System.Int32");
                stockTable.Columns.Add("TmId");

                if (allocationRequest.Requests != null && allocationRequest.Requests.Any())
                {
                    foreach (var request in allocationRequest.Requests)
                    {
                        var row = stockTable.NewRow();
                        row[0] = request.Sku;
                        row[1] = request.Quantity;

                        // HACK: Quick fix to get deallocate to support multiple SKUs
                        if (!string.IsNullOrEmpty(request.TmId))
                        {
                            row[2] = request.TmId;
                        }
                        else
                        {
                            row[2] = "0";
                        }

                        stockTable.Rows.Add(row);
                    }
                }

                stockTable.EndLoadData();

                var parameters = new DynamicParameters(new
                {
                    allocationRequest.IwtAdviceId,
                    allocationRequest.MessageNo,
                    StockRequestTypeId = allocationRequest.StockRequestType,
                    RequestList = stockTable.AsTableValuedParameter("dbo.RequestList")
                });

                conn.Execute("[dbo].[uspCreateStockRequest]", parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}