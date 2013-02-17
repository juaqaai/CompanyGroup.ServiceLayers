using System;
using System.Collections.Generic;

namespace CompanyGroup.WebClient.Models
{
    /// <summary>
    /// látogató adatokat tartalmazó osztály
    /// </summary>
    public class VisitorData
    {
        public VisitorData() : this(String.Empty, String.Empty, false, true, String.Empty, String.Empty, 0, String.Empty) { }

        /// <summary>
        /// felhasználóhoz kapcsolódó beállítások, (sütibe íródik a tartalma)
        /// </summary>
        /// <param name="visitorId"></param>
        /// <param name="language"></param>
        /// <param name="isShoppingCartOpened"></param>
        /// <param name="isCatalogueOpened"></param>
        /// <param name="currency"></param>
        /// <param name="permanentId"></param>
        /// <param name="cartId"></param>
        /// <param name="registrationId"></param>
        public VisitorData(string visitorId, string language, bool isShoppingCartOpened, bool isCatalogueOpened, string currency, string permanentId, int cartId, string registrationId)
        {
            this.VisitorId = visitorId;

            this.Language = language;

            this.IsShoppingCartOpened = isShoppingCartOpened;

            this.IsCatalogueOpened = isCatalogueOpened;

            this.Currency = currency;

            this.PermanentId = permanentId;

            this.CartId = cartId;

            this.RegistrationId = registrationId;
        }

        /// <summary>
        /// egyedi látogató azonosító
        /// </summary>
        public string VisitorId { set; get; }

        /// <summary>
        /// beállított nyelv
        /// </summary>
        public string Language { set; get; }

        /// <summary>
        /// kosár nyitva van-e?
        /// </summary>
        public bool IsShoppingCartOpened { set; get; }

        /// <summary>
        /// katalógus nyitva van-e?
        /// </summary>
        public bool IsCatalogueOpened { set; get; }

        /// <summary>
        /// beállított valutanem
        /// </summary>
        public string Currency { set; get; }

        /// <summary>
        /// megmaradó Id, (korábbi belépés alkalmával használt Id)
        /// </summary>
        public string PermanentId { set; get; }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        public int CartId { set; get; }

        /// <summary>
        /// regisztrációs azonosító
        /// </summary>
        public string RegistrationId { set; get; }
    }
}
