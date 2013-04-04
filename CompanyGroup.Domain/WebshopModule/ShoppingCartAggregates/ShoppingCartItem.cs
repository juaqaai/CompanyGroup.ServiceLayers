using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// bevásárló kosár elem
    /// </summary>
    public class ShoppingCartItem : CompanyGroup.Domain.Core.EntityBase, IValidatableObject
    {
        /// <summary>
        /// konstruktor, üres kosár elem létrehozása (SetProduct értékadások előkészítése)
        /// </summary>
        public ShoppingCartItem()
        {
            this.LineId = 0;

            this.CartId = 0;

            this.ProductId = String.Empty;

            this.ProductName = String.Empty;

            this.ProductNameEnglish = String.Empty;

            this.PartNumber = String.Empty;

            this.ConfigId = String.Empty;

            this.InventLocationId = String.Empty;

            this.CustomerPrice = 0;

            this.Stock = 0;

            this.ItemState = ItemState.Active;

            this.DataAreaId = "";

            this.Quantity = 0;

            this.Status = CartItemStatus.Created;

            this.CreatedDate = DateTime.Now;
        }

        /// <summary>
        /// sor azonosító
        /// </summary>
        public int LineId { get; set; }

        /// <summary>
        /// kosár azonosító
        /// </summary>
        public int CartId { get; set; }

        /// <summary>
        /// termékazonosító
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// terméknév
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// terméknév angolul
        /// </summary>
        public string ProductNameEnglish { set; get; }

        /// <summary>
        /// cikkszám
        /// </summary>
        public string PartNumber { get; set; }

        /// <summary>
        /// konfiguráció, ahonnan a termék származik (ALAP, XX)
        /// </summary>
        public string ConfigId { set; get; }

        /// <summary>
        /// raktárkód (KULSO, vagy 7000, HASZNALT)
        /// </summary>
        public string InventLocationId { set; get; }

        /// <summary>
        /// vevő ára forintban
        /// </summary>
        public int CustomerPrice { get; set; }

        /// <summary>
        /// ár valutaneme
        /// </summary>
        ///public string Currency { get; set; }

        /// <summary>
        /// készlet - hrp, bsc, külső, belső
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// cikk státusza (aktív, passzív, kifutó)
        /// </summary>
        public ItemState ItemState { get; set; }

        /// <summary>
        /// vállalat, ahová a termék tartozik
        /// </summary>
        public string DataAreaId { get; set; }

        /// <summary>
        /// mennyiség
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// kosár elem státusza (Deleted = 0, Created = 1, Stored = 2, Posted = 3)
        /// </summary>
        public CartItemStatus Status { get; set; }

        /// <summary>
        /// adatbázis bejegyzés keletkezésének dátuma
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// termék beállítása
        /// </summary>
        /// <param name="product"></param>
        public void SetProduct(Product product)
        {
            this.ProductId = product.ProductId;

            this.ProductName = product.ProductName;

            this.ProductNameEnglish = product.ProductNameEnglish;

            this.PartNumber = product.PartNumber;

            this.CustomerPrice = Convert.ToInt32(product.CustomerPrice);

            this.Stock = product.Stock;

            this.ItemState = product.ItemState;

            this.DataAreaId = product.DataAreaId;

            this.Quantity = 1;

            this.ConfigId = product.StandardConfigId;

            this.InventLocationId = product.DataAreaId.ToLower().Equals(CompanyGroup.Domain.Core.Constants.DataAreaIdHrp) ? CompanyGroup.Domain.Core.Constants.OuterStockHrp : CompanyGroup.Domain.Core.Constants.OuterStockBsc;
        }

        /// <summary>
        /// használt cikk beállítása
        /// </summary>
        /// <param name="secondHand"></param>
        /// <param name="productName"></param>
        /// <param name="productNameEnglish"></param>
        /// <param name="partNumber"></param>
        /// <param name="itemState"></param>
        public void SetSecondHandProduct(SecondHand secondHand, string productName, string productNameEnglish, string partNumber, ItemState itemState)
        {
            this.ProductId = secondHand.ProductId;

            this.ProductName = productName;

            this.ProductNameEnglish = productNameEnglish;

            this.PartNumber = partNumber;

            this.CustomerPrice = secondHand.Price;

            this.Stock = 0;         // secondHand.Quantity;

            this.ItemState = itemState;

            this.DataAreaId = secondHand.DataAreaId;

            this.ConfigId = secondHand.ConfigId;

            this.InventLocationId = secondHand.InventLocationId;

            this.Quantity = 1;          
        }

        /// <summary>
        /// használt konfigon van-e a cikk? (XX-el kezdődik és 2100, vagy HASZNALT a raktárkódja)
        /// </summary>
        public bool IsInSecondHand
        {
            get { return (this.ConfigId.ToUpper().StartsWith("XX")) && (this.InventLocationId.Equals(CompanyGroup.Domain.Core.Constants.SecondhandStoreBsc) || 
                                                                        this.InventLocationId.Equals(CompanyGroup.Domain.Core.Constants.SecondhandStoreHrp)); }
        }

        /// <summary>
        /// készleten van-e?
        /// </summary>
        public bool IsInStock
        {
            get
            {
                return (this.Stock > 0);
            }
        }

        /// <summary>
        /// cikk ára összesen (egységár * mennyiség)
        /// </summary>
        public int ItemTotal 
        {
            get { return this.CustomerPrice * this.Quantity; }
        }

        /// <summary>
        /// érvényesség ellenőrzés  
        /// </summary>
        /// <param name="validationContext"><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></param>
        /// <returns><see cref="M:System.ComponentModel.DataAnnotations.IValidatableObject.Validate"/></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();

            if (String.IsNullOrEmpty(this.ProductId))
            {
                validationResults.Add(new ValidationResult(CompanyGroup.Domain.Resources.Messages.validation_ItemIdCannotBeNull, new string[] { "ProductId" }));
            }

            return validationResults;
        }

        #region "EntityBase"overrides"

        /// <summary>
        /// entitás tranziens vizsgálat
        /// </summary>
        /// <returns>Igaz ha az entitás tranziens, egyébként hamis</returns>
        public override bool IsTransient()
        {
            return this.LineId == 0;
        }

        /// <summary>
        /// override-olt egyenlőség vizsgálat
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ShoppingCartItem))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            ShoppingCartItem item = (ShoppingCartItem)obj;

            if (item.IsTransient() || this.IsTransient())
            {
                return false;
            }
            else
            {
                return item.LineId == this.LineId;
            }
        }

        /// <summary>
        /// hash code előállítás
        /// </summary>
        /// <returns></returns>
        public override int GetRequestedHashCode()
        {
            return this.LineId.GetHashCode() ^ 31;
        }

        #endregion
    }
}
