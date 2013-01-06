using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Data.WebshopModule
{
    /// <summary>
    /// termékstruktúra repository
    /// </summary>
    public class StructureRepository : CompanyGroup.Data.Dynamics.Repository, CompanyGroup.Domain.WebshopModule.IStructureRepository
    {
        /// <summary>
        /// termékstruktúra repository konstruktor
        /// </summary>
        /// <param name="settings"></param>
        public StructureRepository(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// termékstruktúra lekérdezés 
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="manufacturers"></param>
        /// <param name="category1"></param>
        /// <param name="category2"></param>
        /// <param name="category3"></param>
        /// <param name="discountFilter"></param>
        /// <param name="secondHandFilter"></param>
        /// <param name="isInNewsletterFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="textFilter"></param>
        /// <param name="priceFilter"></param>
        /// <param name="priceFilterRelation"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.Structures GetList(string dataAreaId,
                                                                    string manufacturers,
                                                                    string category1,
                                                                    string category2,
                                                                    string category3,
                                                                    bool discountFilter,
                                                                    bool secondHandFilter,
                                                                    bool isInNewsletterFilter,  
                                                                    bool newFilter,
                                                                    bool stockFilter,
                                                                    string textFilter, 
                                                                    string priceFilter, 
                                                                    int priceFilterRelation)
        {
            try
            {

                NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.StructureSelect")
                                                .SetString("DataAreaId", dataAreaId)
                                                .SetString("Manufacturers", manufacturers)
                                                .SetString("Category1", category1)
                                                .SetString("Category2", category2)
                                                .SetString("Category3", category3)
                                                .SetBoolean("Discount", discountFilter)
                                                .SetBoolean("SecondHand", secondHandFilter)
                                                .SetBoolean("New", newFilter)
                                                .SetBoolean("Stock", stockFilter)
                                                .SetString("FindText", textFilter)
                                                .SetString("PriceFilter", priceFilter)
                                                .SetInt32("PriceFilterRelation", priceFilterRelation)
                                                .SetResultTransformer(
                                                new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.Structure).GetConstructors()[0]));

                List<CompanyGroup.Domain.WebshopModule.Structure> structures = query.List<CompanyGroup.Domain.WebshopModule.Structure>() as List<CompanyGroup.Domain.WebshopModule.Structure>;

                CompanyGroup.Domain.WebshopModule.Structures resultList = new CompanyGroup.Domain.WebshopModule.Structures(structures);

                return resultList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
