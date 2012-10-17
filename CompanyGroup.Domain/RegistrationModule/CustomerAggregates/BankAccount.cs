using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.RegistrationModule
{
    /// <summary>
    /// regisztrációs bankszámlaszám 
    /// </summary>
    public class BankAccount : CompanyGroup.Domain.PartnerModule.BankAccount, CompanyGroup.Domain.Core.INoSqlEntity
    {
        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="number"></param>
        /// <param name="recId"></param>
        public BankAccount(string number, Int64 recId) : base(number, recId) 
        {
            this.Id = MongoDB.Bson.ObjectId.GenerateNewId();
        }

        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="part1"></param>
        /// <param name="part2"></param>
        /// <param name="part3"></param>
        /// <param name="recId"></param>
        public BankAccount(string part1, string part2, string part3, Int64 recId) : base(part1, part2, part3, recId) 
        {
            this.Id = MongoDB.Bson.ObjectId.GenerateNewId();
        }

        /// <summary>
        /// mongodb objectId
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonId(Order = 1)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public MongoDB.Bson.ObjectId Id { set; get; }
    }
}
