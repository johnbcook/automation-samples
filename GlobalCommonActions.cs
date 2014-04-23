using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

using NUnit.Framework;
using System.Threading;
using System.Xml;
using System.Web;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using mshtml;




namespace EntrustFunctionalTest
{

    /// <summary>
    /// Class containing Global Common actions
    /// </summary>
    public class GlobalCommonActions
    {

        #region MouseEvents

        // Use user32 api to simulate user actions

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo);

        // System drawing point leveraged to get coordinates
        // using System.Drawing   and mshtml
        /// <summary>
        /// Method to get Grid Coordinates of an element
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public System.Drawing.Point GetScreenPoint(IEElement element)
        {           
            IHTMLElement nativeElement = element.AsHtmlElement;
            IHTMLElement2 offsetElement = (IHTMLElement2)nativeElement;
            IHTMLRect clientRect = offsetElement.getBoundingClientRect();
            IHTMLDocument2 doc = (IHTMLDocument2)nativeElement.document;
            IHTMLWindow3 window = (IHTMLWindow3)doc.parentWindow;

            int windowLeft = window.screenLeft;
            int windowTop = window.screenTop;
            int elementLeft = clientRect.left;
            int elementTop = clientRect.top;
            int width = nativeElement.offsetWidth;
            int height = nativeElement.offsetHeight;

            int clickX = windowLeft + elementLeft + (width / 2);
            int clickY = windowTop + elementTop + (height / 2);

            return new System.Drawing.Point(clickX, clickY);
        }

        // Mouse Event Flags  
#pragma warning disable 1591
        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010,

        }
