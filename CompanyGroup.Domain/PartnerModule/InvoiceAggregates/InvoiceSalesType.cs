namespace CompanyGroup.Domain.PartnerModule
{

    /// <summary>
    /// számla típusa (0 napló, 1 árajánlat, 2 előfizetés, 3 értékesítés, 4 viszáru, 5 keretrendelés, 6 cikkszükséglet)
    /// </summary>
    public enum InvoiceSalesType { Journal = 0, Quotation = 1, Subscription = 2, Sales = 3, ReturnItem = 4, Blanket = 5, ItemReq = 6 }

}