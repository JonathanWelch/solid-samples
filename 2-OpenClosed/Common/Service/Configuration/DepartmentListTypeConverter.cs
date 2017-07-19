using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Asos.Inventory.Stock.Iwt.Common.Service.Configuration;
using Newtonsoft.Json;

namespace _2_OpenClosed.Common.Service.Configuration
{
    public class DepartmentListTypeConverter : TypeConverter
    {
        private readonly IDepartmentParser _parser;

        public DepartmentListTypeConverter() : this(new DepartmentParser())
        {
        }

        public DepartmentListTypeConverter(IDepartmentParser parser)
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
                throw new ArgumentNullException(valueStr, "Can not convert a IList<Department> object from a NULL or empty String.");
            }

            var departments = JsonConvert.DeserializeObject<IList<Department>>(valueStr);

            _parser.Parse(departments);

            return departments;
        }
    }
}