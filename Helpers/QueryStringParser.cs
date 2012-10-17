using System;
using System.Collections.Generic;
using System.Text;

using System.Globalization;
using System.Web;
using System.Collections.Specialized;

namespace CompanyGroup.Helpers
{
    /// <summary>
    /// reading parameter values from request querystring
    /// </summary>
    public sealed class QueryStringParser
    {
		static QueryStringParser()
		{
        }

        /// <summary>
        /// request querystring is empty, or not
        /// </summary>
        /// <returns></returns>
        public static bool IsEmpty()
        {
            return ( HttpContext.Current.Request.QueryString.Count.Equals( 0 ) ) ||
                   ( HttpContext.Current.Request.QueryString.Count.Equals( 1 ) && ExistParam( "AspxAutoDetectCookieSupport" ) );
        }

        #region "retrieve parameters"

        /// <summary>
		/// get string parameter from querystring params
		/// </summary>
		/// <param name="sParam">parameter value</param>
		/// <param name="sDefaultValue">default parameter</param>
		/// <returns>parameter value</returns>
		public static string GetString( string sParam, string sDefaultValue )
		{
            return CompanyGroup.Helpers.NameValueParser.GetString(HttpContext.Current.Request.QueryString, sParam, sDefaultValue);
		}

		/// <summary>
		/// get string parameter from querystring params
		/// </summary>
		/// <param name="sParam">parameter value</param>
		/// <returns>parameter value</returns>
        public static string GetString( string sParam )
        {
            return GetString( sParam, String.Empty );
        }

        /// <summary>
        /// get string parameter from querystring params
        /// </summary>
        /// <param name="sParam">parameter value</param>
        /// <param name="sDefaultValue">default parameter</param>
        /// <returns>parameter value</returns>
        public static string GetDecodedString( string sParam, string sDefaultValue )
		{
            string sRet = GetString( sParam, sDefaultValue );
            return ( ! sRet.Equals( sDefaultValue ) ) ? HttpUtility.UrlDecode( sRet ) : sRet;
		}

        /// <summary>
        /// get string parameter from querystring params
        /// </summary>
        /// <param name="sParam">parameter value</param>
        /// <returns>parameter value</returns>
        public static string GetDecodedString( string sParam )
		{
            return GetDecodedString( sParam, String.Empty );
		}

		/// <summary>
        /// get int parameter from querystring params
		/// </summary>
        /// <param name="sParam">parameter name value</param>
        /// <param name="iDefaultValue">default parameter</param>
        /// <returns>parameter value</returns>
        public static int GetInt( string sParam, int iDefaultValue )
		{
            return CompanyGroup.Helpers.NameValueParser.GetInt(HttpContext.Current.Request.QueryString, sParam, iDefaultValue);
 	    }

		/// <summary>
        /// get int parameter from querystring params
		/// </summary>
        /// <param name="sParam">parameter name value</param>
        /// <returns>parameter value</returns>
        public static int GetInt( string sParam )
        {
            return GetInt( sParam, 0 );
        }

		/// <summary>
        ///  get int parameter from querystring params
		/// </summary>
        /// <param name="sParam">parameter name</param>
        /// <param name="iDefaultValue">default parameter value</param>
		/// <param name="iMinValue">minimum parameter value</param>
		/// <param name="iMaxValue">maximum parameter value</param>
		/// <returns></returns>
        public static int GetInt( string sParam, int iDefaultValue, int iMinValue, int iMaxValue )
		{
            int iNumericValue = GetInt( sParam, iDefaultValue );

            if ( iNumericValue.Equals( iDefaultValue ) )
            {
                return iDefaultValue;
            }
            else
            {
                return ( ( iMinValue <= iNumericValue ) && ( iNumericValue <= iMaxValue ) ) ? iNumericValue : iDefaultValue;
            }
        }

        public static bool GetBool(string param, bool defaultValue)
        {
            return CompanyGroup.Helpers.NameValueParser.GetBoolean(HttpContext.Current.Request.QueryString, param, defaultValue);
        }

        public static bool GetBool(string param)
        {
            return GetBool(param, false);
        }

        #endregion

        #region check parameters

        /// <summary>
		/// Letezik-e a keresett parameter a hivo parameterek kozott
		/// </summary>
		/// <param name="sParam">Vizsgalt parameter neve</param>
		/// <returns>Logikai: true ha letezik a keresett parameter, false, ha nem.</returns>
        public static bool ExistParam( string sParam )
		{
            return ( !String.IsNullOrEmpty( sParam) ) ? !String.IsNullOrEmpty( GetString( sParam, String.Empty ) ) : false;
        }

