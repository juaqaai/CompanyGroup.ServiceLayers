using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.Core
{
    /// <summary>
    /// enum -> string, string -> enum konverziok
    /// </summary>
    public class Adapter
    {
        private static readonly string DataAreaIdHrp = "hrp";

        private static readonly string DataAreaIdBsc = "bsc";

        private static readonly string DataAreaIdSerbia = "ser";

        private static readonly string LanguageHungarian = "hu";

        private static readonly string LanguageEnglish = "en";

        private static readonly string LanguageSerbian = "ser";

        /// <summary>
        /// vállalatkód enum -> string konverzió
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public static string ConvertDataAreaIdEnumToString(DataAreaId dataAreaId)
        {
            if (dataAreaId.Equals(DataAreaId.Bsc))
                return DataAreaIdBsc;
            else if (dataAreaId.Equals(DataAreaId.Hrp))
                return DataAreaIdHrp;
            else
                return DataAreaIdSerbia;
        }

        /// <summary>
        /// vállalatkód string -> enum konverzió
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        public static DataAreaId ConvertDataAreaIdStringToEnum(string dataAreaId)
        {
            if (dataAreaId.Equals(DataAreaIdBsc))
                return DataAreaId.Bsc;
            else if (dataAreaId.Equals(DataAreaIdHrp))
                return DataAreaId.Hrp;
            else
                return DataAreaId.Serbian;
        }

        public static int ConvertItemStateEnumToInt(ItemState itemState)
        {
            return (int)itemState;
        }

        public static bool ConvertItemStateEnumToBool(ItemState itemState)
        {
            return itemState == ItemState.EndOfSales;
        }

        public static ItemState ConvertItemStateIntToEnum(int itemState)
        {
            return (ItemState)itemState;
        }

        /// <summary>
        /// nyelv enum -> string konverzió
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static string ConvertLanguageEnumToString(Language language)
        {
            if (language.Equals(Language.English))
                return LanguageEnglish;
            else if (language.Equals(Language.Hungarian))
                return LanguageHungarian;
            else
                return LanguageSerbian;
        }

        /// <summary>
        /// nyelv string -> enum konverzió
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public static Language ConvertLanguageStringToEnum(string language)
        {
            if (language.Equals(LanguageEnglish))
                return Language.English;
            else if (language.Equals(LanguageHungarian))
                return Language.Hungarian;
            else
                return Language.Serbian;
        }
    }
}
