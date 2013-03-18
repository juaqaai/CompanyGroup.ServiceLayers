using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.PartnerModule
{
    /// <summary>
    /// lapozó value object
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// utolsó oldalindex
        /// </summary>
        public int LastPageIndex { get; set; }

        /// <summary>
        /// oldal elemek lista
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
        /// lapozó konstruktor
        /// </summary>
        /// <param name="currentPageIndex">aktuális oldalindex pl.: 1..n</param>
        /// <param name="count">szűrőfeltételek által visszaadott elemek száma pl.: 2011</param>
        /// <param name="itemsOnPage">látható elemek száma az oldalon pl.: 30</param>
        public Pager(int currentPageIndex, long count, int itemsOnPage)
        {
            //utolsó oldalindex kalkulálása
            int pageCount = (itemsOnPage > 0) ? Convert.ToInt32(count / itemsOnPage) : 0;
                    
            this.LastPageIndex = ( ( pageCount * itemsOnPage ) < count ) ? pageCount + 1 : pageCount;

            //oldal lapozó elemek lista
            this.PageItemList = new List<PageItem>();

            for (int i = 1; i < (this.LastPageIndex + 1); i++)
            {
                this.PageItemList.Add(new PageItem(i, (i.Equals(currentPageIndex) )));
            }

            //előző oldal elérhető-e
            this.PreviousEnabled = currentPageIndex > 1;

            //következő oldal elérhető-e
            this.NextEnabled = currentPageIndex < this.LastPageIndex;

            //első oldal elérhető-e
            this.FirstEnabled = currentPageIndex > 1;

            //utolsó oldal elérhető-e
            this.LastEnabled = currentPageIndex < this.LastPageIndex;
        }
    }

    public class PageItem
    {
        public int Index { get; set; }

        public string Value { get; set; }

        public bool Selected { get; set; }

        public PageItem(int index, bool selected) 
        {
            this.Index = index;

            this.Value = String.Format("{0}", index);

            this.Selected = selected;
        }
    }
}
