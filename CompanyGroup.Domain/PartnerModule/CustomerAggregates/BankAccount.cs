using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// bankszámlaszám (16, vagy 24 hosszú, 3 * 8 számjegyre szokás felbontani kötőjellel elválasztva)   
    /// </summary>
    public class BankAccount : CompanyGroup.Domain.Core.EntityBase, System.IComparable
    {
        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="number"></param>
        /// <param name="recId"></param>
        public BankAccount(string number, Int64 recId)
        {
            this.Part1 = String.Empty;
            this.Part2 = String.Empty;
            this.Part3 = String.Empty;
            this.Number = number;
            this.RecId = recId;
            this.SplitBankAccount();
        }

        public BankAccount(string part1, string part2, string part3, Int64 recId)
        {
            this.Part1 = part1;
            this.Part2 = part2;
            this.Part3 = part3;
            this.Number = ConcatBankAccountParts();
            this.RecId = recId;
        }

        /// <summary>
        /// üres konstruktor
        /// </summary>
        public BankAccount() : this(String.Empty, 0) { }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Part1")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Part1 { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Part2")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Part2 { set; get; }

        [MongoDB.Bson.Serialization.Attributes.BsonElement("Part3")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Part3 { set; get; }

        /// <summary>
        /// bankszámla száma
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("Number")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public string Number { set; get; }

        /// <summary>
        /// egyedi azonosító
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonElement("RecId")]
        [MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        [MongoDB.Bson.Serialization.Attributes.BsonRequired]
        public Int64 RecId { set; get; }

        /// <summary>
        /// művelet
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("CrudMethod", Order = 3)]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue("")]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        //public CrudMethod CrudMethod { set; get; }

        /// <summary>
        /// sorszám
        /// </summary>
        //[MongoDB.Bson.Serialization.Attributes.BsonElement("Id")]
        //[MongoDB.Bson.Serialization.Attributes.BsonDefaultValue(0)]
        //[MongoDB.Bson.Serialization.Attributes.BsonRequired]
        //public int Id { set; get; }

        public int CompareTo(object o)
        {
            BankAccount tmp = (BankAccount)o;

            string s1 = tmp.Part1 + tmp.Part2 + tmp.Part3;

            string s2 = this.Part1 + this.Part2 + this.Part3;

            return s1.CompareTo(s2);
        }

        /// <summary>
        /// részlánc összefűzés
        /// <returns></returns>
        public string ConcatBankAccountParts()
        {
            return String.Format("{0}-{1}-{2}", this.Part1, this.Part2, this.Part3);
        }

        /// <summary> 
        /// karakterlánc bankszámla obj.-ba konvertálása
        /// </summary>
        /// <param name="sItem"></param>
        /// <returns></returns>
        public void SplitBankAccount()
        {
            if (String.IsNullOrWhiteSpace(this.Number))
            {
                return;
            }

            string[] arr = this.Number.Split('-');

            if (arr.Length.Equals(3))
            {
                this.Part1 = arr[0];
                this.Part2 = arr[1];
                this.Part3 = arr[2];
            }
        }

        /// <summary>
        /// entitás tranziens vizsgálat
        /// </summary>
        /// <returns>Igaz ha az entitás tranziens, egyébként hamis</returns>
        public override bool IsTransient()
        {
            return RecId == 0;
        }

        /// <summary>
        /// egyezőség vizsgálat
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is BankAccount))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            BankAccount item = (BankAccount)obj;

            if (item.IsTransient() || this.IsTransient())
            {
                return false;
            }
            else
            {
                return item.RecId == this.RecId;
            }
        }

        /// <summary>
        /// hash kód
        /// </summary>
        /// <returns></returns>
        public override int GetRequestedHashCode()
        {
            return this.RecId.GetHashCode() ^ 31;
        }
    }
}
