using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Extensions;
using GHX.Core.Messaging;

namespace Utilities.Testing.Utilities
{
    /// <summary>
    /// Class containing verification methods. 
    /// </summary>
    public class VerifyUtil
    {

        /// <summary>
        /// Method to compare to strings.  If strings are equal returns equal else returns a generated diff report.
        /// Used for Fitnesse tests. 
        /// </summary>
        /// <returns></returns>
        public static string VerifyStrings(string expected, string actual, string eoutput, string aoutput, string testname)
        {
            expected = expected.Trim();
            actual = actual.Trim();

            if (expected.Equals(actual))
            {
                return "Equal";
            }
            else
            {
               expected = StringUtil.RemoveAllNewLines(expected);
               actual = StringUtil.RemoveAllNewLines(actual);

                if (expected.Equals(actual))
                {
                    return "Equal";
                }
                else
                { 
                    // Create Actual Result File                   
                    FileUtil.WriteFile(aoutput, actual);
                    // Create Expected Results File
                    FileUtil.WriteFile(eoutput, expected);
                    return GetDiffReport(eoutput, aoutput, testname);
                }
            }

        }

        /// <summary>
        /// Method to Get Diff Report of 2 files
        /// Requires that a Runner.exe.config or RunnerW.exe.config exist in the Runner directory. 
        /// </summary>
        /// <returns></returns>
        public static string GetDiffReport(string expectedfile, string actualfile, string testname)
        {
            FileUtil fu = new FileUtil();
            string result = fu.CreateDifferenceReport(expectedfile, actualfile, testname);
            return fu.CreateAHrefTag(result, "Report");
        }

        /// <summary>
        /// Method to Verify the names all Files in a directory 
        /// were created in accordance to FileWriter requirements. 
        /// Used for FileWriter Hash mode
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool VerifyFileWriterFileName(string directory, string type, string prefix, string suffix, CPlusMessage msg)
        {
            bool result = true;        
            string[] filelist = FileUtil.GetFileList(directory);

            foreach (string x in filelist)
            {
                string[] splitfile = x.Split('\\');
                // Verify prefix
                if (!splitfile[splitfile.Length -1].StartsWith(prefix))
                {
                    Console.Write("Prefix is incorrect. File: " + x + "does not contain" + prefix);
                    result = false;
                }
                // Verify suffix
                if (!splitfile[splitfile.Length - 1].EndsWith(suffix))
                {
                    Console.Write("Suffix is incorrect. File: " + x + "does not contain" + suffix);
                    result = false;
                }
                
                switch (type)
                {
                   
                    case "hash":
                        {
                           
                            if (!splitfile[splitfile.Length - 1].Equals(msg.CRC64))
                            {
                                Console.Write("File Name: " + splitfile[splitfile.Length - 1] + " not equal to msg.CRC64 " + msg.CRC64 + Environment.NewLine);
                            }
                            // Verify filename = CRC
                            break;
                        }

                    default:
                        {
                            throw new Exception(type + " is an invalid mode, must be Timestamp, Hash, Random, or EDI");
                        }
                }

            }

            return result;
        }

