using System;

namespace _1_SingleResponsibility_WebApi.Common.DataAccess.DTO
{
    public class IwtRequest
    {
        public int IwtRequestId { get; set; }

        public string AdviceId { get; set; }

        public string DespatchingWarehouseId { get; set; }

        public string ReceivingWarehouseId { get; set; }

        public int IwtTypeId { get; set; }

        public string StatusCode { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        public DateTime DateClosedUTC { get; set; }

        public DateTime DateModifiedUTC { get; set; }
    }
}