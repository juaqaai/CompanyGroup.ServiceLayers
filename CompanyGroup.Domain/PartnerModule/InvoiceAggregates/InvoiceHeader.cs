using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    ///    InvoiceDate	            SourceCompany	DueDate	                InvoiceAmount	InvoiceCredit	CurrencyCode	InvoiceId	LineAmount	TaxAmount	LineAmountMst	TaxAmountMst
    ///    2013-01-31 00:00:00.000	hrp	            2013-02-08 00:00:00.000	406070.00	    406070.00	    HUF	            HI004045/13	319740.00	86329.80	319740.00	    86329.80
    /// </summary>
    public class InvoiceHeader
    {
        public int Id { get; set; }	

        public DateTime InvoiceDate { get; set; }	            
        
        public string SourceCompany	 { get; set; }
            
        public DateTime DueDate	{ get; set; }

        public decimal InvoiceAmount { get; set; }

        public decimal InvoiceCredit { get; set; }  	
                
        public string CurrencyCode	{ get; set; }  	
                    
        public string InvoiceId	{ get; set; }  	
                    
        public decimal LineAmount	{ get; set; }  	
                        
        public decimal TaxAmount	{ get; set; }  	
                        
        public decimal LineAmountMst	{ get; set; }

        public decimal TaxAmountMst { get; set; }

        public bool OverDue { get; set; }

        public InvoiceHeader(int id, DateTime invoiceDate, string sourceCompany, DateTime dueDate, decimal invoiceAmount, decimal invoiceCredit, string currencyCode, string invoiceId, decimal lineAmount, decimal taxAmount, decimal lineAmountMst, decimal taxAmountMst, bool overDue) 
        {
            this.Id = id;

            this.InvoiceDate = invoiceDate;
            
            this.SourceCompany = sourceCompany;

            this.DueDate = dueDate;

            this.InvoiceAmount = invoiceAmount;

            this.InvoiceCredit = invoiceCredit;

            this.CurrencyCode = currencyCode;

            this.InvoiceId = invoiceId;

            this.LineAmount = lineAmount;

            this.TaxAmount = taxAmount;

            this.LineAmountMst = lineAmountMst;

            this.TaxAmountMst = taxAmountMst;

            this.OverDue = overDue;
        }

        public InvoiceHeader() : this(0, DateTime.MinValue, "", DateTime.MinValue, 0, 0, "", "", 0, 0, 0, 0, false) { }
    }
}
