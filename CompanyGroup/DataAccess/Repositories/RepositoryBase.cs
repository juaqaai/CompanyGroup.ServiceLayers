
namespace Shared.Web.DataAccess.NoSql
{
    using System;
    using System.Collections.Generic;

    public class RepositoryBase<T> where T : class
    {
        //mongodb://127.0.0.1/MongoBlog?strict=false

        private static readonly string MongoServerHost = Shared.Web.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "195.30.7.59");

        private static readonly int MongoServerPort = Shared.Web.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017);

        private static readonly string MongoDatabaseName = Shared.Web.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "web");

        private Norm.Mongo db_server;

        private string _collectionName = String.Empty;

        private string _connectionString = String.Empty;

        /// <summary>
        /// üres konstruktor    
        /// </summary>
        public RepositoryBase() : this(String.Empty) { }

        /// <summary>
        /// konstruktor collection name-el 
        /// </summary>
        public RepositoryBase(string collectionName)
            : this("mongodb://" + MongoServerHost + ":" + MongoServerPort + "/" + MongoDatabaseName, collectionName) { }

        /// <summary>
        /// konstruktor connection stringgel, collection name-el
        /// </summary>
        /// <param name="connectionString"></param>
        public RepositoryBase(string connectionString, string collectionName)
        {            
            _connectionString = connectionString;
            _collectionName = collectionName;
        }

        /// <summary>
        /// destruktor
        /// </summary>
        ~RepositoryBase()
        {
            DisconnectFromDatabase();
        }

        /// <summary>
        /// kapcsolat felepitese az adatbazishoz
        /// </summary>
        protected void ConnectToDatabase()
        {
            db_server = Norm.Mongo.Create(_connectionString);

            return;
        }

        /// <summary>
        /// adatbazis kapcsolat bontasa
        /// </summary>
        protected void DisconnectFromDatabase()
        {
            if (db_server != null)
            {
                db_server.Dispose();
            }
        }

        /// <summary>
        /// szerver példány kiolvasása
        /// </summary>
        protected Norm.Mongo DbServer
        {
            get { return db_server; }
        }

        /// <summary>
        /// kollekció kiolvasása
        /// </summary>
        protected Norm.Collections.IMongoCollection<T> GetCollection(string collectionName)
        {
            return db_server.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// kollekció kiolvasása
        /// </summary>
        protected Norm.Collections.IMongoCollection<T> GetCollection()
        {
            return db_server.GetCollection<T>(_collectionName);
        }

        /// <summary>
        /// kollekció törlése
        /// </summary>
        /// <returns></returns>
        protected bool DropCollection()
        {
            return db_server.Database.DropCollection(_collectionName);
        }

        /// <summary>
        /// kollekció törlése
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        protected bool DropCollection(string collectionName)
        {
            return db_server.Database.DropCollection(collectionName);
        }
    }
}
