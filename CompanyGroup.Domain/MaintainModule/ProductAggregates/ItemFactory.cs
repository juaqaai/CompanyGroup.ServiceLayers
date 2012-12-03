using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Domain.ERPModule
{
    public static class ItemFactory
    {
        public static CompanyGroup.Domain.ERPModule.Item CreateItem()
        { 
            return new CompanyGroup.Domain.ERPModule.Item() 
                       { 
                           Bargain = bargain, 
                           BscInnerStock = bscInnerStock, 
                           BscOuterStock = bscOuterStock, 
                           Category1Id = cayegory1Id, 
                           Category1Name = category1Name, 
                           Category1NameEnglish = category1NameEnglish, 
                           Category2Id = category2Id, 
                           Category2Name = category2Name, 
                           Category2NameEnglish = category2NameEnglish, 
                           Category3Id = category3Id, 
                           Category3Name = category3Name, 
                           Category3NameEnglish = category3NameEnglish, 
                           CreatedDate = createdDate, 
                           CreatedTime = createdTime, 
                           Currency = currency, 
                           DataAreaId = dataAreaId, 
                           Description = description, 
                           DescriptionEnglish = descriptionEnglish, 
                           Discount = discount, 
                           Garanty = garanty, 
                           GarantyMode = garantyMode, 
                           HrpInnerStock = hrpInnerStock, 
        }
    }
}
