using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.
    Text;
using System.Threading.Tasks;

namespace _2_OpenClosed.Common.Service.Configuration
{
    public class ConfigTypeConverter
    {
        public static TypeConverter GetCustomTypeConverter(Type type)
        {
            if (type == typeof(List<int>))
            {
                return new GenericListTypeConverter<int>();
            }

            if (type == typeof(List<decimal>))
            {
                return new GenericListTypeConverter<decimal>();
            }

            if (type == typeof(List<string>))
            {
                return new GenericListTypeConverter<string>();
            }

            if (type == typeof(List<Department>) || type == typeof(IEnumerable<Department>))
            {
                return new DepartmentListTypeConverter();
            }

            if (type == typeof(List<Supplier>) || type == typeof(IEnumerable<Supplier>))
            {
                return new SupplierListTypeConverter();
            }

            if (type == typeof(List<UnitCap>) || type == typeof(IEnumerable<UnitCap>))
            {
                return new UnitCapListTypeConverter();
            }

            return TypeDescriptor.GetConverter(type);
        }
    }
}
