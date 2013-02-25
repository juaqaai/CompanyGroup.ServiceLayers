//------------------------------------------------------------------------------
// <copyright file="CSSqlStoredProcedure.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Http.Headers;

public partial class StoredProcedures
{
    private static readonly string ServiceBaseAddress = "http://1Juhasza/CompanyGroup.WebApi/api/";

    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void CatalogueStockUpdate (SqlString dataAreaId, SqlString productId, SqlInt32 stock)
    {
        try
        {
            if (String.IsNullOrWhiteSpace(dataAreaId.Value)) { throw new ApplicationException("DataAreaId name can not be null or empty!"); }

            if (String.IsNullOrWhiteSpace(productId.Value)) { throw new ApplicationException("ProductId name can not be null or empty!"); }

            CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest request = new CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest(dataAreaId.Value, productId.Value, stock.Value);

            if (request == null) { throw new ApplicationException("Request can not be null!"); };

            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

            client.BaseAddress = new Uri(ServiceBaseAddress);

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            Uri requestUri = null;

            System.Net.Http.HttpResponseMessage response = client.PostAsJsonAsync(String.Format("{0}/{1}", "Product", "StockUpdate"), request).Result;

            if (response.IsSuccessStatusCode)
            {
                requestUri = response.Headers.Location;
            }
            else
            {
                String.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
        }
        catch(Exception ex)
        {
            throw(ex);
        }
    }

}