        /// <summary>
        /// Method to Verify the names all Files in a directory 
        /// were created in accordance to FileWriter requirements. 
        /// used for FileWriter timestamp and edi modes
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool VerifyFileWriterFileName(string inputfile, string directory, string type, string prefix, string suffix, char fielddel)
        {
            bool result = true;
            string date = DateTime.Now.ToString("MMddHH");
            string timestamp = DateTime.Now.ToString("MMddyyyyHH");
          //  Console.Write(date + Environment.NewLine);
            string[] filelist = FileUtil.GetFileList(directory);
           
            foreach (string x in filelist)
            {
                string[] splitfile = x.Split('\\');
                // Verify prefix
                if (!splitfile[splitfile.Length - 1].StartsWith(prefix))
                {
                    Console.Write("Prefix is incorrect. File: " + x + "does not contain" + prefix + Environment.NewLine);
                    result = false;
                }
                // Verify suffix
                if (!splitfile[splitfile.Length - 1].EndsWith(suffix))
                {
                    Console.Write("Suffix is incorrect. File: " + x + "does not contain" + suffix + Environment.NewLine);
                    result = false;
                }

                // Verify Date
                if (!splitfile[splitfile.Length - 1].Contains(date))
                {
                    Console.Write("Date is incorrect. File: " + x + "does not contain" + date + Environment.NewLine);
                    result = false;
                }

                switch (type)
                {
                    case "timestamp":
                        {
                            if (!splitfile[splitfile.Length - 1].Contains(timestamp))
                            {
                                Console.Write("Timestamp is incorrect. File: " + x + "does not contain" + timestamp + Environment.NewLine);
                                result = false;
                            }
                           
                            break;
                        }
                    
                    case "edi":
                        {
                            // Verify filename contans ST.DocType.GS.PO

                            string st = EDIUtil.GetEDIField(inputfile, fielddel, "ST", 1);
                            string gs = EDIUtil.GetEDIField(inputfile, fielddel, "GS", 2);
                        //    Console.Write(EDIUtil.GetEDIField(x, fielddel, "ST", 1) + Environment.NewLine);
                       //     Console.Write(EDIUtil.GetEDIField(x, fielddel, "GS", 2) + Environment.NewLine);
                            string edivalue = st + "." + gs + "." + date;

                            if (!splitfile[splitfile.Length - 1].Contains(edivalue))
                            {
                                Console.Write("EDI is incorrect. File: " + x + "does not contain" + edivalue + Environment.NewLine);
                                result = false;
                            }
                            
                            break;
                        } default:
                        {
                            throw new Exception(type + " is an invalid mode, must be Timestamp, Hash, Random, or EDI");
                        }
                   
                }

            }

            return result;
        }

        /// <summary>
        /// Method to Verify all elements of a cplus log entry
        /// </summary>
        /// <param name="filepath">full path of cplus log</param>
        /// <param name="date">Full date as it appears in cplus log</param>
        /// <param name="type">Type of log entry</param>
        /// <param name="process">Process name</param>
        /// <param name="message">Message string</param>
        /// <returns>true if 1 or more entries meeting criteria exist</returns>
        public static bool VerifyCoreLog(string filepath, string date, string type, string process, string message)
        {
            bool result;

            var query = from c in
                            (from line in File.ReadAllLines(filepath)
                             let cpluslog = line.Split('\t')
                             select new CoreLogHelper()
                             {
                                 Date = cpluslog[0],
                                 Type = cpluslog[1],
                                 Process = cpluslog[2],
                                 Message = cpluslog[3]
                             })
                        where c.Date.Equals(date) && c.Type.Equals(type) && c.Process.Equals(process) && c.Message.Equals(message)
                        select c;
           // Console.Write(query.Count() + System.Environment.NewLine);
            if (query.Count() >= 1)
            {
                result = true;
            }
            else { result = false; }
         //   foreach (var item in query) { Console.WriteLine("{0}, {1}, {2}, {3}", item.Date, item.Type, item.Process, item.Message); }
        //    Console.Write(result);
            return result;
        }

        /// <summary>
        /// Method to Verify all elements of a cplus log entry except date
        /// </summary>
        /// <param name="filepath">full path of cplus log</param>
        /// <param name="type">type of log entry</param>
        /// <param name="process">process name</param>
        /// <param name="message">message string</param>
        /// <returns>true if 1 or more entries meeting criteria exist</returns>
        public static bool VerifyCoreLog(string filepath, string type, string process, string message)
        {
            bool result;
            var query = from c in
                            (from line in File.ReadAllLines(filepath)
                             let cpluslog = line.Split('\t')
                             select new CoreLogHelper()
                             {
                                 Date = cpluslog[0],
                                 Type = cpluslog[1],
                                 Process = cpluslog[2],
                                 Message = cpluslog[3]
                             })
                        where c.Type.Equals(type) && c.Process.Equals(process) && c.Message.Equals(message)
                        select c;
           // Console.Write(query.Count() + System.Environment.NewLine);
            if (query.Count() >= 1)
            {
                result = true;
            }
            else { result = false; }
          //  foreach (var item in query) { Console.WriteLine("{0}, {1}, {2}, {3}", item.Date, item.Type, item.Process, item.Message); }
          //  Console.Write(result);
            return result;
          
        }

