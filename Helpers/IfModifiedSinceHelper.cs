
namespace CompanyGroup.Helpers
{
    using System;
    using System.Web;

    /// <summary>
    /// IfModifiedSinceHelper helper class a cache-hez
    /// </summary>
    public class IfModifiedSinceHelper
    {

        private static bool CachedVersionIsOkay( DateTime lastModification )
        {
            bool bRet = false;

            string ifModified = HttpContext.Current.Request.Headers[ "If-Modified-Since" ];

            if (!String.IsNullOrEmpty(ifModified))
            {
                string lastModified = lastModification.ToUniversalTime().ToString("r");

                bRet = ifModified.Equals(lastModified);
            }
            return bRet;
        }

        public static bool HandleHeader(DateTime lastModification)
        {
            bool ret = false;

            if (CachedVersionIsOkay(lastModification))
            {
                HttpContext.Current.Response.StatusCode = 304;

                HttpContext.Current.Response.SuppressContent = true;

                ret = true;
            }
            else
            {
                HttpContext.Current.Response.Cache.SetLastModified(lastModification);

                HttpContext.Current.Response.Cache.SetCacheability( HttpCacheability.Public );
            }
            return ret;
        }
    }
}