using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.Core
{
    public interface INoSqlEntity
    {
        /// <summary>
        /// Id getter, setter property implementáció
        /// </summary>
        MongoDB.Bson.ObjectId Id { get; set; }
    }
}
