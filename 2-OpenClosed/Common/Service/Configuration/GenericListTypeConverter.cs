﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace _2_OpenClosed.Common.Service.Configuration
{
    public class GenericListTypeConverter<T> : TypeConverter
    {
        protected readonly TypeConverter typeConverter;

        public GenericListTypeConverter()
        {
            typeConverter = TypeDescriptor.GetConverter(typeof(T));
            if (typeConverter == null)
            {
                throw new InvalidOperationException("No type converter exists for type " + typeof(T).FullName);
            }
        }

        protected virtual string[] GetStringArray(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var result = input.Split(',');
                Array.ForEach(result, s => s.Trim());
                return result;
            }
            else
            {
                return new string[0];
            }
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                var items = GetStringArray(sourceType.ToString());
                return items.Any();
            }

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var items = GetStringArray((string)value);
                var result = new List<T>();

                Array.ForEach(
                    items, s =>
                    {
                        var item = typeConverter.ConvertFromInvariantString(s);
                        if (item != null)
                        {
                            result.Add((T)item);
                        }
                    });

                return result;
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                var result = string.Empty;
                if ((IList<T>)value != null)
                {
                    // we don't use string.Join() because it doesn't support invariant culture
                    for (var i = 0; i < ((IList<T>)value).Count; i++)
                    {
                        var str1 = Convert.ToString(((IList<T>)value)[i], CultureInfo.InvariantCulture);
                        result += str1;

                        // don't add comma after the last element
                        if (i != ((IList<T>)value).Count - 1)
                        {
                            result += ",";
                        }
                    }
                }

                return result;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
