using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Xml;
using System.Web;
using System.Windows;
using System.Runtime.InteropServices;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Server;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management;

using NUnit.Framework;


namespace EntrustFunctionalTest
{

    /// <summary>
    /// Class containing Common methods for SQLServer
    /// </summary>
    public class SQLServer
    {
         

        // Fields
        private static Random _random = new Random();
        private Config CF;

        // Methods
        /// <summary>
        /// Config Object
        /// </summary>
        /// <param name="obj"></param>
        public SQLServer(Config obj)
        {
            this.CF = obj;
        }
       
        GlobalCommonActions ca = new GlobalCommonActions();


        /// <summary>
        /// Returns Completion Status in Percent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void CompletionStatusInPercent(object sender, PercentCompleteEventArgs args)
        {
            Console.Clear();
            Console.WriteLine("Percent completed: {0}%.", args.Percent);
        }
        /// <summary>
        /// Returns Backup_Completed message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void Backup_Completed(object sender, ServerMessageEventArgs args)
        {
            Console.WriteLine("Backup completed.");
            Console.WriteLine(args.Error.Message);
        }
        /// <summary>
        /// Returns the Restore_Completed message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void Restore_Completed(object sender, ServerMessageEventArgs args)
        {
            Console.WriteLine("Restore completed.");
            Console.WriteLine(args.Error.Message);
        }
        /// <summary>
        /// Method to Backup a Database
        /// </summary>
        public void CreateDBBackup()
        {

            Server myServer = new Server(CF.Server);
           
            //Using SQL Server authentication
            myServer.ConnectionContext.LoginSecure = false;
            myServer.ConnectionContext.Login = CF.UserName;
            myServer.ConnectionContext.Password = CF.Password;

            // Loop through Databases, when DBName is found perform backup
            foreach (Database myDatabase in myServer.Databases)
            {
                Console.WriteLine(myDatabase.Name);

                if(myDatabase.Name.Equals(CF.DBName))
                {

                    Console.Write("Found DB!" + myDatabase.Name);

                    Backup DBFull = new Backup();

                    // Specify backup of Database
                    DBFull.Action = BackupActionType.Database;

                    // Set Name of Database to backup
                    DBFull.Database = myDatabase.Name;

                    // Save Backup to File
                    DBFull.Devices.AddDevice(CF.DevicePath, DeviceType.File);

                    // Set Backup Set Name
                    DBFull.BackupSetName = CF.BackUpName;

                    // Set Backup Description
                    DBFull.BackupSetDescription = CF.BackUpDescription;

                    // Initialize to False, appends backup as last backup on media. 
                    DBFull.Initialize = false;
                                                        
                    // Will write to console. 
                    DBFull.PercentComplete += CompletionStatusInPercent;                    
                    DBFull.Complete += Backup_Completed;
                                      
                   // Take Backup
                    DBFull.SqlBackup(myServer);

                    return;

                    
                }
            }

            

            // Close Connection
            if (myServer.ConnectionContext.IsOpen)
                myServer.ConnectionContext.Disconnect();
        }
        /// <summary>
        /// Method to Restore a DB from file
        /// </summary>
        public void RestoreDBFromFile()
        {

            // Call Set Up 
            // Not needed if we wrap this in a [Test]

            Server myServer = new Server(CF.Server);
           
            //Using SQL Server authentication
            myServer.ConnectionContext.LoginSecure = false;
            myServer.ConnectionContext.Login = CF.UserName;
            myServer.ConnectionContext.Password = CF.Password;
          
            Restore restoreDB = new Restore();

            foreach (Database myDatabase in myServer.Databases)
            {
                Console.WriteLine(myDatabase.Name);

                if (myDatabase.Name.Equals(CF.DBName))
                {
                    
                    restoreDB.Database = myDatabase.Name;
                  
                    // Restore Action = Database
                    restoreDB.Action = RestoreActionType.Database;

                    restoreDB.Devices.AddDevice(CF.RestoreDevicePath, DeviceType.File);

                    // Replace the existing database
                    restoreDB.ReplaceDatabase = true;
                    
                    // Allows for DB recovery after restore
                    restoreDB.NoRecovery = false;

                    /* Wiring up events for progress monitoring */
                    restoreDB.PercentComplete += CompletionStatusInPercent;
                    restoreDB.Complete += Restore_Completed;

                    // Start the restore
                    restoreDB.SqlRestore(myServer);

                   

                }

            }

        }
        /// <summary>
        /// Method to Get the Latest Outgoing Invoice ID by User
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetLatestOutgoingInvoiceID(string user)
        {
            string query = "Select Max(InvoiceID) as InvoiceID from txn.Invoices where InvoiceType = 'Outgoing' and ModUser='"+user+"'";

           // Console.Write(query);
            DataSet INV = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(INV, "INV");

            DataRow[] rows = INV.Tables[0].Select(null, null);
            DataRow row = rows[0];

            int id = Convert.ToInt32(row["InvoiceID"]);

            return id;
        }
        /// <summary>
        /// Method to Retrieve the Invoice Number by ID
        /// </summary>
        /// <param name="invid"></param>
        /// <returns></returns>
        public string GetInvoiceNumberById(int invid)
        {
            string query = "Select InvoiceNumber from txn.Invoices where InvoiceID = '"+invid+"'";

           // Console.Write("Invoiced Query" + query + Environment.NewLine);

            DataSet INV = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(INV, "INV");

            DataRow[] rows = INV.Tables[0].Select(null, null);
            DataRow row = rows[0];

            string id = row["InvoiceNumber"].ToString();
            return id;
        }

