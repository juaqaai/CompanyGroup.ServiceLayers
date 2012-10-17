using System;

namespace CompanyGroup.Domain.Core
{
    /// <summary>
    /// NoSql Entity absztrakt ősosztály (csak származtatáshoz)
    /// </summary>
    public abstract class NoSqlEntity : EntityBase, INoSqlEntity
    {
        /// <summary>
        /// perzisztens objektum azonosító beállítása, kiolvasása
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonId(Order = 1)]    //IdGenerator = typeof(MongoDB.Bson.Serialization.IdGenerators.BsonObjectIdGenerator)
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public MongoDB.Bson.ObjectId Id { set; get; }

        /// <summary>
        /// entitás tranziens vizsgálat
        /// </summary>
        /// <returns>Igaz ha az entitás tranziens, egyébként hamis</returns>
        public override bool IsTransient()
        {
            return this.Id == MongoDB.Bson.BsonObjectId.Empty;
        }

        /// <summary>
        /// override-olt egyenlőség vizsgálat
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is NoSqlEntity))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            NoSqlEntity item = (NoSqlEntity)obj;

            if (item.IsTransient() || this.IsTransient())
            {
                return false;
            }
            else
            {
                return item.Id == this.Id;
            }
        }

        /// <summary>
        /// hash code segédmetódus
        /// </summary>
        /// <returns></returns>
        public override int GetRequestedHashCode()
        {
            return this.Id.GetHashCode() ^ 31;
        }

    }
}
