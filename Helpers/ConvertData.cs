using System;

namespace CompanyGroup.Helpers
{
    public class ConvertData
    {

        public static int ConvertBoolToInt(bool b)
        {
            return b ? 1 : 0;
        }

        public static bool ConvertIntToBool(int i)
        {
            return i.Equals(1) ? true : false;
        }

        public static string ConvertBoolToString(bool b, string trueValue, string falseValue)
        {
            return b ? trueValue : falseValue;
        }

        public static string ConvertObjectToString(object o)
        {
            try
            {
                return Convert.ToString( o );
            }
            catch
            {
                return String.Empty;
            }
        }

        public static bool ConvertStringToBool(string s)
        {
            try
            {
                bool result;
                if ( bool.TryParse( s, out result) )
                {
                    return result;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static long ConvertStringToLong(string s)
        {
            try
            {
                long result;
                if (long.TryParse(s, out result))
                {
                    return result;
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Stringet integer-be konvertalo fuggveny
        /// </summary>
        /// <param name="sParam">Konvertalando parameter string</param>
        /// <param name="iDefaultValue">Alapertelmezett integer visszatero ertek</param>
        /// <returns>Konvertalt int ertek, vagy iDefaultValue</returns>
        public static int ConvertStringToInt(string param, int defaultValue)
        {
            int ret = defaultValue;
            try
            {
                return String.IsNullOrEmpty(param) ? defaultValue : Convert.ToInt32(param);
            }
            catch { }
            return ret;
        }

        /// <summary>
        /// Stringet integer-be konvertalo fuggveny
        /// </summary>
        /// <param name="sParam">Konvertalando parameter string</param>
        /// <returns>Konvertalt int ertek, vagy 0</returns>
        public static int ConvertStringToInt(string param)
        {
            return ConvertStringToInt(param, 0);
        }

        /// <summary>
        /// Integert string-be konvertalo metodus
        /// </summary>
        /// <param name="param">Konvertalando int</param>
        /// <param name="defaultValue">Alapertelmezett visszateresi ertek</param>
        /// <returns>Konvertalt string ertek</returns>
        public static string ConvertIntToString(int param, string defaultValue)
        {
            string sRet = defaultValue;
            try
            {
                if (!param.Equals(0))
                {
                    sRet = Convert.ToString(param, System.Globalization.CultureInfo.CurrentCulture);
                }
            }
            catch
            {
            }
            return sRet;
        }

        /// <summary>
        /// Integert string-be konvertalo metodus
        /// </summary>
        /// <param name="iParam">Konvertalando int</param>
        /// <returns>Konvertalt string ertek</returns>
        public static string ConvertIntToString(int iParam)
        {
            return ConvertIntToString(iParam, String.Empty);
        }

        /// <summary>
        /// string listát szeparált sztring listába konvertál
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string ConvertStringListToDelimitedString(System.Collections.Generic.List<string> list)
        {
            return ConvertStringListToDelimitedString(list, ",");
        }

        /// <summary>
        /// string listát szeparált sztring listába konvertál
        /// </summary>
        /// <param name="list"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ConvertStringListToDelimitedString(System.Collections.Generic.List<string> list, string separator)
        {
            if (list.Count.Equals(0)) { return String.Empty; }

            System.Text.StringBuilder result = new System.Text.StringBuilder();

            int index = 0;

            list.ForEach(x => {

                result.Append(x);

                index++;

                if (index < list.Count)
                {
                    result.Append(separator);
                }
            });

            return result.ToString();
        }
    }
}
