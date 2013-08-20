using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Xml;
using System.Web;
using System.Windows;
using System.Runtime.InteropServices;
using System.Reflection;
using NUnit.Framework;



namespace 
    EntrustFunctionalTest
{
    /// <summary>
    /// Class containing useful utilities
    /// </summary>
    public class Utilities
    {
        // Fields
        private static Random _random = new Random();
        private Config CF;

        // Methods
        /// <summary>
        /// Config Object
        /// </summary>
        /// <param name="obj"></param>
        public Utilities(Config obj)
        {
            this.CF = obj;
        }

        /// <summary>
        /// Method to provide a Random int within a range.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int RandomIntWithinRange(int start, int end)
        {
           
            int results = _random.Next(start, end);
            return results;


        }

        /// <summary>
        /// Method to generate a random string
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < size; i++)
            {
                builder.Append(Convert.ToChar(Convert.ToInt32(Math.Floor((double)((26.0 * _random.NextDouble()) + 65.0)))));
            }
            return builder.ToString();
        }

        /// <summary>
        /// Method to create an XML document
        /// </summary>
        /// <param name="path"></param>
        /// <param name="title"></param>
        /// <param name="filename"></param>
        /// <param name="startelement"></param>
        /// <param name="stylesheet"></param>
        public void XMLCreate(string path, string title, string filename, string startelement, string stylesheet)
        {
            if (!File.Exists(path))
            {
                string CurrentDate = DateTime.Now.ToString("G");
                XmlTextWriter textWriter = new XmlTextWriter(path, null);
                textWriter.WriteStartDocument();
                textWriter.WriteProcessingInstruction("xml-stylesheet", stylesheet);
                textWriter.WriteComment(title);
                textWriter.WriteComment(CurrentDate);
                textWriter.WriteComment(filename);
                textWriter.WriteStartElement(startelement);
                textWriter.WriteEndElement();
                textWriter.WriteEndDocument();
                textWriter.Close();
            }
        }

        /// <summary>
        /// Method to create a directory
        /// </summary>
        /// <param name="path"></param>
        public void CreateDirectory(string path)
        {

            if (!Directory.Exists(path))
            {

                Directory.CreateDirectory(path);


            }

        }

        /// <summary>
        /// Method to Create a File
        /// </summary>
        /// <param name="path"></param>
        public void CreateFile(string path)
        {

            if (!File.Exists(path))
            {

                File.Create(path);


            }

        }

        /// <summary>
        /// Method to recursively delete a directory
        /// </summary>
        /// <param name="path"></param>
        public void DeleteDirectoryRecursive(string path)
        {

            if (Directory.Exists(path))
            {

                Directory.Delete(path, true);

            }

        }

        /// <summary>
        /// Method to Delete all files in a directory
        /// </summary>
        /// <param name="path"></param>
        public void DirectoryDeleteAllFiles(string path)
        {

            if (Directory.Exists(path))
            {

                string[] files = System.IO.Directory.GetFiles(path);

                foreach (String x in files)
                {

                    DeleteFile(x);
                }


            }

        }

        /// <summary>
        /// Method to delete all directories
        /// </summary>
        /// <param name="path"></param>
        public void DirectoryDeleteAllDirectories(string path)
        {

            if (Directory.Exists(path))
            {
                string[] dirs = System.IO.Directory.GetDirectories(path);
                foreach (String x in dirs)
                {

                    DeleteDirectoryRecursive(x);
                }


            }

        }

        /// <summary>
        /// Method to Delete a Directory
        /// </summary>
        /// <param name="path"></param>
        public void DeleteDirectory(string path)
        {

            if (Directory.Exists(path))
            {

                Directory.Delete(path);

            }

        }

        /// <summary>
        /// Method to Delete a File
        /// </summary>
        /// <param name="path"></param>
        public void DeleteFile(string path)
        {

            if (File.Exists(path))
            {
                File.Delete(path);
            }

        }

        /// <summary>
        /// Method to Read XML Settings
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public string ReadXMLSetting(string path, string key)
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            Assert.IsNotNull(doc, "XML Document is Null");

            // retrieve appSettings node
            XmlNode node = doc.SelectSingleNode("//appSettings");

            if (node == null)
                throw new InvalidOperationException("appSettings section not found in config file.");

            try
            {
                // select the 'add' element that contains the key
                XmlElement elem = (XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key));


                if (elem != null)
                {
                    Console.Write("This is value: " + elem.GetAttribute("value"));
                    return elem.GetAttribute("value"); ;

                }
                else
                {

                    string value = "Element not found";
                    return value;
                }

            }
            catch
            {

                Console.Write("Cannot Read Setting");
                throw;
            }

        }

        /// <summary>
        /// Method to update a setting of an xml file.
        /// </summary>
        /// <param name="configpath"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateSetting(string configpath, string key, string value)
        {
            // load config document for current assembly
            XmlDocument doc = new XmlDocument();

            doc.Load(configpath);

            Assert.IsNotNull(doc, "XML Document is Null");

            // retrieve appSettings node
            XmlNode node = doc.SelectSingleNode("//appSettings");

            if (node == null)
                throw new InvalidOperationException("appSettings section not found in config file.");

            try
            {
                // select the 'add' element that contains the key
                XmlElement elem = (XmlElement)node.SelectSingleNode(string.Format("//add[@key='{0}']", key));

                if (elem != null)
                {
                    // add value for key
                    elem.SetAttribute("value", value);
                }
                else
                {
                    // key was not found so create the 'add' element 
                    // and set it's key/value attributes 
                    elem = doc.CreateElement("add");
                    elem.SetAttribute("key", key);
                    elem.SetAttribute("value", value);
                    node.AppendChild(elem);
                }
                doc.Save(configpath);
            }
            catch
            {

                Console.Write("Config not updated");
                throw;
            }
        }

        /// <summary>
        /// Method to remove a setting from the current executed code app config
        /// </summary>
        /// <param name="key"></param>
        public void RemoveSetting(string key)
        {
            // load config document for current assembly
            XmlDocument doc = loadConfigDocument();

            // retrieve appSettings node
            XmlNode node = doc.SelectSingleNode("//appSettings");

            try
            {
                if (node == null)
                    throw new InvalidOperationException("appSettings section not found in config file.");
                else
                {
                    // remove 'add' element with coresponding key
                    node.RemoveChild(node.SelectSingleNode(string.Format("//add[@key='{0}']", key)));
                    doc.Save(getConfigFilePath());
                }
            }
            catch (NullReferenceException e)
            {
                throw new Exception(string.Format("The key {0} does not exist.", key), e);
            }
        }

        /// <summary>
        /// Method to load a fi
        /// </summary>
        /// <returns></returns>
        public XmlDocument loadConfigDocument()
        {
            XmlDocument doc = null;
            try
            {
                doc = new XmlDocument();
                doc.Load(getConfigFilePath());
                return doc;
            }
            catch (System.IO.FileNotFoundException e)
            {
                throw new Exception("No configuration file found.", e);
            }
        }

        /// <summary>
        /// Method to get AppConfig File Path
        /// </summary>
        /// <returns></returns>
        public string getConfigFilePath()
        {
            return Assembly.GetExecutingAssembly().Location + ".config";
        }

        /// <summary>
        /// Method to replace a string within Line of a file
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="currentstring"></param>
        /// <param name="newstring"></param>
        public void ReplaceStringInFileLine(string filepath, string currentstring, string newstring)
        {
            StringBuilder newFile = new StringBuilder();

            string temp = "";
            string[] file = System.IO.File.ReadAllLines(filepath);

            foreach (string line in file)
            {
                if (line.Contains(currentstring))
                {
                    temp = line.Replace(currentstring, newstring);
                    newFile.Append(temp + "\r\n");
                    continue;
                }
                newFile.Append(line + "\r\n");
            }

            System.IO.File.WriteAllText(filepath, newFile.ToString());



        }
    }
}
