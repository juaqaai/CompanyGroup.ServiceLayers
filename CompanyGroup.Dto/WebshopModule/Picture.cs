using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// Kép DTO lista
    /// </summary>
    public class Pictures
    {
        public List<Picture> Items { get; set; }
    }

    /// <summary>
    /// Kép DTO
    /// </summary>
    public class Picture
    {
        public string FileName { get; set; }

        public bool Primary { get; set; }

        public long RecId { get; set; }

        public int Id { get; set; }
    }
}
