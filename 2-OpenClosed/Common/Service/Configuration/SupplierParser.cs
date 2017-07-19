using System.Collections.Generic;

namespace _2_OpenClosed.Common.Service.Configuration
{
    public interface ISupplierParser
    {
        void Parse(IList<Supplier> suppliers);
    }

    public class SupplierParser : ISupplierParser
    {
        public void Parse(IList<Supplier> suppliers)
        {
            foreach (var supplier in suppliers)
            {
            }
        }
    }
}