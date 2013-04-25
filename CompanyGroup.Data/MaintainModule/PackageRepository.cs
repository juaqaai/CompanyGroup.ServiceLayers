using System;
using System.Collections.Generic;
using System.Linq;
//using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Server;  

namespace CompanyGroup.Data.MaintainModule
{
    /// <summary>
    /// dtsx csomag kezelését végző osztály
    /// </summary>
    public class PackageRepository : CompanyGroup.Domain.MaintainModule.IPackageRepository
    {
        /// <summary>
        /// konstruktor
        /// </summary>
        /// <param name="session"></param>
        public PackageRepository() { }

        /// <summary>
        /// részletes számla sorok listája AX-ből vállalatonként
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="dataAreaId"></param>
        /// <returns></returns>
        //public List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo> GetInvoiceDetailedLineInfo(string dataAreaId)
        //{
        //    CompanyGroup.Domain.Utils.Check.Require(!string.IsNullOrEmpty(dataAreaId), "dataAreaId may not be null or empty");

        //    NHibernate.IQuery query = Session.GetNamedQuery("InternetUser.cms_InvoiceList")
        //                                                    .SetString("CustomerId", String.Empty)
        //                                                    .SetString("DataAreaId", dataAreaId)
        //                                                    .SetResultTransformer(
        //                                                    new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo).GetConstructors()[0]));

        //    return query.List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo>() as List<CompanyGroup.Domain.MaintainModule.InvoiceDetailedLineInfo>;
        //}

        /*
           Dim folderName As String
            Dim projectName As String
            Dim serverName As String
            Dim packageName As String
            Dim connectionString As String
            Dim use32BitRuntime As Boolean
            Dim executionId As Integer

            Dim integrationServices As Microsoft.SqlServer.Management.IntegrationServices.IntegrationServices
            Dim catalog As Microsoft.SqlServer.Management.IntegrationServices.Catalog
            Dim catalogFolder As Microsoft.SqlServer.Management.IntegrationServices.CatalogFolder
            Dim package As Microsoft.SqlServer.Management.IntegrationServices.PackageInfo

            ' Dimensions in your example
            folderName = "SSISHackAndSlash"         Web
            ' dimCalendar in your example
            projectName = "SSISHackAndSlash2012"    CompanyGroup.IntegrationServices
            serverName = "localhost\dev2012"

            ' dimCalendar in your example (no file extension)
            packageName = "TokenTest.dtsx"
            connectionString = String.Format("Data Source={0};Initial Catalog=msdb;Integrated Security=SSPI;", serverName)

            integrationServices = New Microsoft.SqlServer.Management.IntegrationServices.IntegrationServices(New System.Data.SqlClient.SqlConnection(connectionString))
            ' There is only one option for an SSIS catalog name as of this posting
            catalog = integrationServices.Catalogs("SSISDB")

            ' Find the catalog folder. Dimensions in your example
            catalogFolder = catalog.Folders(folderName)

            ' Find the package in the project folder
            package = catalogFolder.Projects(projectName).Packages(packageName)

            ' Run the package. The second parameter is for environment variables
            executionId = package.Execute(use32BitRuntime, Nothing) 
         */

        public void ExecutePriceChangePackage()
        {
            string folderName = "Web";

            string projectName = "CompanyGroup.IntegrationServices";

            string serverName = "srv2";

            string packageName = "StockUpdater.dtsx";

            string connectionString = String.Format("Data Source={0};Initial Catalog=msdb;Integrated Security=SSPI;", serverName);

            bool use32BitRuntime = false;

            Microsoft.SqlServer.Management.IntegrationServices.IntegrationServices integrationServices = new Microsoft.SqlServer.Management.IntegrationServices.IntegrationServices(new System.Data.SqlClient.SqlConnection(connectionString));

            Microsoft.SqlServer.Management.IntegrationServices.Catalog catalog = integrationServices.Catalogs["SSISDB"];

            Microsoft.SqlServer.Management.IntegrationServices.CatalogFolder catalogFolder = catalog.Folders[folderName];

            Microsoft.SqlServer.Management.IntegrationServices.PackageInfo package = catalogFolder.Projects[projectName].Packages[packageName];

            long executionId = package.Execute(use32BitRuntime, null);
  
        }
    }
}
