using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace CompanyGroup.DataChangeWatcher
{
    /// <summary>
    /// árlista változás figyelő
    /// </summary>
    public class PriceDiscNotifier
    {
        //private SqlConnection connection = null;

        //private SqlCommand command = null;

        private string connectionString = String.Empty;

        System.Data.DataTable dt = null;

        //private SqlNotificationType NotificationType;

        public PriceDiscNotifier() : this(Helpers.ConfigSettingsParser.ConnectionString("ConStr")) { }

        /// <summary>
        /// SQL dependency start
        /// connection, command initialization
        /// </summary>
        public PriceDiscNotifier(string connectionString)
        {
            Helpers.DesignByContract.Require(!String.IsNullOrEmpty(connectionString), "Then connectionstring can not be empty");

            this.connectionString = connectionString;

            SqlDependency.Stop(connectionString);

            //SqlDependency.Start(connectionString);

            //connection = new SqlConnection(connectionString);

            //string query = "SELECT ItemId FROM dbo.InventTable WHERE DataAreaId IN ('hrp', 'bsc') AND WEBARUHAZ = 1 AND ITEMSTATE IN ( 0, 1) AND " + 
            //               "AMOUNT1 > 0 AND AMOUNT2 > 0 AND AMOUNT3 > 0 AND AMOUNT4 > 0 AND AMOUNT5 > 0";

            //command = new SqlCommand(query, connection);  //"SELECT ITEMRELATION ,ACCOUNTRELATION ,AMOUNT ,CURRENCY, DATAAREAID " + 
            //                                                            //   "FROM dbo.PRICEDISCTABLE WHERE ACCOUNTRELATION IN ('1', '2', '3', '4', '5') AND " + 
            //                                                            //    "Currency = 'HUF' AND DataAreaId IN ('bsc', 'hrp')"

            //command.CommandType = System.Data.CommandType.Text;

            //SqlParameter parameter = new SqlParameter("@RecId", System.Data.SqlDbType.BigInt);

            //parameter.Direction = System.Data.ParameterDirection.Input;

            //parameter.Value = 5641532500;

            //command.Parameters.Add(parameter);
        }

        public void RefreshData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT ItemId FROM dbo.InventTable WHERE DataAreaId IN ('hrp', 'bsc') AND WEBARUHAZ = 1 AND ITEMSTATE IN ( 0, 1) AND " +
                                   "AMOUNT1 > 0 AND AMOUNT2 > 0 AND AMOUNT3 > 0 AND AMOUNT4 > 0 AND AMOUNT5 > 0";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;

                        // Make sure the command object does not already have a notification object associated with it.
                        command.Notification = null;

                        // Hookup sqldependency eventlistener (re-register for change events).
                        SqlDependency dependency = new SqlDependency(command);
                        dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                        // Open connection to database.
                        connection.Open();

                        dt = new System.Data.DataTable();

                        dt.Load(command.ExecuteReader(System.Data.CommandBehavior.CloseConnection));

                    }
                }
            }
            catch (Exception ex)
            {

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

        //public delegate void PriceDiscWatcherEventHandler();

        //public event PriceDiscWatcherEventHandler OnChange;

        //private event EventHandler<SqlNotificationEventArgs> newMessage;

        //public event EventHandler<SqlNotificationEventArgs> NewMessage
        //{
        //    add
        //    {
        //        this.newMessage += value;
        //    }
        //    remove
        //    {
        //        this.newMessage -= value;
        //    }
        //}

        //public virtual void OnNewMessage(SqlNotificationEventArgs notification)
        //{
        //    if (this.newMessage != null)
        //    {
        //        this.newMessage(this, notification);
        //    }
        //}

        //private void RegisterDependency()
        //{
        //    this.command.Notification = null;

        //    SqlDependency dependency = new SqlDependency(this.command);

        //    dependency.OnChange += new OnChangeEventHandler(Handle_OnChange);

        //    if (connection.State == System.Data.ConnectionState.Closed)
        //    {
        //        connection.Open();
        //    }

        //    dt = new System.Data.DataTable();

        //    dt.Load(command.ExecuteReader(System.Data.CommandBehavior.CloseConnection));

        //    //OnChange();
        //}

        //private void Handle_OnChange(object sender, SqlNotificationEventArgs e)
        //{
        //    //if (e.Type != SqlNotificationType.Change)
        //    //    throw new ApplicationException("Failed to create queue notification subscription!");

        //    SqlDependency dependency = (SqlDependency)sender;

        //    dependency.OnChange -= Handle_OnChange;

        //    RegisterDependency();
        //}

        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                // Re-register for query notification SqlDependency Change events.
                RefreshData();
            }
        }

        //public DataTable RegisterDependency()
        //{

        //    this.CurrentCommand = new SqlCommand("Select [MID],[MsgString],
        //                [MsgDesc] from dbo.Message", this.CurrentConnection);
        //    this.CurrentCommand.Notification = null;

        //    SqlDependency dependency = new SqlDependency(this.CurrentCommand);
        //    dependency.OnChange += this.dependency_OnChange;

        //    if (this.CurrentConnection.State == ConnectionState.Closed)
        //        this.CurrentConnection.Open();
        //    try
        //    {

        //        DataTable dt = new DataTable();
        //        dt.Load(this.CurrentCommand.ExecuteReader
        //    (CommandBehavior.CloseConnection));
        //        return dt;
        //    }
        //    catch { return null; }
        //}

        /// <summary>
        /// eseménykezelő
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        //{
        //    SqlDependency dependency = sender as SqlDependency;

        //    dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

        //    this.OnNewMessage(e);
        //}

        /// <summary>
        /// figyelés elindítása
        /// </summary>
        public void Start()
        {
            SqlDependency.Start(connectionString);

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

                //connection.Close();
            }
            catch { }
        }
    }
}
