using System;
using System.IO;

namespace HTTPServer
{
    class Logger
    {
        
        public static void LogException(Exception ex)
        {
            StreamWriter sr = new StreamWriter("log.txt", true);
            // TODO: Create log file named log.txt to log exception details in it
            //Datetime:
            string datetime = DateTime.Now.ToString();
            //message:
            String msg = ex.Message;
            // for each exception write its details associated with datetime 
            sr.WriteLine(datetime);
            sr.WriteLine(msg);
            sr.WriteLine();
            sr.Close();
        }
    }
}
