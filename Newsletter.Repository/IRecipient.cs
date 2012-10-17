using System;
using System.Collections.Generic;

namespace Newsletter.Repository
{
    /// <summary>
    /// vevő repository
    /// </summary>
    public interface IRecipient
    {
        /// <summary>
        /// kiküldendő lista
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Newsletter.Dto.Recipient> GetRecipientList(int id);

        /// <summary>
        /// hozzáadott címlista lekérdezése
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        List<Newsletter.Dto.Address> GetExtraAddressList(string dataAreaId);
    }
}
