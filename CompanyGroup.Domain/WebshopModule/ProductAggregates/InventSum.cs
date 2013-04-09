using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// Livesystem készlet lekérdezés elemeit összefogó osztály
    /// </summary>
    public class InventSum
    {
        public InventSum(int version, string itemId, int availPhysical, string dataAreaId, string inventLocationId, string configId)
        {
            this.Version = version;
            this.ItemId = itemId;
            this.AvailPhysical = availPhysical;
            this.DataAreaId = dataAreaId;
            this.InventLocationId = inventLocationId;
            this.ConfigId = configId;
        }

        public InventSum() : this(0, String.Empty, 0, String.Empty, String.Empty, String.Empty) { }

        /// <summary>
        /// Change tracking verziószáma
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// cikk azonosító
        /// </summary>
        public string ItemId { get; set; }
            	
        /// <summary>
        /// eladható készlet
        /// </summary>
        public int AvailPhysical { get; set; }
        
        /// <summary>
        /// vállalatkód
        /// </summary>
        public string DataAreaId { get; set; }
        
        /// <summary>
        /// raktárkód (KULSO, 7000, HASZNALT, 2100)
        /// </summary>
        public string InventLocationId { get; set; }

        /// <summary>
        /// konfiguráció (ALAP)
        /// </summary>
        public string ConfigId { get; set; }

    }

    /// <summary>
    /// Livesystem készlet lista
    /// </summary>
    public class InventSumList : List<InventSum>
    {
        /// <summary>
        /// konstruktor InventSum listával
        /// </summary>
        /// <param name="list"></param>
        public InventSumList(List<InventSum> list)
        {
            this.AddRange(list);
        }

        /// <summary>
        /// benne van-e a termék a listában?
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="inventLocationId"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        public bool IsInList(string productId, string dataAreaId, string inventLocationId, string configId)
        {
            return this.Exists(x => { return x.DataAreaId.Equals(dataAreaId, StringComparison.OrdinalIgnoreCase) && x.ItemId.Equals(productId, StringComparison.OrdinalIgnoreCase)
                                                                                                                 && x.InventLocationId.Equals(inventLocationId, StringComparison.OrdinalIgnoreCase)
                                                                                                                 && x.ConfigId.Equals(configId, StringComparison.OrdinalIgnoreCase);
            });
        }

        /// <summary>
        /// megkeresi a terméket a listában, ha megvan, akkor visszaadja annak AvailPhysical értékét, ha nincs meg, akkor visszaadja az availPhysical paraméter értékét
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="availPhysical"></param>
        /// <param name="dataAreaId"></param>
        /// <param name="inventLocationId"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        public int GetStock(string productId, int availPhysical, string dataAreaId, string inventLocationId, string configId)
        {
            InventSum inventSum = this.Find(x => x.DataAreaId.Equals(dataAreaId, StringComparison.OrdinalIgnoreCase) && x.ItemId.Equals(productId, StringComparison.OrdinalIgnoreCase) 
                                                                                                                     && x.InventLocationId.Equals(inventLocationId, StringComparison.OrdinalIgnoreCase)
                                                                                                                     && x.ConfigId.Equals(configId, StringComparison.OrdinalIgnoreCase));

            return (inventSum == null) ? availPhysical : inventSum.AvailPhysical;
        }
    }
}
