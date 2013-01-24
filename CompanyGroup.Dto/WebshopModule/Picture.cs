using System;
using System.Collections.Generic;

namespace CompanyGroup.Dto.WebshopModule
{
    /// <summary>
    /// Kép DTO lista
    /// </summary>
    public class Pictures
    {
        public Pictures() : this(new List<Picture>()) { }

        public Pictures(List<Picture> items)
        {
            this.Items = items;
        }

        public List<Picture> Items { get; set; }
    }

    /// <summary>
    /// Kép DTO
    /// </summary>
    public class Picture
    {
        public Picture(string fileName, bool primary, long recId, int id)
        {
            this.FileName = fileName;

            this.Primary = primary;

            this.RecId = recId;

            this.Id = id;
        }

        public string FileName { get; set; }

        public bool Primary { get; set; }

        public long RecId { get; set; }

        public int Id { get; set; }
    }
}
