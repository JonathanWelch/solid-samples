using System.Collections.Generic;
using _2._1_OpenClosed.Common.ApiModels;

namespace _2._1_OpenClosed.Common.DataAccess.DTO
{
    public class StockRequest
    {
        public StockRequest()
        {
            this.Requests = new List<StockRequestItemQueue>();
        }

        public string IwtAdviceId { get; set; }

        public int MessageNo { get; set; }

        public StockRequestType StockRequestType { get; set; }

        public IList<StockRequestItemQueue> Requests { get; set; }
    }
}
