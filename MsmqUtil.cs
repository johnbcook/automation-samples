using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Reflection;
using System.Text.RegularExpressions;


namespace Utilities.Testing.Utilities
{
    /// <summary>
    /// Utility class for MSMQ.  Provides methods for working with MSMQ.
    /// </summary>
    public class MsmqUtil
    {              
        
        
        /// <summary>
        /// Method to purge messages from a target queue
        /// </summary>
        /// <param name="mq">Message Queue Name</param>
        public void PurgeMessages(System.Messaging.MessageQueue mq)
        {
            mq.Purge();
        }

        /// <summary>
        /// Peek into a Queue without any Timeout
        /// usage: PeekWithoutTimeout(q, cursor, PeekAction.Next)
        /// </summary>
        /// <param name="q">Message Queue Name</param>
        /// <param name="cursor">Cursor</param>
        /// <param name="action">Action</param>
        /// <returns></returns>
        public static System.Messaging.Message PeekWithoutTimeout(MessageQueue q, System.Messaging.Cursor cursor, PeekAction action)
        {
            System.Messaging.Message ret = null;

            try
            {
                ret = q.Peek(new TimeSpan(1), cursor, action);

            }
            catch (MessageQueueException mqe)
            {
                if (!mqe.Message.ToLower().Contains("timeout"))

                { throw; }

            }

            return ret;
        }       

        /// <summary>
        /// Reads a message from target queue as a String
        /// </summary>
        /// <param name="mq">Message Queue Name</param>
        public void ReadMessageAsString(System.Messaging.MessageQueue mq)
        {

            System.Messaging.Cursor cursor = mq.CreateCursor();
            System.Messaging.Message m = PeekWithoutTimeout(mq, cursor, PeekAction.Current);

            m.Formatter = new XmlMessageFormatter(new String[] { "System.String, mscorlib", });
            string body = (string)m.Body;

            Console.Write(body);
            Console.Write(Environment.NewLine);

            {
                while ((m = PeekWithoutTimeout(mq, cursor, PeekAction.Next)) != null)
                {
                    // Construct an XMLMessageFormatter
                    m.Formatter = new XmlMessageFormatter(new String[] { "System.String, mscorlib", });
                    body = (string)m.Body;
                    Console.Write(body);
                    Console.Write(Environment.NewLine);
                }

            }

        }

        /// <summary>
        /// Method to delete all Private queues on the current machine 
        /// </summary>
        /// <param name="machinename"></param>
        public static void DeleteAllPrivateQueues()
        {
            MessageQueue[] privatequeues = MessageQueue.GetPrivateQueuesByMachine(System.Environment.MachineName);

            foreach (MessageQueue z in privatequeues)
            {
                MessageQueue.Delete(z.Path);
            }
        }

        /// <summary>
        /// Method to get a count of all queues in a queue folder
        /// </summary>
        /// <param name="queuetype"></param>
        public static int GetQueueCount(string queuetype)
        {

            switch (queuetype)
            {
                case "Private":
                MessageQueue[] privatequeues = MessageQueue.GetPrivateQueuesByMachine(System.Environment.MachineName);
                return privatequeues.Length;

                case "Public":
                MessageQueue[] publicqueues = MessageQueue.GetPublicQueuesByMachine(System.Environment.MachineName);
                return publicqueues.Length;

                default:
                throw new ArgumentNullException("Invalid Selection Specified: Options are Private or Public");            

            }

        }

       


    }
}
