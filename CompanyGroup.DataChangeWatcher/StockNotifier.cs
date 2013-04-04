using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace CompanyGroup.DataChangeWatcher
{
    /// <summary>
    /// készletváltozás figyelő
    /// </summary>
    public class StockNotifier
    {

        private string connectionString = String.Empty;

        System.Data.DataTable dt = null;

        public StockNotifier() : this(Helpers.ConfigSettingsParser.ConnectionString("ConStr")) { }

        /// <summary>
        /// SQL dependency stop
        /// connectionstring initialization
        /// </summary>
        public StockNotifier(string connectionString)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrEmpty(connectionString), "Then connectionstring can not be empty");

            this.connectionString = connectionString;

            SqlDependency.Stop(connectionString);
        }

        /// <summary>
        /// adatfrissítés, eseményre feliratkozás
        /// </summary>
        public void RefreshData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT invent.ItemId " + 
			                       "FROM Axdb_20130131.dbo.InventTable as invent " + 
			                       "INNER JOIN Axdb_20130131.dbo.InventDim AS ind on ind.configId = invent.StandardConfigId and  " + 
															                         "ind.dataAreaId = invent.DataAreaId and  " + 
															                         "ind.InventLocationId in ( '7000', '2100', 'KULSO', 'HASZNALT' ) " + 
			                       "INNER JOIN Axdb_20130131.dbo.InventSum AS ins on ins.DataAreaId = invent.DataAreaId and  " + 
															                        "ins.inventDimId = ind.inventDimId and  " + 
															                        "ins.ItemId = invent.ItemId and  " + 
															                        "ins.Closed = 0 " + 
			                       "WHERE invent.WEBARUHAZ = 1 AND invent.ITEMSTATE in ( 0, 1 ) ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;

                        command.Notification = null;

                        SqlDependency dependency = new SqlDependency(command);
                        dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                        connection.Open();

                        dt = new System.Data.DataTable();

                        dt.Load(command.ExecuteReader(System.Data.CommandBehavior.CloseConnection));

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// application must have the SqlClientPermission permission.
        /// </summary>
        /// <returns></returns>
        private bool CanRequestNotifications()
        {
            try
            {
                SqlClientPermission perm = new SqlClientPermission(System.Security.Permissions.PermissionState.Unrestricted);

                perm.Demand();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// készletváltozás esemény
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                this.RefreshData();
            }
        }

        /// <summary>
        /// figyelés elindítása
        /// </summary>
        public void Start()
        {
            SqlDependency.Start(this.connectionString);

            this.RefreshData();
        }

        /// <summary>
        /// figyelés megállítása
        /// </summary>
        public void Stop()
        {            
            try
            {
                SqlDependency.Stop(this.connectionString);
            }
            catch { }
        }
    }
}
