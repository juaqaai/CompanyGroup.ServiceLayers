using System;
using System.Collections.Generic;
using System.Web;

namespace CompanyGroup.Helpers
{
    /// <summary>
    /// http cookie helper
    /// </summary>
    public static class CookieHelper
    {
        /// <summary>
        /// http süti kiolvasása http kérésből
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static T ReadCookie<T>(HttpRequestBase request, string cookieName) where T : class
        {
            try
            {
                HttpCookie cookie = request.Cookies.Get(cookieName);

                if (cookie == null) { return default(T); }

                return CompanyGroup.Helpers.JsonConverter.FromJSON<T>(cookie.Value);
            }
            catch { return default(T); }
        }

        /// <summary>
        /// http süti írása http válaszba
        /// </summary>
        /// <param name="response"></param>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        public static void WriteCookie(HttpResponseBase response, string cookieName, string value)
        {
            try
            {
                if ((String.IsNullOrEmpty(value))) { return; }

                HttpCookie cookie = response.Cookies.Get(cookieName);

                if (cookie == null)
                {
                    response.Cookies.Add(new HttpCookie(cookieName, value) { Expires = DateTime.Now.AddDays(30d), HttpOnly = true });
                }
                else
                {
                    cookie.Value = value;

                    response.Cookies.Set(cookie);
                }
            }
            catch { }
        }

    }
}
