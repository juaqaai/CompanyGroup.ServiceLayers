using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// van-e a termékből elegendő - elérhető ellenőrzés
    /// </summary>
    public class ProductOrderCheck
    {
        /// <summary>
        /// rendelés feladás előtti ellenörzés
        /// </summary>
        /// <param name="resultCode">
        ///  1; -- teljesíthető
        /// -1;	-- nincs meg a cikk, nincs ConfigId
        /// -2;	-- nem webes a cikk
        /// -3;	-- kifutott cikk 
        /// -4;	-- kifuto cikk és nincs elegendő
        /// -5; -- cikk nem rendelhető </param>
        /// <param name="availableQuantity">elérhető mennyiség</param>
        public ProductOrderCheck(int resultCode, int availableQuantity)
        {
            this.ResultCode = resultCode;

            this.AvailableQuantity = availableQuantity;
        }

        public int ResultCode { get; set; }

        public int AvailableQuantity { get; set; }
    }
}
