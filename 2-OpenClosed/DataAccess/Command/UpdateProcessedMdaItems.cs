using System;
using System.Data;
using Dapper;

namespace _2_OpenClosed.DataAccess.Command
{
    public class UpdateProcessedMdaItem : ICommand
    {
        private readonly string _mdaNumber;
        private readonly int _skuExternalId;
        private readonly short _receiptThreshold;
        private readonly int _quantity;
        private readonly int _receivedQuantity;
        private readonly int _transferPercent;
        private readonly int _transferQuantity;
        private readonly DateTime _processedDate;
        private readonly string _destinationWarehouse;

        public UpdateProcessedMdaItem(string mdaNumber, int skuExternalId, short receiptThreshold, int quantity, int receivedQuantity, int transferPercent, int transferQuantity, DateTime processedDate, string destinationWarehouse)
        {
            _mdaNumber = mdaNumber;
            _skuExternalId = skuExternalId;
            _receiptThreshold = receiptThreshold;
            _quantity = quantity;
            _receivedQuantity = receivedQuantity;
            _transferPercent = transferPercent;
            _transferQuantity = transferQuantity;
            _processedDate = processedDate;
            _destinationWarehouse = destinationWarehouse;
        }

        public void Execute()
        {
            using (var conn = DbConnection.GetReliableOpenConnection(DbInstance.InterWarehouseTransfer))
            {
                conn.Execute(
                    "dbo.uspUpdateProcessedMdaItem",
                    new
                    {
                        MDANumber = _mdaNumber,
                        SkuExternalId = _skuExternalId,
                        ReceiptThreshold = _receiptThreshold,
                        Quantity = _quantity,
                        ReceivedQuantity = _receivedQuantity,
                        TransferQuantity = _transferQuantity,
                        TransferPercent = _transferPercent,
                        ProcessedDate = _processedDate,
                        DestinationWarehouse = _destinationWarehouse
                    },
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: DbSettings.CommandTimeout);
            }
        }
    }
}
