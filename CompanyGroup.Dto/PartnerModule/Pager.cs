using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// lapozó (számlák, megrendelések)
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// utolso oldalindex
        /// </summary>
        public int LastPageIndex { get; set; }

        /// <summary>
        /// oldalindex lista
        /// </summary>
        public List<PageItem> PageItemList { get; set; }

        /// <summary>
        /// előző oldal elérhető-e
        /// </summary>
        public bool PreviousEnabled { get; set; }

        /// <summary>
        /// következő oldal elérhető-e
        /// </summary>
        public bool NextEnabled { get; set; }

        /// <summary>
        /// első oldal elérhető-e
        /// </summary>
        public bool FirstEnabled { get; set; }

        /// <summary>
        /// utolsó oldal elérhető-e
        /// </summary>
        public bool LastEnabled { get; set; }

        /// <summary>
        /// elemek száma az oldalon 
        /// </summary>
        //[System.Runtime.Serialization.DataMember(Name = "ItemsOnPage", Order = 7)]
        //public int ItemsOnPage { get; set; }

        public List<VisibleItem> VisibleItemList { get; set; }

        /// <summary>
        /// előző oldalindex
        /// </summary>
        public int PreviousPageIndex { get; set; }

        /// <summary>
        /// következő oldalindex
        /// </summary>
        public int NextPageIndex { get; set; }
    }

    public class PageItem
    {
        public int Index { get; set; }

        public string Value { get; set; }

        public bool Selected { get; set; }
    }

    public class VisibleItem
    {
        public int Value { get; set; }

        public bool Selected { get; set; }
    }
}
