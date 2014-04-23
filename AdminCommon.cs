using System;
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
    /// Class contains the Common Actions for the Admin Functionality
    /// </summary>
    public class AdminCommon
    {
        GlobalCommonActions gc = new GlobalCommonActions();

        #region Admin User
        /// <summary>
        /// Method performs a Search for a User on the Admin -> Search User page. 
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="firstname">First Name Value</param>
        /// <param name="lastname">Last Name Value</param>
        /// <param name="username">User Name Value</param>
        /// <param name="includeinactive">Will set Include Inactive Checked value</param>
        public void SearchUser(Browser browser, string firstname, string lastname, string username, bool includeinactive)

        {
            try
            {
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_FirstNameTextBox"))).SetAttributeValue("value", firstname);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_LastNameTextBox"))).SetAttributeValue("value", lastname);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_UserCodeTextBox"))).TypeText(username);
                browser.CheckBox(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_IncludeInactiveCheckBox"))).Checked = includeinactive;
                browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_Button1"))).Click();

                browser.Refresh();
            }
            catch
            {

                browser.Close();

            }

            
        }
       

        /// <summary>
        /// Method Creates a New User on the Admin -> New User page. 
        /// Will Save if save == true else will flash the Save button.
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="firstname">First Name Value</param>
        /// <param name="mi">Middle Initial Value</param>
        /// <param name="lastname">Last Name Value</param>
        /// <param name="username">User Name Value</param>
        /// <param name="language">Language Value</param>
        /// <param name="approvallimit">Approval Limit Value</param>
        /// <param name="efrom">Effective From Value</param>
        /// <param name="eto">Effective To Value</param>
        /// <param name="isinternal">Sets Is Internal Checkbox to value</param>
        /// <param name="role">Role Value</param>
        /// <param name="password">Password Value</param>
        /// <param name="save">Save flag  true will save false will flash</param>
        public void NewUserSave(Browser browser, string firstname, string mi, string lastname,
                            string username, string language, string approvallimit,
                            string efrom, string eto, bool isinternal, string role, string password, bool save)
        {


            // TODO:  Make these separate methods.
            browser.Button(Find.ById(new Regex("ctl00_MainContent_NewUserButton"))).Click();
            browser.WaitUntilContainsText("Add New User");

            // Enter Names
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_FirstNameTextBox_I"))).SetAttributeValue("value", firstname);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_MiddleInitialTextBox_I"))).SetAttributeValue("value", mi);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_LastNameTextBox_I"))).SetAttributeValue("value", lastname);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_UserCodeTextBox_I"))).SetAttributeValue("value", username);

            if (!language.Equals(""))
            {

                SelectLanguage(browser, language);

            }

            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_ApprovalLimitTextBox_I"))).SetAttributeValue("value", approvallimit);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_EffectiveFromDateEdit_I"))).FireEventNoWait("onkeydown");
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_EffectiveFromDateEdit_I"))).TypeText(efrom);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_EffectiveFromDateEdit_I"))).FireEventNoWait("onkeydown");
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_EffectiveThruDateEdit_I"))).TypeText(eto);
            browser.CheckBox(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_InternalCheckBox_I"))).Checked = isinternal;

            browser.WaitForComplete();

            if (isinternal == false)
            {
                               
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_UserPasswordTextBox_I"))).SetAttributeValue("value", password);


            }

            browser.CheckBox(Find.ByLabelText(role)).Checked = true;

            // TODO  Add Save Button

            if (save == true)
            {

                browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_SaveButton"))).Click();
            }
            else
            {

                browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_SaveButton"))).Flash();

            }

        }

        /// <summary>
        /// Method fills out the Admin -> New User form then Cancels the action.
        /// Will Cancel if cancel == true  If Cancel == false will just flash the Cancel button.
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="firstname">First Name Value</param>
        /// <param name="mi">Middle Initial Value</param>
        /// <param name="lastname">Last Name Value</param>
        /// <param name="username">User Name Value</param>
        /// <param name="language">Language Value</param>
        /// <param name="approvallimit">Approval Limit Value</param>
        /// <param name="efrom">Effective From Value</param>
        /// <param name="eto">Effective To Value</param>
        /// <param name="isinternal">Is Internal Value</param>
        /// <param name="role">Role Value</param>
        /// <param name="password">Password Value</param>
        /// <param name="cancel">Cancel Value  true will cancel false will flash</param>
        public void NewUserCancel(Browser browser, string firstname, string mi, string lastname,
                            string username, string language, string approvallimit,
                            string efrom, string eto, bool isinternal, string role, string password, bool cancel)
        {


            // TODO:  Make these separate methods.
            browser.Button(Find.ById(new Regex("ctl00_MainContent_NewUserButton"))).Click();
            browser.WaitUntilContainsText("Add New User");

            // Enter Names
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_FirstNameTextBox_I"))).SetAttributeValue("value", firstname);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_MiddleInitialTextBox_I"))).SetAttributeValue("value", mi);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_LastNameTextBox_I"))).SetAttributeValue("value", lastname);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_UserCodeTextBox_I"))).SetAttributeValue("value", username);

            if (!language.Equals(""))
            {

                SelectLanguage(browser, language);

            }

            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_ApprovalLimitTextBox_I"))).SetAttributeValue("value", approvallimit);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_EffectiveFromDateEdit_I"))).FireEventNoWait("onkeydown");
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_EffectiveFromDateEdit_I"))).TypeText(efrom);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_EffectiveThruDateEdit_I"))).TypeText(eto);
            browser.CheckBox(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_InternalCheckBox_I"))).Checked = isinternal;
            browser.CheckBox(Find.ByLabelText(role)).Checked = true;

            if (isinternal == false)
            {

                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_EffectiveFromDateEdit_I"))).SetAttributeValue("value", efrom);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_UserPasswordTextBox_I"))).SetAttributeValue("value", password);


            }

          

            if (cancel == true)
            {

                browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_CancelButton"))).Click();
            }
            else
            {

                browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_CancelButton"))).Flash();

            }

        }
        
        /// <summary>
        /// Method that Clicks the Cancel Button on the New User page
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        public void CancelNewUser(Browser browser)
        {

            browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_CancelButton"))).Click();

        }

        /// <summary>
        /// Method selects the Language on the Admin User -> New User screen
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="language">Language Value</param>
        public void SelectLanguage(Browser browser, string language)
        {


            Element me = browser.Element(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel2_ASPxRoundPanel1_LanguageDropDown_B-1Img")));
            IEElement newele = (IEElement)me.NativeElement;
            System.Drawing.Point clickPoint = gc.GetScreenPoint(newele);
            gc.Move(clickPoint);
            gc.LeftClick(clickPoint);

            Element sl = browser.Element(Find.ByText(language));
            IEElement slele = (IEElement)sl.NativeElement;
            System.Drawing.Point slclickPoint = gc.GetScreenPoint(slele);
            gc.Move(slclickPoint);
            gc.LeftClick(slclickPoint);



        }

        /// <summary>
        /// Method returns a collection of rows that represent the Admin User Search Results table
        ///  Returns all rows with Class = dxgvDataRow
        /// Row class = dxgvDataRow
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <returns>WatiN TableRowCollection Object</returns>
        public TableRowCollection GetUserSearchResults(Browser browser)
        {
            TableRowCollection results = browser.Table(Find.ById(new Regex("ctl00_MainContent_UserResultsRoundPanel_UserGridView_DXMainTable"))).TableRows.Filter(Find.ByClass("dxgvDataRow"));
            return results;
        }
        
        #endregion

        #region Admin Labor Standards

        /// <summary>
        /// Method to search Admin Labor Standards by a specific job code
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="code">Code Value</param>
        /// <param name="includeinactive">Include Inactive Value</param>
        /// 
        public void SearchJobCode(Browser browser, string code, bool includeinactive)
        {
            try
            {
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_RoundSearchPanel_JCEdit"))).SetAttributeValue("value", code);
                browser.CheckBox(Find.ById(new Regex("ctl00_MainContent_RoundSearchPanel_IncludeExpired"))).Checked = includeinactive;
               
                
                browser.Button(Find.ById(new Regex("ctl00_MainContent_RoundSearchPanel_SubmitSearch"))).Click();
                   
            }
            catch
            {

                browser.Close();

            }

        }
      
        /// <summary>
        /// Method Creates a New User on the Admin -> New User page. 
        /// Will Save if save == true else will flash the Save button.
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="jobcode">Job Code Value</param>
        /// <param name="laborhours">Labor Hours Value</param>
        /// <param name="notes">Notes Value</param>
        /// <param name="company">Company Value</param>
        /// <param name="busunit">Business Unit Value</param>
        /// <param name="shop">Shop Value</param>
        /// <param name="efrom">Effective From Value</param>
        /// <param name="eto">Effective To Value</param>
        /// <param name="save">Save Value true will save false will flash</param>
        public void NewLaborStandardSave(Browser browser, string jobcode, string laborhours, string notes,
                            string company, string busunit, string shop, 
                            string efrom, string eto, bool save)
        {


            // TODO:  Make these separate methods.
            browser.Button(Find.ById(new Regex("ctl00_MainContent_NewButton"))).Click();
            browser.WaitUntilContainsText("Add Labor Standard");

            // Enter Names
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_JobCodeCtl"))).SetAttributeValue("value", jobcode);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_LaborHoursCtl"))).SetAttributeValue("value", laborhours);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_NotesCtl"))).SetAttributeValue("value", notes);


            if (!company.Equals(""))
            {

                SelectCompany(browser, company);

            }

            if (!busunit.Equals(""))
            {

                SelectBusinessUnit(browser, busunit);

            }

            if (!shop.Equals(""))
            {

                SelectShop(browser, shop);

            }


            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_EffectiveFromCtl"))).TypeText(efrom);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_EffectiveThruCtl"))).TypeText(eto);
         
           
            if (save == true)
            {

                browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SaveCtl"))).Click();
            }
            else
            {

                browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SaveCtl"))).Flash();

            }

        }
            
        /// <summary>
        /// Method Creates a New Labor Standard on the Admin -> Add Labor Standard Page and Cancels. 
        /// Will Cancel if cancel == true else will flash the Cancel button.
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="jobcode">Job Code Value</param>
        /// <param name="laborhours">Labor Hours Value</param>
        /// <param name="notes">Notes Value</param>
        /// <param name="company">Company Value</param>
        /// <param name="busunit">Business Unit Value</param>
        /// <param name="shop">Shop Value</param>
        /// <param name="efrom">Effective From Value</param>
        /// <param name="eto">Effective To Value</param>
        /// <param name="cancel">Cancel value true will cancel false will flash</param>
        public void NewLaborStandardCancel(Browser browser, string jobcode, string laborhours, string notes,
                            string company, string busunit, string shop,
                            string efrom, string eto, bool cancel)
        {


            // TODO:  Make these separate methods.
            browser.Button(Find.ById(new Regex("ctl00_MainContent_NewButton"))).Click();
            browser.WaitUntilContainsText("Add Labor Standard ");

            // Enter Names
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_JobCodeCtl"))).SetAttributeValue("value", jobcode);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_LaborHoursCtl"))).SetAttributeValue("value", laborhours);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_NotesCtl"))).SetAttributeValue("value", notes);


            if (!company.Equals(""))
            {

                SelectCompany(browser, company);

            }

            if (!busunit.Equals(""))
            {

                SelectBusinessUnit(browser, busunit);

            }

            if (!shop.Equals(""))
            {

                SelectShop(browser, shop);

            }


            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_EffectiveFromCtl"))).TypeText(efrom);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_EffectiveThruCtl"))).TypeText(eto);
            
            if (cancel == true)
            {

                browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_CancelCtl0"))).Click();
            }
            else
            {

                browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_CancelCtl0"))).Flash();

            }

        }
     
        /// <summary>
        /// Method selects the Company on the Admin Labor Standard -> New Labor Standard screent
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="company">Company Value</param>
        public void SelectCompany(Browser browser, string company)
        {



            TextField me = browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_BizUnitControl_CompanyDropDownTextBox")));
            IEElement newele = (IEElement)me.NativeElement;
            System.Drawing.Point clickPoint = gc.GetScreenPoint(newele);
            gc.Move(clickPoint);
            gc.LeftClick(clickPoint);

            string companyvalue = company.Substring(0, 4);

            me.AppendText(companyvalue);

            Element sl = browser.Element(Find.ByText(company));
            IEElement slele = (IEElement)sl.NativeElement;
            System.Drawing.Point slclickPoint = gc.GetScreenPoint(slele);
            gc.Move(slclickPoint);
            gc.LeftClick(slclickPoint);


        }

        /// <summary>
        /// Method selects the Business Unit on the Admin Labor Standard -> New Labor Standard screent
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="busunit">Business Unit Value</param>
        public void SelectBusinessUnit(Browser browser, string busunit)
        {


            TextField me = browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_BizUnitControl_BizUnitDropDownTextBox")));
            IEElement newele = (IEElement)me.NativeElement;
            System.Drawing.Point clickPoint = gc.GetScreenPoint(newele);

            string bvalue = busunit.Substring(0, 4);

            me.AppendText(bvalue);
            
          
            Element sl = browser.Element(Find.ByText(busunit));
            IEElement slele = (IEElement)sl.NativeElement;
            System.Drawing.Point slclickPoint = gc.GetScreenPoint(slele);
            gc.Move(slclickPoint);
            gc.LeftClick(slclickPoint);



        }

        /// <summary>
        /// Method selects the Shop on the Admin Labor Standard -> New Labor Standard screent
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="shop">Shop Value</param>
        public void SelectShop(Browser browser, string shop)
        {

           

            Element edit = browser.Element(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ShopComboBox_I")));
            IEElement editele = (IEElement)edit.NativeElement;
            System.Drawing.Point editclickPoint = gc.GetScreenPoint(editele);
            gc.Move(editclickPoint);
            gc.LeftClick(editclickPoint);


            //  As of 12/13/2011 this field is Read Only.   
           //browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ShopComboBox_I"))).AppendText(shop.Substring(0, 4));

            TableRowCollection rows = browser.Table(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ShopComboBox_DDD_L_LBT"))).TableRows;


            foreach (TableRow x in rows)
            {


                TableCellCollection test = x.TableCells;

                foreach (TableCell y in test)
                {

                    if (y.Text.Equals(shop))
                    {

                        IEElement innerele = (IEElement)y.NativeElement;
                        System.Drawing.Point innerclickPoint = gc.GetScreenPoint(innerele);
                        gc.Move(innerclickPoint);
                        gc.LeftClick(innerclickPoint);
                        y.Click();
                        return;
                    }
                }

            }

                       



        }
        
        /// <summary>
        /// Method returns a collection of rows that represent the Admin Job Code Search Results table
        ///  Returns all rows with Class = dxgvDataRow
        /// Row class = dxgvDataRow
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <returns>WatiN TableRowCollection Object</returns>       
        public TableRowCollection GetJobCodeSearchResults(Browser browser)
        {
            TableRowCollection results = browser.Table(Find.ById(new Regex("ctl00_MainContent_ResultPanel_ASPxGridView1_DXMainTable"))).TableRows.Filter(Find.ByClass("dxgvDataRow"));
            return results;
        }
        
        #endregion

        #region Admin Work Specifications

        /// <summary>
        /// Method to Search for a Work Specification
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="code">Code Value</param>
        /// <param name="description">Description Value</param>
        /// <param name="shop">Shop Value</param>
        /// <param name="company">Company Value</param>
        /// <param name="includeinactive">Include Inactive Value</param>
        public void SearchWorkSpec(Browser browser, string code, string description, string shop, string company,  bool includeinactive)
        {
            try
            {
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_SearchPanel_SpecCodeTextBox"))).SetAttributeValue("value", code);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_SearchPanel_SpecNameTextBox"))).SetAttributeValue("value", description);

                if (!shop.Equals(""))
                {
                    SelectWSShop(browser, shop);

                }

                if (!company.Equals(""))
                {
                    SelectWSCompany(browser, company);
                }

               
                browser.CheckBox(Find.ById(new Regex("ctl00_MainContent_SearchPanel_InactiveCheckBox"))).Checked = includeinactive;
                browser.Button(Find.ById(new Regex("ctl00_MainContent_SearchPanel_SearchButton"))).Click();

            }
            catch
            {

                browser.Close();

            }

        }
        
        /// <summary>
        /// Method selects the Company on the Admin Work Specification -> Search Work Specification screen
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="company">Company Value</param>
        public void SelectWSCompany(Browser browser, string company)
        {



            Element edit = browser.Element(Find.ById(new Regex("ctl00_MainContent_SearchPanel_CompanyComboBox_I")));
            IEElement editele = (IEElement)edit.NativeElement;
            System.Drawing.Point editclickPoint = gc.GetScreenPoint(editele);
            gc.Move(editclickPoint);
            gc.LeftClick(editclickPoint);

            browser.TextField(Find.ById(new Regex("ctl00_MainContent_SearchPanel_CompanyComboBox_I"))).AppendText(company.Substring(0, 4));

            TableRowCollection rows = browser.Table(Find.ById(new Regex("ctl00_MainContent_SearchPanel_CompanyComboBox_DDD_L_LBT"))).TableRows;
                       
            foreach (TableRow x in rows)
            {


                TableCellCollection test = x.TableCells;

                foreach (TableCell y in test)
                {

                    if (y.Text.Equals(company))
                    {

                        IEElement innerele = (IEElement)y.NativeElement;
                        System.Drawing.Point innerclickPoint = gc.GetScreenPoint(innerele);
                        gc.Move(innerclickPoint);
                        gc.LeftClick(innerclickPoint);
                        y.Click();
                        return;
                    }
                }

            }
           



        }

        /// <summary>
        /// Method selects the Shop on the Admin Work Specification -> Search Work Specification screen
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="shop">Shop Value</param>
        public void SelectWSShop(Browser browser, string shop)
        {

            Element edit = browser.Element(Find.ById(new Regex("ctl00_MainContent_SearchPanel_ShopComboBox_I")));
            IEElement editele = (IEElement)edit.NativeElement;
            System.Drawing.Point editclickPoint = gc.GetScreenPoint(editele);
            gc.Move(editclickPoint);
            gc.LeftClick(editclickPoint);

            browser.TextField(Find.ById(new Regex("ctl00_MainContent_SearchPanel_ShopComboBox_I"))).AppendText(shop.Substring(0, 4));

            TableRowCollection rows = browser.Table(Find.ById(new Regex("ctl00_MainContent_SearchPanel_ShopComboBox_DDD_L_LBT"))).TableRows;

          
            foreach (TableRow x in rows)
            {


                TableCellCollection test = x.TableCells;

                foreach (TableCell y in test)
                {

                    if (y.Text.Equals(shop))
                    {

                        IEElement innerele = (IEElement)y.NativeElement;
                        System.Drawing.Point innerclickPoint = gc.GetScreenPoint(innerele);
                        gc.Move(innerclickPoint);
                        gc.LeftClick(innerclickPoint);
                        y.Click();
                        return;
                    }
                }

            }

           

        }

        /// <summary>
        /// Method selects the Company on the Admin Work Specification -> Search Work Specification, New Work Specification screen
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="company">Company Value</param>
        public void SelectNewWSCompany(Browser browser, string company)
        {

            Element edit = browser.Element(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SpecsGridView_DXEditor4_I")));
            IEElement editele = (IEElement)edit.NativeElement;
            System.Drawing.Point editclickPoint = gc.GetScreenPoint(editele);
            gc.Move(editclickPoint);
            gc.LeftClick(editclickPoint);          

            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SpecsGridView_DXEditor4_I"))).AppendText(company.Substring(0, 4));

            TableRowCollection rows = browser.Table(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SpecsGridView_DXEditor4_DDD_L_LBT"))).TableRows;
          
            Console.Write(rows.Count);
            foreach (TableRow x in rows)
            {

               
                TableCellCollection test = x.TableCells;

                foreach (TableCell y in test)
                {
           
                    if(y.Text.Equals(company))
                    {

                        IEElement innerele = (IEElement)y.NativeElement;
                        System.Drawing.Point innerclickPoint = gc.GetScreenPoint(innerele);
                        gc.Move(innerclickPoint);
                        gc.LeftClick(innerclickPoint);
                        y.Click();
                        return;
                    }
                }

            }

     

        }

        /// <summary>
        /// Method selects the Shop on the Admin Work Specification -> Search Work Specification, New Work Specification screen
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="shop">Shop Value</param>
        public void SelectNewWSShop(Browser browser, string shop)
        {

            string shopvalue = browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SpecsGridView_DXEditor3_I"))).Value;

            if(shopvalue.Equals(shop))
            {

                 SendKeys.SendWait("{TAB}");
            }
            else
            {
                    browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SpecsGridView_DXEditor3_I"))).SetAttributeValue("value", "");
                    browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SpecsGridView_DXEditor3_I"))).AppendText(shop.Substring(0, 4));

                    Element sl = browser.Element(Find.ByText(shop));
                    IEElement slele = (IEElement)sl.NativeElement;
                    System.Drawing.Point slclickPoint = gc.GetScreenPoint(slele);
                    gc.Move(slclickPoint);
                    gc.LeftClick(slclickPoint);
        
                    SendKeys.SendWait("{TAB}");
            }

        }      
          
        /// <summary>
        ///  Method Creates a New User on the Admin -> New User page. 
        ///   Will Save if save == true else will flash the Save button.
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="code">Code Value</param>
        /// <param name="description">Description Value</param>
        /// <param name="shop">Shop Value</param>
        /// <param name="company">Company Value</param>
        /// <param name="efrom">Effective From Value</param>
        /// <param name="eto">Effective To Value</param>
        /// <param name="save">Save Value true will save false will cancel</param>
        public void NewWorkSpecification(Browser browser, string code, string description, string shop,
                           string company, string efrom, string eto, bool save)
        {

            browser.Button(Find.ById(new Regex("ctl00_MainContent_NewButton"))).Click();
            browser.WaitUntilContainsText("Effective From");

            // Enter Names
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SpecsGridView_DXEditor1_I"))).SetAttributeValue("value", code);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SpecsGridView_DXEditor2_I"))).SetAttributeValue("value", description);

            if (!shop.Equals(""))
            {

                SelectNewWSShop(browser, shop);

            }

            if (!company.Equals(""))
            {

                SelectNewWSCompany(browser, company);

            }
            
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SpecsGridView_DXEditor5_I"))).TypeText(efrom);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SpecsGridView_DXEditor6_I"))).TypeText(eto);


            if (save == true)
            {

                browser.Link(Find.ByText("Update")).Click();
            }
            else
            {
                browser.Link(Find.ByText("Cancel")).Click();
              
            }

        }

        /// <summary>
        /// Method returns a collection of rows that represent the Admin Work Specification Search Results table
        ///  Returns all rows with Class = dxgvDataRow
        /// Row class = dxgvDataRow
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <returns>WatiN TableRowCollection Object</returns>               
        public TableRowCollection GetWorkSpecificationSearchResults(Browser browser)
        {


            TableRowCollection results = browser.Table(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SpecsGridView_DXMainTable"))).TableRows.Filter(Find.ByClass("dxgvDataRow"));

            return results;

        }

        #endregion

        #region Admin Exception Reasons

        /// <summary>
        /// Method to search Admin Search Exception Reasons
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="code">Code Value</param>
        /// <param name="description">Description Value</param>
        /// <param name="category">Category Value</param>
        /// <param name="includeinactive">Include Inactive Value</param>
        /// 
        public void SearchExceptionReasons(Browser browser, string code, string description, string category, bool includeinactive)
        {
                
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_SearchRoundPanel_ReasonCodeTextBox"))).SetAttributeValue("value", code);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_SearchRoundPanel_DescriptionTextBox"))).SetAttributeValue("value", description);
                browser.SelectList(Find.ById(new Regex("ctl00_MainContent_SearchRoundPanel_CategoryDropDownList"))).Select(category.Trim());                     
                browser.CheckBox(Find.ById(new Regex("ctl00_MainContent_SearchRoundPanel_InactiveCheckBox"))).Checked = includeinactive;
                browser.Button(Find.ById(new Regex("ctl00_MainContent_SearchRoundPanel_SearchButton"))).Click();

        }
        
        /// <summary>
        /// Method returns a collection of rows that represent the Admin Exception Reasons Search Results table
        ///  Returns all rows with Class = dxgvDataRow
        /// Row class = dxgvDataRow
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <returns>WatiN TableRowCollection Object</returns>
        public TableRowCollection GetExceptionReasonsSearchResults(Browser browser)
        {
            

            TableRowCollection results = browser.Table(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonsGridView_DXMainTable"))).TableRows.Filter(Find.ByClass("dxgvDataRow"));

            return results;

        }     
        
        /// <summary>
        /// Method Creates a New User on the Admin -> New User page. 
        /// Will Save if save == true else will flash the Save button.
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="code">Code Value</param>
        /// <param name="category">Category Value</param>
        /// <param name="description">Description Value</param>
        /// <param name="probability">Probability Value</param>
        /// <param name="source">Source Value</param>
        /// <param name="type">Type Value</param>
        /// <param name="efrom">Effective From Value</param>
        /// <param name="eto">Effective To Value</param>
        /// <param name="save">Save Value</param>
        public void NewExceptionReason(Browser browser, string code, string category, string description, string probability, string source, string type, string efrom, string eto, bool save)
        {

            browser.Button(Find.ById(new Regex("ctl00_MainContent_NewButton"))).Click();
            browser.WaitUntilContainsText("Effective From");


            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonsGridView_DXEditor1_I"))).SetAttributeValue("value", code);

            if (!category.Equals(""))
            {

                SelectERCategory(browser, category);

            }

            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonsGridView_DXEditor3_I"))).SetAttributeValue("value", description);
                        

            if (!probability.Equals(""))
            {

                SelectERProbability(browser, probability);

            }

            if (!source.Equals(""))
            {

                SelectERSource(browser, source);

            }

            if (!type.Equals(""))
            {

                SelectERType(browser, type);

            }



            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonsGridView_DXEditor7_I"))).TypeText(efrom);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonsGridView_DXEditor8_I"))).TypeText(eto);


            if (save == true)
            {

                browser.Image(Find.ByTitle("Update")).Click();
            }
            else
            {
                 browser.Image(Find.ByTitle("Cancel")).Click();

            }

        }
       
        /// <summary>
       /// Method to Navigate to Exception Reason Categories using the Link on Exception Reason Page. 
       /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        public void NavExceptionReasonCategory(Browser browser)
        {

            browser.Link(Find.ByText("Edit Exception Reason Categories")).Click();

            browser.WaitUntilContainsText("Exception Reason Categories");


        }

        /// <summary>
        /// Method selects the Category on the Admin Exception Reasons -> New Exception Reason screen
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="category">Category Value</param>
        public void SelectERCategory(Browser browser, string category)
        {

            Element edit = browser.Element(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonsGridView_DXEditor2_I")));
            IEElement editele = (IEElement)edit.NativeElement;
            System.Drawing.Point editclickPoint = gc.GetScreenPoint(editele);
            gc.Move(editclickPoint);
            gc.LeftClick(editclickPoint);


            TableRowCollection rows = browser.Table(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonsGridView_DXEditor2_DDD_L_LBT"))).TableRows;


            foreach (TableRow x in rows)
            {


                TableCellCollection test = x.TableCells;

                foreach (TableCell y in test)
                {

                    if (y.Text.Equals(category))
                    {

                        IEElement innerele = (IEElement)y.NativeElement;
                        System.Drawing.Point innerclickPoint = gc.GetScreenPoint(innerele);
                        gc.Move(innerclickPoint);
                        gc.LeftClick(innerclickPoint);
                        y.Click();
                        return;
                    }
                }

            }





        }
        
        /// <summary>
        /// Method selects the Probability on the Admin Exception Reasons -> New Exception Reason screen
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="probability">Probability Value</param>
        public void SelectERProbability(Browser browser, string probability)
        {

            Element edit = browser.Element(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonsGridView_DXEditor4_I")));
            IEElement editele = (IEElement)edit.NativeElement;
            System.Drawing.Point editclickPoint = gc.GetScreenPoint(editele);
            gc.Move(editclickPoint);
            gc.LeftClick(editclickPoint);


            TableRowCollection rows = browser.Table(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonsGridView_DXEditor4_DDD_L_LBT"))).TableRows;


            foreach (TableRow x in rows)
            {


                TableCellCollection test = x.TableCells;

                foreach (TableCell y in test)
                {
                   
                    if (y.Text.Equals(probability))
                    {

                        IEElement innerele = (IEElement)y.NativeElement;
                        System.Drawing.Point innerclickPoint = gc.GetScreenPoint(innerele);
                        gc.Move(innerclickPoint);
                        gc.LeftClick(innerclickPoint);
                        y.Click();
                        return;
                    }
                }

            }





        }

        /// <summary>
        /// Method selects the Source on the Admin Exception Reasons -> New Exception Reason screen
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="source">Source Value</param>
        public void SelectERSource(Browser browser, string source)
        {

            Element edit = browser.Element(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonsGridView_DXEditor5_I")));
            IEElement editele = (IEElement)edit.NativeElement;
            System.Drawing.Point editclickPoint = gc.GetScreenPoint(editele);
            gc.Move(editclickPoint);
            gc.LeftClick(editclickPoint);


            TableRowCollection rows = browser.Table(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonsGridView_DXEditor5_DDD_L_LBT"))).TableRows;


            foreach (TableRow x in rows)
            {


                TableCellCollection test = x.TableCells;

                foreach (TableCell y in test)
                {

                    if (y.Text.Equals(source))
                    {

                        IEElement innerele = (IEElement)y.NativeElement;
                        System.Drawing.Point innerclickPoint = gc.GetScreenPoint(innerele);
                        gc.Move(innerclickPoint);
                        gc.LeftClick(innerclickPoint);
                        y.Click();
                        return;
                    }
                }

            }





        }

        /// <summary>
        /// Method selects the Type on the Admin Exception Reasons -> New Exception Reason screen
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="type">Type Value</param>
        public void SelectERType(Browser browser, string type)
        {

            Element edit = browser.Element(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonsGridView_DXEditor6_I")));
            IEElement editele = (IEElement)edit.NativeElement;
            System.Drawing.Point editclickPoint = gc.GetScreenPoint(editele);
            gc.Move(editclickPoint);
            gc.LeftClick(editclickPoint);


            TableRowCollection rows = browser.Table(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonsGridView_DXEditor6_DDD_L_LBT"))).TableRows;


            foreach (TableRow x in rows)
            {


                TableCellCollection test = x.TableCells;

                foreach (TableCell y in test)
                {

                    if (y.Text.Equals(type))
                    {

                        IEElement innerele = (IEElement)y.NativeElement;
                        System.Drawing.Point innerclickPoint = gc.GetScreenPoint(innerele);
                        gc.Move(innerclickPoint);
                        gc.LeftClick(innerclickPoint);
                        y.Click();
                        return;
                    }
                }

            }





        }

        #endregion

        #region Admin Exception Reason Categories

        /// <summary>
        /// Method to search Admin Search Exception Reason Categories
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="code">Code Value</param>
        /// <param name="description">Description Value</param>
        /// <param name="includeinactive">Include Inactive Value</param>
        /// 
        public void SearchExceptionReasonCategories(Browser browser, string code, string description, bool includeinactive)
        {

            browser.TextField(Find.ById(new Regex("ctl00_MainContent_SearchRoundPanel_CategoryCodeTextBox"))).SetAttributeValue("value", code);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_SearchRoundPanel_DescriptionTextBox"))).SetAttributeValue("value", description);
            browser.CheckBox(Find.ById(new Regex("ctl00_MainContent_SearchRoundPanel_InactiveCheckBox"))).Checked = includeinactive;
            browser.Button(Find.ById(new Regex("ctl00_MainContent_SearchRoundPanel_SearchButton"))).Click();

        }

        /// <summary>
        /// Method returns a collection of rows that represent the Admin Exception Reason Categories Search Results table
        ///  Returns all rows with Class = dxgvDataRow
        /// Row class = dxgvDataRow
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <returns>WatiN TableRowCollection Object</returns>
        public TableRowCollection GetExceptionReasonCategoriesSearchResults(Browser browser)
        {


            TableRowCollection results = browser.Table(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonCatGridView_DXMainTable"))).TableRows.Filter(Find.ByClass("dxgvDataRow"));

            return results;

        }     

        /// <summary>
        /// Method Creates a New User on the Admin -> New User page. 
        ///   Will Save if save == true else will flash the Save button.
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="code">Code Value</param>
        /// <param name="description">Description Value</param>
        /// <param name="efrom">Effective From Value</param>
        /// <param name="eto">Effective To Value</param>
        /// <param name="save">Save Value true will save false will cancel</param>
        public void NewExceptionReasonCategory(Browser browser, string code, string description, string efrom, string eto, bool save)
        {

            browser.Button(Find.ById(new Regex("ctl00_MainContent_NewButton"))).Click();
            browser.WaitUntilContainsText("Effective From");
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonCatGridView_DXEditor1_I"))).SetAttributeValue("value", code);          
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonCatGridView_DXEditor2_I"))).SetAttributeValue("value", description);            
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonCatGridView_DXEditor3_I"))).TypeText(efrom);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_ExceptionReasonCatGridView_DXEditor4_I"))).TypeText(eto);
            
            if (save == true)
            {

                browser.Image(Find.ByTitle("Update")).Click();
            }
            else
            {
                browser.Image(Find.ByTitle("Cancel")).Click();

            }

        }               

        #endregion

        #region Admin Edits
        /// <summary>
        /// Method to Add a Vertical Edit
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="name">Name Value</param>
        /// <param name="level">Level Value</param>
        /// <param name="efrom">Effective From Value</param>
        /// <param name="eto">Effective To Value</param>
        /// <param name="exception">Exception Value</param>
        /// <param name="errortype">Error Type Value</param>
        /// <param name="recprob">Recovery Probability Value</param>
        /// <param name="source">Source Value</param>
        /// <param name="errorlevel">Error Level Value</param>
        /// <param name="reqcon">Requires or Conflicts With</param>
        /// <param name="configpath">Path to Config File</param>
        /// <param name="basematches">Inlcude Base matches true or false</param>
        /// <param name="associatedmatches">Include Associated matches true or false</param>
        /// <param name="commit">Commit base matches and associated matches</param>
        /// <param name="save">Save Value true will save false will cancel</param>
        
        public void AddVerticalEdit(Browser browser, string name, string level, string efrom, string eto, string exception, string errortype,
            string recprob, string source, string errorlevel, string reqcon, string configpath, bool basematches, bool associatedmatches, bool commit, bool save)
        {

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(configpath);

            XmlNode xn = xmldoc.SelectSingleNode("//config/admin/matchpath");
            string basepath = xn.InnerText;
           
            browser.Button(Find.ById(new Regex("ctl00_MainContent_rpEdits_cmdNewVE"))).Click();
            browser.WaitUntilContainsText("Effective From");

            EditDetails(browser, name, level, efrom, eto, exception, errortype, recprob, source, errorlevel);          

            if (basematches == true)
            {
                EditBaseMatches(browser, basepath, commit);
            }
            if (associatedmatches == true)
            {
               
                EditAssociatedMatches(browser, basepath, commit);
             }

            SaveDetails(browser, save);

           

        }

        /// <summary>
        /// Method to Add a Special Edit
        /// See AddVerticalEdit for Param description
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="name">Name Value</param>
        /// <param name="level">Level Value</param>
        /// <param name="efrom">Effective From Value</param>
        /// <param name="eto">Effective To Value</param>
        /// <param name="exception">Exception Value</param>
        /// <param name="errortype">Error Type Value</param>
        /// <param name="recprob"></param>
        /// <param name="source"></param>
        /// <param name="errorlevel"></param>
        /// <param name="reqcon"></param>
        /// <param name="configpath"></param>
        /// <param name="basematches"></param>
        /// <param name="associatedmatches"></param>
        /// <param name="commit"></param>
        /// <param name="save">Save Value true will save false will cancel</param>
        public void AddSpecialEdit(Browser browser, string name, string level, string efrom, string eto, string exception, string errortype,
            string recprob, string source, string errorlevel, string reqcon, string configpath, bool basematches, bool associatedmatches, bool commit, bool save)
        {

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(configpath);


            XmlNode xn = xmldoc.SelectSingleNode("//config/admin/matchpath");
            string basepath = xn.InnerText;

            browser.Button(Find.ById(new Regex("ctl00_MainContent_rpEdits_cmdNewSE"))).Click();

            browser.WaitUntilContainsText("Effective From");

            EditDetails(browser, name, level, efrom, eto, exception, errortype, recprob, source, errorlevel);          
                       

            if (basematches == true)
            {
                EditBaseMatches(browser, basepath, commit);
            }

            SaveDetails(browser, save);

        }

       /// <summary>
       /// Method to Add a Bad Actor
       /// See AddVerticalEdit for param description
       /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
       /// <param name="name"></param>
       /// <param name="level"></param>
       /// <param name="efrom"></param>
       /// <param name="eto"></param>
       /// <param name="exception"></param>
       /// <param name="errortype"></param>
       /// <param name="days"></param>
       /// <param name="errcount"></param>
       /// <param name="countat"></param>
       /// <param name="recprob"></param>
       /// <param name="source"></param>
       /// <param name="errorlevel"></param>
       /// <param name="reqcon"></param>
       /// <param name="configpath"></param>
       /// <param name="basematches"></param>
       /// <param name="associatedmatches"></param>
       /// <param name="commit"></param>
       /// <param name="save"></param>
       /// 
        public void AddBadActor(Browser browser, string name, string level, string efrom, string eto, string exception, string errortype, string days, string errcount, 
          string countat,  string recprob, string source, string errorlevel, string reqcon, string configpath, bool basematches, bool associatedmatches, bool commit, bool save)
        {

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(configpath);
            
            XmlNode xn = xmldoc.SelectSingleNode("//config/admin/matchpath");
            string basepath = xn.InnerText;

            browser.Button(Find.ById(new Regex("ctl00_MainContent_rpEdits_cmdNewBA"))).Click();

            browser.WaitUntilContainsText("Effective From");

            EditDetails(browser, name, level, efrom, eto, exception, errortype, recprob, source, errorlevel);

            browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_tBoxTSID"))).SetAttributeValue("value", days);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_tBoxNoOfTimes"))).SetAttributeValue("value", errcount);
            browser.RadioButton(Find.ByLabelText(countat)).Checked = true;


            if (basematches == true)
            {
                EditBaseMatches(browser, basepath, commit);
            }

            SaveDetails(browser, save);

        }
        
        /// <summary>
        /// Method Edits a Match (Base or Associated) on the Edits -> Add Vertical Edit page
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="name"></param>
        /// <param name="fieldtype"></param>
        /// <param name="field"></param>
        /// <param name="oper"></param>
        /// <param name="value"></param>
        /// <param name="save">Save Value true will save false will cancel</param>
        public void EditMatch(Browser browser, string name, string fieldtype, string field, string oper, string value, string save)

            {

                browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpMatchDetails_ASPxRoundPanel1_tBoxMatchName"))).SetAttributeValue("value", name);

                browser.Button(Find.ById(new Regex("ctl00_MainContent_rpMatchDetails_ASPxRoundPanel1_cmdAddEditCondition"))).Click();

                browser.WaitForComplete();

                browser.TextField(Find.ById(new Regex("ctl00_MainContent_EditConditionControl_rpEditCondition_tBoxECType"))).Focus();

                if (!fieldtype.Equals(""))
                {
                   
                    SelectFieldType(browser, fieldtype);

                }


                browser.TextField(Find.ById(new Regex("ctl00_MainContent_EditConditionControl_rpEditCondition_tBoxECField"))).Focus();

                if (!field.Equals(""))
                {

                    SelectField(browser, field);

                }


                browser.TextField(Find.ById(new Regex("ctl00_MainContent_EditConditionControl_rpEditCondition_tBoxECOperator"))).Focus();


                if (!oper.Equals(""))
                {

                    SelectListItem(browser, oper);

                }

                browser.TextField(Find.ById(new Regex("ctl00_MainContent_EditConditionControl_rpEditCondition_tBoxECValue"))).SetAttributeValue("value", value);
                            


                if (save.Equals("Y"))
                {

                    browser.Button(Find.ById(new Regex("ctl00_MainContent_EditConditionControl_rpEditCondition_cmdUpdate"))).Click();

                }
                else
                {

                    browser.Button(Find.ById(new Regex("ctl00_MainContent_EditConditionControl_rpEditCondition_cmdCancel"))).Click();

                }


            }
  
        /// <summary>
        /// Method Edits Base Matches on the Add Vertical Edit -> Base Match page
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="configpath">Config File Path</param>
        /// <param name="commit"></param>
        public void EditBaseMatches(Browser browser, string configpath, bool commit)
        {

            #region XMLDoc
            XmlDocument admindoc = new XmlDocument();
            admindoc.Load(configpath);

            XmlNodeList xl = admindoc.SelectNodes("//GMSMatches/basematch");

            #endregion


            foreach (XmlNode xn in xl)
            {

                string name = xn["name"].InnerText;
                string fieldtype = xn["fieldtype"].InnerText;
                string field = xn["field"].InnerText;
                string oper = xn["operator"].InnerText;
                string value = xn["value"].InnerText;
                string save = xn["save"].InnerText;

                browser.Button(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_rpBaseMatch_cmdAddBaseMatch"))).Click();

                EditMatch(browser, name, fieldtype, field, oper, value, save);

                MatchCommit(browser, commit);
                               

            }
        }

        /// <summary>
        ///  Method Commits a Match
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="commit"></param>
        public void MatchCommit(Browser browser, bool commit)
        {

            if (commit==true)
            {

                browser.Button(Find.ById(new Regex("ctl00_MainContent_rpMatchDetails_cmdUpdate"))).Click();
            }
            else
            {

                browser.Button(Find.ById(new Regex("ctl00_MainContent_rpMatchDetails_cmdCancel"))).Click();
            }

        }

        /// <summary>
        /// Method Edits Associated Matches on the Add Vertical Edit -> Associated Match page
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="configpath">Path to config file GMSMatches.xml</param>
        /// <param name="commit">Commit Value true will commit, false will cancel</param>
        public void EditAssociatedMatches(Browser browser, string configpath, bool commit)
        {

            #region XMLDoc
            XmlDocument admindoc = new XmlDocument();
            admindoc.Load(configpath);

            XmlNodeList xl = admindoc.SelectNodes("//GMSMatches/associatedmatch");

            #endregion


            foreach (XmlNode xn in xl)
            {

                string name = xn["name"].InnerText;
                string fieldtype = xn["fieldtype"].InnerText;
                string field = xn["field"].InnerText;
                string oper = xn["operator"].InnerText;
                string value = xn["value"].InnerText;
                string save = xn["save"].InnerText;
               

                browser.Button(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_rpAssociatedMatch_cmdAddAssoMatch"))).Click();

                EditMatch(browser, name, fieldtype, field, oper, value, save);

                MatchCommit(browser, commit);
                

            }

            

        }

        /// <summary>
        ///  Selects an Element from a displayed list on a Field
        ///  Requires focus be set on the Parent field. 
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="listvalue">List Item Value</param>
        public void SelectListItem(Browser browser, string listvalue)
        {


            // Find Element By Text

            Element value = browser.Element(Find.ByText(listvalue));
            IEElement valueele = (IEElement)value.NativeElement;
            System.Drawing.Point vclickpoint = gc.GetScreenPoint(valueele);
            gc.Move(vclickpoint);
            gc.LeftClick(vclickpoint);


        }

        /// <summary>
        /// Method selects the Admin -> Edits -> Add Vertical Edits -> BaseMatch -> FieldType
        /// Requires Focus be set on the target element. 
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="fieldtype">Field type to select</param>
        public void SelectFieldType(Browser browser, string fieldtype)
        {


            browser.TextField(Find.ById(new Regex("ctl00_MainContent_EditConditionControl_rpEditCondition_tBoxECType"))).Focus();
          
            // Find Element By Text
            Element value = browser.Element(Find.ByText(fieldtype));
            IEElement valueele = (IEElement)value.NativeElement;
            System.Drawing.Point vclickpoint = gc.GetScreenPoint(valueele);
            gc.Move(vclickpoint);
            gc.LeftClick(vclickpoint);


        }

        /// <summary>
        /// Method selects the Admin -> Edits -> Add Vertical Edits -> BaseMatch -> Field
        /// Requires Focus be set on the target element. 
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="field"></param>
        public void SelectField(Browser browser, string field)
        {

            browser.TextField(Find.ById(new Regex("ctl00_MainContent_EditConditionControl_rpEditCondition_tBoxECField"))).Focus();

            // Find Element By Text

            Element value = browser.Element(Find.ByText(field));
            IEElement valueele = (IEElement)value.NativeElement;
            System.Drawing.Point vclickpoint = gc.GetScreenPoint(valueele);
            gc.Move(vclickpoint);
            gc.LeftClick(vclickpoint);


        }

        /// <summary>
        /// Method Edits the Details of an Added Edit
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="name"></param>
        /// <param name="level"></param>
        /// <param name="efrom"></param>
        /// <param name="eto"></param>
        /// <param name="exception"></param>
        /// <param name="errortype"></param>
        /// <param name="recprob"></param>
        /// <param name="source"></param>
        /// <param name="errorlevel"></param>
        public void EditDetails(Browser browser, string name, string level, string efrom, string eto, string exception, string errortype, string recprob, string source, string errorlevel)
        {


            browser.RadioButton(Find.ByLabelText(errorlevel)).Checked = true;
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_tBoxEditName"))).SetAttributeValue("value", name);
            browser.SelectList(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_ddlEditLevel"))).Select(level);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_deEffFrom_I"))).TypeText(efrom);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_deEffThru_I"))).TypeText(eto);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_ctrlException_rpException_tBoxException"))).SetAttributeValue("value", exception);
            browser.SelectList(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_ctrlException_rpException_ddlErrorType"))).SelectByValue(errortype);
            browser.SelectList(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_ctrlException_rpException_ddlProbability"))).Select(recprob);
            browser.SelectList(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_ctrlException_rpException_ddlSource"))).Select(source);

        }

        /// <summary>
        /// Method Saves the details of a recently added Edit
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="save">Save Value true will save false will cancel</param>
        public void SaveDetails(Browser browser, bool save)
        {

            if (save == true)
            {
                browser.Button(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_cmdSave"))).Click();
            }
            else
            {
                browser.Button(Find.ById(new Regex("ctl00_MainContent_rpEditDetails_cmdCancel"))).Click();
            }
            
        }       

        /// <summary>
        /// Method sets the Page Size on the Edits page
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="size"></param>
        public void EditSetPageSize(Browser browser, string size)
        {

            browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpEdits_tBoxSetPageSize"))).SetAttributeValue("value", size);
            browser.Button(Find.ById(new Regex("ctl00_MainContent_rpEdits_cmdSetPageSize"))).Click();

        }

        #endregion

        #region Admin Contacts

        /// <summary>
        /// Method to Search Admin Contacts
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="companyname"></param>
        /// <param name="scac"></param>
        /// <param name="companynumber"></param>
        /// <param name="includeinactive"></param>
        public void SearchContacts(Browser browser, string companyname, string scac, string companynumber, bool includeinactive)
        {

            browser.TextField(Find.ById(new Regex("ctl00_MainContent_RoundSearchPanel_CompanyDropDownTextBox"))).SetAttributeValue("value", companyname);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_RoundSearchPanel_ScacEdit"))).SetAttributeValue("value", scac);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_RoundSearchPanel_CompanyNumberDropDownTextBox"))).SetAttributeValue("value", companynumber);
            browser.CheckBox(Find.ById(new Regex("ctl00_MainContent_RoundSearchPanel_IncludeExpired"))).Checked = includeinactive;
            browser.Button(Find.ById(new Regex("ctl00_MainContent_RoundSearchPanel_SubmitSearch"))).Click();

        }
        
        /// <summary>
        /// Method returns a collection of rows that represent the Admin Contacts Search Results table
        ///  Returns all rows with Class = dxgvDataRow
        /// Row class = dxgvDataRow
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <returns></returns>
        public TableRowCollection GetContactsSearchResults(Browser browser)
        {
            
            TableRowCollection results = browser.Table(Find.ById(new Regex("ctl00_MainContent_ResultPanel_ASPxGridView1"))).TableRows.Filter(Find.ByClass("dxgvDataRow"));
            return results;

        }


       
        /// <summary>
        ///  Method Creates a New User on the Admin -> New User page. 
        /// Will Save if save == true else will flash the Save button.
        /// Contact Info is stored in Contacts XML. Method requires GMSAddContact.xml be created. 
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="configpath">Path to Config File GMSAddContact.xml</param>
        /// <param name="save">Save Value true will save false will cancel</param>
        public void NewContact(Browser browser, string configpath, bool save)
        {

            //TODO:  Possibly streamline this method         

            #region XMLDoc
            XmlDocument admindoc = new XmlDocument();
            admindoc.Load(configpath);

            XmlNodeList xl = admindoc.SelectNodes("//GMSAddContact/contact");

            #endregion
              

            foreach (XmlNode xn in xl)
            {

                string companynumber = xn["conum"].InnerText;
                string scac = xn["scac"].InnerText;
                string name = xn["name"].InnerText;
                string efrom = xn["effectivefrom"].InnerText;
                string eto = xn["effectiveto"].InnerText;
                string addy1 = xn["addy1"].InnerText;
                string addy2 = xn["addy2"].InnerText;
                string addy3 = xn["addy3"].InnerText;               
                string addy4 = xn["addy4"].InnerText;
                string city = xn["city"].InnerText;
                string state = xn["state"].InnerText;
                string zip = xn["zip"].InnerText;
                string fname = xn["fname"].InnerText;
                string lname = xn["lname"].InnerText;
                string title = xn["title"].InnerText;
                string phone = xn["phone"].InnerText;
                string fax = xn["fax"].InnerText;
                string email = xn["email"].InnerText;
                string invoiceformat = xn["invoiceformat"].InnerText;
                string invoicedelivery = xn["invoicedelivery"].InnerText;
                string railinc = xn["railinc"].InnerText;
                string includebr = xn["includebr"].InnerText;
                string gbentity = xn["gbentity"].InnerText;
                string additionalscac = xn["additionalscac"].InnerText;
                




                browser.Button(Find.ById(new Regex("ctl00_MainContent_NewButton"))).Click();
                browser.WaitUntilContainsText("Effective From");
                
                // Company Info

                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_CompanyCodeCtl"))).SetAttributeValue("value", companynumber);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ScacCtl"))).SetAttributeValue("value", scac);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_NameCtl"))).SetAttributeValue("value", name);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_EffectiveFromCtl"))).TypeText(efrom);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_EffectiveThruCtl"))).TypeText(eto);

                // Address Info
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_Address1Ctl"))).SetAttributeValue("value", addy1);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_Address2Ctl"))).SetAttributeValue("value", addy2);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_Address3Ctl"))).SetAttributeValue("value", addy3);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_Address4Ctl"))).SetAttributeValue("value", addy4);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_CityCtl"))).SetAttributeValue("value", city);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_StateCtl"))).SetAttributeValue("value", state);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ZipCtl"))).SetAttributeValue("value", zip);

                // Detail Info

                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_FirstNameCtl"))).SetAttributeValue("value", fname);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_LastNameCtl"))).SetAttributeValue("value", lname);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_TitleCtl"))).SetAttributeValue("value", title);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_PhoneCtl"))).SetAttributeValue("value", phone);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_FaxCtl"))).SetAttributeValue("value", fax);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_EmailCtl"))).SetAttributeValue("value", email);

                // Artifacts

                browser.SelectList(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_InvoiceFormatCtl"))).Select(invoiceformat);
                browser.SelectList(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_InvoiceDeliveryCtl"))).Select(invoicedelivery);

                if (railinc.Equals("Y"))
                {

                    browser.CheckBox(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_RailincCtl"))).Checked = true;

                }

                if (includebr.Equals("Y"))
                {

                    browser.CheckBox(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_IncludeBrcCtl"))).Checked = true;

                }

                if (gbentity.Equals("Y"))
                {

                    browser.CheckBox(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_GBEntityCtl"))).Checked = true;

                }


                if (additionalscac.Equals("Y"))
                {

                    XmlNodeList xscac = admindoc.SelectNodes("//GMSAddContact/contact/newscac");

                    Console.Write(xscac.Count);


                    foreach (XmlNode z in xscac)
                    {

                        string newscac = z["scacname"].InnerText;
                        string effrom = z["effectivefrom"].InnerText;
                        string efto = z["effectiveto"].InnerText;
                        string savescac = z["savescac"].InnerText;

                        browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_NewScacButton"))).Click();

                        browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ScacGridView_DXPEForm_efnew_DXEditor0_I"))).SetAttributeValue("value", newscac);                       
                        browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ScacGridView_DXPEForm_efnew_DXEditor1_I"))).TypeText(effrom);
                        browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ScacGridView_DXPEForm_efnew_DXEditor2_I"))).TypeText(efto);

                        if (savescac.Equals("Y"))
                        {

                            browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ScacGridView_DXPEForm_efnew_SaveScacButton"))).Click();

                        }

                        else
                        {

                            browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ScacGridView_DXPEForm_efnew_CancelScacButton"))).Click();

                        }

                        
                    }
                    


                }


                if (save == true)
                {

                    browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_SaveCtl"))).Click();
                }
                else
                {

                    browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_CancelCtl0"))).Click();

                }

            }

        }
     
        /// <summary>
        /// Method to Edit a newly created Contact SCAC
        /// Takes scac name, effective from and effective to dates as paramete
       /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
       /// <param name="newscac"></param>
       /// <param name="effrom"></param>
       /// <param name="efto"></param>
       /// <param name="save">Save Value true will save false will Cancel</param>
        public void ContactEditSCAC(Browser browser, string newscac, string effrom, string efto, bool save)
        {


            browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_EditScacButton"))).Click();

            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ScacGridView_DXPEForm_ef0_DXEditor0_I"))).SetAttributeValue("value", newscac);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ScacGridView_DXPEForm_ef0_DXEditor1_I"))).TypeText(effrom);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ScacGridView_DXPEForm_efnew_DXEditor2_I"))).TypeText(efto);

            if (save == true)
            {

                browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ScacGridView_DXPEForm_ef0_SaveScacButton"))).Click();

            }

            else
            {

                browser.Button(Find.ById(new Regex("ctl00_MainContent_ASPxRoundPanel1_ScacGridView_DXPEForm_ef0_CancelScacButton"))).Click();

            }


        }

        #endregion

        #region AdminWarrantyVendors

        /// <summary>
        /// Performs a Search for Admin Warranty Vendors
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="includeinactive"></param>
        public void SearchWarrantyVendors(Browser browser, string code, string name, bool includeinactive)
        {
            try
            {
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_SearchRoundPanel_CompanyCodeTextBox"))).SetAttributeValue("value", code);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_SearchRoundPanel_CompanyNameTextBox"))).SetAttributeValue("value", name);
                browser.CheckBox(Find.ByLabelText("Include Inactive")).Checked = includeinactive;

                browser.Button(Find.ById(new Regex("ctl00_MainContent_SearchRoundPanel_SearchButton"))).Click();

            }
            catch
            {

                browser.Close();

            }

        }

        /// <summary>
        /// Method returns a collection of rows that represent the Warranty Search Results table
        ///  Returns all rows with Class = dxgvDataRow
        /// Row class = dxgvDataRow
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <returns>WatiN TableRowCollection</returns>
        // TODO - Get actual table name - need valid search criteria
        public TableRowCollection GetWarrantyVendorSearchResults(Browser browser)
        {


            TableRowCollection results = browser.Table(Find.ById(new Regex("ctl00_MainContent_ResultsRoundPanel_WarrantyVendorsGridView_DXMainTable"))).TableRows.Filter(Find.ByClass("dxgvDataRow"));

            return results;

        }
               
       
        /// <summary>
        ///  Method Creates a New Warranty Vendor on the Admin -> New Warranty Vendor page. 
        ///  Will Save if save == true else will flash the Save button.
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="company"></param>
        /// <param name="code"></param>
        /// <param name="efrom"></param>
        /// <param name="eto"></param>
        /// <param name="configpath"></param>
        /// <param name="wvaccount"></param>
        /// <param name="save">Save Value if true will save else cancel</param>
        public void NewWarrantyVendor(Browser browser, string company, string code, string efrom, string eto, string configpath, bool wvaccount,  bool save)
        {
            #region XML Doc
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(configpath);

            XmlNode xn = xmldoc.SelectSingleNode("//config/admin/warrantyvendorpath");
            string xmlpath = xn.InnerText;
#endregion
                        
            browser.Button(Find.ById(new Regex("ctl00_MainContent_NewButton"))).Click();
            browser.WaitUntilContainsText("Warranty Vendor Accounts");

            // Enter Names
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_CompanyName"))).SetAttributeValue("value", company);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_CompanyCode"))).SetAttributeValue("value", code);         
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_EffectiveFrom_I"))).TypeText(efrom);
            browser.TextField(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_EffectiveThru_I"))).TypeText(eto);


            if (wvaccount == true)
            {
                NewWarrantyVendorAccount(browser, xmlpath);

            }
            

            if (save == true)
            {
                browser.Button(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_SaveButton"))).Flash();
                browser.Button(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_SaveButton"))).Click();
            }
            else
            {
               // browser.Button(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_CancelButton"))).Flash();
                browser.Button(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_CancelButton"))).Click();
            }

        }

        /// <summary>
        /// Creates a New Warranty Vendor Account 
        /// Config Path = path to FTConfig.xml
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="configpath">Config Path</param>
        public void NewWarrantyVendorAccount(Browser browser, string configpath)
        {

            #region XMLDoc
            XmlDocument admindoc = new XmlDocument();
            admindoc.Load(configpath);

            XmlNodeList xl = admindoc.SelectNodes("//GMSWarrantyVendorAccounts/account");
            #endregion

            browser.Button(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_ResultsRoundPanel_NewButton"))).Click();

            Console.Write(xl.Count);
            foreach (XmlNode xn in xl)
            {
                
                string gbentity = xn["gbentity"].InnerText;
                string accountingcode = xn["accountingcode"].InnerText;
                string rac = xn["rac"].InnerText;
                string efrom = xn["effectivefrom"].InnerText;
                string eto = xn["effectiveto"].InnerText;
                string save = xn["save"].InnerText;

                browser.TextField(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_ResultsRoundPanel_WarrantyVendorAccountsGridView_DXEditor1_I"))).Focus();

               SelectGBEntity(browser, gbentity);

                browser.TextField(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_ResultsRoundPanel_WarrantyVendorAccountsGridView_DXEditor2_I"))).SetAttributeValue("value", accountingcode);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_ResultsRoundPanel_WarrantyVendorAccountsGridView_DXEditor3_I"))).SetAttributeValue("value", rac);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_ResultsRoundPanel_WarrantyVendorAccountsGridView_DXEditor4_I"))).TypeText(efrom);
                browser.TextField(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_ResultsRoundPanel_WarrantyVendorAccountsGridView_DXEditor5_I"))).TypeText(eto);

                if (save.Equals("Y"))
                {

                    browser.Image(Find.ByTitle("Update")).Click();

                }
                else
                {
                    browser.Image(Find.ByTitle("Cancel")).Click();

                }


            }

         
        }

        /// <summary>
        /// Method selects the Company on the Admin Labor Standard -> New Labor Standard screent
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="gbentity"></param>
        public void SelectGBEntity(Browser browser, string gbentity)
        {


            TextField me = browser.TextField(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_ResultsRoundPanel_WarrantyVendorAccountsGridView_DXEditor1_I")));
            IEElement newele = (IEElement)me.NativeElement;
            System.Drawing.Point clickPoint = gc.GetScreenPoint(newele);
            gc.Move(clickPoint);
            gc.LeftClick(clickPoint);
            
            TableRowCollection rows = browser.Table(Find.ById(new Regex("ctl00_MainContent_WarrantyVendorPanel_ResultsRoundPanel_WarrantyVendorAccountsGridView_DXEditor1_DDD_L_LBT"))).TableRows;
            
            foreach (TableRow x in rows)
            {
                
                TableCellCollection test = x.TableCells;

                foreach (TableCell y in test)
                {
                    //Console.Write(y.Text);
                   // Console.Write(Environment.NewLine);
                    if (y.Text.Equals(gbentity))
                    {
                        
                        IEElement innerele = (IEElement)y.NativeElement;
                        System.Drawing.Point innerclickPoint = gc.GetScreenPoint(innerele);
                        gc.Move(innerclickPoint);
                        gc.LeftClick(innerclickPoint);
                        y.Click();
                        return;
                    }
                }

            }

           


        }

        #endregion

        #region Integrated System Errors
      
        /// <summary>
        /// Method sets the Page Size on the Integrated Systems Error page
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="size"></param>
        public void ISESetPageSize(Browser browser, string size)
        {

            browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpIntegrationSystemErrors_PageSizeTextBox"))).SetAttributeValue("value", size);
            browser.Button(Find.ById(new Regex("ctl00_MainContent_rpIntegrationSystemErrors_PageSizeButton"))).Click();

        }
        /// <summary>
        /// Method to click the Resolve button on Integrated System Errors Page
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="size"></param>
        public void ISEResolve(Browser browser, string size)
        {
          
            browser.Button(Find.ById(new Regex("ctl00_MainContent_rpIntegrationSystemErrors_cmdResolve"))).Click();

        }

        /// <summary>
        /// Method to Filter the Integrated System Errors
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="selection">Column Name</param>
        /// <param name="searchstring">Column Value</param>
        /// <param name="filterby">Filter Selection ie Begins With</param>
        public void ISEFilterBy(Browser browser, string selection, string searchstring,  string filterby)
        {

            ImageCollection filters = browser.Images.Filter(Find.ByClass("dxGridView_gvFilterRowButton"));
            switch (selection)
            {
               
               case "IntSys":

                    filters[0].Click();
                    browser.Element(Find.ByText(filterby)).Click();
                    browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpIntegrationSystemErrors_gvIntegrationSystemErrors_DXFREditorcol1_I"))).TypeText(searchstring);
                    break;

               case "IntSysDesc":

                    filters[1].Click();
                    browser.Element(Find.ByText(filterby)).Click();
                    browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpIntegrationSystemErrors_gvIntegrationSystemErrors_DXFREditorcol2_I"))).TypeText(searchstring);
                    break;

               case "ErrDesc":

                    filters[2].Click();
                    browser.Element(Find.ByText(filterby)).Click();
                    browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpIntegrationSystemErrors_gvIntegrationSystemErrors_DXFREditorcol3_I"))).TypeText(searchstring);
                    break;

                case "Resolved":

                    filters[3].Click();
                    browser.Element(Find.ByText(filterby)).Click();
                    browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpIntegrationSystemErrors_gvIntegrationSystemErrors_DXFREditorcol4_I"))).TypeText(searchstring);
                    break;

                case "AddDt":

                    filters[4].Click();
                    browser.Element(Find.ByText(filterby)).Click();
                    browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpIntegrationSystemErrors_gvIntegrationSystemErrors_DXFREditorcol5_I"))).TypeText(searchstring);
                    break;

                case "AddUser":

                    filters[5].Click();
                    browser.Element(Find.ByText(filterby)).Click();
                    browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpIntegrationSystemErrors_gvIntegrationSystemErrors_DXFREditorcol6_I"))).TypeText(searchstring);
                    break;

                case "ModDt":

                    filters[6].Click();
                    browser.Element(Find.ByText(filterby)).Click();
                    browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpIntegrationSystemErrors_gvIntegrationSystemErrors_DXFREditorcol7_I"))).TypeText(searchstring);
                    break;

                case "ModUser":

                    filters[7].Click();
                    browser.Element(Find.ByText(filterby)).Click();
                    browser.TextField(Find.ById(new Regex("ctl00_MainContent_rpIntegrationSystemErrors_gvIntegrationSystemErrors_DXFREditorcol8_I"))).TypeText(searchstring);
                    break;
            }

        }
        /// <summary>
        /// Method to Remove or Clear Integrated System Errors from Filter
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="indexorname"></param>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="removeorclear"></param>
        public void ISERemoveClearFilter(Browser browser, string indexorname, int index, string name, string removeorclear)
        {

            TableRowCollection trc = browser.Table(Find.ById("ctl00_MainContent_rpIntegrationSystemErrors_gvIntegrationSystemErrors_DXFilterBar")).TableRows;        
            
            switch(indexorname)
           
            {

                case "Index":

                TableCellCollection tc = trc[index].TableCells;
                if (removeorclear.Equals("Remove"))
                {
                    WatiN.Core.CheckBox cb = tc[0].CheckBox(Find.First());
                    cb.Checked = false;
                }
                if (removeorclear.Equals("Clear"))
                {
                   
                    tc[4].Link(Find.ByText("Clear")).Click();

                }
                break;                

                case "Name":            

                if(removeorclear.Equals("Remove"))
                {
                foreach (TableRow x in trc)
                {                   
                    TableCellCollection tcollect = x.TableCells.Filter(Find.ByText(new Regex(name)));               
                    if (tcollect.Count != 0)
                    {
                        TableCellCollection cr = x.TableCells;
                        WatiN.Core.CheckBox crcb = cr[0].CheckBox(Find.First());
                        crcb.Checked = false;
                        break;          
                    }                                 
                }
                }
                if(removeorclear.Equals("Clear"))
                {

                    foreach (TableRow x in trc)
                    {
                        TableCellCollection tcollect = x.TableCells.Filter(Find.ByText(new Regex(name)));
                        if (tcollect.Count != 0)
                        {
                            TableCellCollection cr = x.TableCells;
                            cr[4].Link(Find.ByText("Clear")).Click();                            
                            break;
                        }
                    }

                }
                break;
            
            }
        }


        #endregion

        /// <summary>
        /// Method sets the ShowAll checkbox.  
        /// true = checked
        /// </summary>
        /// <param name="browser">WatiN Browser Object</param>
        /// <param name="all"></param>
        public void ShowAll(Browser browser, bool all)
        {

            browser.CheckBox(Find.ByLabelText("ShowAll")).Checked = all;

        }



    }
}
