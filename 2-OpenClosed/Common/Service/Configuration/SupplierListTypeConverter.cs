using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;

namespace _2_OpenClosed.Common.Service.Configuration
{
    public class SupplierListTypeConverter : TypeConverter
    {
        private readonly ISupplierParser _parser;

        public SupplierListTypeConverter() : this(new SupplierParser())
        {
        }

        public SupplierListTypeConverter(ISupplierParser parser)
        {
            _parser = parser;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var valueStr = value as string;
            if (string.IsNullOrEmpty(valueStr))
            {
                throw new ArgumentNullException(valueStr, "Can not convert a IList<Supplier> object from a NULL or empty String.");
            }

            var suppliers = JsonConvert.DeserializeObject<IList<Supplier>>(valueStr);

            _parser.Parse(suppliers);

            return suppliers;
        }
    }
}