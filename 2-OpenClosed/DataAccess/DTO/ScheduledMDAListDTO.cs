using System;
using System.Collections.Generic;

namespace _2_OpenClosed.DataAccess.DTO
{
    public class ScheduledMDAListDTO
    {
        public string MDANumber { get; set; }

        public short TransferPercent { get; set; }

        public List<ScheduledMDAItemDTO> Items { get; set; }
    }

    public class ScheduledMDAItemDTO
    {
        public int SkuExternalId { get; set; }

        public int ReceiptThreshold { get; set; }

        public int MDAQuantity { get; set; }

        public int? MDAReceivedQuantity { get; set; }

        public short? TransferPercent { get; set; }

        public int? TransferQuantity { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public bool IsProcessed
        {
            get { return ProcessedDate.HasValue; }
        }

        public void Validate()
        {
            if (!MDAReceivedQuantity.HasValue)
            {
                throw new InvalidOperationException("No value found for MDAReceivedQuantity.");
            }

            if (!TransferPercent.HasValue)
            {
                throw new InvalidOperationException("No value found for TransferPercent.");
            }

            if (!TransferQuantity.HasValue)
            {
                throw new InvalidOperationException("No value found for MDAReceivedQuantity.");
            }
        }
    }
}
