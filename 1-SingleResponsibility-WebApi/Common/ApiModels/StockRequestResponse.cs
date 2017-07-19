namespace _1_SingleResponsibility_WebApi.Common.ApiModels
{
    public class StockRequestResponse
    {
        private string error = null;

        public StockRequestResponse()
        {
        }

        public StockRequestResponse(string iwtAdviceId, int messageNo, string operation)
        {
            IwtAdviceId = iwtAdviceId;
            MessageNo = messageNo;
            Operation = operation;
        }

        public int MessageNo { get; set; }

        public string IwtAdviceId { get; set; }

        public string Operation { get; set; }

        public bool? RequestProcessed { get; set; }

        public int? TotalSkus { get; set; }

        public int? TotalQuantity { get; set; }

        public string Error
        {
            get
            {
                if (string.IsNullOrEmpty(error))
                {
                    return null;
                }

                return error;
            }

            set
            {
                error = value;
            }
        }
    }
}