        /// <summary>
        /// Method to verify a System log entry.  
        /// Provides ability to verify full date. 
        /// /// 
        /// </summary>
        /// <param name="filepath">full path of system log</param>
        /// <param name="date">either full or partial date as it appears in log entry. Pass at least 11/14/2012</param>
        /// <param name="type">type of log entry</param>
        /// <param name="message">message string</param>
        /// <param name="verifydate">indicate date verification true will verify full date false will verify date contains date entry</param>
        /// <returns>true if 1 or more entries meeting criteria exist</returns>
        public static bool VerifyCoreLog(string filepath, string date, string type, string message, bool verifydate)
        {
            bool result;
           
            switch(verifydate)
            {
            case true:
            var query = from c in
                            (from line in File.ReadAllLines(filepath)
                             let cpluslog = line.Split('\t')
                             select new CoreLogHelper()
                             {
                                 Date = cpluslog[0],
                                 Type = cpluslog[1],
                                 Message = cpluslog[2]
                             })
                            
                        where c.Date.Equals(date) && c.Type.Equals(type) && c.Message.Equals(message)
                        select c;
            
            if (query.Count() >= 1)
            {
                result = true;
            }
            else { result = false; }
           
            return result;

            case false:
            var query1 = from c in
                            (from line in File.ReadAllLines(filepath)
                             let cpluslog = line.Split('\t')
                             select new CoreLogHelper()
                             {
                                 Date = cpluslog[0],
                                 Type = cpluslog[1],
                                 Message = cpluslog[2]
                             })

                        where c.Date.Contains(date) && c.Type.Equals(type) && c.Message.Equals(message)
                        select c;

            if (query1.Count() >= 1)
            {
                result = true;
            }
            else { result = false; }
          
            return result;
            default:
            {
                throw new Exception("Only 2 options in a bool true or false.  There is no gray area!");
            }
            
        }
        }

        /// <summary>
        /// Method to verify cplus_Error log.  Provides ability to verify full date. 
        /// </summary>
        /// <param name="filepath">full path of cplus_Error log</param>
        /// <param name="date">either full or partial date as it appears in log entry. Pass at least 11/14/2012</param>
        /// <param name="message">message string</param>
        /// <param name="verifydate">bool indicating date verification</param>
        /// <returns>either full or partial date as it appears in log entry. Pass at least 11/14/2012</returns>
        public static bool VerifyCoreLog(string filepath, string date, string message, bool verifydate)
        {
            bool result;

            switch (verifydate)
            {
                case true:
                    var query = from c in
                                    (from line in File.ReadAllLines(filepath)
                                     let cpluslog = line.Split('\t')
                                     select new CoreLogHelper()
                                     {
                                         Date = cpluslog[0],
                                         Message = cpluslog[1]
                                     })

                                where c.Date.Equals(date) && c.Message.Equals(message)
                                select c;

                    if (query.Count() >= 1)
                    {
                        result = true;
                    }
                    else { result = false; }

                    return result;

                case false:
                    var query1 = from c in
                                     (from line in File.ReadAllLines(filepath)
                                      let cpluslog = line.Split('\t')
                                      select new CoreLogHelper()
                                      {
                                          Date = cpluslog[0],
                                          Message = cpluslog[1]
                                          
                                      })

                                 where c.Date.Contains(date) && c.Message.Equals(message)
                                 select c;

                    if (query1.Count() >= 1)
                    {
                        result = true;
                    }
                    else { result = false; }

                    return result;
                default:
                    {
                        throw new Exception("Only 2 options in a bool true or false.  There is no gray area!");
                    }

            }
           
        }

    }
}
