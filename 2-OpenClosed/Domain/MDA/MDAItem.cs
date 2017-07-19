using System;
using _2_OpenClosed.Common.Utilities;

namespace _2_OpenClosed.Domain.MDA
{
    public class MDAItem
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly MDA _parent;

        public MDAItem(
            IDateTimeService dateTimeService,
            MDA parent,
            string sku,
            int skuExternalId,
            int quantity,
            int receivedQuantity) : this(dateTimeService, parent, sku, skuExternalId, quantity, receivedQuantity, null)
        {
        }

        public MDAItem(
            IDateTimeService dateTimeService,
            MDA parent,
            string sku,
            int skuExternalId,
            int quantity,
            int receivedQuantity,
            MDAItemProcessingInformation mdaItemProcessingInformation)
        {
            _dateTimeService = dateTimeService;
            _parent = parent;
            MDAItemProcessingInformation = mdaItemProcessingInformation;
            SKU = sku;
            SkuExternalId = skuExternalId;
            Quantity = quantity;
            ReceivedQuantity = receivedQuantity;
        }

        public string Key
        {
            get { return string.Format("{0}_{1}", _parent.MDANumber, SkuExternalId); }
        }

        public string SKU { get; private set; }

        public int SkuExternalId { get; private set; }

        public int Quantity { get; private set; }

        public int ReceivedQuantity { get; private set; }

        public bool IsProcessed
        {
            get { return MDAItemProcessingInformation != null; }
        }

        public MDAItemProcessingInformation MDAItemProcessingInformation { get; private set; }

        public void Process(
            short receiptThresholdAtTimeOfTransfer,
            int receivedQuantityAtTimeOfTransfer,
            short transferPercent,
            int transferQuantity,
            string destinationWarehouse)
        {
            if (IsProcessed)
            {
                throw new InvalidOperationException("MDA Item is already marked as processed.");
            }

            MDAItemProcessingInformation = new MDAItemProcessingInformation(receivedQuantityAtTimeOfTransfer, transferPercent, transferQuantity);
            _parent.UncommittedEvents.Add(new MDAItemProcessedEvent(_parent.MDANumber, SkuExternalId, receiptThresholdAtTimeOfTransfer, Quantity, receivedQuantityAtTimeOfTransfer, transferPercent, transferQuantity, _dateTimeService.GetDate(), destinationWarehouse));
            _parent.MarkAsProcessedIfNecessary();
        }
    }
}
