namespace _2._1_OpenClosed.Common.ApiModels
{
    public class StockRequestItemQueue
    {
        public string IwtAdviceId { get; set; }

        public StockRequestType StockRequestTypeId { get; set; }

        public int MessageNo { get; set; }

        public string Sku { get; set; }

        public int QtyRequested { get; set; }

        public int? QtyConfirmed { get; set; }

        public int StockRequestItemStatusId { get; set; }

        public string TmId { get; set; }
    }
}
