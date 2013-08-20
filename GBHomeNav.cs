using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Xml;
using WatiN.Core;
using WatiN.Core.Native.InternetExplorer;
using WatiN.Core.Native.Windows;
using NUnit.Framework;
using EntrustFunctionalTest;
using System.Threading;


namespace EntrustSmokeTest
{
   

    //** Tests Run Slow in IE9 and Win7 on NUnit.  The reason is the browser.Close() method.    
    //** These tess should be refactored if upgrading to IE9 
    //**  Possible solutions is launch 1 browser in Setup and use that browser for all tests, performing a Logout after each test
    //**  another option is to use the ca.StopIEExplore() method vice broser.Close();

    [TestFixture]
    public class GBHomeNav
    {
        
        Config CF = new Config();
        GlobalCommonActions ca = new GlobalCommonActions();      
     
        string configpath = "";
        string envconfig = "";
               
        #region Setup
        // Setup for all tests in Test Fixture
        [TestFixtureSetUp]

        public void SmokeTestSetup()
        {

            configpath = Properties.Settings.Default.ConfigPath;
            envconfig = Properties.Settings.Default.EnvConfigPath;
            ca.StopIEExplore();
            Thread.Sleep(2000);

            #region Get XML Values
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(configpath);
            Assert.IsNotNull(xmldoc, "XML Document is Null");

            XmlDocument envdoc = new XmlDocument();
            envdoc.Load(envconfig);
            Assert.IsNotNull(envdoc, "XML Document is Null");



            // Environment Variables
            CF.EnspireDomain = ca.GetXMLNodeInnerText(envdoc, "//env/enspireurl");
            CF.EntrustDomain = ca.GetXMLNodeInnerText(envdoc, "//env/entrusturl");
            CF.UserName = ca.GetXMLNodeInnerText(envdoc, "//env/username");
            CF.Password = ca.GetXMLNodeInnerText(envdoc, "//env/password");
          
            // Core Test Variables
            CF.HomePage = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/homepage");
            CF.AppUser = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/appuser");
            CF.LaborStandards = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/laborstandard");
            CF.WorkSpec = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/workspec");
            CF.ExceptionReason = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/exceptionreason");
            CF.ExceptionReasonCat = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/exreacat");
            CF.EditSearch = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/editsearch");
            CF.ContactSearch = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/contactsearch");
            CF.WarrantyVend = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/warrantyvend");
            CF.IntSys = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/integrationsystem");
            CF.LoadingEstimate = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/loadingestimate");
            CF.Loading = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/loading");
            CF.ManualEntry = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/manualentry");
            CF.Assignment = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/assignment");
            CF.Auditing = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/auditing");
            CF.ManageInv = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/manageinv");
            CF.ExChar = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/exchar");
            CF.OnHold = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/onhold");
            CF.Invoiceable = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/invoiceable");
            CF.ApproveAP = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/approveap");
            CF.ApproveAR = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/approvear");
            CF.Invoiced = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/invoiced");
            CF.CBA = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/cba");
            CF.RPC = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/rpc");
            CF.InvoiceResp = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/invoiceresp");
            CF.FileStore = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/filestore");
            CF.EventTran = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/eventtran");
            CF.Tran = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/tran");
            CF.CacheMgmt = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/cachemgmt");
            CF.Logging = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/logging");
            CF.Broadcast = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/broadcastm");
            CF.OnlineUsers = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/ousers");
            CF.InvoiceSearch = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/invoicesearch");
            CF.Reports = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/reports");
            CF.InReports = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/inreports");
            CF.About = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/about");
            CF.GBRXHome = ca.GetXMLNodeInnerText(xmldoc, "//smoketest/gbrxhome");



            #endregion

        }
        #endregion

        #region TearDown
        [TearDown]
        public void TearDown()
        {
            ca.StopIEExplore();
            Thread.Sleep(2000);

        }

        #endregion

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Home Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void HomeGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.HomeSelect(newbrowser, "Home");

            ca.NavToGBHome(newbrowser);

            newbrowser.WaitForComplete();

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            ca.StopIEExplore();
            //newbrowser.Close();


        }

