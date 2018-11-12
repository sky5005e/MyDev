using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSftp
{
    public static class LogHelper
    {
        private static LogBase logger = null;
        public static void Log(string message, LogTarget target = LogTarget.File)
        {
            switch (target)
            {
                case LogTarget.File:
                    logger = new FileLogger();
                    logger.Log(message);
                    break;
                case LogTarget.Database:
                    logger = new DBLogger();
                    logger.Log(message);
                    break;
                case LogTarget.EventLog:
                    logger = new EventLogger();
                    logger.Log(message);
                    break;
                default:
                    return;
            }
        }
        public static void LogError(Exception ex, LogTarget target = LogTarget.File)
        {
            logger = new FileLogger();
            logger.LogError(ex);
        }
    }
}
