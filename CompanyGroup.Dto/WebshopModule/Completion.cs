using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// autosuggestion listaelem
    /// </summary>
    public class Completion
    {
        public string ProductId { get; set; }

        public string ProductName { get; set; }

        public int PictureId { get; set; }

        public string DataAreaId { get; set; }
    }

    /// <summary>
    /// autosuggestion lista
    /// </summary>
    public class CompletionList
    {
        /// <summary>
        /// listaelemek
        /// </summary>
        public List<Completion> Items { get; set; }
    }
}
