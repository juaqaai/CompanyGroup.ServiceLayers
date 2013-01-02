using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{

    /// <summary>
    /// domain product picture
    /// </summary>
    public class Picture
    {
        public Picture(int id, string fileName, bool primary, long recId)
        {
            this.Id = id;

            this.FileName = fileName;

            this.Primary = primary;

            this.RecId = recId;
        }

        public Picture() : this(0, String.Empty, false, 0) {}

        public int Id { get; set; }

        public string FileName { get; set; }

        public byte[] RawContent { get; set; }

        public bool Primary { get; set; }

        public long RecId { get; set; }
    }

    /// <summary>
    /// domain product pictures
    /// </summary>
    public class Pictures : List<Picture> { }
}
