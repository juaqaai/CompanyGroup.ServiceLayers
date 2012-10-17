using System;

namespace CompanyGroup.Dto.ServiceRequest
{
    /// <summary>
    /// kijelentkezés adatait összefogó osztály
    /// </summary>
    public class SignOut
    {
        public string DataAreaId { get; set; }

        public string ObjectId { get; set; }
    }
}
