using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSftp
{
    public enum LogTarget
    {
        File, Database, EventLog
    }

    internal abstract class LogBase
    {
        protected readonly object lockObj = new object();
        public abstract void Log(string message);
        public virtual void LogError(Exception ex)
        {

        }
    }

    internal class FileLogger : LogBase
    {
        public string LogPath = @"D:\CRM\CRM\WWWRoot\CustomPages\OrderTracking\CatalystXML\ErrorLog_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";
        public override void Log(string logMessage)
        {
            lock (lockObj)
            {
                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += logMessage;
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                using (StreamWriter streamWriter = new StreamWriter(LogPath, true))
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
            }
        }

        public override void LogError(Exception ex)
        {
            lock (lockObj)
            {
                string message = string.Format("Time: {0}", DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;
                message += string.Format("Message: {0}", ex.Message);
                message += Environment.NewLine;
                message += string.Format("StackTrace: {0}", ex.StackTrace);
                message += Environment.NewLine;
                message += string.Format("Source: {0}", ex.Source);
                message += Environment.NewLine;
                message += string.Format("TargetSite: {0}", ex.TargetSite.ToString());
                message += Environment.NewLine;
                message += "-----------------------------------------------------------";
                message += Environment.NewLine;

                using (var writer = new System.IO.StreamWriter(LogPath, true))
                {
                    writer.WriteLine(message);
                    writer.Close();
                }
            }
        }
    }

    internal class EventLogger : LogBase
    {
        public override void Log(string message)
        {
            lock (lockObj)
            {
                EventLog m_EventLog = new EventLog("");
                m_EventLog.Source = "Idemia Event";
                m_EventLog.WriteEntry(message);
            }
        }
    }

    internal class DBLogger : LogBase
    {
        string connectionString = string.Empty;
        public override void Log(string message)
        {
            lock (lockObj)
            {
                //Code to log data to the database
            }
        }
    }
}
