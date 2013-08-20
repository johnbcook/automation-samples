using System;
using System.Data.SqlClient;
using System.Xml;
using NUnit.Framework;

namespace EntrustFunctionalTest
{
    public class BaseTest
    {

        public Config CF = new Config();
        public GlobalCommonActions ca = new GlobalCommonActions();

        public AssignmentCommon ac = new AssignmentCommon();
        public AuditCommon auc = new AuditCommon();
        public FullAuditCommon fc = new FullAuditCommon();
        public InvoiceCommon ic = new InvoiceCommon();
        public ManageInvoiceCommon mc = new ManageInvoiceCommon();
        public QuickAuditCommon qc = new QuickAuditCommon();
        public ApproveAPCommon aap = new ApproveAPCommon();
        public ApproveARCommon aar = new ApproveARCommon();
        public InvoiceableCommon ivc = new InvoiceableCommon();
        public InvoicedCommon ivd = new InvoicedCommon();
        public InvoiceResponseCommon ir = new InvoiceResponseCommon();
        public CBACommon cba = new CBACommon();
        public ReviewInvoiceCommon ri = new ReviewInvoiceCommon();
        public InvoiceSummaryCommon invsum = new InvoiceSummaryCommon();
        public ContactsCommon con = new ContactsCommon();
        public TestWrapperMethods twm = new TestWrapperMethods();       
        public ManualEntryCommon mec = new ManualEntryCommon();
        
      

        public string configpath = "";
        public string envconfig = "";

        #region Test Fixture Setup
        [TestFixtureSetUp]
        public void SmokeTestSetup()
        {
            configpath = Properties.Settings.Default.ConfigPath;
            envconfig = Properties.Settings.Default.EnvConfigPath;

            #region Get XML Values
            CF.ConfigDoc.Load(configpath);
            Assert.IsNotNull(CF.ConfigDoc, "XML Document is Null");

            XmlDocument envdoc = new XmlDocument();
            envdoc.Load(envconfig);
            Assert.IsNotNull(envdoc, "XML Document is Null");

            // Environment Variables
            CF.EnspireDomain = ca.GetXMLNodeInnerText(envdoc, "//env/enspireurl");
            CF.EntrustDomain = ca.GetXMLNodeInnerText(envdoc, "//env/entrusturl");
            CF.UserName = ca.GetXMLNodeInnerText(envdoc, "//env/username");
            CF.Password = ca.GetXMLNodeInnerText(envdoc, "//env/password");
            CF.Server = ca.GetXMLNodeInnerText(envdoc, "//env/database/server");
            CF.DBName = ca.GetXMLNodeInnerText(envdoc, "//env/database/database");
            CF.DBUserName = ca.GetXMLNodeInnerText(envdoc, "//env/database/username");
            CF.DBPassword = ca.GetXMLNodeInnerText(envdoc, "//env/database/password");
            CF.MyConnection = new SqlConnection("server=" + CF.Server + ";database=" + CF.DBName + ";user id=" + CF.DBUserName + ";password=" + CF.DBPassword + ";");
            CF.BrowserVersion = ca.GetXMLNodeInnerText(envdoc, "//env/browser");

            CF.HomePage = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/homepage");
            CF.AppUser = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/appuser");
            CF.LaborStandards = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/laborstandard");
            CF.WorkSpec = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/workspec");
            CF.ExceptionReason = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/exceptionreason");
            CF.ExceptionReasonCat = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/exreacat");
            CF.EditSearch = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/editsearch");
            CF.ContactSearch = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/contactsearch");
            CF.WarrantyVend = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/warrantyvend");
            CF.IntSys = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/integrationsystem");
            CF.LoadingEstimate = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/loadingestimate");
            CF.Loading = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/loading");
            CF.ManualEntry = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/manualentry");
            CF.Assignment = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/assignment");
            CF.Auditing = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/auditing");
            CF.ManageInv = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/manageinv");
            CF.ExChar = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/exchar");
            CF.OnHold = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/onhold");
            CF.Invoiceable = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/invoiceable");
            CF.ApproveAP = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/approveap");
            CF.ApproveAR = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/approvear");
            CF.Invoiced = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/invoiced");
            CF.CBA = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/cba");
            CF.RPC = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/rpc");
            CF.InvoiceResp = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/invoiceresp");
            CF.FileStore = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/filestore");
            CF.EventTran = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/eventtran");
            CF.Tran = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/tran");
            CF.CacheMgmt = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/cachemgmt");
            CF.Logging = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/logging");
            CF.Broadcast = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/broadcastm");
            CF.OnlineUsers = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/ousers");
            CF.InvoiceSearch = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/invoicesearch");
            CF.Reports = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/reports");
            CF.InReports = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/inreports");
            CF.About = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/about");
            CF.GBRXHome = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/gbrxhome");
            CF.ErrorLog = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//log/errorlogpath");
            CF.CBAInvoice = ca.GetXMLNodeInnerText(CF.ConfigDoc, "//smoketest/cbainvoice");

            #endregion
        }
        #endregion

        #region Test Case Setup
        [SetUp]
        public void TCSetUp()
        {

            CF.FailMessage = "";
            CF.Fail = 0;
            CF.Passed = true;
            ca.StopIEExplore();
        }

      
        #endregion
    }
}
