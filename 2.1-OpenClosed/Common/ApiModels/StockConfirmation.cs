using System.ComponentModel.DataAnnotations;

namespace _2._1_OpenClosed.Common.ApiModels
{
    public class StockConfirmation
    {
        [Required]
        public string Sku { get; set; }

        [Required]
        public int QuantityRequested { get; set; }

        public string TmId { get; set; }

        public int QuantityConfirmed { get; set; }
    }
}
