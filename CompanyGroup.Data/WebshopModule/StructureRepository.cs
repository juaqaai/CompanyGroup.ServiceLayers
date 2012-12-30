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
        /// a gyártó és termékjelleg szűrőfeltételek csak a szeriz rétegig mennek, mert a több gyártó, több jelleg szűrés sql-ben problémás
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="discountFilter"></param>
        /// <param name="secondHandFilter"></param>
        /// <param name="isInNewsletterFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="textFilter"></param>
        /// <param name="priceFilter"></param>
        /// <param name="priceFilterRelation"></param>
        /// <returns></returns>
        /// <remarks>
        ///  [InternetUser].[StructureSelect] (@DataAreaId nvarchar(4) = 'hrp',
		///									   @Discount bit = 0,      
		///									   @SecondHand bit = 0,     
		///									   @New bit = 0,         
		///									   @Stock bit = 0,     
		///									   @FindText nvarchar(64) = '', 
		///									   @PriceFilter nvarchar(16) = '',
		///									   @PriceFilterRelation INT = 0
        /// </remarks>
        public CompanyGroup.Domain.WebshopModule.Structures GetList(string dataAreaId,
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
