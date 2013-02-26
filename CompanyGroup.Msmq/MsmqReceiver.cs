using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompanyGroup.Msmq
{
    public class MsmqReceiver
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
                //    SyncEntity entity = ConstructSyncEntity(Shared.Web.Helpers.ConvertData.ConvertObjectToString(msg.Body));

                //    //visszasorosított parancsobjektummal nosql adatbázis frissítése    
                //    Shared.Web.DataAccess.IAdapter adapter = new Shared.Web.DataAccess.Adapter();

                //    //insert:1, update:2, delete:3
                //    if (entity.SyncOperation.Equals(1) || entity.SyncOperation.Equals(2))
                //    {
                //        adapter.InsertOrUpdateCatalogueItem(entity.ItemId, entity.DataAreaId, entity.SyncOperation);
                //    }
                //    else if (entity.SyncOperation.Equals(3))
                //    {
                //        adapter.DeleteCatalogueItem(entity.ItemId, entity.DataAreaId);
                //    }
                }

                queue.BeginReceive();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Stock létrehozása
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
        //private static SyncEntity ConstructSyncEntity(string xml)
        //{
        //    if (String.IsNullOrEmpty(xml)) { return new SyncEntity(); }

        //    System.Xml.Linq.XDocument xmlDoc = System.Xml.Linq.XDocument.Parse(xml);

        //    SyncEntity entity = (from element in xmlDoc.Elements()
        //                         select new SyncEntity
        //                         {
        //                             DataAreaId = element.Element("DataAreaId").Value,
        //                             ItemId = element.Element("ItemId").Value,
        //                             SyncOperation = Shared.Web.Helpers.ConvertData.ConvertStringToInt(element.Element("SyncOperation").Value)
        //                         }).FirstOrDefault();

        //    return (entity != null) ? entity : new SyncEntity();
        //}
    }
}
