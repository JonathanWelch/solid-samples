using System.ComponentModel.DataAnnotations;

namespace _1_SingleResponsibility_WebApi.Common.ApiModels
{
    public class Stock
    {
        [Required]
        [MaxLength(20)]
        public string Sku { get; set; }

        [Required]
        public int Quantity { get; set; }

        public string TmId { get; set; }
    }
}