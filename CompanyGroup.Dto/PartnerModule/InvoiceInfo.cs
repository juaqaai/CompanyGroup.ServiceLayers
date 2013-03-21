using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// számla információ (fejléc elemek, lapozó, elemek száma)
    /// </summary>
    public class InvoiceInfo
    {
        /// <summary>
        /// számla fejléc lista
        /// </summary>
        public List<Invoice> Items { get; set; }

        /// <summary>
        /// lapozó
        /// </summary>
        public Pager Pager { get; set; }

        /// <summary>
        /// elemek száma
        /// </summary>
        public long ListCount { get; set; }

        /// <summary>
        /// összes tartozás
        /// </summary>
        //public string TotalNettoCredit { get; set; }

        /// <summary>
        /// összes lejárt tartozás
        /// </summary>
        //public string AllOverdueDebts { get; set; }
    }

    /// <summary>
    /// számla listaelem
    /// </summary>
    //public class InvoiceInfo
    //{
    //    /// <summary>
    //    /// VR, vagy BR azonosító
    //    /// </summary>
    //    public string SalesId { set; get; }

    //    /// <summary>
    //    /// bizonylat elkészülte
    //    /// </summary>
    //    public string InvoiceDate { set; get; }

    //    /// <summary>
    //    /// bizonylat lejárati ideje
    //    /// </summary>
    //    public string DueDate { set; get; }

    //    /// <summary>
    //    /// számla végösszege
    //    /// </summary>
    //    public string InvoiceAmount { set; get; }

    //    /// <summary>
    //    /// számlán lévő tartozás
    //    /// </summary>
    //    public string InvoiceCredit { set; get; }

    //    /// <summary>
    //    /// pénznem
    //    /// </summary>
    //    public string CurrencyCode { set; get; }

    //    /// <summary>
    //    /// számla azonosító
    //    /// </summary>
    //    public string InvoiceId { set; get; }

    //    /// <summary>
    //    /// fizetési feltétel
    //    /// </summary>
    //    public string Payment { set; get; }

    //    /// <summary>
    //    /// számla típusa (0 napló, 1 árajánlat, 2 előfizetés, 3 értékesítés, 4 viszáru, 5 keretrendelés, 6 cikkszükséglet)
    //    /// </summary>
    //    public int SalesType { set; get; }

    //    /// <summary>
    //    /// vevői hivatkozás
    //    /// </summary>
    //    public string CusomerRef { set; get; }

    //    /// <summary>
    //    /// számlán szereplő név
    //    /// </summary>
    //    public string InvoicingName { set; get; }

    //    /// <summary>
    //    /// számlázási cím
    //    /// </summary>
    //    public string InvoicingAddress { set; get; }

    //    /// <summary>
    //    /// kapcsolattartó
    //    /// </summary>
    //    public string ContactPersonId { set; get; }

    //    /// <summary>
    //    /// nyomtatva
    //    /// </summary>
    //    public bool Printed { set; get; }

    //    /// <summary>
    //    /// visszáru 
    //    /// </summary>
    //    public string ReturnItemId { set; get; }

    //    /// <summary>
    //    /// sorok
    //    /// </summary>
    //    public List<InvoiceLineInfo> Lines { get; set; }
    //}
}
