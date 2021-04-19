using System;
using System.IO;

namespace DownloaderSD
{
    public class Logger
    {
        private string logPath = string.Empty;

        //Constructor
        public Logger()
        {
            logPath = System.Configuration.ConfigurationManager.AppSettings["LogFileName"];

            if (string.IsNullOrEmpty(logPath)) logPath = "DownloaderSD.log";
        }

        /// <summary>
        /// Write log
        /// </summary>
        /// <param name="text"></param>
        private void Write(string text)
        {
            StreamWriter sw = new StreamWriter(logPath, true);
            sw.WriteLine(text);
            sw.Flush();
            sw.Close();
        }

        public void WriteToFile(string log)
        {
            string timeStamp = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");

            Write(string.Format("{0} - {1}", timeStamp,log));
        }

        public void WriteToFile(string methodName, string log)
        {
  
            string timeStamp = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");
            
            Write(string.Format("{0} - {1} - {2}", timeStamp,methodName, log));
        }

        public void Banner(string title, string value)
        {
            Write("~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~");
            Write(string.Format("{0}: {1}",title,value));
            Write("~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~");
        }

        public void BigBanner(string title, string value)
        {
            Write("*******************************************************************************************");
            Write("~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~");
            Write("|");
            Write(string.Format("| {0} - {1}", title, value));
            Write("|");
            Write("~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~=~");
            Write("*******************************************************************************************");
        }
    }
}
