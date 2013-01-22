using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.Core
{
    /// <summary>
    /// absztrakt ősosztály (csak származtatáshoz)
    /// </summary>
    public abstract class Entity : EntityBase
    {
        public int Id { set; get; }

        /// <summary>
        /// entitás tranziens vizsgálat
        /// </summary>
        /// <returns>Igaz ha az entitás tranziens, egyébként hamis</returns>
        public override bool IsTransient()
        {
            return (this.Id == 0);
        }

        /// <summary>
        /// override-olt egyenlőség vizsgálat
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            Entity item = (Entity)obj;

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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
