using System;
using System.Collections.Generic;

namespace CompanyGroup.GlobalServices.Dto
{
    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Pictures", Namespace = "CompanyGroup.GlobalServices.Dto")]
    public class Pictures
    {
        [System.Runtime.Serialization.DataMember(Name = "Items", Order = 1)]
        public List<Picture> Items { get; set; }
    }

    [Serializable]
    [System.Runtime.Serialization.DataContract(Name = "Picture", Namespace = "CompanyGroup.GlobalServices.Dto")]
    public class Picture
    {
        [System.Runtime.Serialization.DataMember(Name = "FileName", Order = 1)]
        public string FileName { get; set; }

        [System.Runtime.Serialization.DataMember(Name = "Id", Order = 2)]
        public long Id { get; set; }
    }
}
