using _2_OpenClosed.DataAccess.DTO;

namespace _2_OpenClosed.Domain.MDA
{
    // TODO: Do we need this class?
    public class MDAItemProcessingInformation
    {
        public MDAItemProcessingInformation(int receivedQuantityAtTimeOfTransfer, int transferPercent, int transferQuantity)
        {
            ReceivedQuantityAtTimeOfTransfer = receivedQuantityAtTimeOfTransfer;
            TransferPercent = transferPercent;
            TransferQuantity = transferQuantity;
        }

        public MDAItemProcessingInformation(ScheduledMDAItemDTO scheduledMdaItemDto)
        {
            ReceivedQuantityAtTimeOfTransfer = scheduledMdaItemDto.MDAReceivedQuantity.Value;
            TransferPercent = scheduledMdaItemDto.TransferPercent.Value;
            TransferQuantity = scheduledMdaItemDto.TransferQuantity.Value;
        }

        public int ReceivedQuantityAtTimeOfTransfer { get; private set; }

        public int TransferPercent { get; private set; }

        public int TransferQuantity { get; private set; }
    }
}
