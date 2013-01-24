using System;

namespace CompanyGroup.Dto.PartnerModel
{
    /// <summary>
    /// képlista lekérdezés paramétereit összefogó adattípus
    /// </summary>
    public class PictureFilterRequest
    {
        public PictureFilterRequest(string dataAreaId, string productId)
        {
            this.DataAreaId = dataAreaId;

            this.ProductId = productId;
        }

        public string DataAreaId { get; set; }

        public string ProductId { get; set; }
    }
}