        /// <summary>
        /// Method to Get Count of Rows on txn.AAR500Submissions using ApprovalNumber and FileName as search criteria
        /// </summary>
        /// <param name="approvalnum"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public int GetAAR500SubCountByAppNumFileName(string approvalnum, string filename)
        {

            string query = "Select ApprovalNumber, FileName from txn.AAR500Submissions where ApprovalNumber = '"+approvalnum+"' and FileName = '"+filename+"'";

           // Console.Write("ApprovalNumberQuery " + query + Environment.NewLine);

            DataSet AAR500 = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(AAR500, "AAR500");

            DataRow[] rows = AAR500.Tables[0].Select(null, null);            

            int count = rows.Count(); ;
            return count;

        }

        /// <summary>
        /// Method to get EventSequence from txn.CarEvents using CarInitials, CarNumber, Mod User
        /// </summary>
        /// <param name="carno"></param>
        /// <param name="moduser"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public string GetEventSequenceNumberByCarDateUser(string carno, string adduser, string date)
        {

            try
            {
                string[] carparts = carno.Split(' ');
                string query = "Select EventSequence from txn.CarEvents where CarInitials = '" + carparts[0] + "' and CarNumber = '" + carparts[1] + "' and Convert(varchar, AddDate, 1) = '" + date + "' and AddUser = '" + adduser + "'";

              //  string queryw = String.Concat(query + Environment.NewLine);
              //  System.IO.File.AppendAllText(@"C:\Users\John.Cook\Documents\querytest.txt", queryw);

              //   Console.Write("Invoiced Query" + query + Environment.NewLine);

                DataSet ESN = new DataSet();
                SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
                QueryAdapter.SelectCommand.CommandTimeout = 0;

                // Fill adapter with results
                QueryAdapter.Fill(ESN, "ESN");

                DataRow[] rows = ESN.Tables[0].Select(null, null);

                DataRow row = rows[0];

                string esn = row["EventSequence"].ToString();
                return esn;
            }
            catch
            {
                string esn = "No Sequence Number Found";
                return esn;
                
            }


        }

        /// <summary>
        /// Method to get EventSequence from txn.CarEvents using CarInitials, CarNumber, Mod User
        /// </summary>
        /// <param name="carno"></param>
        /// <param name="moduser"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public string[] GetProgramIDApprovalNumFromCEGMS(string esn)
        {
           
            string query = "select ProgramID, ApprovalNumber from txn.CarEventsGMS where EventSequence = '"+esn+"'";
         
           // string queryw = String.Concat(query + Environment.NewLine);
           // System.IO.File.AppendAllText(@"C:\Users\John.Cook\Documents\querytest.txt", queryw);

           // Console.Write("Invoiced Query" + query + Environment.NewLine);

            DataSet PAN = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(PAN, "PAN");

            DataRow[] rows = PAN.Tables[0].Select(null, null);

            DataRow row = rows[0];

            string[] results = {row["ProgramID"].ToString(), row["ApprovalNumber"].ToString()};
            return results;


        }

        /// <summary>
        /// Method to retrieve the UserID from the ref.AppUsers table using UserCode as search criteria
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        public string GetUserIDByUserCode(string usercode)
        {

            string query = "select UserID from ref.AppUsers where UserCode = '" + usercode + "'";

            DataSet UC = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(UC, "UC");

            DataRow[] rows = UC.Tables[0].Select(null, null);

            DataRow row = rows[0];

            string results = row["UserID"].ToString(); 
            return results;


        }

        /// <summary>
        /// Method to Retrieve the number of Car Events assigned to a user for Audit
        /// </summary>
        /// <param name="approvalnum"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public int GetAssignedCARCount(string userid)
        {

            string query = "Select count(1) as Count from txn.CarEventsAudit where AssignedUserID = '"+userid+"'";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int count = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return count;


        }

        /// <summary>
        /// Method to retrieve Audited Invoice Information from the txn.Invoices table
        /// </summary>
        /// <param name="invno"></param>
        /// <returns></returns>
        public string[] GetAuditedInvoicesByInvNumber(string invno)
        {

            string query = "Select Convert(varchar, InvoiceDate, 101) as InvoiceDate, InvoicingScac, BilledParty, Source, CoverSheetReceivedIndicator from txn.Invoices where InvoiceNumber = '" + invno + "'";

            DataSet AI = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;
            // Fill adapter with results
            QueryAdapter.Fill(AI, "AI");
            DataRow[] rows = AI.Tables[0].Select(null, null);
            DataRow row = rows[0];

            string[] results = { row["InvoiceDate"].ToString(), row["InvoicingScac"].ToString(), row["BilledParty"].ToString(), row["Source"].ToString(), row["CoverSheetReceivedIndicator"].ToString() };

            return results;

        }

    }
}