        #endregion

        /// <summary>
        /// convert querystring to a simple string
        /// </summary>
        /// <param name="nvQueryStr"></param>
        /// <returns></returns>
        public static string ConvertToString( NameValueCollection nvQueryStr )
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder( nvQueryStr.Count );
            try
            {
                string sItemValue = String.Empty;
                string sItemKey = String.Empty;
                int iCount = nvQueryStr.Count;
                for ( int i = 0; i <= iCount; i++ )
                {
                    sItemKey = nvQueryStr.GetKey( i );
                    sItemValue = nvQueryStr.Get( i );
                    sb.Append( ( i == 0 ) ? "?" : "&" );
                    sb.Append( sItemKey );
                    sb.Append( "=" );
                    sb.Append( sItemValue );
                }
            }
            catch { }
            return sb.ToString();        
        }

        /// <summary>
        /// convert querystring to a simple string
        /// </summary>
        public static string ConvertToString()
        {
            return ConvertToString( HttpContext.Current.Request.QueryString );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sParam"></param>
        /// <returns></returns>
        public static string Remove( string sParam )
        {
            NameValueCollection nvQueryStr = HttpContext.Current.Request.QueryString;
            try
            {
                nvQueryStr.Remove( sParam );
            }
            catch { }
            return ConvertToString( nvQueryStr );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sParam"></param>
        /// <param name="sValue"></param>
        /// <returns></returns>
        public static string Add( string sParam, string sValue )
        {
            NameValueCollection nvQueryStr = HttpContext.Current.Request.QueryString;
            string s = String.Empty;
            try
            {
                s = ConvertToString( nvQueryStr );
                s += string.IsNullOrEmpty( s ) ? "?" : "&";
                s += sParam + "=" + sValue;

            }
            catch{}
            return s;
        }

        #region "url parameter concatenation"

        /// <summary>
        /// url parameter concatenation
        /// </summary>
        /// <param name="sConcatParam">concatenation parameter</param>
        /// <param name="sParamName">parameter name</param>
        /// <param name="sParamValue">parameter value</param>
        /// <returns></returns>
        public static string ConcatUrlParams( string sConcatParam, string sParamName, string sParamValue )
        {
            return ( !String.IsNullOrEmpty( sParamValue ) && ( !sParamValue.Equals( "0", StringComparison.CurrentCulture ) ) ) ? sConcatParam + sParamName + "=" + sParamValue : String.Empty;
        }

        /// <summary>
        /// url parameter concatenation
        /// </summary>
        /// <param name="sParamName">parameter name</param>
        /// <param name="sParamValue">parameter value</param>
        /// <returns></returns>
        public static string ConcatUrlParams( string sParamName, string sParamValue )
        {
            return ConcatUrlParams( "&", sParamName, sParamValue );
        }

        /// <summary>
        /// url parameter concatenation
        /// </summary>
        /// <param name="sConcatParam">concatenation parameter</param>
        /// <param name="sParamName">parameter name</param>
        /// <param name="iParamValue">parameter value</param>
        /// <returns></returns>
        public static string ConcatUrlParams( string sConcatParam, string sParamName, int iParamValue )
        {
            return ConcatUrlParams(sConcatParam, sParamName, CompanyGroup.Helpers.ConvertData.ConvertIntToString(iParamValue));
        }

        /// <summary>
        /// url parameter concatenation
        /// </summary>
        /// <param name="sParamName">parameter name</param>
        /// <param name="iParamValue">parameter value</param>
        /// <returns></returns>
        public static string ConcatUrlParams( string sParamName, int iParamValue )
        {
            return ConcatUrlParams("&", sParamName, CompanyGroup.Helpers.ConvertData.ConvertIntToString(iParamValue));
        }

        /// <summary>
        /// url parameter concatenation
        /// </summary>
        /// <param name="sConcatParam">concatenation parameter</param>
        /// <param name="sParamName">parameter name</param>
        /// <param name="bParamValue">parameter value</param>
        /// <returns></returns>
        public static string ConcatUrlParams( string sConcatParam, string sParamName, bool bParamValue )
        {
            return ConcatUrlParams( sConcatParam, sParamName, ( bParamValue ) ? "1" : String.Empty );
        }

        /// <summary>
        /// url parameter concatenation
        /// </summary>
        /// <param name="sParamName"></param>
        /// <param name="bParamValue"></param>
        /// <returns></returns>
        public static string ConcatUrlParams( string sParamName, bool bParamValue )
        {
            return ConcatUrlParams( "&", sParamName, bParamValue );
        }

        #endregion
    }
}
