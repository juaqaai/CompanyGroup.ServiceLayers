
namespace CompanyGroup.Helpers
{
    using System;
    using System.Collections.Specialized;
    using System.Text;

    /// <summary>
    /// Seg�doszt�ly, �rt�kek kiolvas�s�ra n�v �rt�k p�rokat tartalmaz� gy�jtem�nyb�l
    /// </summary>
    public class NameValueParser
    {
        static NameValueParser() { }

        /// <summary>
        /// logikai �rt�k kiolvas�sa n�v �rt�k p�rokat tartalmaz� gy�jtem�nyb�l
        /// </summary>
        /// <param name="nvColl"></param>
        /// <param name="valueName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool GetBoolean( NameValueCollection nvColl, string name, bool defaultValue )
        {
            var result = defaultValue;
            try
            {
                var value = nvColl.Get( name );

                bool.TryParse(value, out result);
                    
                return result;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// logikai �rt�k kiolvas�sa n�v �rt�k p�rokat tartalmaz� gy�jtem�nyb�l
        /// </summary>
        /// <param name="nvColl"></param>
        /// <param name="valueName"></param>
        /// <returns></returns>
        public static bool GetBoolean( NameValueCollection nvColl, string name )
        {
            return GetBoolean(nvColl, name, false);
        }

        /// <summary>
        /// eg�sz sz�m kiolvas�sa n�v �rt�k p�rokat tartalmaz� gy�jtem�nyb�l
        /// </summary>
        /// <param name="nvColl"></param>
        /// <param name="sName"></param>
        /// <param name="iDefaultValue"></param>
        /// <param name="bZeroAllowed"></param>
        /// <param name="iMaxValue"></param>
        /// <returns></returns>
        public static int GetInt( NameValueCollection nvColl, string sName, int iDefaultValue, bool bZeroAllowed, int iMaxValue )
        {
            var iRet = iDefaultValue;

            try
            {
                var sValue = nvColl.Get( sName );

                if ( !String.IsNullOrEmpty( sValue ) )
                {
                    var iValue = ConvertData.ConvertStringToInt( sValue, iDefaultValue );

                    if ( ( !bZeroAllowed && iValue == 0 ) || ( ( iMaxValue > 0 && iValue > iMaxValue ) ) )
                    {
                        iRet = iDefaultValue;
                    }
                    else
                    {
                        iRet = iValue;
                    }
                }
            }
            catch { }
            return iRet;
        }

        /// <summary>
        /// eg�sz sz�m kiolvas�sa n�v �rt�k p�rokat tartalmaz� gy�jtem�nyb�l
        /// </summary>
        /// <param name="nvColl"></param>
        /// <param name="sName"></param>
        /// <param name="iDefaultValue"></param>
        /// <param name="bZeroAllowed"></param>
        /// <returns></returns>
        public static int GetInt( NameValueCollection nvColl, string sName, int iDefaultValue, bool bZeroAllowed )
        {
            return GetInt( nvColl, sName, iDefaultValue, bZeroAllowed, 0 );
        }

        /// <summary>
        /// eg�sz sz�m kiolvas�sa n�v �rt�k p�rokat tartalmaz� gy�jtem�nyb�l
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sName"></param>
        /// <param name="iDefaultValue"></param>
        /// <returns></returns>
        public static int GetInt( NameValueCollection nvColl, string sName, int iDefaultValue )
        {
            return GetInt( nvColl, sName, iDefaultValue, true, 0 );
        }

        /// <summary>
        /// eg�sz sz�m kiolvas�sa n�v �rt�k p�rokat tartalmaz� gy�jtem�nyb�l
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sName"></param>
        /// <returns></returns>
        public static int GetInt( NameValueCollection nvColl, string sName )
        {
            return GetInt( nvColl, sName, 0, true, 0 );
        }

        /// <summary>
        /// sz�veges �rt�k kiolvas�sa n�v �rt�k p�rokat tartalmaz� gy�jtem�nyb�l
        /// </summary>
        /// <param name="nvColl"></param>
        /// <param name="sName"></param>
        /// <param name="sDefaultValue"></param>
        /// <returns></returns>
        public static string GetString( NameValueCollection nvColl, string sName, string sDefaultValue )
        {
            var sRet = sDefaultValue;
            try
            {
                sRet = nvColl.Get( sName );
                if ( string.IsNullOrEmpty( sRet ) )
                {
                    sRet = sDefaultValue;
                }
            }
            catch { }
            return sRet;
        }

        /// <summary>
        /// sz�veges �rt�k kiolvas�sa n�v �rt�k p�rokat tartalmaz� gy�jtem�nyb�l
        /// </summary>
        /// <param name="nvColl"></param>
        /// <param name="sName"></param>
        /// <returns></returns>
        public static string GetString( NameValueCollection nvColl, string sName )
        {
            return GetString( nvColl, sName, String.Empty );
        }

    }
}
