using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// Livesystem árkedvezmények elemeit összefogó osztály
    /// Version	Operation	ItemRelation	AccountRelation	Amount	Currency	DataAreaId
    /// </summary>
    public class PriceDiscTable
    {
        public PriceDiscTable(int version, string itemRelation, string accountRelation, int amount, string dataAreaId, string currency)
        {
            this.Version = version;
            this.ItemRelation = itemRelation;
            this.AccountRelation = accountRelation;
            this.Amount = amount;
            this.DataAreaId = dataAreaId;
            this.Currency = currency;

        }

        public PriceDiscTable() : this(0, String.Empty, String.Empty, 0, String.Empty, String.Empty) { }

        /// <summary>
        /// Change tracking verziószáma
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// cikk azonosító
        /// </summary>
        public string ItemRelation { get; set; }

        /// <summary>
        /// vagy 1..5 ár, vagy vevőkód
        /// </summary>
        public string AccountRelation { get; set; }
            	
        /// <summary>
        /// összeg
        /// </summary>
        public int Amount { get; set; }
        
        /// <summary>
        /// vállalatkód
        /// </summary>
        public string DataAreaId{ get; set; }
        
        /// <summary>
        /// pénznem
        /// </summary>
        public string Currency{ get; set; }

    }

    /// <summary>
    /// Livesystem készlet lista
    /// </summary>
    public class PriceDiscTableList : List<PriceDiscTable>
    {
        public PriceDiscTableList(List<PriceDiscTable> list)
        {
            this.AddRange(list);
        }

        /// <summary>
        /// benne van-e a termék a változás listában?
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="accountRelation"></param>
        /// <returns></returns>
        public bool IsInList(string productId, string dataAreaId, string accountRelation)
        {
            if (String.IsNullOrEmpty(productId) || String.IsNullOrEmpty(dataAreaId) || String.IsNullOrEmpty(accountRelation))
            {
                return false;
            }

            return this.Exists(x => {

                return x.DataAreaId.Equals(dataAreaId, StringComparison.OrdinalIgnoreCase) && x.ItemRelation.Equals(productId, StringComparison.OrdinalIgnoreCase)
                                                                                           && x.AccountRelation.Equals(accountRelation, StringComparison.OrdinalIgnoreCase); 
            });
        }

        /// <summary>
        /// megkeresi a terméket a listában, ha megvan, akkor visszaadja annak árát
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="accountRelation"></param>
        /// <returns></returns>
        public int GetPrice(string productId, string dataAreaId, string accountRelation)
        {
            PriceDiscTable priceDiscTable = this.Find(x => 
            {
                return x.DataAreaId.Equals(dataAreaId, StringComparison.OrdinalIgnoreCase) && x.ItemRelation.Equals(productId, StringComparison.OrdinalIgnoreCase)
                                                                                           && (x.AccountRelation.Equals(accountRelation, StringComparison.OrdinalIgnoreCase)); 
            });

            int amount = (priceDiscTable == null) ? 0 : priceDiscTable.Amount;

            return amount;
        }

        /// <summary>
        /// visszaadja az 1..5 sorszámokat egy listában, illetve ha nem üres a customerId paraméter, akkor a listához hozzáadja annak értékét is
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private List<string> GetAccountRelationIntervall(string customerId)
        {
            List<string> intervall = new List<string>() { "1", "2", "3", "4", "5" };

            if (!String.IsNullOrEmpty(customerId))
            {
                intervall.Add(customerId);
            }

            return intervall;
        }

        /// <summary>
        /// overloading for GetAccountRelationIntervall
        /// </summary>
        /// <returns></returns>
        private List<string> GetAccountRelationIntervall()
        {
            return GetAccountRelationIntervall(String.Empty);
        }
    }
}
