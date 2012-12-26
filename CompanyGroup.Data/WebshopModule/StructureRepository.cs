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
        private readonly static string CollectionName = Helpers.ConfigSettingsParser.GetString("StructureCollectionName", "ProductList");

        /// <summary>
        /// termékstruktúra repository konstruktor
        /// </summary>
        /// <param name="settings"></param>
        public StructureRepository(NHibernate.ISession session) : base(session) { }

        /// <summary>
        /// termékstruktúra lekérdezés
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <param name="actionFilter"></param>
        /// <param name="secondHandFilter"></param>
        /// <param name="isInNewsletterFilter"></param>
        /// <param name="newFilter"></param>
        /// <param name="stockFilter"></param>
        /// <param name="textFilter"></param>
        /// <param name="priceFilter"></param>
        /// <param name="priceFilterRelation"></param>
        /// <param name="nameOrPartNumberFilter"></param>
        /// <returns></returns>
        /// <remarks>
        ///  [InternetUser].[StructureSelect] (@DataAreaId nvarchar(4) = 'hrp',
        ///     @ManufacturerId nvarchar (4) = '',	
        ///     @Category1Id nvarchar (4) = '',       
        ///     @Category2Id nvarchar (4) = '',       
        ///     @Category3Id nvarchar (4) = '',       
        ///     @Discount bit = 0,      
        ///     @SecondHand bit = 0,     
        ///     @New bit = 0,         
        ///     @Stock bit = 0,     
        ///     @FindText nvarchar(64) = ''
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
                                                                            .SetBoolean("New", newFilter)
                                                                            .SetBoolean("Stock", stockFilter)
                                                                            .SetString("FindText", textFilter)
                                                                            .SetBoolean("SecondHand", secondHandFilter)
                                                                            .SetResultTransformer(
                                                                            new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.Structure).GetConstructors()[0]));

                List<CompanyGroup.Domain.WebshopModule.Structure> structures = query.List<CompanyGroup.Domain.WebshopModule.Structure>() as List<CompanyGroup.Domain.WebshopModule.Structure>;

                CompanyGroup.Domain.WebshopModule.Structures resultList = new CompanyGroup.Domain.WebshopModule.Structures(structures);

                return resultList;
            }
            catch(Exception ex)
            {
                throw ex;
                //return new CompanyGroup.Domain.WebshopModule.Structures();
            }
        }


    }
}
