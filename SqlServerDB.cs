using System;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Threading;
using System.Xml;
using System.Web;
using System.Windows;
using System.Runtime.InteropServices;


namespace FunctionalTest
{

     
    class SqlServerDB
    {
        // Fields
        private Config CF;

        // Methods
        public SqlServerDB(Config obj)
        {
            this.CF = obj;
        }

   
// Common Actions

/// <summary>
/// Obtain Rows from Search results by Type
/// Returns table containing all rows of that type
/// </summary>
/// <param name="type"></param>
/// <param name="table"></param>
/// <returns></returns>
        public DataTable RowsByType(string type, DataTable table)
        {

            
            switch (type)
            {

                   case "User":
                   string typeuser;
                   typeuser = "s1type = 'User'";
                   DataRow[] userRows;
                   userRows = table.Select(typeuser);

                   DataTable newtable = userRows.CopyToDataTable();
                  // int newcount = newtable.Rows.Count;
                  // Console.Write("This is NewTableCount: " + newcount);

                  //      DataColumnCollection test = table.Columns;
                  //      foreach (DataColumn column in test)
                  //      {
                  //         Console.WriteLine("Column" + column.ColumnName);
                  //         Console.WriteLine("Column" + column.DataType);
                  //     }

                   return newtable;
                  
                 case "Customer":
                        string typecustomer;
                        typecustomer = "s1type = 'Customer'";
                        DataRow[] customerRows;
                        customerRows = table.Select(typecustomer);
                        DataTable custable = customerRows.CopyToDataTable();                        

                        return custable;

                 case "AircraftModel":
                        string typeacmodel;
                        typeacmodel = "s1type = 'AircraftModel'";
                        DataRow[] acmodelRows;
                        acmodelRows = table.Select(typeacmodel);
                        DataTable acmtable = acmodelRows.CopyToDataTable();

                        return acmtable;

                 case "Event":
                        string typeevent;
                        typeevent = "s1type = 'Event'";
                        DataRow[] eventRows;
                        eventRows = table.Select(typeevent);
                        DataTable eventtable = eventRows.CopyToDataTable();

                        return eventtable;

                 case "Exceedance":
                        string typeexceedance;
                        typeexceedance = "s1type = 'Exceedance'";
                        DataRow[] exceedanceRows;
                        exceedanceRows = table.Select(typeexceedance);
                        DataTable exceedancetable = exceedanceRows.CopyToDataTable();

                        return exceedancetable;

                 case "Operation":
                        string typeoperation;
                        typeoperation = "s1type = 'Operation'";
                        DataRow[] operationRows;
                        operationRows = table.Select(typeoperation);
                        DataTable operationtable = operationRows.CopyToDataTable();

                        return operationtable;

                 case "Parameter":
                        string typeparameter;
                        typeparameter = "s1type = 'Parameter'";
                        DataRow[] parameterRows;
                        parameterRows = table.Select(typeparameter);
                        DataTable parametertable = parameterRows.CopyToDataTable();

                        return parametertable;

                 case "Part":
                        string typepart;
                        typepart = "s1type = 'Part'";
                        DataRow[] partRows;
                        partRows = table.Select(typepart);
                        DataTable parttable = partRows.CopyToDataTable();

                        return parttable;

                 case "Graph":
                        string typegraph;
                        typegraph = "s1type = 'Graph'";
                        DataRow[] graphRows;
                        graphRows = table.Select(typegraph);
                        DataTable graphtable = graphRows.CopyToDataTable();
                    
                        return graphtable;

                 case "Query":
                        string typesearch;
                        typesearch = "s1type = 'Query'";
                        DataRow[] searchRows;
                        searchRows = table.Select(typesearch);
                        DataTable searchtable = searchRows.CopyToDataTable();

                        return searchtable;

                 case "SerializedAircraft":
                        string typesa;
                        typesa = "s1type = 'SerializedAircraft'";
                        DataRow[] saRows;
                        saRows = table.Select(typesa);
                        DataTable satable = saRows.CopyToDataTable();

                        return satable;

                 case "MCI":
                        string typemci;
                        typemci = "s1type = 'MDConditionIndicator'";
                        DataRow[] mciRows;
                        mciRows = table.Select(typemci);
                        DataTable mcitable = mciRows.CopyToDataTable();

                        return mcitable;

                 case "Config":
                        string typeconfig;
                        typeconfig = "s1type = 'Config'";
                        DataRow[] cfRows;
                        cfRows = table.Select(typeconfig);
                        DataTable cftable = cfRows.CopyToDataTable();

                        return cftable;

                default:
                      CF.Failure = true;    
                      throw new ArgumentOutOfRangeException(); 
                              
            }
                               
         }

        /// <summary>
        /// Obtains all Operations within the tblOperation table.
        /// </summary>
        /// <param name="MyConnection"></param>
        /// <param name="DS"></param>
        /// <param name="dtname"></param>
        /// <returns></returns>
        public DataSet GetOperations(SqlConnection MyConnection, DataSet DS, string dtname)
        {

            //DataSet DS = new DataSet();
            string query = "Select * from dbo.tblOperation";

            DS = FillDataSet(DS, query, dtname);

            return DS;

        }


        /// <summary>
        /// Gets Event, Event Definition, LimitSet and Event Type data
        /// </summary>
        /// <param name="opid"></param>
        /// <returns></returns>
        public DataTable GetEventDefLSByOperationID(int opid)
        {

            string x = "select ed.EventShortName, ed.EventLongName, e.EventStartTime, e.EventEndTime, e.Duration, e.WasTerminated, e.IsExceedance, a.Description, ";
            string y = "e.isEndTime, d.FunctionName ,ls.SetName , et.TypeDescription from tblEvents e ";
            string e = "Inner Join tblOperation op on op.OperationID = e.OperationID Inner Join tblAutomationMode a on a.AutomationModeID = e.AutomationMode ";
            string z = "Inner Join tblDiagnosticFunctions d on d.DiagnosticFunctionID = e.DiagnosticFunction left join tblEventDefinition ed on e.EventDefID = ed.EventDefID ";
            string a = "left join tblLimitSets as ls on ed.LimitSetID = ls.LimitSetID ";
            string b = "left join tblEventTypes as et on e.EventTypeID = et.EventTypeID ";

            string query = String.Concat(x, y, e, z, a, b);

            DataSet EDS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(EDS, "EventDef");

            return EDS.Tables[0];

        }

        /// <summary>
        /// Method to obtain a count from the tblOperations table
        /// </summary>
        /// <param name="MyConnection"></param>
        /// <param name="dtname"></param>
        /// <returns></returns>
        public int GetOperationCount(SqlConnection MyConnection, string dtname)
        {
           
            DataSet DS = new DataSet();
            DS = GetOperations(CF.MyConnection, DS, dtname);
            int opcount = DS.Tables[0].Rows.Count;

            DS.Dispose();

            return opcount;  

        }

        /// <summary>
        /// Method to obtain a count from the tblGetLimitDataSetParms table
        /// </summary>
        /// <returns></returns>
        public int GetLimitDataSetParamsCount()
        {
            string query = "Select count(1) as count from tblLimitDataSetParms";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }

