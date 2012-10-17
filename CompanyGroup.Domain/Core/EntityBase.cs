using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.Core
{
    /// <summary>
    /// Entity absztrakt ősosztály (csak származtatáshoz)
    /// </summary>
    public abstract class EntityBase
    {

        protected int? requestedHashCode;

         /// <summary>
        /// egyenlőség operátor
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator == (EntityBase left, EntityBase right)
        {
            if (Object.Equals(left, null))
            {
                return (Object.Equals(right, null)) ? true : false;
            }
            else
            {
                return left.Equals(right);
            }
        }

        /// <summary>
        /// nem egyenlő operátor
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator != (EntityBase left, EntityBase right)
        {
            return !(left == right);
        }

        /// <summary>
        /// equals metódust kötelező felülírni
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public abstract override bool Equals(object obj);

        /// <summary>
        /// felülírandó az isTransient metódus
        /// </summary>
        /// <returns></returns>
        public abstract bool IsTransient();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract int GetRequestedHashCode();

        /// <summary>
        /// override-olt hash kód lekérdezés
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!requestedHashCode.HasValue)
                {
                    requestedHashCode = GetRequestedHashCode();
                }
                return requestedHashCode.Value;
            }
            else
            {
                return base.GetHashCode();
            }
        }
    }
}
