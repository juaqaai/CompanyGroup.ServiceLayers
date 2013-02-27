using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace CompanyGroup.Sync.Msmq
{
    public class Receiver
    {

        private System.Messaging.MessageQueue queue = null;

        private static readonly string queueName = CompanyGroup.Helpers.ConfigSettingsParser.GetString("AxSyncQueueName", @"FormatName:DIRECT=OS:srv2\AxSync");

        /// <summary>
        /// msmq figyelésének indítása
        /// </summary>
        public void Start()
        {
            try
            {
                CompanyGroup.Helpers.DesignByContract.Require(!String.IsNullOrEmpty(queueName), "A várakozósor neve nem lehet üres!");

                //CompanyGroup.Helpers.DesignByContract.Require(!System.Messaging.MessageQueue.Exists(queueName), "A várakozósor nem létezik!");

                queue = new System.Messaging.MessageQueue(queueName);

                queue.ReceiveCompleted += new System.Messaging.ReceiveCompletedEventHandler(ProcessMessage);

                queue.BeginReceive();

                //System.Xml.Linq.XDocument xdoc = System.Xml.Linq.XDocument.Parse("<root/>");

                //var message = new System.Messaging.Message(xdoc.ToString());

                //queue.Send(message, "test");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// msmq figyelésének leállítása
        /// </summary>
        public void Stop()
        {
            try
            {
                if (queue != null)
                {
                    queue.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string BaseAddress = CompanyGroup.Helpers.ConfigSettingsParser.GetString( "WebApiBaseAddress", "http://1Juhasza/CompanyGroup.WebApi/api/");

        /// <summary>
        /// üzenetfeldolgozás
        /// </summary>
        /// <param name="source"></param>
        /// <param name="asyncResult"></param>
        private static void ProcessMessage(Object source, System.Messaging.ReceiveCompletedEventArgs args)
        {
            try
            {
                System.Messaging.MessageQueue queue = (System.Messaging.MessageQueue)source;

                queue.Formatter = new System.Messaging.XmlMessageFormatter(new Type[] { typeof(String) });

                System.Messaging.Message msg = queue.EndReceive(args.AsyncResult);

                System.Xml.Linq.XDocument xmlDoc = System.Xml.Linq.XDocument.Parse(CompanyGroup.Helpers.ConvertData.ConvertObjectToString(msg.Body));

                if (msg.Label.Equals("Stock"))
                {
                    CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest request = ConstructCatalogueStockUpdateRequest(CompanyGroup.Helpers.ConvertData.ConvertObjectToString(msg.Body));

                    HttpClient client = new HttpClient();

                    client.BaseAddress = new Uri(BaseAddress);

                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.PostAsJsonAsync("Maintain/StockUpdate", request).Result;
                    //insert:1, update:2, delete:3
                }

                queue.BeginReceive();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Stock request létrehozása
        /// <?xml version="1.0"?>
        /// <string>&lt;?xml version="1.0" encoding="utf-16"?&gt;&lt;Stock&gt;
        /// &lt;SyncOperation&gt;4&lt;/SyncOperation&gt;
        /// &lt;ItemId&gt;V2520-2&lt;/ItemId&gt;
        /// &lt;InventLocationId&gt;KULSO&lt;/InventLocationId&gt;
        /// &lt;DataAreaId&gt;hrp&lt;/DataAreaId&gt;
        /// &lt;/Stock&gt;</string>
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        private static CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest ConstructCatalogueStockUpdateRequest(string xml)
        {
            if (String.IsNullOrEmpty(xml)) { return new CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest(); }

            System.Xml.Linq.XDocument xmlDoc = System.Xml.Linq.XDocument.Parse(xml);

            CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest entity = xmlDoc.Elements().Select( x => 
            {
                return new CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest(
                    ReadXmlElementValue(x, "DataAreaId"),
                    ReadXmlElementValue(x, "InventLocationId"),
                    ReadXmlElementValue(x, "ItemId"));
            }).FirstOrDefault();

            return (entity != null) ? entity : new CompanyGroup.Dto.WebshopModule.CatalogueStockUpdateRequest();
        }

        private static string ReadXmlElementValue(System.Xml.Linq.XElement element, string name)
        {
            try
            {
                return element.Element(name).Value;
            }
            catch { return String.Empty; }
        }
    }
}
