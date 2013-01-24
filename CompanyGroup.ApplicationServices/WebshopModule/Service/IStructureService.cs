using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace CompanyGroup.ApplicationServices.WebshopModule
{

    /// <summary>
    /// struktúra szolgáltatás interfész
    /// </summary>
    public interface IStructureService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CompanyGroup.Dto.WebshopModule.Structures GetAll(CompanyGroup.Dto.WebshopModule.GetAllStructureRequest request);

    }
}
