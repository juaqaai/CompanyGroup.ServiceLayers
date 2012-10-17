using System;
using System.Linq;
using System.Collections.Generic;
using MongoDB.Bson.Serialization;

namespace CompanyGroup.Data.NoSql
{
    public class Initializer
    {
        public void RegisterVisitor()
        {
            BsonClassMap.RegisterClassMap<CompanyGroup.Domain.PartnerModule.Visitor>(cm => {
                cm.MapIdProperty(c => c.Id);
                cm.MapProperty(c => c.CompanyId);
                cm.MapProperty(c => c.CompanyName);
                cm.MapProperty(c => c.Id);
                cm.MapProperty(c => c.LoginType);
                cm.MapProperty(c => c.PartnerModel);
                cm.MapProperty(c => c.Permission);
                cm.MapProperty(c => c.PersonId);
                cm.MapProperty(c => c.PersonName);
                cm.MapProperty(c => c.Profile);
            });

            //BsonClassMap.RegisterClassMap<CompanyGroup.Domain.PartnerModule.Visitor>(cm =>
            //{
            //    cm.AutoMap();
            //    cm.GetMemberMap(c => c.SomeProperty).SetElementName("sp");
            //});
        }
    }
}
