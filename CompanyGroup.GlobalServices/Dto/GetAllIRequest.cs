using System;
using System.Collections.Generic;

namespace CompanyGroup.GlobalServices.Dto
{
    /// <summary>
    /// terméklista kérés elemeit összefogó osztály
    /// </summary>
    public class GetAllRequest
    {
        /// <summary>
        /// az a vállalat, amelyikre a kérés vonatkozik 
        /// </summary>
        /// <remarks>hrp / bsc. Alapértelmezett érték hrp</remarks>
        public string DataAreaId { get; set; }

        /// <summary>
        /// hozzáférési kód
        /// </summary>
        /// <remarks>64 hosszú partnerhez rendelt egyedi azonosító, melyet minden kérésnél el kell küldeni</remarks>
        public string AuthenticationCode { get; set; }

        /// <summary>
        /// akciós cikkek
        /// </summary>
        /// <remarks>true / false alapértelmezett érték a false</remarks>
        public bool ActionFilter { get; set; }

        /// <summary>
        /// gyártóazonosítók lista 
        /// </summary>
        /// <remarks>
        /// ha üres, akkor nincs szűrés, az eredménylista minden elemet tartalmaz
        /// ha egy elemet tartalmaz, pl.: A004, akkor a válaszban az összes olyan termék szerepel, mely gyártójának azonosítója A004
        /// ha több elemet tartalmaz, akkor ugyanaz érvényes mint az előző esetben, a gyártói azonosítók közötti VAGY kapcsolattal
        /// </remarks>
        public List<string> ManufacturerIdList { get; set; }

        /// <summary>
        /// elsődleges termékjelleg lista
        /// </summary>
        /// <remarks>
        /// ha üres, akkor nincs szűrés, az eredménylista minden elemet tartalmaz
        /// ha egy elemet tartalmaz, pl.: B052, akkor a válaszban az összes olyan termék szerepel, mely elsődleges termékjellegének azonosítója B052
        /// ha több elemet tartalmaz, akkor ugyanaz érvényes mint az előző esetben, az elsődleges termékjellegazonosítók közötti VAGY kapcsolattal
        /// </remarks>
        public List<string> Category1IdList { get; set; }

        /// <summary>
        /// másodlagos termékjelleg lista
        /// </summary>
        /// <remarks>
        /// ha üres, akkor nincs szűrés, az eredménylista minden elemet tartalmaz
        /// ha egy elemet tartalmaz, pl.: B052, akkor a válaszban az összes olyan termék szerepel, mely másodlagos termékjellegének azonosítója B052
        /// ha több elemet tartalmaz, akkor ugyanaz érvényes mint az előző esetben, a másodlagos termékjellegazonosítók közötti VAGY kapcsolattal
        /// </remarks>
        public List<string> Category2IdList { get; set; }

        /// <summary>
        /// harmadlagos termékjelleg lista
        /// </summary>
        /// <remarks>
        /// ha üres, akkor nincs szűrés, az eredménylista minden elemet tartalmaz
        /// ha egy elemet tartalmaz, pl.: C087, akkor a válaszban az összes olyan termék szerepel, mely harmadlagos termékjellegének azonosítója C087
        /// ha több elemet tartalmaz, akkor ugyanaz érvényes mint az előző esetben, a harmadlagos termékjellegazonosítók közötti VAGY kapcsolattal
        /// </remarks>
        public List<string> Category3IdList { get; set; }

        /// <summary>
        /// készleten szűrő 
        /// </summary>
        /// <remarks>true / false. Alapértelmezett érték false</remarks>
        public bool StockFilter { get; set; }

        /// <summary>
        /// kért oldalindex
        /// </summary>
        /// <remarks>
        /// [0..N] intervallumban megadható egész szám. Alapértelmezett értéke 0
        /// A rendszer a válaszüzenetben ezt az oldalt adja vissza.
        /// </remarks>
        public int CurrentPageIndex { get; set; }

        /// <summary>
        /// elemek száma az oldalon
        /// </summary>
        /// <remarks>Egész szám. Alapértelmezett értéke 50</remarks>
        public int ItemsOnPage { get; set; }

        /// <summary>
        /// konstruktor, alapértelmezett értékek beállításával
        /// </summary>
        public GetAllRequest()
        {
            this.DataAreaId = "hrp";

            AuthenticationCode = String.Empty; 

            this.ActionFilter = false;

            this.ManufacturerIdList = new List<string>();

            this.Category1IdList = new List<string>();

            this.Category2IdList = new List<string>();

            this.Category3IdList = new List<string>();

            this.StockFilter = false;

            this.CurrentPageIndex = 0;

            this.ItemsOnPage = 50;
        }
    }
}
