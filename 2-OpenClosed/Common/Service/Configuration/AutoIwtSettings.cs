namespace _2_OpenClosed.Common.Service.Configuration
{
    public class AutoIwtSettings : ISetting
    {
        public short ReceiptThreshold { get; set; }

        public int ScheduledMdasToRetrieve { get; set; }

        public int MaxTransferPercentage { get; set; }
    }
}
