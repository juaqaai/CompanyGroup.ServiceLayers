using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace CompanyGroup.Data.NoSql
{
    public class MongoHelper<T> where T : class
    {
        public MongoCollection<T> Collection { get; private set; }

        public MongoHelper(ISettings settings)
        {
            MongoConnectionStringBuilder connectionStringBuilder = new MongoConnectionStringBuilder(settings.ConnectionString);

            MongoServer server = MongoServer.Create(connectionStringBuilder);

            MongoDatabase db = server.GetDatabase(connectionStringBuilder.DatabaseName);

            Collection = db.GetCollection<T>(typeof(T).Name.ToLower());
        }
    }
}
