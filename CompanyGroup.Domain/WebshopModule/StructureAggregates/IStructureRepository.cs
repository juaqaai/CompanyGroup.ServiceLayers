﻿using System;
using System.Collections.Generic;

namespace CompanyGroup.Domain.WebshopModule
{
    public interface IStructureRepository
    {
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
        CompanyGroup.Domain.WebshopModule.Structures GetList(string dataAreaId,
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
                                                             int priceFilterRelation);
    }
}
