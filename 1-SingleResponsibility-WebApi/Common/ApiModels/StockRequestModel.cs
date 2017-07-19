using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace _1_SingleResponsibility_WebApi.Common.ApiModels
{
    public class StockRequestModel
    {
        [Required]
        [Range(0, 999999)]
        public int MessageNo { get; set; }

        [Required]
        [MaxLength(20)]
        public string IwtAdviceId { get; set; }

        public StockRequestType StockRequestType { get; set; }

        public IEnumerable<Stock> Requests { get; set; }

        public bool IsValid(Func<int, bool> doesMessageExist, Func<string, bool> doesAdviceExist, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!Regex.IsMatch(IwtAdviceId, @"^[IWT]{3}[0-9]{13}"))
            {
                errorMessage = "This has an invalid IwtAdviceId.";
            }
            else if (MessageNo <= 0)
            {
                errorMessage = "The has an invalid MessageNo.";
            }
            else if (StockRequestType != StockRequestType.CancelAllocate && (Requests == null || !Requests.Any()))
            {
                errorMessage = "The request has no SKUS/Quantities.";
            }
            else if (StockRequestType == StockRequestType.CancelAllocate && Requests != null && Requests.Any())
            {
                errorMessage = "Cancel allocate messages should not contain SKUS/Quantities.";
            }
            else if (doesMessageExist(MessageNo))
            {
                errorMessage = "This message request has already been processed.";
            }
            else if (!doesAdviceExist(IwtAdviceId))
            {
                errorMessage = "This iwt advice does not exist.";
            }
            else if (StockRequestType != StockRequestType.CancelAllocate && Requests != null)
            {
                foreach (var item in Requests)
                {
                    if (item.Quantity == 0)
                    {
                        errorMessage = "0 Quantity not allowed.";
                    }
                }

                if (errorMessage == string.Empty)
                {
                    foreach (var item in Requests)
                    {
                        if (item.Sku == string.Empty)
                        {
                            errorMessage = "Empty Sku id not allowed.";
                        }
                    }
                }
            }

            return string.IsNullOrEmpty(errorMessage);
        }
    }
}