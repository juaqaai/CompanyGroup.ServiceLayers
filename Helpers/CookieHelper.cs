using System;
using System.Collections.Generic;
using System.Web;

namespace CompanyGroup.Helpers
{
    public static class CookieHelper
    {
        /// <summary>
        /// session id-t kiolvassa http sütiből és visszaadja a hívónak
        /// ha nincs a sessionId-nek értéke és a createIfEmpty igaz, akkor generál egy új értéket a sütinek és beleteszi a http válaszba     
        /// </summary>
        /// <param name="req"></param>
        /// <param name="res"></param>
        /// <param name="createIfEmpty"></param>
        /// <returns></returns>
        public static string ReadSessionId(HttpRequestBase req, HttpResponseBase res, bool createIfEmpty)
        {
            string sessionId = String.Empty;
            HttpCookie cookie = req.Cookies.Get("hrp_rs_sess");
            if (cookie != null)
            {
                sessionId = cookie.Value;
            }
            if (String.IsNullOrEmpty(sessionId) && createIfEmpty)
            {
                sessionId = Guid.NewGuid().ToString("N");
                res.Cookies.Add(new HttpCookie("hrp_rs_sess", sessionId) { Expires = DateTime.Now.AddDays(30d) });
            }
            return sessionId;
        }

        /// <summary>
        /// kiolvassa az objectId beállítást tartalmazó cookie értékét
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static string ReadObjectId(HttpRequestBase req, string cookieName)
        {
            if (cookieName == null) { return String.Empty; }

            string result = String.Empty;
            HttpCookie cookie = req.Cookies.Get(cookieName);
            if (cookie != null)
            {
                result = cookie.Value;
            }
            return (result != null) ? result : String.Empty;
        }

        public static void WriteObjectId(HttpResponseBase res, string cookieName, string value)
        {
            try
            {
                if (value == null) { return; }

                res.Cookies.Add(new HttpCookie(cookieName, value) { Expires = DateTime.Now.AddDays(1d), HttpOnly = true });
            }
            catch { }
        }

        /// <summary>
        /// kiolvassa a nyelvi beállítást tartalmazó cookie értékét
        /// ha nincs a langId-nek értéke és a createIfEmpty igaz, akkor beállítja az alapértelmezett értéket és beleteszi a sütit a http válaszba
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static string ReadLangIdCookieValue(HttpRequestBase req, HttpResponseBase res, bool createIfEmpty)
        {
            string langId = String.Empty;
            HttpCookie cookie = req.Cookies.Get("hrp_rs_lang");
            if (cookie != null)
            {
                langId = cookie.Value;
            }
            if (String.IsNullOrEmpty(langId) && createIfEmpty)
            {
                langId = Shared.Web.Helpers.ConfigSettingsParser.GetString("LangId", "sb");
                res.Cookies.Add(new HttpCookie("hrp_rs_lang", langId) { Expires = DateTime.Now.AddDays(30d) });
            }
            return (String.IsNullOrEmpty(langId)) ? "sb" : langId;
        }

        /// <summary>
        /// beállítja a nyelvi beállítást tartalmazó cookie értékét
        /// </summary>
        /// <param name="req"></param>
        /// <param name="res"></param>
        /// <param name="langId"></param>
        /// <returns></returns>
        public static bool WriteLangIdCookieValue(HttpRequestBase req, HttpResponseBase res, string langId)
        {
            try
            {
                if (String.IsNullOrEmpty(langId)) { return false; }
                HttpCookie cookie = req.Cookies.Get("hrp_rs_lang");
                if (cookie == null)
                {
                    res.Cookies.Add(new HttpCookie("hrp_rs_lang", langId) { Expires = DateTime.Now.AddDays(30d) });
                }
                else
                {
                    cookie.Value = langId;
                    res.Cookies.Set(cookie);
                }
                return true;
            }
            catch { return false; }
        }
    }
}
