using System;

namespace _1_SingleResponsibility_WebApi.Common.DataAccess.DTO
{
    public class StockRequestMessage
    {
        public string IwtAdviceId { get; set; }

        public int MessageNo { get; set; }

        public int StockRequestTypeId { get; set; }

        public DateTime DateCreatedUtc { get; set; }

        public DateTime LastModifiedUtc { get; set; }
    }
}