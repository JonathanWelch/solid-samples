using _5._1_DependencyInversion.Common.Infrastructure;

namespace _5._1_DependencyInversion.Domain
{
    public class ListerHillsStockLineValidator : CsvLineValidator
    {
        private const int SkuFieldIndex = 0;
        private const int SkuFieldMaxLength = 64;
        private const int QuantityFieldIndex = 1;

        protected override int Fields
        {
            get
            {
                return 2;
            }
        }

        protected override void RunValidation(string[] line)
        {
            ErrorOnNullOrEmptyString(line, SkuFieldIndex, SkuFieldMaxLength, "Sku");
            ErrorIfNotInteger(line, QuantityFieldIndex, "Available");
        }
    }
}