        /// <summary>
        /// Obtains the count from the tblLimitSetParams table
        /// </summary>
        /// <returns></returns>
        public int GetLimitSetParamCount()
        {
            string query = "Select count(1) as count from tblLimitSetParams";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }
       
        /// <summary>
        /// Obtain a count from the tblAircraftModel table. 
        /// </summary>
        /// <returns></returns>
        public int GetAircraftModelCount()
        {
            string query = "Select count(1) as count from tblAircraftModel";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }

        /// <summary>
        /// Obtain the count from the tblComponentsTypes table
        /// </summary>
        /// <returns></returns>
        public int GetComponentTypeCount()
        {
            string query = "Select count(1) as count from tblComponentTypes";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }

        /// <summary>
        /// Obtain the count from the tblDataTypes table. 
        /// </summary>
        /// <returns></returns>
        public int GetDataTypeCount()
        {
            string query = "Select count(1) as count from tblDataTypes";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }

        /// <summary>
        /// Method to return count from tblUsageData
        /// </summary>
        /// <returns></returns>
        public int GetUsageDataCount()
        {
            string query = "Select count(1) as count from tblUsageData";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }


        /// <summary>
        /// Obtain the count from the tblDiagnosticFunctions table. 
        /// </summary>
        /// <returns></returns>
        public int GetDiagnosticFunctionCount()
        {
            string query = "Select count(1) as count from tblDiagnosticFunctions";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }

        /// <summary>
        /// Obtain the count from the tblManufacturer table. 
        /// </summary>
        /// <returns></returns>
        public int GetManufacturerCount()
        {
            string query = "Select count(1) as count from tblManufacturer";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }

        /// <summary>
        /// Obtain the count from the LimitSet table. 
        /// </summary>
        /// <returns></returns>
        public int GetLimitSetCount()
        {
            string query = "Select count(1) as count from tblLimitSets";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }

        /// <summary>
        /// Obtain the count from the tblThreshold table. 
        /// </summary>
        /// <returns></returns>
        public int GetThresholdCount()
        {
            string query = "Select count(1) as count from tblThreshold";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }

        /// <summary>
        /// Obtain the count from the tblAutomationMode table
        /// </summary>
        /// <returns></returns>
        public int GetAutomationModeCount()
        {
            string query = "Select count(1) as count from tblAutomationMode";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int amcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return amcount;

        }


        /// <summary>
        /// Obtain the count from the tblAircraftSerial table. 
        /// </summary>
        /// <returns></returns>
        public int GetAircraftSerialCount()
        {
            string query = "Select count(1) as count from tblAircraftSerial";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }

        /// <summary>
        /// Obtain the count from the tblEvents table
        /// </summary>
        /// <returns></returns>
        public int GetEventCount()
        {
            string query = "Select count(1) as count from tblEvents";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);
            return opcount;

        }

