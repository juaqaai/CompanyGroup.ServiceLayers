using System;

namespace CompanyGroup.Dto.PartnerModule
{
    /// <summary>
    /// kijelentkezés kérés paramétereit összefogó osztály
    /// </summary>
    public class SignOutRequest
    {
        public SignOutRequest(string dataAreaId, string visitorId)
        {
            this.DataAreaId = dataAreaId;

            this.VisitorId = visitorId;
        }

        public SignOutRequest() : this("", "") { }

        public string DataAreaId { get; set; }

        public string VisitorId { get; set; }
    }
}
