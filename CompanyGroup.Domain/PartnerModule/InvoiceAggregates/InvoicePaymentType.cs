namespace CompanyGroup.Domain.PartnerModule
{

    /// <summary>
    /// számla fizetés típusa (0 összes, 1 kifizetetlen, 2 lejárt)
    /// </summary>
    public enum InvoicePaymentType { All = 0, Unpaid = 1, OverDue = 2 }

}