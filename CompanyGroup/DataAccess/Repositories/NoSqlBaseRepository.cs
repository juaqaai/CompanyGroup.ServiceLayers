
namespace CompanyGroup.DataAccess
{
    using System;
    using System.Collections.Generic;

    using MongoDB.Bson;
    using MongoDB.Driver;

    public class NoSqlBaseRepository<T> where T : class
    {
        private static readonly string ServerHost = Shared.Web.Helpers.ConfigSettingsParser.GetString("MongoServerHost", "195.30.7.59");

        private static readonly int ServerPort = Shared.Web.Helpers.ConfigSettingsParser.GetInt("MongoServerPort", 27017);

        private static readonly string DatabaseName = Shared.Web.Helpers.ConfigSettingsParser.GetString("MongoDatabaseName", "web");

        protected static readonly string DataAreaIdSerbia = Shared.Web.Helpers.ConfigSettingsParser.GetString("DataAreaIdSerbia", "ser");

        private string _collectionName = String.Empty;

        private string _connectionString = String.Empty;

        private MongoServer _server = null;

        private MongoDatabase _database = null;

        /// <summary>
        /// üres konstruktor    
        /// </summary>
        public NoSqlBaseRepository() : this(String.Empty) { }

        /// <summary>
        /// konstruktor collection name-el 
        /// </summary>
        /// <param name="collectionName"></param>
        public NoSqlBaseRepository(string collectionName) : this( String.Format("mongodb://{0}:{1}/{2}", ServerHost, ServerPort, DatabaseName), collectionName ) { }

        /// <summary>
        /// konstruktor connection stringgel, collection name-el
        /// </summary>
        /// <param name="connectionString">
        /// mongodb://[username:password@]hostname[:port][/database]
        /// mongodb:://server1,server2:27017,server2:27018
        /// </param>
        /// <param name="collectionName"></param>
        public NoSqlBaseRepository(string connectionString, string collectionName)
        {            
            _connectionString = connectionString;
            _collectionName = collectionName;
        }

        /// <summary>
        /// kapcsolat felepitese az adatbazishoz
        /// </summary>
        protected void ConnectToDatabase()
        {
            try
            {
                if (_server != null)
                {
                    _database = null;
                    _server.Disconnect();
                }

                _server = MongoServer.Create(_connectionString);

                _database = _server.GetDatabase(DatabaseName);

                return;
            }
            catch { }
        }

        /// <summary>
        /// adatbazis kapcsolat bontasa
        /// </summary>
        protected void DisconnectFromDatabase()
        {
            try
            {
                if (_server != null)
                {
                    _database = null;

                    _server.Disconnect();
                }
            }
            catch { }
        }

        /// <summary>
        /// kollekció kiolvasása kollekció nevének megadása alapján
        /// </summary>
        protected MongoCollection<T> GetCollection(string collectionName)
        {
            return _database.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// kollekció kiolvasása a konstruktor paraméterében megadott kollekciónév alapján
        /// </summary>
        protected MongoCollection<T> GetCollection()
        {
            return _database.GetCollection<T>(_collectionName);
        }

        /// <summary>
        /// kollekció törlése a konstruktor paraméterében megadott kollekciónév alapján
        /// </summary>
        /// <returns></returns>
        protected void DropCollection()
        {
            try
            {
                if (!String.IsNullOrEmpty(_collectionName))
                {
                    _database.DropCollection(_collectionName);
                }
            }
            catch { }
        }

        /// <summary>
        /// nosql kollekció törlése kollekció nevének megadása alapján
        /// </summary>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        protected void DropCollection(string collectionName)
        {
            try
            {
                if (!String.IsNullOrEmpty(collectionName))
                {
                    _database.DropCollection(collectionName);
                }
            }
            catch { }
        }

        /// <summary>
        /// nosql adatbázis példány visszaadása
        /// </summary>
        protected MongoDatabase Database
        {
            get { return _database; }
        }

        /// <summary>
        /// törli a kollekcióhoz tartozó összes indexet 
        /// </summary>
        /// <returns></returns>
        public bool DeleteIndexes()
        {
            try
            {
                ConnectToDatabase();

                MongoCollection<T> collection = this.GetCollection();

                collection.DropAllIndexes();

                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                DisconnectFromDatabase();
            }
        }

        /// <summary>
        /// string azonosító nosql objectId-re konvertálása
        /// </summary>
        /// <param name="objectId"></param>
        /// <returns></returns>
        protected ObjectId ConvertStringToBsonObjectId(string objectId)
        {
            try
            {
                if (String.IsNullOrEmpty(objectId))
                {
                    return ObjectId.Empty;
                }

                ObjectId _objectId = ObjectId.Empty;

                bool b = ObjectId.TryParse(objectId, out _objectId);

                return (b) ? _objectId : ObjectId.Empty;
            }
            catch { return ObjectId.Empty; }
        }

    }
}
