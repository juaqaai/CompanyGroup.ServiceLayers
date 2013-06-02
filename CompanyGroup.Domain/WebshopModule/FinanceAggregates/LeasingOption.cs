using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// lízing opció
    /// InternetUser.cms_LeasingByFinancedAmount( @FinancedAmount int = 0 )
    /// </summary>
    /// <remarks>
    /// Id	FromValue	ToValue	NumOfMonth	PercentValue	InterestRate	PresentValue
    /// 5	900000	    1999999	24	        5.351000000000	13.400000000000	1.046000000000
    /// </remarks>
    public class LeasingOption //: CompanyGroup.Domain.Core.Entity
    {

        public LeasingOption(int id, int intervalFrom, int intervalTo, int numOfMonth, double percentValue, double presentValue, double interestRate)
        {
            this.Id = id;

            this.IntervalFrom = intervalFrom;

            this.IntervalTo = IntervalTo;

            this.NumOfMonth = numOfMonth;

            this.PercentValue = percentValue;

            this.PresentValue = presentValue;

            this.InterestRate = interestRate;

            this.CalculatedValue = 0;
        }

        public LeasingOption() : this(0, 0, 0, 0, 0, 0, 0) { }

        /// <summary>
        /// FinanceParameterId
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// érték intervallum alsó határa
        /// </summary>
        public int IntervalFrom { get; set; }

        /// <summary>
        /// érték intervallum felső határa
        /// </summary>
        public int IntervalTo { get; set; }

        /// <summary>
        /// hónapok száma
        /// </summary>
        public int NumOfMonth { get; set; }

        /// <summary>
        /// százalékos érték
        /// </summary>
        public double PercentValue { get; set; }

        /// <summary>
        /// maradvány érték
        /// </summary>
        public double PresentValue { get; set; }

        /// <summary>
        /// kamat
        /// </summary>
        public double InterestRate { get; set; }

        /// <summary>
        /// számított érték
        /// </summary>
        public double CalculatedValue { get; set; }
      
    }

    /// <summary>
    /// Lízing opciók
    /// </summary>
    public class LeasingOptions : List<LeasingOption>
    {
        public LeasingOptions(MinMaxLeasingValue minMaxLeasingValue, List<LeasingOption> leasingOptions)
        {
            this.MinMaxLeasingValue = minMaxLeasingValue;

            this.Message = "";

            this.AddRange(leasingOptions);

            this.Amount = 0;
        }

        /// <summary>
        /// finanszírozandó összeg
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// "A kalkulálható maximum érték " + maxValue + " forint. Ennél nagyobb értékre már kedvező, egyedi ajánlat vonatkozik, melyet kérjen személyesen munkatársunktól!"
        /// A megadott érték " + minValue + " és " + maxValue + " határok közötti egész szám legyen!
        /// 
        /// </summary>
        public string Message { get; private set; }

        /// <summary>
        /// tartós bérlet számítás legkissebb és legnagyobb értékét tartalmazó adattípus
        /// </summary>
        public MinMaxLeasingValue MinMaxLeasingValue { get; set; }

        /// <summary>
        /// érték ellenörzése 
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool ValidateAmount()
        {
            if (this.Amount > this.MinMaxLeasingValue.MinValue && this.Amount < this.MinMaxLeasingValue.MaxValue)
            { 
                return true;
            }
            else if (this.Amount > this.MinMaxLeasingValue.MaxValue)
            {
                this.Message = "A kalkulálható maximum érték " + this.MinMaxLeasingValue.MaxValue + " forint. Ennél nagyobb értékre már kedvező, egyedi ajánlat vonatkozik, melyet kérjen személyesen munkatársunktól!";

                return false;
            }
            else
            {
                this.Message = "A megadott érték " + this.MinMaxLeasingValue.MinValue + " és " + this.MinMaxLeasingValue.MaxValue + " határok közötti egész szám legyen!";

                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="amount">finanszírozandó összeg</param>
        /// <param name="d">GetLeasingByFinancedAmount lekérdezés eredményeképpen előállt PercentValue adatbázis mező érték</param>
        /// <returns></returns>
        public static double CalculateValue(double d, double amount)
        {
            double ret = 0;
            try
            {
                ret = (d / 100) * amount;
            }
            catch
            {
                return ret;
            }
            return ret;
        }

        /// <summary>
        /// beállítja az összes CalculatedValue nevű mező értékét
        /// </summary>
        /// <param name="amount"></param>
        public void CalculateAllValue(double amount)
        {
            this.ForEach( x => {
                
                x.CalculatedValue = CalculateValue(x.PercentValue, amount);
                
            });
        }

        /// <summary>
        /// beállítja az összes CalculatedValue nevű mező értékét az Amount mező alapján
        /// </summary>
        public void CalculateAllValue()
        {
            this.CalculateAllValue(this.Amount);
        }


    }
}
