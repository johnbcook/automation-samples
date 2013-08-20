automation-samples
==================

Sample of development and automation

Classes demonstrate examples of automation frameworks that I've implemented.

The projects I was involved in were high speed low drag projects where I was
responsible for the all of the automation and had to have it completed in 
3-6 months. 

AdminCommon.cs
   
   This is a page based class that contains methods allowing for the interaction with 
   objects on the Admin page.    Written leveraging WatiN and NUnit.   Moving forward I 
   would use SeleniumWebdriver vice WatiN as it is more robust and better supported.  
   Both libraries interact with the browser using the browsers native language
   
  BaseTest.cs
  
   This is an abstract base test containing items that need to occur for ever test or 
   test fixture (test class).
	  
GBHomeNav.cs
   This class contains a list of quick navigation smoke tests.   Executed as part of 
   continuous integration to verify build acceptance
   
GlobalCommonActions.cs
   This class contains common actions that can be used throughout the application. 
   An example of UIAutomation can be viewed in the IE9FileDownload(string name) method. 
   
MsmqUtil.cs
    This is an example of a utility class.  Contains methods to work with MSMQ object.

NewAssert.cs
    I refactored the NUnit Assert class to allow for multiple assertions to occur within
	1 test.  
	
SQLServer.cs
    Contains methods for some common SQL Server actions and reused queries. 

SqlServerDB.cs
     Contains stored proc calls and other methods for SQL Server. 
	
Utilities.cs
    An example of a utilities class
	
VerifyUtility
    A utility class containing some commonly used verification points. 
    	

   
