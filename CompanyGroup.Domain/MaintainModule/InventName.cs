using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.MaintainModule
{
    /// <summary>
    /// [InternetUser].[cms_InventNameEnglish]( @DataAreaId nvarchar(3) = 'hrp' )
    /// ItemId	ItemName	DataAreaId
    /// </summary>
    public class InventName
    {
        public InventName(string itemId, string itemName, string dataAreaId)
        { 
            ItemId = itemId;
            ItemName = itemName;
            DataAreaId = dataAreaId;
        }

        public string ItemId { get; set; }

        public string ItemName { get; set; }

        public string DataAreaId { get; set; }
    }
}
