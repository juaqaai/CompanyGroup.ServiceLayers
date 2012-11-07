using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompanyGroup.WebClient.Models
{
    public class CompletionList : CompanyGroup.Dto.WebshopModule.CompletionList
    {
        public CompletionList(CompanyGroup.Dto.WebshopModule.CompletionList completionList)
        {
            this.Items = completionList.Items;
        }
    }
}