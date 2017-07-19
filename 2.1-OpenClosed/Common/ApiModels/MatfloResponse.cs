namespace _2._1_OpenClosed.Common.ApiModels
{
    public class MatfloResponse
    {
        public int MessageNo { get; set; }

        public string IwtAdviceId { get; set; }

        public string Operation { get; set; }

        public string Error { get; set; }

        public bool RequestProcessed { get; set; }

        public StockCallbackRequest RequestMessage { get; set; }
    }
}
