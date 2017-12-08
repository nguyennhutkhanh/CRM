using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WcfService.Common
{
   
    public class Logs
    {
        private static string sLogFormat;
        private static string sErrorTime;

        public static void CreateLogFiles()
        {
            //sLogFormat used to create log files format :
            // dd/mm/yyyy hh:mm:ss AM/PM ==> Log Message
            sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";

            //this variable used to create log filename format "
            //for example filename : ErrorLogYYYYMMDD
            string sYear = DateTime.Now.Year.ToString();
            string sMonth = DateTime.Now.Month.ToString();
            string sDay = DateTime.Now.Day.ToString();
            sErrorTime = sYear + sMonth + sDay;
        }

        public static void ErrorLog(string sPathName, string sErrMsg)
        {
            StreamWriter sw = new StreamWriter(sPathName + sErrorTime, true);
            sw.WriteLine(sLogFormat + sErrMsg);
            sw.Flush();
            sw.Close();
        }

        public static void writeToLogFile(string logMessage)
        {
            string strLogMessage = string.Empty;
            string strLogFile = System.AppDomain.CurrentDomain.BaseDirectory + System.Configuration.ConfigurationManager.AppSettings["logFilePath"].ToString();
            StreamWriter swLog;

            strLogMessage = string.Format("{0}: {1}", DateTime.Now, logMessage);

            if (!File.Exists(strLogFile))
            {
                swLog = new StreamWriter(strLogFile);
            }
            else
            {
                swLog = File.AppendText(strLogFile);
            }

            swLog.WriteLine(strLogMessage);
            swLog.WriteLine();

            swLog.Close();

        }
    }
}
