using System;

namespace _2_OpenClosed.Domain.MDA
{
    public class MDAItemProcessedEvent : IEvent
    {
        public MDAItemProcessedEvent(
            string mdaNumber,
            int skuExternalId,
            short receiptThresholdAtTimeOfTransfer,
            int quantity,
            int receivedQuantityAtTimeOfTransfer,
            short transferPercent,
            int transferQuantity,
            DateTime processedDate,
            string destinationWarehouse)
        {
            MDANumber = mdaNumber;
            SKUExternalId = skuExternalId;
            ReceiptThresholdAtTimeOfTransfer = receiptThresholdAtTimeOfTransfer;
            Quantity = quantity;
            ReceivedQuantityAtTimeOfTransfer = receivedQuantityAtTimeOfTransfer;
            TransferPercent = transferPercent;
            TransferQuantity = transferQuantity;
            ProcessedDate = processedDate;
            DestinationWarehouse = destinationWarehouse;
        }

        public string MDANumber { get; set; }

        public int SKUExternalId { get; set; }

        public short ReceiptThresholdAtTimeOfTransfer { get; set; }

        public int Quantity { get; set; }

        public int ReceivedQuantityAtTimeOfTransfer { get; set; }

        public short TransferPercent { get; set; }

        public int TransferQuantity { get; set; }

        public DateTime ProcessedDate { get; set; }

        public string DestinationWarehouse { get; set; }
    }
}
