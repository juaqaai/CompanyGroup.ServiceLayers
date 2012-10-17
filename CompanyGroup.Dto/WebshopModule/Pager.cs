using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Pager", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class Pager
    {
        /// <summary>
        /// utolso oldalindex
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "LastPageIndex", Order = 1)]
        public int LastPageIndex { get; set; }

        /// <summary>
        /// oldalindex lista
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "PageItemList", Order = 2)]
        public List<PageItem> PageItemList { get; set; }

        /// <summary>
        /// előző oldal elérhető-e
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "PreviousEnabled", Order = 3)]
        public bool PreviousEnabled { get; set; }
        /// <summary>
        /// következő oldal elérhető-e
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "NextEnabled", Order = 4)]
        public bool NextEnabled { get; set; }

        /// <summary>
        /// első oldal elérhető-e
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "FirstEnabled", Order = 5)]
        public bool FirstEnabled { get; set; }

        /// <summary>
        /// utolsó oldal elérhető-e
        /// </summary>
        [System.Runtime.Serialization.DataMember(Name = "LastEnabled", Order = 6)]
        public bool LastEnabled { get; set; }

        /// <summary>
        /// elemek száma az oldalon 
        /// </summary>
        //[System.Runtime.Serialization.DataMember(Name = "ItemsOnPage", Order = 7)]
        //public int ItemsOnPage { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "VisibleItemList", Order = 7)]
        public List<VisibleItem> VisibleItemList { get; set; }
    }

    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "PageItem", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class PageItem
    {
        [System.Runtime.Serialization.DataMember(Name = "Index", Order = 1)]
        public int Index { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Value", Order = 2)]
        public string Value { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Selected", Order = 3)]
        public bool Selected { get; set; }
    }

    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "VisibleItem", Namespace = "CompanyGroup.Dto.WebshopModule")]
    public class VisibleItem
    {
        [System.Runtime.Serialization.DataMember(Name = "Value", Order = 1)]
        public int Value { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Selected", Order = 2)]
        public bool Selected { get; set; }
    }
}
