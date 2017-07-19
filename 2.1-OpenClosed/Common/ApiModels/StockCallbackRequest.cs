using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2._1_OpenClosed.Common.ApiModels
{
    public class StockCallbackRequest
    {
        public StockCallbackRequest()
        {
            Requests = new List<StockConfirmation>();
        }

        public int MessageNo { get; set; }

        public string IwtAdviceId { get; set; }

        public string Operation { get; set; }

        public string Error { get; set; }

        public IList<StockConfirmation> Requests { get; set; }
    }
}
