using System;
using System.Collections.Generic;

namespace CompanyGroup.Helpers
{
    public class TextUtils
    {
        public static string SeparateString(decimal d)
        {
            return SeparateString(d, 0);
        }

        public static string SeparateString(decimal d, int decimalDigits)
        {
            System.Globalization.NumberFormatInfo nfi = (System.Globalization.NumberFormatInfo) System.Globalization.CultureInfo.InvariantCulture.NumberFormat.Clone();

            nfi.NumberGroupSeparator = " ";

            nfi.CurrencyDecimalDigits = decimalDigits;

            return d.ToString("n", nfi);
        }
    }
}