        #region Admin
        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Admin Application Users Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>      
        [TestCase("IE")]
        [Test]
        public void AdminAUGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();
            ca.MenuSelect(newbrowser, "Admin", "Application Users");
           

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();


        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Admin Labor Standards Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void AdminLSGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();
            ca.MenuSelect(newbrowser, "Admin", "Labor Standards");  
           

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Admin Work Specifications Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void AdminWSGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Admin", "Work Specifications");        

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Admin Exception Reasons Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void AdminERGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Admin", "Exception Reasons");
           
            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the Admin Exception Reason Category Home Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void AdminERCGBhome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Admin", "Exception Reason Categories");
          
            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Admin Edits Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void AdminEditsNavGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();                     

            ca.MenuSelect(newbrowser, "Admin", "Edits");
            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Admin Contacts Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void AdminContactsNavToGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Admin", "Contacts");
          
            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Admin Warranty Vendors Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]       
        public void AdminWVNavToGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();
           
            ca.MenuSelect(newbrowser, "Admin", "Warranty Vendors");

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Admin Integration System Errors Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]       
        public void AdminISNavToGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Admin", "Integration System Errors");           

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }


    #endregion

        #region Loading

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Loading Estimate Loading Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void EstimateLoadingGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Loading", "Estimate Loading");         

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();


        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Loading Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void LoadingGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Loading", "Loading");
           
            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();


        }



        #endregion

        #region Manual Entry
        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Manual Entry Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void ManualEntryGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Manual Entry", "Manual Entry");         

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();


        }
        #endregion

        #region Assignment

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Assignment Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void AssignmentGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Assignment", "Assignment");           

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();


        }
        #endregion

        #region Manage Inv

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Manage Inv Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void ManageInvGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Manage Inv", "Manage Inv");
            

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        #endregion

        #region Auditing

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Auditing Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
         [TestCase("IE")]
         [Test]
         public void AuditingGBHome(string browser)
         {

             Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

             newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

             newbrowser.WaitUntilContainsText("Log In");

             ca.Login(newbrowser, CF.UserName, CF.Password);

             newbrowser.WaitForComplete();

             ca.MenuSelect(newbrowser, "Auditing", "Auditing");
             
             ca.NavToGBHome(newbrowser);

             Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

             newbrowser.Close();


         }
         #endregion

        #region AP/AR
         /// <summary>
         /// Test to Navigate to GBRX Home Page from the GMS AP/AR Extra Charges Page
         /// </summary>
         /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
         [TestCase("IE")]
         [Test]
         public void ExtraChargesGBHome(string browser)
         {

             Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

             newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

             newbrowser.WaitUntilContainsText("Log In");

             ca.Login(newbrowser, CF.UserName, CF.Password);

             newbrowser.WaitForComplete();

             ca.MenuSelect(newbrowser, "AP/AR", "Extra Charges");
            
             ca.NavToGBHome(newbrowser);

             Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

             newbrowser.Close();

         }

         /// <summary>
         /// Test to Navigate to GBRX Home Page from the GMS AP/AR On Hold Page
         /// </summary>
         /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
         [TestCase("IE")]
         [Test]
         public void OnHoldGBHome(string browser)
         {

             Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

             newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

             newbrowser.WaitUntilContainsText("Log In");

             ca.Login(newbrowser, CF.UserName, CF.Password);

             newbrowser.WaitForComplete();

             ca.MenuSelect(newbrowser, "AP/AR", "On Hold");           

             ca.NavToGBHome(newbrowser);

             Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

             newbrowser.Close();


         }

         /// <summary>
         /// Test to Navigate to GBRX Home Page from the GMS AP/AR On Hold Page
         /// </summary>
         /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
         [TestCase("IE")]
         [Test]
         public void InvoiceableGBHome(string browser)
         {

             Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

             newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

             newbrowser.WaitUntilContainsText("Log In");

             ca.Login(newbrowser, CF.UserName, CF.Password);

             newbrowser.WaitForComplete();

             ca.MenuSelect(newbrowser, "AP/AR", "Invoiceable");             

             ca.NavToGBHome(newbrowser);

             Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

             newbrowser.Close();

         }

         /// <summary>
         /// Test to Navigate to GBRX Home Page from the GMS AP/AR Approve AP Page
         /// </summary>
         /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
         [TestCase("IE")]
         [Test]
         public void ApproveAPGBHome(string browser)
         {

             Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

             newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

             newbrowser.WaitUntilContainsText("Log In");

             ca.Login(newbrowser, CF.UserName, CF.Password);

             newbrowser.WaitForComplete();

             ca.MenuSelect(newbrowser, "AP/AR", "Approve AP");           

             ca.NavToGBHome(newbrowser);

             Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "Approve AP Page Not Displayed");

             newbrowser.Close();

         }

         /// <summary>
         /// Test to Navigate to GBRX Home Page from the GMS AP/AR Approve AR Page
         /// </summary>
         /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
         [TestCase("IE")]
         [Test]
         public void ApproveARGBHome(string browser)
         {

             Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

             newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

             newbrowser.WaitUntilContainsText("Log In");

             ca.Login(newbrowser, CF.UserName, CF.Password);

             newbrowser.WaitForComplete();

             ca.MenuSelect(newbrowser, "AP/AR", "Approve AR");           

             ca.NavToGBHome(newbrowser);

             Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

             newbrowser.Close();

         }

         /// <summary>
         /// Test to Navigate to GBRX Home Page from the GMS AP/AR Invoiced Page
         /// </summary>
         /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
         [TestCase("IE")]
         [Test]
         public void InvoicedGBRXHome(string browser)
         {

             Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

             newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

             newbrowser.WaitUntilContainsText("Log In");

             ca.Login(newbrowser, CF.UserName, CF.Password);

             newbrowser.WaitForComplete();

             ca.MenuSelect(newbrowser, "AP/AR", "Invoiced");            

             ca.NavToGBHome(newbrowser);

             Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

             newbrowser.Close();


         }

       

        #endregion

        #region CBA


         /// <summary>
         /// Test to Navigate to GBRX Home Page from the GMS CBA Page
         /// </summary>
         /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void CBAGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "CBA", "CBA");

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        #endregion

        #region InvoiceResponses
        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Invoice Responses Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void InvoiceResponsesGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Invoice Responses", "Invoice Responses");          

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();
        }
        #endregion

        #region Reports

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Reports Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void ReportsGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Reports", "Reports");            

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "Reports Page Not Displayed");

            newbrowser.Close();

        }
        #endregion

        #region Reprocess Cars

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Reprocess Cars Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void ReprocessCarsGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Re-Process Cars", "Re-Process Cars");
                       
            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        #endregion

        #region Help

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Help Industry Reports Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void IndustryReportsGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Help", "Industry Reports");

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Help About Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
         public void AboutGBHome(string browser)
         {

             Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

             newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

             newbrowser.WaitUntilContainsText("Log In");

             ca.Login(newbrowser, CF.UserName, CF.Password);

             newbrowser.WaitForComplete();

             ca.MenuSelect(newbrowser, "Help", "About");
            

             ca.NavToGBHome(newbrowser);
                        
            
           //  Thread.Sleep(2000);             

             Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "Industry Reports Page Not Displayed");

             
             newbrowser.Close();

         }
         #endregion

        #region Support

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Support File Store Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void FileStoreGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Support", "File Store");           

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Support Event Transaction Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void EventTransactionGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Support", "Event & Transactions");
          
            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Support Transaction Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void TransactionGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Support", "Transactions");           

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Support Cache Management Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void CacheManagementGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();
            
            ca.MenuSelect(newbrowser, "Support", "Cache Management");          

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Support Logging Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void LoggingGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();
            ca.MenuSelect(newbrowser, "Support", "Logging");          

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Support Broadcast Messages Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void BroadcastMessagesGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Support", "Broadcast Messages");           

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Support Online Users Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void OnlineUsersGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Support", "Online Users");

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }

        /// <summary>
        /// Test to Navigate to GBRX Home Page from the GMS Support Invoice Search & Rollback Page
        /// </summary>
        /// <param name="browser">"Type of browser. For example "IE" for internet explorer</param>
        [TestCase("IE")]
        [Test]
        public void InvoiceSearchRollBackGBHome(string browser)
        {

            Browser newbrowser = ca.GetBrowser(browser, CF.EnspireDomain);

            newbrowser.ShowWindow(NativeMethods.WindowShowStyle.ShowMaximized);

            newbrowser.WaitUntilContainsText("Log In");

            ca.Login(newbrowser, CF.UserName, CF.Password);

            newbrowser.WaitForComplete();

            ca.MenuSelect(newbrowser, "Support", "Invoice Search & Rollback");           

            ca.NavToGBHome(newbrowser);

            Assert.AreEqual(CF.GBRXHome, newbrowser.Url, "GBRX Home Page Not Displayed");

            newbrowser.Close();

        }


        #endregion
    }
    }

