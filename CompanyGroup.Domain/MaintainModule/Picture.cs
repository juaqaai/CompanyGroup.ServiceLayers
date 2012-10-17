using System;

namespace CompanyGroup.Domain.MaintainModule
{
    /// <summary>
    /// picture (item)
    /// </summary>
    public class Picture
    {
        public Picture(Int64 recId, bool primary, string fileName, string itemId, string dataAreaId)
        {
            this.RecId = recId;

            this.Primary = primary;

            this.FileName = fileName;

            this.ItemId = itemId;

            this.DataAreaId = dataAreaId;
        }

        public bool Primary { get; set; }

        public Int64 RecId { get; set; }

        public string FileName { get; set; }

        public string ItemId { get; set; }

        public string DataAreaId { get; set; }
    }
}
