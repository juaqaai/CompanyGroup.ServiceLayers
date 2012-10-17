
namespace CompanyGroup.Helpers
{
    using System;
    using System.Web;

    public static class CacheHelper
    {
        /// <summary>
        /// egy nap időtartam decimális típusban
        /// </summary>
        private const double EXPIRATION = 1440d;

        /// <summary>
        /// Insert value into the cache using
        /// appropriate name/value pairs
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of item</param>
        /// <param name="data">Item to be cached</param>
        /// <param name="cd"></param>
        /// <param name="dt"></param>
        public static void Add<T>(string key, T data, System.Web.Caching.CacheDependency cd, DateTime dt) where T : class
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "The key parameter cannot be null, or empty!");
            }
            if (data == null)
            {
                throw new ArgumentNullException("data", "The data parameter cannot be null, or empty!");
            }
            if (dt == null)
            {
                throw new ArgumentNullException("dt", "The dt parameter cannot be null, or empty!");
            }
            try
            {
                System.Web.HttpContext.Current.Cache.Insert(
                    key,
                    data,
                    cd,
                    dt,
                    System.Web.Caching.Cache.NoSlidingExpiration);
            }
            catch { } 
        }

        public static void Add<T>(string key, T data, DateTime dt) where T : class
        {
            try
            {
                Add<T>(key, data, null, dt);
            }
            catch { }
        }

        public static void Add<T>(string key, T data) where T : class
        {
            Add<T>(key, data, DateTime.Now.AddMinutes(EXPIRATION));
        }

        public static void Add<T>(string key, string filePath, T data, DateTime dt) where T : class
        {
            if (String.IsNullOrEmpty(filePath))
            {
                throw new ArgumentNullException("filePath", "The filePath parameter cannot be null, or empty!");
            }

            if (!System.IO.File.Exists(filePath))
            {
                throw new System.IO.FileNotFoundException("The file cannot be found!", filePath);
            }

            System.Web.Caching.CacheDependency cd = new System.Web.Caching.CacheDependency(filePath);

            Add<T>(key, data, cd, dt);
            
        }

        /// <summary>
        /// Remove item from cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        public static void Clear(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "The key parameter cannot be null, or empty!");
            }
            try
            {
                System.Web.HttpContext.Current.Cache.Remove(key);
            }
            catch { }
        }

        /// <summary>
        /// eltávolítás a cache-ből kulcs előtag alapján 
        /// </summary>
        /// <param name="sKeyPrefix"></param>
        /// <returns></returns>
        public static void RemoveItemByKeyPrefix(string keyPrefix)
        {
            if (String.IsNullOrEmpty(keyPrefix))
            {
                throw new ArgumentNullException("keyPrefix", "Key prefix cannot be null or empty");
            }

            try
            {
                System.Collections.IDictionaryEnumerator dictEnumCache = System.Web.HttpContext.Current.Cache.GetEnumerator();

                System.Collections.DictionaryEntry entry;

                string key = String.Empty;

                while (dictEnumCache.MoveNext())
                {
                    entry = (System.Collections.DictionaryEntry)dictEnumCache.Current;

                    key = CompanyGroup.Helpers.ConvertData.ConvertObjectToString(entry.Key);

                    if (key.StartsWith(keyPrefix))
                    {
                        System.Web.HttpContext.Current.Cache.Remove(key);
                    }
                }
            }
            catch {  }
        }

        /// <summary>
        /// Check for item in cache
        /// </summary>
        /// <param name="key">Name of cached item</param>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "The key parameter cannot be null, or empty!");
            }
            try
            {
                return System.Web.HttpContext.Current.Cache[key] != null;
            }
            catch { return false; }
        }

        /// <summary>
        /// Retrieve cached item
        /// </summary>
        /// <typeparam name="T">Type of cached item</typeparam>
        /// <param name="key">Name of cached item</param>
        /// <returns>Cached item as type</returns>
        public static T Get<T>(string key) where T : class
        {
            if (String.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key", "The key parameter cannot be null, or empty!");
            }
            try
            {
                return (T) System.Web.HttpContext.Current.Cache[key];
            }
            catch
            {
                return Activator.CreateInstance<T>();
            }
        }

        /// <summary>
        /// cache abszolút lejárati dátuma percben
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static double CalculateAbsExpirationInMinutes(double timeout)
        {
            double result = 0;

            DateTime now = DateTime.Now;

            DateTime backup = new DateTime(now.Year, now.Month, now.Day, 7, 40, 0);

            if (now > backup)
            {
                result = timeout - Math.Abs(((now.Hour * 60) + now.Minute) - 450);
            }
            else
            {
                result = Math.Abs(450 - ((now.Hour * 60) + now.Minute));
            }

            return result;
        }
    }

}
