using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// kiegészítő lista típusa
    /// 0 : nincs megadva
    /// 1 : terméknév vagy cikkszám
    /// 2 : teljes keresés
    /// </summary>
    public enum CompletionType
    {
        None = 0, ProductIdOrPartnumber = 1, Full = 2
    }
}
