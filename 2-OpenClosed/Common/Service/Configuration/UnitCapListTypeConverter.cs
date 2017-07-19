using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;

namespace _2_OpenClosed.Common.Service.Configuration
{
    public class UnitCapListTypeConverter : TypeConverter
    {
        private readonly IUnitCapParser _parser;

        public UnitCapListTypeConverter() : this(new UnitCapParser())
        {
        }

        public UnitCapListTypeConverter(IUnitCapParser parser)
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
                throw new ArgumentNullException(valueStr, "Can not convert a List<UniCap> object from a NULL or empty String.");
            }

            var unitCap = JsonConvert.DeserializeObject<IList<UnitCap>>(valueStr);

            _parser.Parse(unitCap);

            return unitCap;
        }
    }
}