#pragma warning restore 1591

       
        /// <summary>
        /// Method to perform a Mouse Left Click
        /// Usage Example: Element me = browser.Element(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_LanguageDropDown_B-1Img")));
        ///    IEElement newele = (IEElement)me.NativeElement;
       ///     System.Drawing.Point clickPoint = gc.GetScreenPoint(newele);
       ///     gc.Move(clickPoint);
       ///     gc.LeftClick(clickPoint);
        /// </summary>
        /// <param name="clickpoint"></param>
        public void LeftClick(System.Drawing.Point clickpoint)
        {            

            // Cursor.Position = new System.Drawing.Point(x, y);
            Cursor.Position = clickpoint;
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);


        }
        /// <summary>
        /// Method to move the mouse cursor to a target Grid Coordinate
        /// </summary>
        /// <param name="clickpoint"></param>
        public void Move(System.Drawing.Point clickpoint)
        {
           
            // Cursor.Position = new System.Drawing.Point(x, y);
            Cursor.Position = clickpoint;
            mouse_event((int)(MouseEventFlags.MOVE), 0, 0, 0, 0);
        }
      
        #endregion

        #region Login/Logout
        /// <summary>
        /// Method to Login to the Entrust Application
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void Login(Browser browser, string username, string password)
        {

            Element usernamefield = browser.Element(Find.ById(new Regex("username")));
            System.Drawing.Point clickPoint = GetScreenPoint((IEElement)usernamefield.NativeElement);
            Move(clickPoint);

           browser.TextField(Find.ById(new Regex("username"))).SetAttributeValue("value", username);
           browser.TextField(Find.ById(new Regex("password"))).SetAttributeValue("value", password);
           browser.Button(Find.ByValue(new Regex("Log In"))).ClickNoWait();
           browser.WaitForComplete();           
                
        }

        /// <summary>
        /// Method to Logout
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        public void Logout(Browser browser)
        {

            if (browser.Link(Find.ByText("Logout")).Exists)
            {
                browser.Link(Find.ByText("Logout")).Flash();
                browser.Link(Find.ByText("Logout")).Click();
            }
            else
            {
                Console.Write("Log Out Not Available Initiating Recovery");
                browser.Back();
                browser.Link(Find.ByText("Logout")).Flash();
                browser.Link(Find.ByText("Logout")).Click();
            }

        }

        #endregion

        #region Get Browser
        /// <summary>
        /// Method to Get a Browser Object 
        /// Conversts IE or FireFox object to Browser Object
        /// </summary>
        /// <param name="browser">IE or FireFox</param>
        /// <param name="url"></param>
        /// <returns>WatiN Browser Object</returns>
        public Browser GetBrowser(string browser, string url)
        {

            switch (browser)
            {

                case "IE":
                Browser ie = new IE(url);
                return ie;             

                case "FireFox":
                Browser ff = new FireFox(url);
                return ff;

                default:
                throw new ArgumentOutOfRangeException();

            }


        }

        #endregion

        #region Menu Select      

        /// <summary>
        /// Method to Select a Menu and Submenu by Text.
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="menu"></param>
        /// <param name="selection"></param>
        public void MenuSelect(Browser browser, string menu, string selection)
        {

            Element loading = browser.Element(Find.ByText(new Regex(menu)));
            System.Drawing.Point clickPoint = GetScreenPoint((IEElement)loading.NativeElement);
            Move(clickPoint);
            try
            {
                browser.WaitUntilContainsText(selection, 5);
            }
            catch
            {
                return;
            }
            // Get Parent of Selection
            Element test = browser.Link(Find.ByText(selection)).Parent;
            test.Click();

        }

        #endregion

        #region ClearCache
        /// <summary>
        /// Method to clear the cache of a browser
        /// will clear cache of IE or FireFox browser
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="browsertype">IE or FireFox</param>
        public void ClearCache(Browser browser, string browsertype)
        {

            switch (browsertype)
            {

                case "IE":

                    IE ie = (IE)browser;

                    ie.ClearCache();
                    ie.ClearCookies();
                break;

                case "FireFox":

                FireFox ff = (FireFox)browser;

                Console.Write("Unsure how to clear cache in FireFox");

                ff.Refresh();

               
                break;



            }

        }

        #endregion

        #region Home

        /// <summary>
        /// Method to Select the Home Menu Item
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="selection"></param>
        public void HomeSelect(Browser browser, string selection)
        {
            // Click Home menu item
            browser.Element(Find.ByText(new Regex("Home"))).Click();            
        }

        #endregion
     
        #region  Get XML Node
        /// <summary>
        /// Method searches an XML doc for xpath and returns InnerText of XMLNode
        /// </summary>
        /// <param name="xmldoc"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public string GetXMLNodeInnerText(XmlDocument xmldoc, string xpath)
        {

            XmlNode newnode = xmldoc.SelectSingleNode(xpath);
            string text = newnode.InnerText;
            return text;


        }

        #endregion

        /// <summary>
        /// Method to Get the Role of a User from the Config Doc
        /// </summary>
        /// <param name="ConfigDoc">Config File</param>
        /// <param name="rolename">Name of Role</param>
        /// <returns></returns>
        public string[] GetRoleUser(XmlDocument ConfigDoc, string rolename)
        {
            XmlNode xn = ConfigDoc.SelectSingleNode("//role[@name = '"+rolename+"']");           
            string username = xn["username"].InnerText;
            string pw = xn["password"].InnerText;
            string name = xn["name"].InnerText;
            string moduser = xn["moduser"].InnerText;
            string[] results = { username, pw, name, moduser };
            return results;
        }
        /// <summary>
        /// Method to retrieve the Epicor credentials
        /// </summary>
        /// <param name="ConfigDoc">Config File</param>
        /// <param name="rolename">Name of Roel</param>
        /// <returns></returns>
        public string[] GetEpicorCred(XmlDocument ConfigDoc, string rolename)
        {
            XmlNode xn = ConfigDoc.SelectSingleNode("//role[@name = '" + rolename + "']");
            string username = xn["eusername"].InnerText;
            string pw = xn["epassword"].InnerText;            
            string[] results = { username, pw};
            return results;
        }

        /// <summary>
        /// Method to get the Menu Nodes from the Config Doc
        /// </summary>
        /// <param name="ConfigDoc">Config File</param>
        /// <param name="rolename">Name of Role</param>
        /// <returns>XMLNodeList</returns>
        public XmlNodeList GetMenuNodes(XmlDocument ConfigDoc, string rolename)
        {
            XmlNodeList xl = ConfigDoc.SelectSingleNode("//role[@name = '" + rolename + "']/expectedmenus").ChildNodes;
            return xl;
        }

        /// <summary>
        /// Method to Retrieve Submenus of a Menu item from Config File
        /// </summary>
        /// <param name="xl"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        public List<String> GetExpectedSubMenus(XmlNodeList xl, string menu)
        {
            List<String> results = new List<String>();       
         
            foreach (XmlNode xn in xl)
            {                               
                string menuname = xn.Attributes["name"].Value; 
                if(menu.Equals(menuname))
                {

                    XmlNodeList newlist = xn.ChildNodes;
                    foreach (XmlNode node in newlist)
                    {
                        results.Add(node.InnerText);
                    }

                }              
            }

            return results;
        }

        /// <summary>
        /// Method to retrieve all submenus from a visible menu
        /// </summary>
        /// <param name="browser"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        public List<String> GetActualSubMenus(Browser browser, string menu)
        {
            List<String> results = new List<String>();
            try
            {
                string menuid = browser.Element(Find.ByText(menu)).Id;
                int start = menuid.Length - 2;
                string searchid = menuid.Remove(start, 2);

                ElementCollection submenus = browser.Elements.Filter(Find.ById(new Regex(searchid))).Filter(Find.ByClass("dxmSubMenuItem"));

                foreach (Element x in submenus)
                {
                    // Console.Write(x.Text + Environment.NewLine);
                    results.Add(x.Text);

                }

                return results;
            }
            catch(Exception ex)
            {
                Console.Write("Get Actual Sub Menus Failed with Exception: " + ex.Message);
                return results;


            }
        }

        /// <summary>
        /// Method to retrieve all expected Menus for a specific Role
        /// Returns a list of strings containing all menus expected
        /// </summary>
        /// <param name="ConfigDoc"></param>
        /// <param name="rolename"></param>
        /// <returns></returns>
        public List<String> GetExpectedMenus(XmlDocument ConfigDoc, string rolename)
        {
            XmlNodeList xl = ConfigDoc.SelectSingleNode("//role[@name = '" + rolename + "']/expectedmenus").ChildNodes; 
            
            List<String> results = new List<String>();
            foreach(XmlNode xn in xl)
            {                
                XmlAttributeCollection test = xn.Attributes;
                foreach (XmlAttribute x in test)
                {
                    results.Add(x.Value);
                }                                
            }
          return results;
        }

        /// <summary>
        /// Method gets a count of all visible menus
        /// </summary>
        /// <param name="browser"></param>
        /// <returns></returns>
        public int GetMenuItemCount(Browser browser)
        {
            ElementCollection ec = browser.Elements.Filter(Find.ByClass("dxmMenuItem"));
            ElementCollection ec1 = browser.Elements.Filter(Find.ByClass("dxmMenuItem dxmMenuItemSelected"));
            int result = ec.Count + ec1.Count;
            return result;
        }

        /// <summary>
       /// Method to retrieve visible Menus from Browser
       /// </summary>
       /// <param name="browser"></param>
       /// <returns></returns>
        public List<String> GetMenus(Browser browser)
        {
            List<String> result = new List<String>();
            ElementCollection ec = browser.Elements.Filter(Find.ByClass("dxmMenuItem"));
            foreach (Element menu in ec)
            {
                result.Add(menu.Text);
            }

            ElementCollection ec1 = browser.Elements.Filter(Find.ByClass("dxmMenuItem dxmMenuItemSelected"));

            foreach (Element menu in ec1)
            {
                result.Add(menu.Text);
            }

            
            return result;
        }

        /// <summary>
        /// Method to Navigate to the Greenbrier Home Page
        /// </summary>
        /// <param name="browser"></param>
        public void NavToGBHome(Browser browser)
        {
            browser.Link(Find.ByText(new Regex("The Greenbrier Companies"))).Click();
            browser.WaitForComplete();    

        }

        /// <summary>
        /// Method to stop all instances of IE Explorer on a Server
        /// </summary>
        public void StopIEExplore()
        {
            string strCommandParameters = "/F /IM iexplore.exe /T";                       

           System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(@"C:/Windows/System32/taskkill.exe");
           psi.Arguments = strCommandParameters;
           psi.RedirectStandardOutput = true;          

           psi.UseShellExecute = false;
           // Hide Window
           psi.CreateNoWindow = true;
           psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden; 

           System.Diagnostics.Process proc = new System.Diagnostics.Process();
           proc.StartInfo = psi;
           proc.Start();                   
           
          // System.Diagnostics.Process.Start(@"C:/VPUSIM/extest.bat");
                    
     

        }

        /// <summary>
        /// Method to Get a New Browser Popup
        /// Attaches Browser PopUp and returns as a new browser. 
        /// Use Title or URL
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="criteria"></param>
        /// <param name="searchstring"></param>
        /// <returns>WatiN Browser Object</returns>
        public Browser AttachBrowser(Browser browser, string criteria, string searchstring)
        {            
            switch(criteria)
            {           
            case "Title":
            Browser titlebrowser = Browser.AttachTo(browser.GetType(), Find.ByTitle(searchstring));
            return titlebrowser;
            case "URL":
            Browser urlbrowser = Browser.AttachTo(browser.GetType(), Find.ByTitle(searchstring));
            return urlbrowser;
            default:
            throw new ArgumentOutOfRangeException();   
            }
        }

        /// <summary>
        /// Method to Get a Process by Name
        /// </summary>
        /// <param name="processname"></param>
        /// <returns>Process Object</returns>
        public Process GetProcessByName(string processname)
        {

            Process[] aProc = Process.GetProcessesByName(processname);
            if (aProc.Length > 0)
            {
                return aProc[0];
            }
            else
            {
                return null;

            }

        }
       
        /// <summary>
        /// Method to Kill a Process
        /// </summary>
        /// <param name="processname"></param>
        public void KillProcess(string processname)
        {

            Process myprc =  GetProcessByName(processname);
                
            myprc.Kill();

        }

        /// <summary>
        /// Method to Reset Personalization Data
        /// This lives in Help->About 
        /// May want to create a separate Help common action class
        /// </summary>
        /// <param name="browser"></param>
        public void ResetPersonalizationData(Browser browser)
        {
            browser.Button(Find.ById("ctl00_MainContent_RestLayoutButton")).Click();
        }

        /// <summary>
        /// Method to handle the IE9 File Download Notification Bar
        /// Uses UI Automation
        /// May want to add this as a dialog handler inheriting BaseDialogHandler
        /// </summary>
        /// <param name="name"></param>
        public void IE9FileDownload(string name)
        {
            UIAutomationClass uac = new UIAutomationClass();
            System.Windows.Automation.AutomationElementCollection test = uac.GetDeskTopChildren();
            foreach (System.Windows.Automation.AutomationElement x in test)
            {
                if (x.Current.ClassName.Equals("IEFrame"))
                {
                    // Console.Write(x.Current.ClassName + " " + x.Current.ControlType + " " + x.Current.AutomationId + " " + x.Current.ItemType + " " + x.Current.Name + Environment.NewLine);
                    System.Windows.Automation.AutomationElementCollection y = x.FindAll(System.Windows.Automation.TreeScope.Children, System.Windows.Automation.Condition.TrueCondition);
                    foreach (System.Windows.Automation.AutomationElement z in y)
                    {
                        // Console.Write(x.Current.ClassName + " " + z.Current.ClassName + " " + z.Current.ControlType + " " + z.Current.AutomationId + " " + z.Current.ItemType + " " + z.Current.Name + Environment.NewLine);
                        if (z.Current.ClassName.Equals("Frame Notification Bar"))
                        {
                            System.Windows.Automation.AutomationElementCollection g = x.FindAll(System.Windows.Automation.TreeScope.Children, System.Windows.Automation.Condition.TrueCondition);
                            foreach (System.Windows.Automation.AutomationElement bn in g)
                            {
                                // Console.Write("First Child" + "Class Name: " + bn.Current.ClassName + " " + "Control Type: " + bn.Current.ControlType + " " + "Automation ID: " + bn.Current.AutomationId + " " + "Item Type: " + bn.Current.ItemType + " " + "Name: " + bn.Current.Name + Environment.NewLine);
                                System.Windows.Automation.AutomationElementCollection l = bn.FindAll(System.Windows.Automation.TreeScope.Children, System.Windows.Automation.Condition.TrueCondition);
                                foreach (System.Windows.Automation.AutomationElement zn in l)
                                {
                                    //   Console.Write("2nd Child: " + "Class Name: " + zn.Current.ClassName + "Control Type: " +  zn.Current.ControlType + " Automation ID " + zn.Current.AutomationId + "Item type " + zn.Current.ItemType + " Name " + zn.Current.Name + Environment.NewLine);
                                    if (zn.Current.Name.Equals("Notification bar"))
                                    {
                                        System.Windows.Automation.AutomationElementCollection lg = zn.FindAll(System.Windows.Automation.TreeScope.Children, System.Windows.Automation.Condition.TrueCondition);
                                        foreach (System.Windows.Automation.AutomationElement xx in lg)
                                        {
                                            // Console.Write("3rd Child: " + "Class Name: " + xx.Current.ClassName + "Control Type: " + xx.Current.ControlType + " Automation ID " + xx.Current.AutomationId + "Item type " + xx.Current.ItemType + " Name " + xx.Current.Name + Environment.NewLine);
                                            if (xx.Current.Name.Equals(name))
                                            {
                                                System.Windows.Automation.AutomationPattern[] pats = xx.GetSupportedPatterns();
                                                foreach (System.Windows.Automation.AutomationPattern pat in pats)
                                                {
                                                    // Console.Write("Pattern ID: " + pat.Id + "Name:" + pat.ProgrammaticName);
                                                    // '10000' button click event id 
                                                    if (pat.Id == 10000)
                                                    {
                                                        System.Windows.Automation.InvokePattern click = (System.Windows.Automation.InvokePattern)xx.GetCurrentPattern(pat);
                                                        click.Invoke();
                                                    }
                                                }
                                                return;
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }

        }

      



    }



      
}
