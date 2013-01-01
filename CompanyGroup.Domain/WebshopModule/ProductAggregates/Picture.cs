using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{

    /// <summary>
    /// domain product picture
    /// </summary>
    public class Picture
    {
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