        /// <summary>
        /// Obtain the count from the tblParameters table
        /// </summary>
        /// <returns></returns>
        public int GetParameterCount()
        {
            string query = "Select count(1) as count from tblParameters";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);
            return opcount;

        }

        /// <summary>
        /// Obtain the count from the tblParameters table. 
        /// </summary>
        /// <returns></returns>
        public int GetDistinctParameterCount()
        {
            string query = "Select count(distinct ParameterName) as count from tblParameters";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);
            return opcount;

        }

        /// <summary>
        /// Obtain the count from the tblEventTypes table
        /// </summary>
        /// <returns></returns>
        public int GetEventTypeCount()
        {
            string query = "Select count(1) as count from tblEventTypes";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }

        /// <summary>
        /// Obtain the count from the tblEventStatus table. 
        /// </summary>
        /// <returns></returns>
        public int GetEventStatusCount()
        {
            string query = "Select count(1) as count from tblEventStatus";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }

        /// <summary>
        /// Obtain the count from the tblEventDefinition table. 
        /// </summary>
        /// <returns></returns>
        public int GetEventDefinitionCount()
        {
            string query = "Select count(1) as count from tblEventDefinition";
            DataSet DS = new DataSet();

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, "Count");

            int opcount = Convert.ToInt32(DS.Tables[0].Rows[0]["Count"]);

            return opcount;

        }

        /// <summary>
        /// Calls the dbo.spPreloadOperation stored procedure
        /// </summary>
        /// <param name="operationname"></param>
        /// <param name="opfilename"></param>
        /// <param name="statusid"></param>
        /// <param name="reloadoperation"></param>
        /// <param name="customerid"></param>
        /// <param name="templateid"></param>
        /// <param name="priority"></param>
        /// <param name="MyConnection"></param>
        public void CallPreloadProc(string operationname, string opfilename, int statusid, int reloadoperation, int customerid, int templateid,  int priority, SqlConnection MyConnection)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("dbo.spPreLoadOperation", CF.MyConnection);

          //  if (customerid == 0)
          //  {

          //      ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
          //      ProcAdapter.SelectCommand.Parameters["@UserID"].Value = null;

          //  }
          //  else
          //  {
                
          //      ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
           //     ProcAdapter.SelectCommand.Parameters["@UserID"].Value = customerid;
                

           // }

          //  Console.Write("This is Customer ID" + customerid);
          //  Console.Write(Environment.NewLine);
         
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@OperationName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@OperationName"].Value = operationname;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@OpFileName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@OpFileName"].Value = opfilename;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatusID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@StatusID"].Value = statusid;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ReloadOperation", SqlDbType.Bit));
            ProcAdapter.SelectCommand.Parameters["@ReloadOperation"].Value = reloadoperation;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@UserID"].Value = DBNull.Value;            
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@TemplateID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@TemplateID"].Value = templateid;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Priority", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@Priority"].Value = statusid;

            ProcAdapter.SelectCommand.CommandTimeout = 0;
            MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            MyConnection.Close();
            ProcAdapter.Dispose();
           
          
        }

        /// <summary>
        /// Calls the spRunSearch stored procedure
        /// </summary>
        /// <param name="keywordsearch"></param>
        /// <param name="userid"></param>
        /// <param name="reccount"></param>
        /// <param name="countonly"></param>
        /// <param name="MyConnection"></param>
        /// <returns></returns>
        public SqlDataAdapter CallSearchProc(string keywordsearch, string userid, string reccount, string countonly, SqlConnection MyConnection)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter(CF.ProcName, CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@SearchString", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@SearchString"].Value = keywordsearch;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));

            // Hardcoded to Userid 3  - need to dynamically obtain this value
            ProcAdapter.SelectCommand.Parameters["@UserID"].Value = userid;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@RecCount", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@RecCount"].Value = reccount;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CountOnly", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@CountOnly"].Value = countonly;

            return ProcAdapter;

        }

        /// <summary>
        /// Calls the spInsertAircraftModel stored procedure
        /// </summary>
        /// <param name="manufacturerID"></param>
        /// <param name="modeldescription"></param>
        /// <param name="MyConnection"></param>
        public void InsertAircraftModel(int manufacturerID, string modeldescription, SqlConnection MyConnection)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertAircraftModel", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ManufacturerID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@ManufacturerID"].Value = manufacturerID;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ModelDescription", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@ModelDescription"].Value = modeldescription;

            MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Calls the spInsertAutomationMode stored procedure
        /// </summary>
        /// <param name="modeID"></param>
        /// <param name="description"></param>
        /// <param name="MyConnection"></param>
        public void InsertAutomationMode(int modeID, string description, SqlConnection MyConnection)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertAutomationMode", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@AutomationModeID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@AutomationModeID"].Value = modeID;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@Description"].Value = description;

            MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Calls the spInsertComponentType stored procedure
        /// </summary>
        /// <param name="ctID"></param>
        /// <param name="description"></param>
        public void InsertComponentType(int ctID, string description)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertComponentType", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ComponentTypeID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@ComponentTypeID"].Value = ctID;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@TypeDescription", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@TypeDescription"].Value = description;

            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Cals the spInsertDataType stored procedure
        /// </summary>
        /// <param name="ctID"></param>
        /// <param name="description"></param>
        public void InsertDataType(int ctID, string description)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertDataType", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DatatypeID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@DatatypeID"].Value = ctID;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@TypeDescription", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@TypeDescription"].Value = description;

            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Calls the spInsertEventType stored procedure
        /// </summary>
        /// <param name="ctID"></param>
        /// <param name="description"></param>
        public void InsertEventType(int ctID, string description)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertEventType", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@EventtypeID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@EventtypeID"].Value = ctID;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@TypeDescription", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@TypeDescription"].Value = description;

            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Calls the spInsertEventStatus stored procedure
        /// </summary>
        /// <param name="ctID"></param>
        /// <param name="description"></param>
        public void InsertEventStatus(int ctID, string description)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertEventStatus", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatusID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@StatusID"].Value = ctID;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@StatusName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@StatusName"].Value = description;

            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Calls the spInsertDiagnosticFunctions stored procedure
        /// </summary>
        /// <param name="ctID"></param>
        /// <param name="description"></param>
        public void InsertDiagnosticFunction(int ctID, string description)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertDiagnosticFunctions", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DiagnosticFunctionID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@DiagnosticFunctionID"].Value = ctID;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@FunctionName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@FunctionName"].Value = description;

            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Calls the spInsertManufacturer stored procedure
        /// </summary>
        /// <param name="manufacturername"></param>
        public void InsertManufacturer(string manufacturername)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertManufacturer", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ManufacturerName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@ManufacturerName"].Value = manufacturername;
           

            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Calls the spInsertParameterData stored procedure
        /// </summary>
        /// <param name="erow"></param>
        public void InsertParameterData(DataRow erow)
        {

            
            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertParameterData", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@EventID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@EventID"].Value = erow["EventID"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@LimitSetID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@LimitSetID"].Value = erow["LimitSetID"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ParameterID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@ParameterID"].Value = erow["ParameterID"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ValueInt", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@ValueInt"].Value = erow["ValueInt"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ValueUInt", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@ValueUInt"].Value = erow["ValueUInt"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ValueFloat", SqlDbType.Float));
            ProcAdapter.SelectCommand.Parameters["@ValueFloat"].Value = erow["ValueFloat"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CfgID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@CfgID"].Value = erow["CfgID"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ValueType", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@ValueType"].Value = erow["ValueType"];
            
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ActivityTime", SqlDbType.DateTime));
            ProcAdapter.SelectCommand.Parameters["@ActivityTime"].Value = erow["ActivityTime"];
                   
          


            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }


        /// <summary>
        /// Calls the spInsertEvent stored procedure
        /// </summary>
        /// <param name="erow"></param>
        public void InsertEvents(DataRow erow)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertEvent", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@OperationID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@OperationID"].Value = erow["OperationID"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@EventDefID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@EventDefID"].Value = erow["EventDefinitionID"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@EventTypeID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@EventTypeID"].Value = erow["EventTypeID"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@EventStartTime", SqlDbType.DateTime));
            ProcAdapter.SelectCommand.Parameters["@EventStartTime"].Value = erow["EventStartTime"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@EventEndTime", SqlDbType.DateTime));
            ProcAdapter.SelectCommand.Parameters["@EventEndTime"].Value = erow["EventEndTime"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ThresholdID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@ThresholdID"].Value = erow["ThresholdID"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@WasTerminated", SqlDbType.Bit));
            ProcAdapter.SelectCommand.Parameters["@WasTerminated"].Value = erow["WasTerminated"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@IsExceedance", SqlDbType.Bit));
            ProcAdapter.SelectCommand.Parameters["@IsExceedance"].Value = erow["IsExceedance"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@AutomationMode", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@AutomationMode"].Value = erow["AutomationMode"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@IsEndTime", SqlDbType.Bit));
            ProcAdapter.SelectCommand.Parameters["@IsEndTime"].Value = erow["IsEndTime"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DiagnosticFunction", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@DiagnosticFunction"].Value = erow["DiagnosticFunction"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@GroupID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@GroupID"].Value = erow["GroupID"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Status", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@Status"].Value = erow["Status"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CreateDate", SqlDbType.DateTime));
            ProcAdapter.SelectCommand.Parameters["@CreateDate"].Value = erow["CreateDate"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@CreateBy"].Value = erow["CreateBy"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ModelID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@ModelID"].Value = erow["ModelID"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@EventShortName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@EventShortName"].Value = erow["EventShortName"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CfgID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@CfgID"].Value = erow["CfgID"];


            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Calls the spInsertEventDefinition stored procedure
        /// </summary>
        /// <param name="erow"></param>
        public void InsertEventDefinition(DataRow erow)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertEventDefinition", CF.MyConnection);

          
            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ModelID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@ModelID"].Value = erow["ModelID"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@EventLongName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@EventLongName"].Value = erow["EventLongName"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@EventShortName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@EventShortName"].Value = erow["EventShortName"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@SetName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@SetName"].Value = erow["SetName"];
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CFGID", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@CFGID"].Value = erow["CFGID"];           
                  
            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Calls the spInsertThreshold stored procedure
        /// </summary>
        /// <param name="description"></param>
        /// <param name="minval"></param>
        /// <param name="maxval"></param>
        /// <param name="cfgid"></param>
        public void InsertThreshold(string description, string minval, string maxval, int cfgid)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertThreshold", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Description", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@Description"].Value = description;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@MinValue", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@MinValue"].Value = minval;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@MaxValue", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@MaxValue"].Value = maxval;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CFGID", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@CFGID"].Value = cfgid;

            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Calls the spInsertLimitSets stored procedure
        /// </summary>
        /// <param name="setname"></param>
        /// <param name="modelid"></param>
        /// <param name="cfgid"></param>
        public void InsertLimitSet(string setname, int modelid, int cfgid)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertLimitSets", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@SetName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@SetName"].Value = setname;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ModelID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@ModelID"].Value = modelid;        
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CFGID", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@CFGID"].Value = cfgid;

            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Calls the spInsertLimitSetParams stored procedure
        /// </summary>
        /// <param name="setname"></param>
        /// <param name="name"></param>
        /// <param name="cfgid"></param>
        public void InsertLimitSetParams(string setname, string name, int cfgid)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertLimitSetParams", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@SetName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@SetName"].Value = setname;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ParameterName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@ParameterName"].Value = name;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CFGID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@CFGID"].Value = cfgid;

            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Calls the spInsertParameters stored procedure. 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="datatypeid"></param>
        /// <param name="cfgid"></param>
        /// <param name="unit"></param>
        /// <param name="longname"></param>
        public void InsertParameter(string name, int datatypeid, int cfgid, string unit, string longname)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertParameters", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ParameterName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@ParameterName"].Value = name;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@DatatypeID", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@DatatypeID"].Value = datatypeid;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@CFGID", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@CFGID"].Value = cfgid;           

            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }

        /// <summary>
        /// Calls the spInsertAircraftSerial stored procedure
        /// </summary>
        /// <param name="SerialNumber"></param>
        /// <param name="ModelDescription"></param>
        /// <param name="ManufacturerName"></param>
        public void InsertAircraftSerial(string SerialNumber, string ModelDescription, string ManufacturerName)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter("spInsertAircraftSerial", CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@SerialNumber", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@SerialNumber"].Value = SerialNumber;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ModelDescription", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@ModelDescription"].Value = ModelDescription;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@ManufacturerName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@ManufacturerName"].Value = ManufacturerName;

            CF.MyConnection.Open();
            ProcAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            ProcAdapter.Dispose();

        }



        /// <summary>
        /// Calls the spInsertQuery stored procedure
        /// </summary>
        /// <param name="queryname"></param>
        /// <param name="userid"></param>
        /// <param name="searchstring"></param>
        /// <param name="overwrite"></param>
        /// <param name="guid"></param>
        /// <param name="MyConnection"></param>
        /// <returns></returns>
        public SqlDataAdapter InsertQueryProc(string queryname, string userid, string searchstring, int overwrite, string guid, SqlConnection MyConnection)
        {

            SqlDataAdapter ProcAdapter = new SqlDataAdapter(CF.ProcName, CF.MyConnection);

            // Call Stored Proc
            ProcAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@QueryName", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@QueryName"].Value = queryname;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int));
            // Hardcoded to Userid 3  - need to dynamically obtain this value
            ProcAdapter.SelectCommand.Parameters["@UserID"].Value = userid;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@SearchString", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@SearchString"].Value = searchstring;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@Overwrite", SqlDbType.Int));
            ProcAdapter.SelectCommand.Parameters["@Overwrite"].Value = overwrite;
            ProcAdapter.SelectCommand.Parameters.Add(new SqlParameter("@GuidID", SqlDbType.VarChar));
            ProcAdapter.SelectCommand.Parameters["@GuidID"].Value = searchstring;

            return ProcAdapter;

        }

        /// <summary>
        /// Deletes a row from the tblAutomationMode table by AutomationModeID
        /// </summary>
        /// <param name="id"></param>
        public void DeleteAutomationModeByID(int id)
        {
            string query = "Delete from tblAutomationMode where AutomationModeID = "+id;

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);

            CF.MyConnection.Open();
            QueryAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            QueryAdapter.Dispose();  

        }

        /// <summary>
        /// Delets a row from the tblComponentTypes table by ComponentTypeID
        /// </summary>
        /// <param name="id"></param>
        public void DeleteComponentTypeByID(int id)
        {
            string query = "Delete from tblComponentTypes where ComponentTypeID = " + id;
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);

            CF.MyConnection.Open();
            QueryAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            QueryAdapter.Dispose();

        }

        /// <summary>
        /// Deletes a row from the tblDataTypes table by DataTypeID
        /// </summary>
        /// <param name="id"></param>
        public void DeleteDataTypeByID(int id)
        {
            string query = "Delete from tblDataTypes where DataTypeID = " + id;
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);

            CF.MyConnection.Open();
            QueryAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            QueryAdapter.Dispose();

        }

        /// <summary>
        /// Deletes a row from the tblEventTypes table by EventTypeID
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEventTypeByID(int id)
        {
            string query = "Delete from tblEventTypes where EventTypeID = " + id;
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);

            CF.MyConnection.Open();
            QueryAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            QueryAdapter.Dispose();

        }
        /// <summary>
        /// Deletes a row from the tblEventStatus table by StatusID
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEventStatusByID(int id)
        {
            string query = "Delete from tblEventStatus where StatusID = " + id;
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);

            CF.MyConnection.Open();
            QueryAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            QueryAdapter.Dispose();

        }
        /// <summary>
        /// Deletes a row from the tblDiagnosticFunctions table by DiagnosticFunctionID
        /// </summary>
        /// <param name="id"></param>
        public void DeleteDiagnosticFunctionByID(int id)
        {
            string query = "Delete from tblDiagnosticFunctions where DiagnosticFunctionID = " + id;
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);

            CF.MyConnection.Open();
            QueryAdapter.SelectCommand.ExecuteNonQuery();
            CF.MyConnection.Close();
            QueryAdapter.Dispose();

        }

        /// <summary>
        /// Searches a table for a specific value
        /// </summary>
        /// <param name="search"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public DataRow[] SearchTable(string search, DataTable table)
        {

            DataRow[] results;

            // Use Select method to find results in table
            results = table.Select(search);

            return results;

        }

        /// <summary>
        /// Method to compile Like String
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public List<string> LikeString(string[] search)
        {

            int x = search.Count();
            List<string> statement =  new List<string>();

            foreach(string i in search)
            {

                string test = "Like '%" + i + "%'";

                statement.Add(test);


            }

             return statement;

         }

        /// <summary>
        /// Fills a Data set with a result of a query
        /// </summary>
        /// <param name="DS"></param>
        /// <param name="query"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public DataSet FillDataSet(DataSet DS, string query, string tablename)
        {
            
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, tablename);
            return DS;

        }
        /// <summary>
        /// Obtains the latest row from the tblOperation table
        /// </summary>
        /// <returns></returns>
        public DataRow GetLatestOperationByID(int operationid)
        {

           string query = "select * from tblOperation where OperationID = "+operationid;
                   
           DataSet MAX = new DataSet();
           SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
           QueryAdapter.SelectCommand.CommandTimeout = 0;

           // Fill adapter with results
           QueryAdapter.Fill(MAX, "OPMAX");

           DataRow[] rows = MAX.Tables[0].Select(null, null);
           DataRow row = rows[0];

           return row;

        }

        public int GetLatestOperationID()
        {

            string query = "select MAX(OperationID) as OperationID from tblOperation";

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "OPMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            int id = Convert.ToInt32(row["OperationID"]);

            return id;


        }

                     

        /// <summary>
        /// Obtain the latest row from the tblAircraftSerial table
        /// </summary>
        /// <returns></returns>
        public int GetLatestAircraftSerialID()
        {

            string query = "select MAX(AircraftSerialID) as AircraftSerialID from tblAircraftSerial";

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "OPMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            int id = Convert.ToInt32(row["AircraftSerialId"]);

            return id;


        }

        /// <summary>
        /// Obtain the latest LimitSetID from the tblLimitSets table
        /// </summary>
        /// <returns>int</returns>
        public int GetLatestLimitSetID()
        {

            string query = "select MAX(LimitSetID) as LimitSetID from tblLimitSets";

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "OPMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            int id = Convert.ToInt32(row["LimitSetID"]);

            return id;

        }

        /// <summary>
        /// Obtain the latest SetParamID from the tblLimitSetParams table
        /// </summary>
        /// <returns></returns>
        public int GetLatestLimitSetParamID()
        {

            string query = "select MAX(SetParamID) as SetParamID from tblLimitSetParams";

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "OPMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            int id = Convert.ToInt32(row["SetParamID"]);

            return id;

        }

        /// <summary>
        /// Obtain the latest LimitsDataSetID from the tlblLimitDataSetParms table
        /// </summary>
        /// <returns>int</returns>
        public int GetLatestLimitDataSetParamID()
        {

            string query = "select MAX(LimitsDataSetID) as LimitsDataSetID from tblLimitDataSetParms";

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;
            // Fill adapter with results
            QueryAdapter.Fill(MAX, "OPMAX");
            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            int id = Convert.ToInt32(row["LimitsDataSetID"]);
            return id;

        }

        /// <summary>
        /// Obtains the PPUCfgID from the tblConfigurations table by cfgid
        /// </summary>
        /// <param name="cfgid"></param>
        /// <returns>string</returns>
        public string GetConfigByID(int cfgid)
        {

            string query = "select PPUCfgID from tblConfigurations where cfgid = '"+cfgid+"'";
            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;
            // Fill adapter with results
            QueryAdapter.Fill(MAX, "OPMAX");
            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];
            string name = row["PPUCfgID"].ToString();

            return name;

        }
        /// <summary>
        /// Obtains the latests ParameterID from the tblParameters table. 
        /// </summary>
        /// <returns></returns>
        public int GetLatestParameterID()
        {

            string query = "select MAX(ParameterID) as ParameterID from tblParameters";
            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;
            // Fill adapter with results
            QueryAdapter.Fill(MAX, "OPMAX");
            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];
            int id = Convert.ToInt32(row["ParameterID"]);

            return id;

        }
       
        /// <summary>
        /// Obtains the latest ManufacturerID from the tblManufacturer table
        /// </summary>
        /// <returns></returns>
        public int GetLatestManufacturerID()
        {

            string query = "select MAX(ManufacturerID) as ManufacturerID from tblManufacturer";

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "OPMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            int id = Convert.ToInt32(row["ManufacturerID"]);

            return id;


        }

        /// <summary>
        /// Obtains the latests EventTypeID from the tblEventTypes table
        /// </summary>
        /// <returns></returns>
        public int GetLatestEventTypeID()
        {

            string query = "select MAX(EventTypeID) as EventTypeID from tblEventTypes";

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "OPMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            int id = Convert.ToInt32(row["EventTypeID"]);

            return id;


        }

        /// <summary>
        /// Obtains the latest CfgID from the tblConfigurations table. 
        /// </summary>
        /// <returns></returns>
         public int GetLatestConfigID()
        {

            string query = "select MAX(CfgID) as CfgID from tblConfigurations";

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "OPMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            int id = Convert.ToInt32(row["CfgID"]);

            return id;


        }

        /// <summary>
        /// Obtains the latests ThresholdID from the tblThreshold table 
        /// </summary>
        /// <returns></returns>
         public int GetLatestThresholdID()
         {

             string query = "select MAX(ThresholdID) as ThresholdID from tblThreshold";

             DataSet MAX = new DataSet();
             SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
             QueryAdapter.SelectCommand.CommandTimeout = 0;

             // Fill adapter with results
             QueryAdapter.Fill(MAX, "OPMAX");

             DataRow[] rows = MAX.Tables[0].Select(null, null);
             DataRow row = rows[0];

             int id = Convert.ToInt32(row["ThresholdID"]);

             return id;


         }



        /// <summary>
        /// Obtain the EventID from the tblEvents table 
        /// </summary>
        /// <returns></returns>
        public int GetLatestEventID()
        {

            string query = "select MAX(EventID) as EventID from tblEvents";           

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "OPMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            int id = Convert.ToInt32(row["EventID"]);

            return id;
                       

        }

        /// <summary>
        /// Obtain the LimitSetID from the tblLimitsSet table by SetName
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetLimitDataSetIDByName(string name)
        {

            string query = "select LimitSetID from tblLimitSets where SetName = '"+name+"'";
                       

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "OPMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            int id = Convert.ToInt32(row["LimitSetID"]);

            return id;


        }


       

        /// <summary>
        /// Obtain the latest EventDefID from the tblEventDefinition table
        /// </summary>
        /// <returns></returns>
        public int GetLatestEventDefinitionID()
        {

            string query = "select MAX(EventDefID) as EventDefID from tblEventDefinition";
            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;
            // Fill adapter with results
            QueryAdapter.Fill(MAX, "OPMAX");
            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];
            int id = Convert.ToInt32(row["EventDefID"]);

            return id;

        }

        /// <summary>
        /// Obtain the latest AircraftModelID from the tblAircraftModel table
        /// </summary>
        /// <returns></returns>
        public int GetLatestAircraftModelID()
        {

            string query = "select MAX(ModelID) as ModelID from tblAircraftModel ";         
            
            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "AMMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            int id = Convert.ToInt32(row["ModelID"]);

            return id;

        }

        /// <summary>
        /// Obtain the latest AircraftModel row from the tblAircraftModel table by ModelID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataRow GetLatestAircraftModelByID(int id)
        {

            string query = "select * from tblAircraftModel where ModelID = "+id;
            
           

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "AMMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }

    

        /// <summary>
        /// Obtain a row from the tblManufacturer table by ManufacturerID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataRow GetLatestManufacturerByID(int id)
        {

            string query = "select * from tblManufacturer where ManufacturerID = "+id;          
           
            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }

        /// <summary>
        /// Get row from tblLimitSets by LimitSetID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataRow GetLatestLimitSetByID(int id)
        {

            string query = "select * from tblLimitSets where LimitSetID = "+id;

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }
        /// <summary>
        /// Get row from tblLimitDataSetParms by LimitsDataSetID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataRow GetLatestLimitDataSetParamByID(int id)
        {

            string query = "select * from tblLimitDataSetParms where LimitsDataSetID = " + id;

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }
        /// <summary>
        /// Get row from tbleLimitSetParams by SetParamID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataRow GetLatestLimitSetParamByID(int id)
        {

            string query = "select * from tblLimitSetParams where SetParamID = " + id;

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }

        /// <summary>
        /// Get row from tblParameters from ParameterID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataRow GetLatestParameterByID(int id)
        {

            string query = "select * from tblParameters where ParameterID = " + id;

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }


        /// <summary>
        /// Get row from tblAutomationMode by AutomationModeID
        /// </summary>
        /// <param name="modelid"></param>
        /// <returns></returns>
        public DataRow GetAutomationModeByID(int modelid)
        {

            string query = "select * from tblAutomationMode where AutomationModeID = "+modelid;
           
            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }

        /// <summary>
        /// Get row from tblEvents by EventID
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public DataRow GetEventByID(int eid)
        {

            string query = "select * from tblEvents where EventID = " + eid;

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }

        /// <summary>
        /// Get row from tblEventTypes by EventTypeID
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public DataRow GetEventTypeByID(int eid)
        {

            string query = "select * from tblEventTypes where EventTypeID = " + eid;

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }

        /// <summary>
        /// Get row from tblEventStatus by StatusID
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public DataRow GetEventStatusByID(int eid)
        {

            string query = "select * from tblEventStatus where StatusID = " + eid;

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }

        /// <summary>
        /// Get row from tbl Threshold by ThresholdID
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public DataRow GetThresholdByID(int eid)
        {

            string query = "select * from tblThreshold where ThresholdID = " + eid;

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }

        /// <summary>
        /// Get row from tblEventDefinition by EventDefID
        /// </summary>
        /// <param name="eid"></param>
        /// <returns></returns>
        public DataRow GetEventDefinitionByID(int eid)
        {

            string query = "select * from tblEventDefinition where EventDefID = " + eid;
            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;
            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");
            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }

        /// <summary>
        /// Get row from tblComponentTypes by ComponentTypeID
        /// </summary>
        /// <param name="ctid"></param>
        /// <returns></returns>
        public DataRow GetComponentTypeByID(int ctid)
        {

            string query = "select * from tblComponentTypes where ComponentTypeID = " + ctid;
            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;
            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");
            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }

        /// <summary>
        /// Get row from tblDataTypes by DataTypeID
        /// </summary>
        /// <param name="ctid"></param>
        /// <returns></returns>
        public DataRow GetDataTypeByID(int ctid)
        {

            string query = "select * from tblDataTypes where DatatypeID = " + ctid;

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }


        /// <summary>
        /// Get row from tblDiagnosticFunctions by DiagnosticFunctionID
        /// </summary>
        /// <param name="ctid"></param>
        /// <returns></returns>
        public DataRow GetDiagnosticFunctionByID(int ctid)
        {

            string query = "select * from tblDiagnosticFunctions where DiagnosticFunctionID = " + ctid;

            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(MAX, "MMAX");

            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }

        /// <summary>
        /// Get row from tblAircraftSerial by AircraftSerialID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataRow GetAircraftSerialByID(int id)
        {

            string query = "select * from tblAircraftSerial where AircraftSerialID = "+id;
            
            DataSet MAX = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;
            // Fill adapter with results
            QueryAdapter.Fill(MAX, "ASMAX");
            DataRow[] rows = MAX.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }
        
        /// <summary>
        /// Get row from tblOperationLoadStatus by OperationID
        /// </summary>
        /// <param name="operationid"></param>
        /// <returns></returns>
        public DataRow GetOperationLoadStatus(int operationid)
        {

            string query = "select * from tblOperationLoadStatus where OperationID = "+ operationid;
            
            DataSet OLS = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(OLS, "OLS");

            DataRow[] rows = OLS.Tables[0].Select(null, null);
            DataRow row = rows[0];

            return row;

        }

        /// <summary>
        /// Get row from tblAircraftModel by ModelDescription and ManufacturerID
        /// </summary>
        /// <param name="modeldescription"></param>
        /// <param name="manufacturerid"></param>
        /// <returns></returns>
        public DataRow GetAircraftModel(string modeldescription, int manufacturerid)
        {

            string query = "select * from tblAircraftModel where ModelDescription = '" + modeldescription + "' and ManufacturerID = '"+manufacturerid+"'";

            DataSet ACM = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(ACM, "ACM");

            DataRow[] rows = ACM.Tables[0].Select(null, null);
            if (rows.Count() > 1)
            {
                Console.Write("More than 1 Aircraft Model/ManufacturerID Combo returned, verify requirements");
                Console.Write(Environment.NewLine);

            }
            DataRow row = rows[0];

            return row;

        }
        /// <summary>
        /// Get row from tblAircraftSerial by ModelID and SerialNumber
        /// </summary>
        /// <param name="modelid"></param>
        /// <param name="serialnumber"></param>
        /// <returns></returns>
        public DataRow GetAircraftSerial(int modelid, string serialnumber)
        {

            string query = "select * from tblAircraftSerial where ModelID = '" + modelid + "' and SerialNumber = '" + serialnumber + "'";

            DataSet ACS = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(ACS, "ACS");

            DataRow[] rows = ACS.Tables[0].Select(null, null);
            if (rows.Count() > 1)
            {
                Console.Write("More than 1 Aircraft Model/Serial Number Combo returned, verify requirements");
                Console.Write(Environment.NewLine);

            }
            DataRow row = rows[0];

            return row;

        }


        /// <summary>
        /// Get row from tblOperationBatchStatus by OperationID
        /// </summary>
        /// <param name="OperationID"></param>
        /// <returns></returns>
        public DataTable GetOperationBatchStatus(int OperationID)
        {

            string query = "select * from tblOperationBatchStatus where OperationID = "+OperationID;          
            
            DataSet OBS = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(OBS, "OBS");           

            return OBS.Tables[0];

        }

        /// <summary>
        /// Get row from tblReferenceTemplateBatch by TemplateID
        /// </summary>
        /// <param name="TemplateID"></param>
        /// <returns></returns>
        public DataTable GetTableReferenceBatch(int TemplateID)
        {

            string query = "select * from tblReferenceTemplateBatch where TemplateID= " + TemplateID;

            DataSet TRB = new DataSet();
            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, CF.MyConnection);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(TRB, "TRB");

            return TRB.Tables[0];

        }

        /// <summary>
        /// Fill Data Set with results from query using any connection
        /// </summary>
        /// <param name="con"></param>
        /// <param name="DS"></param>
        /// <param name="query"></param>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public DataSet VarConFDS(SqlConnection con, DataSet DS, string query, string tablename)
        {

            SqlDataAdapter QueryAdapter = new SqlDataAdapter(query, con);
            QueryAdapter.SelectCommand.CommandTimeout = 0;

            // Fill adapter with results
            QueryAdapter.Fill(DS, tablename);

            return DS;

        }
        /// <summary>
        /// Method to exeucte Double Quote tests
        /// </summary>
        /// <param name="ssdb"></param>
        public void execute_doublequote(SqlServerDB ssdb)
        {
                     
                   


            for (int i = 107; i < 157; i++)
            {
                
                Type _x = typeof(SqlServerDB);

                string x = "DBTC_" + i;

                try
                {
                    Assert.IsNotNull(x);
                                  
                }

                catch
                {

                    continue;
                   
                }
               
                _x.GetMethod(x).Invoke(ssdb, null);
                
               
            }
        }



        /// <summary>
        /// Method to execute all DB tests
        /// </summary>
        /// <param name="type"></param>
        public void execute_tests(string type)
        {

            // Case for each type Keyword, Filter, DoubleQuote
            // set variables for all 3 types

                       

            switch (type)
            {
                case "Keyword":

                Keyword kw = new Keyword(CF);

                Type _z = typeof(Keyword);

                _MethodInfo[] m = _z.GetMethods();

               
                foreach(_MethodInfo y in m)
                 
                {
                    try
                    {

                        string x = y.ToString();
                        Console.Write("MethodName:" + x);
                        Console.Write(Environment.NewLine);
                        y.Invoke(kw, null);
                    }
                    catch
                    {

                        // Catch Non Executable Methods  (Not Tests)
                        
                    }
                }

                break;


                case "DQ":
                DoubleQuote dq = new DoubleQuote(CF);

                Type _x = typeof(DoubleQuote);


                _MethodInfo[] a = _x.GetMethods();


                foreach (_MethodInfo z in a)
                 
                {
                    try
                    {
                        
                        z.Invoke(dq, null);

                    }
                    catch
                    {
                        // Catch Non Executable Methods  (Not Tests)

                    }

                }


                break;
                    

                case "Filter":

                KWFilter kwf = new KWFilter(CF);

                Type _y = typeof(KWFilter);

                _MethodInfo[] b = _y.GetMethods();

               
               
                
                foreach(_MethodInfo y in b)
                 
               
                {
                    try
                    {
                        y.Invoke(kwf, null);
                    }
                    catch
                    {

                        // Catch Non Executable Methods  (Not Tests)

                    }

                }

                
                break;



            }


        }

        /// <summary>
        /// Method to extract data from VVT0AP37 and Load into target DB 
        /// used for post Data Load data massage
        /// </summary>
        /// <param name="tbl"></param>
        public void dbinsert(string tbl)

         {

             string Server = @"VVT0AP37\Ellipsis";
             string DBName = "Ellipsis_92A";
             
            

             SqlConnection known = new SqlConnection("server=" + Server + ";database=" + DBName + "; Integrated Security=SSPI;");
            // 2 methods to get data either extract from known source and load or use the appropriate stored procs to create

            // Connect to known good DB 
            
            // get data from Users  - Insert into 30 
             switch (tbl)
             {

                 case "Users":
                     string query = "Select * from dbo.tblUsers";
                     DataSet DS = new DataSet();

                     // Instantiate Data Adapter with user query result

                     VarConFDS(known, DS, query, "UserQuery");

                     CF.MyConnection.Open();

                     foreach (DataRow x in DS.Tables[0].Rows)
                     {

                         // Generate insert query using Row columns  17 columns
                         
                         string q1 = "Insert into tblUsers (UserName, EmailAddress, CustomerID)";
                         string q2 = "Values('" + x[1] + "','test@gmail.com','" + x[14] + "')";

                         string query1 = String.Concat(q1, q2);

                         System.IO.File.AppendAllText(CF.QueryPath, query1);

                         try
                         {

                             SqlCommand cmdnon = new SqlCommand(query1, CF.MyConnection);
                             Console.WriteLine("Executing statement {0}", cmdnon.CommandText);
                             cmdnon.ExecuteNonQuery();

                         }
                         catch (SqlException ex)
                         {
                             Console.WriteLine("Error" + ex.ToString());
                         }
                         
                         // Execute insert query

                     }

                     CF.MyConnection.Close();

                     break;



                 // get data from UserRoles - Insert into 30
                     // FK Constraint on Roles
                
                     case "UserRoles":
                     string qrole = "Select * from dbo.tblUserRoles";
                     DataSet LDS = new DataSet();

                     // Select * from tblRoles   If result set is NULL break;

                     // Instantiate Data Adapter with user query result

                     VarConFDS(known, LDS, qrole, "UserQuery");

                     CF.MyConnection.Open();

                     foreach (DataRow x in LDS.Tables[0].Rows)
                     {
                       
                         string q1 = "Insert into tblUserRoles (RoleID, UserID) ";
                         string q2 = "Values('" + x[1] + "','"+x[2]+"')";

                         string query1 = String.Concat(q1, q2);

                         System.IO.File.AppendAllText(CF.QueryPath, query1);

                         try
                         {

                             SqlCommand cmdnon = new SqlCommand(query1, CF.MyConnection);
                             Console.WriteLine("Executing statement {0}", cmdnon.CommandText);
                             cmdnon.ExecuteNonQuery();

                         }
                         catch (SqlException ex)
                         {
                             Console.WriteLine("Error" + ex.ToString());
                         }
                                             

                     }

                     CF.MyConnection.Close();
                     break;

                 

                 // get data from Roles - Insert into 30
                     case "Roles":
                     string role = "Select * from dbo.tblRoles";
                     DataSet RLDS = new DataSet();

                 // Instantiate Data Adapter with user query result

                     VarConFDS(known, RLDS, role, "UserQuery");

                     CF.MyConnection.Open();

                     foreach (DataRow x in RLDS.Tables[0].Rows)
                     {

                         string q1 = "Insert into tblRoles (RoleName) ";
                         string q2 = "Values('" + x[1]+ "')";

                         string query1 = String.Concat(q1, q2);

                         System.IO.File.AppendAllText(CF.QueryPath, query1);

                         try
                         {

                             SqlCommand cmdnon = new SqlCommand(query1, CF.MyConnection);
                             Console.WriteLine("Executing statement {0}", cmdnon.CommandText);
                             cmdnon.ExecuteNonQuery();

                         }
                         catch (SqlException ex)
                         {
                             Console.WriteLine("Error" + ex.ToString());
                         }


                     }

                     CF.MyConnection.Close();
                     break;


                 // get data from RolePermissions  - Insert into 30

                     case "RolePermissions":
                     string rp = "Select * from dbo.tblRolePermissions";
                   
                     DataSet RPD = new DataSet();

                     VarConFDS(known, RPD, rp, "UserQuery");

                     CF.MyConnection.Open();

                     foreach (DataRow x in RPD.Tables[0].Rows)
                     {

                         string q1 = "Insert into tblRolePermissions (PermissionID, RoleID) ";
                         string q2 = "Values('"+ x[1] + "', '"+x[2]+"')";

                         string query1 = String.Concat(q1, q2);

                         System.IO.File.AppendAllText(CF.QueryPath, query1);

                         try
                         {

                             SqlCommand cmdnon = new SqlCommand(query1, CF.MyConnection);
                             Console.WriteLine("Executing statement {0}", cmdnon.CommandText);
                             cmdnon.ExecuteNonQuery();

                         }
                         catch (SqlException ex)
                         {
                             Console.WriteLine("Error" + ex.ToString());
                         }

                     }


                     CF.MyConnection.Close();


                     break;

                 // get data from UserGroups - Insert into 30 

                 // get data from Permissions - Insert into 30

                     case "Permissions":
                     string perm = "Select * from dbo.tblPermissions";
                     string update = "Update dbo.tblPermissions set ControlPointID = '0' where PermissionName in('AccessUser', 'AccessOrg', 'AccessAll')";
                     DataSet LD = new DataSet();

                     // Select * from tblRoles   If result set is NULL break;

                     // Instantiate Data Adapter with user query result

                     VarConFDS(known, LD, perm, "UserQuery");

                     CF.MyConnection.Open();

                     foreach (DataRow x in LD.Tables[0].Rows)
                     {

                         string q1 = "Insert into tblPermissions ";
                         string q2 = "Values('"+x[0]+"','" + x[1] + "','"+x[2]+"', '0')";

                         string query1 = String.Concat(q1, q2);

                         System.IO.File.AppendAllText(CF.QueryPath, query1);

                         try
                         {

                             SqlCommand cmdnon = new SqlCommand(query1, CF.MyConnection);
                             Console.WriteLine("Executing statement {0}", cmdnon.CommandText);
                             cmdnon.ExecuteNonQuery();

                         }
                         catch (SqlException ex)
                         {
                             Console.WriteLine("Error" + ex.ToString());
                         }
                         
                     }

                     SqlCommand cmdupdate = new SqlCommand(update, CF.MyConnection);

                     CF.MyConnection.Close();

                     break;

                 // get data from Customer - Insert into 30

                     case "Customer":
                     string cust = "Select * from dbo.tblCustomer";
                     DataSet CD = new DataSet();

                     // Instantiate Data Adapter with user query result

                     VarConFDS(known, CD, cust, "UserQuery");

                     CF.MyConnection.Open();

                     foreach (DataRow x in CD.Tables[0].Rows)
                     {

                         // Generate insert query using Row columns  17 columns

                         string q1 = "Insert into tblCustomer (CustomerName, RDFPath, PhysicalLocation)";
                         string q2 = "Values('" + x[1] + "','"+x[2]+"','" + x[3] + "')";

                         string query1 = String.Concat(q1, q2);

                         System.IO.File.AppendAllText(CF.QueryPath, query1);

                         try
                         {

                             SqlCommand cmdnon = new SqlCommand(query1, CF.MyConnection);

                             Console.WriteLine("Executing statement {0}", cmdnon.CommandText);
                             cmdnon.ExecuteNonQuery();

                         }
                         catch (SqlException ex)
                         {
                             Console.WriteLine("Error" + ex.ToString());
                         }

                         // Execute insert query

                     }

                     CF.MyConnection.Close();

                     break;

                 // get data from QueryHistory - Insert into 30

                     case "QueryHistory":
                     string qh = "Select * from dbo.tblQueryHistory";
                     DataSet QD = new DataSet();

                     // Instantiate Data Adapter with user query result

                     VarConFDS(known, QD, qh, "QueryHistory");

                     CF.MyConnection.Open();

                     foreach (DataRow x in QD.Tables[0].Rows)
                     {

                         // Generate insert query using Row columns  17 columns

                         string q1 = "Insert into tblQueryHistory (QueryName, SearchDate, UserID, SearchString, GuidID)";
                         string q2 = "Values('" + x[1] + "','" + x[2] + "','" + x[3] + "','"+x[4]+"','"+x[5]+"')";

                         string query1 = String.Concat(q1, q2);

                         System.IO.File.AppendAllText(CF.QueryPath, query1);

                         try
                         {

                             SqlCommand cmdnon = new SqlCommand(query1, CF.MyConnection);

                             Console.WriteLine("Executing statement {0}", cmdnon.CommandText);
                             cmdnon.ExecuteNonQuery();

                         }
                         catch (SqlException ex)
                         {
                             Console.WriteLine("Error" + ex.ToString());
                         }

                         // Execute insert query

                     }

                     CF.MyConnection.Close();

                     break;


                     case "Graph":
                     string graph = "Select * from dbo.tblGraphSuite";
                     DataSet GD = new DataSet();

                     // Instantiate Data Adapter with user query result

                     VarConFDS(known, GD, graph, "GraphSuite");

                     CF.MyConnection.Open();

                     foreach (DataRow x in GD.Tables[0].Rows)
                     {

                         string q1 = "Insert into tblGraphSuite (GraphSuiteID, UserID, SuiteName, xAxisID, DefaultSearchID, isParamVsParamGraph, Enabled, PublishLevelID, CreateDate)";
                         string q2 = "Values('" + x[1] + "','" + x[2] + "','" + x[3] + "','"+x[4]+"','"+x[5]+"','"+x[6]+"','"+x[7]+"','"+x[8]+"','"+x[9]+"')";

                         string query1 = String.Concat(q1, q2);

                         System.IO.File.AppendAllText(CF.QueryPath, query1);

                         try
                         {

                             SqlCommand cmdnon = new SqlCommand(query1, CF.MyConnection);

                             Console.WriteLine("Executing statement {0}", cmdnon.CommandText);
                             cmdnon.ExecuteNonQuery();

                         }
                         catch (SqlException ex)
                         {
                             Console.WriteLine("Error" + ex.ToString());
                         }

                         // Execute insert query

                     }

                     CF.MyConnection.Close();

                     break;


                     case "CT":
                     string config = "Select * from dbo.tblConfigurationTypes";
                     DataSet CT = new DataSet();

                     // Instantiate Data Adapter with user query result

                     VarConFDS(known, CT, config, "Config");

                     CF.MyConnection.Open();

                     foreach (DataRow x in CT.Tables[0].Rows)
                     {

                         
                         string q1 = "Insert into tblConfigurationTypes (ConfigTypeID, TypeDescription)";
                         string q2 = "Values('" + x[0]+"','"+x[1]+"')";

                         string query1 = String.Concat(q1, q2);

                         System.IO.File.AppendAllText(CF.QueryPath, query1);

                         try
                         {

                             SqlCommand cmdnon = new SqlCommand(query1, CF.MyConnection);

                             Console.WriteLine("Executing statement {0}", cmdnon.CommandText);
                             cmdnon.ExecuteNonQuery();

                         }
                         catch (SqlException ex)
                         {
                             Console.WriteLine("Error" + ex.ToString());
                         }

                         // Execute insert query

                     }

                     CF.MyConnection.Close();

                     break;


                     case "Config":
                     string configuration= "Select * from dbo.tblConfigurations";
                     DataSet Config = new DataSet();

                     // Instantiate Data Adapter with user query result

                     VarConFDS(known, Config, configuration, "Config");

                     CF.MyConnection.Open();

                     foreach (DataRow x in Config.Tables[0].Rows)
                     {
                         
                         string q1 = "Insert into tblConfigurations (PPUCfgid, ModelID, ConfigTypeID)";
                         string q2 = "Values('" + x[1] + "','" + x[2] + "','" + x[3] + "')";

                         string query1 = String.Concat(q1, q2);

                         System.IO.File.AppendAllText(CF.QueryPath, query1);

                         try
                         {

                             SqlCommand cmdnon = new SqlCommand(query1, CF.MyConnection);

                             Console.WriteLine("Executing statement {0}", cmdnon.CommandText);
                             cmdnon.ExecuteNonQuery();

                         }
                         catch (SqlException ex)
                         {
                             Console.WriteLine("Error" + ex.ToString());
                         }

                         // Execute insert query

                     }

                     CF.MyConnection.Close();

                     break;






             }

                      

   


         }
       

   
    }








}
