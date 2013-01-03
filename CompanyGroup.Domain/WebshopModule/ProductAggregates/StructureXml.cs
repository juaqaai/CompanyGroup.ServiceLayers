using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    /// <summary>
    /// gyártólista, jelleglista paraméter átadáshoz szükséges osztály
    /// </summary>
    public class StructureXml : CompanyGroup.Domain.Core.ValueObject<StructureXml>
    {
        public StructureXml()
        {
            this.ManufacturerIdList = new List<string>();
            this.Category1IdList = new List<string>();
            this.Category2IdList = new List<string>();
            this.Category3IdList = new List<string>();
        }

        public StructureXml(List<string> manufacturerIdList, List<string> category1IdList, List<string> category2IdList, List<string> category3IdList)
        {
            this.ManufacturerIdList = manufacturerIdList;

            this.Category1IdList = category1IdList;

            this.Category2IdList = category2IdList;

            this.Category3IdList = category3IdList;
        }

        public List<string> ManufacturerIdList { get; set; }

        public List<string> Category1IdList { get; set; }

        public List<string> Category2IdList { get; set; }

        public List<string> Category3IdList { get; set; }

        /// <summary>
        /// <Structure>
        /// <Manufacturer>
        ///    <Id>A142</Id>
        ///    <Id>A169</Id>
        /// </Manufacturer>
        /// <Category1>
        ///    <Id>B004</Id>
        /// </Category1>
        /// <Category2>
        /// </Category2>
        /// <Category3>
        /// </Category3>
        /// </Structure>
        /// </summary>
        /// <param name="writer"></param>
        public string SerializeToXml()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder(); 
            
            sb.Append("<Structure><Manufacturer>");
            this.ManufacturerIdList.ForEach( x => {
                sb.Append("<Id>");
                sb.Append(x);
                sb.Append("</Id>");
            });
            sb.Append("</Manufacturer>");

            sb.Append("<Category1>");
            this.Category1IdList.ForEach(x =>
            {
                sb.Append("<Id>");
                sb.Append(x);
                sb.Append("</Id>");
            });
            sb.Append("</Category1>");

            sb.Append("<Category2>");
            this.Category2IdList.ForEach(x =>
            {
                sb.Append("<Id>");
                sb.Append(x);
                sb.Append("</Id>");
            });
            sb.Append("</Category2>");

            sb.Append("<Category3>");
            this.Category3IdList.ForEach(x =>
            {
                sb.Append("<Id>");
                sb.Append(x);
                sb.Append("</Id>");
            });
            sb.Append("</Category3></Structure>");

            return sb.ToString();
        }
    }
}
