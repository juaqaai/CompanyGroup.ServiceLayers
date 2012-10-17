using System;
using System.Collections.Generic;

namespace CompanyGroup.Helpers
{
    public class ContextKeyManager
    {
        static ContextKeyManager() { }

        private const string DEFAULT_CONCAT_PARAM = "_";

        /// <summary>
        /// elem feltételtő függő hozzáadása a kulcshoz, összefűzéshez használt karakterrel
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="cacheKey"></param>
        /// <param name="itemKey"></param>
        /// <param name="concatParam"></param>
        /// <returns></returns>
        public static string AddToKey(bool condition, string cacheKey, string itemKey, string concatParam)
        {
            return (condition) ? cacheKey + concatParam + itemKey : cacheKey;
        }

        /// <summary>
        /// elem feltételtő függő hozzáadása a kulcshoz
        /// </summary>
        /// <param name="bCondition"></param>
        /// <param name="sCacheKey"></param>
        /// <param name="sItemKey"></param>
        /// <returns></returns>
        public static string AddToKey(bool condition, string cacheKey, string itemKey)
        {
            return AddToKey(condition, cacheKey, itemKey, DEFAULT_CONCAT_PARAM);
        }

        public static string AddToKey(bool condition, string cacheKey, int itemKey)
        {
            return AddToKey(condition, cacheKey, Convert.ToString(itemKey));
        }

        public static string AddToKey(string cacheKey, string itemKey)
        {
            return AddToKey(true, cacheKey, itemKey, DEFAULT_CONCAT_PARAM);
        }

        public static string AddToKey(string cacheKey, int itemKey)
        {
            return AddToKey(cacheKey, Convert.ToString(itemKey));
        }

        public static string CreateKey(string cacheKey, string itemKey)
        {
            return AddToKey(cacheKey, itemKey);
        }

        public static string CreateKey(string cacheKey, string itemKey, string concatParam)
        {
            return AddToKey(true, cacheKey, itemKey, concatParam);
        }

    }
}
