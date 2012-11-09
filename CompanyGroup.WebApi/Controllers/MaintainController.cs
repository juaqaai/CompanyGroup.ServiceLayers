﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.WebApi.Controllers
{
    /// <summary>
    /// terméklista cache karbantartó műveletek
    /// </summary>
    public class MaintainController : ApiController
    {
        private CompanyGroup.ApplicationServices.MaintainModule.IProductService service;

        /// <summary>
        /// konstruktor terméklista cache karbantartó interfészt megvalósító példánnyal
        /// </summary>
        /// <param name="service"></param>
        MaintainController(CompanyGroup.ApplicationServices.MaintainModule.IProductService service)
        {
            this.service = service;
        }

        /// <summary>
        /// terméklista cache újra töltése
        /// </summary>
        [ActionName("FillProductCache/{dataAreaId}")]
        [HttpGet]
        public bool FillProductCache(string dataAreaId)
        {
            return service.FillProductCache(dataAreaId);
        }

        /// <summary>
        /// terméklista cache törlése
        /// </summary>
        /// <returns></returns>
        [ActionName("ClearProductCache")]
        [HttpGet]
        public bool ClearProductCache()
        {
            return service.ClearProductCache();
        }

        /// <summary>
        /// terméklista cache törlése, betöltése
        /// </summary>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        [ActionName("RefillProductCache")]
        [HttpGet]
        public bool RefillProductCache(string dataAreaId)
        {
            return service.RefillProductCache(dataAreaId);
        }
    }
}