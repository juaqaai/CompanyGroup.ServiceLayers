using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.NoSql
{
    public interface ISettings
    {
        string Server { get; }

        int Port { get; }

        string Database { get; }

        string Collection { get; }

        string ConnectionString { get; }
    }
}
