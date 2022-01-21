using System;
using System.ComponentModel;
using System.Globalization;

namespace GoLive.Blazor.Controls
{
    public static class StringExtensions
    {
        public static bool IsEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }
        
        public static int AsInt(this string value)
        {
            return AsInt(value, 0);
        }
        
        public static int AsInt(this string value, int defaultValue)
        {
            int result;
            return Int32.TryParse(value, out result) ? result : defaultValue;
        }

        public static decimal AsDecimal(this string value)
        {
            // Decimal.TryParse does not work consistently for some locales. For instance for lt-LT, it accepts but ignores decimal values so "12.12" is parsed as 1212.
            return As<Decimal>(value);
        }

        public static decimal AsDecimal(this string value, decimal defaultValue)
        {
            return As<Decimal>(value, defaultValue);
        }
        
        public static float AsFloat(this string value)
        {
            return AsFloat(value, default(float));
        }
        
        public static float AsFloat(this string value, float defaultValue)
        {
            float result;
            return Single.TryParse(value, out result) ? result : defaultValue;
        }

        public static DateTime AsDateTime(this string value)
        {
            return AsDateTime(value, default(DateTime));
        }

        public static DateTime AsDateTime(this string value, DateTime defaultValue)
        {
            DateTime result;
            return DateTime.TryParse(value, out result) ? result : defaultValue;
        }

        public static TValue As<TValue>(this string value)
        {
            return As<TValue>(value, default(TValue));
        }
        
        public static bool AsBool(this string value)
        {
            return AsBool(value, default(bool));
        }
        
        public static bool AsBool(this string value, bool defaultValue)
        {
            bool result;
            return Boolean.TryParse(value, out result) ? result : defaultValue;
        }
        
        public static TValue As<TValue>(this string value, TValue defaultValue)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(TValue));
                if (converter.CanConvertFrom(typeof(string)))
                {
                    return (TValue)converter.ConvertFrom(value);
                }
                // try the other direction
                converter = TypeDescriptor.GetConverter(typeof(string));
                if (converter.CanConvertTo(typeof(TValue)))
                {
                    return (TValue)converter.ConvertTo(value, typeof(TValue));
                }
            }
            catch
            {
                // eat all exceptions and return the defaultValue, assumption is that its always a parse/format exception
            }
            return defaultValue;
        }
        
        public static bool IsBool(this string value)
        {
            bool result;
            return Boolean.TryParse(value, out result);
        }
        
        public static bool IsInt(this string value)
        {
            int result;
            return Int32.TryParse(value, out result);
        }

        public static bool IsDecimal(this string value)
        {
            // For some reason, Decimal.TryParse incorrectly parses floating point values as decimal value for some cultures.
            // For example, 12.5 is parsed as 125 in lt-LT.
            return Is<Decimal>(value);
        }
        
        public static bool IsFloat(this string value)
        {
            float result;
            return Single.TryParse(value, out result);
        }

        public static bool IsDateTime(this string value)
        {
            DateTime result;
            return DateTime.TryParse(value, out result);
        }
        
        public static bool Is<TValue>(this string value)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(TValue));
            if (converter != null)
            {
                try
                {
                    if ((value == null) || converter.CanConvertFrom(null, value.GetType()))
                    {
                        // TypeConverter.IsValid essentially does this - a try catch - but uses InvariantCulture to convert. 
                        converter.ConvertFrom(null, CultureInfo.CurrentCulture, value);
                        return true;
                    }
                }
                catch
                {
                }
            }
            return false;
        }
    }
}