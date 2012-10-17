using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.NoSql
{
    /// <summary>
    /// MongoDb-hez tartozó beállításokat összefogó osztály
    /// </summary>
    public class Settings : ISettings
    {
        /// <summary>
        /// server host name
        /// </summary>
        public string Server { get; private set; }

        /// <summary>
        /// server port
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// database name
        /// </summary>
        public string Database { get; private set; }

        /// <summary>
        /// collection name
        /// </summary>
        public string Collection { get; private set; }

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="server"></param>
        /// <param name="port"></param>
        /// <param name="database"></param>
        /// <param name="collection"></param>
        public Settings(string server, int port, string database, string collection)
        {
            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(server), "Server host name may not be null nor empty");

            CompanyGroup.Domain.Utils.Check.Require(port > 0, "Port may not be zero");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(database), "Database name may not be null nor empty");

            CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(collection), "Collection name may not be null nor empty");

            this.Server = server;

            this.Port = port;

            this.Database = database;

            this.Collection = collection;
        }

        /// <summary>
        /// read mongodb connection string
        /// </summary>
        public string ConnectionString
        {
            get { return string.Format("mongodb://{0}:{1}/{2}", Server, Port, Database); }
        }
    }

    /// <summary>
    /// MongoDb-hez tartozó beállításokat összefogó osztály gyártó osztálya 
    /// </summary>
    public class SettingsFactory
    {
        public static Settings Create()
        {
            return new Settings(CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "axeps.hrp.hu"),
                                CompanyGroup.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017),
                                CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "CompanyGroup"),
                                CompanyGroup.Helpers.ConfigSettingsParser.GetString("MongoCollection", "ProductList"));
        }

        static SettingsFactory() { }
    }

}
