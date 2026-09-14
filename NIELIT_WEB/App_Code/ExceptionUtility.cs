using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Text;
/// <summary>
/// Summary description for ExceptionUtility
/// </summary>
public class ExceptionUtility
{
		// All methods are static, so this can be private
    private ExceptionUtility()
    { }

    // Log an Exception
    public static void LogException(Exception exc, string source)
    {
        try
        {
            // Include enterprise logic for logging exceptions
            // Get the absolute path to the log file
            string logFile = "App_Data/ErrorLog.txt";
            logFile = HttpContext.Current.Server.MapPath(logFile);

            // Open the log file for append and write the log
            StreamWriter sw = new StreamWriter(logFile, true);
            sw.WriteLine("********** {0} **********", DateTime.Now);
            if (exc.InnerException != null)
            {
                sw.Write("Inner Exception Type: ");
                sw.WriteLine(exc.InnerException.GetType().ToString());
                sw.Write("Inner Exception: ");
                sw.WriteLine(exc.InnerException.Message);
                sw.Write("Inner Source: ");
                sw.WriteLine(exc.InnerException.Source);
                if (exc.InnerException.StackTrace != null)
                {
                    sw.WriteLine("Inner Stack Trace: ");
                    sw.WriteLine(exc.InnerException.StackTrace);
                }
            }
            sw.Write("Exception Type: ");
            sw.WriteLine(exc.GetType().ToString());
            sw.WriteLine("Exception: " + exc.Message);
            sw.WriteLine("Source: " + source);
            sw.WriteLine("Stack Trace: ");
            if (exc.StackTrace != null)
            {
                sw.WriteLine(exc.StackTrace);
                sw.WriteLine();
            }
            sw.Close();

            //StringBuilder sb = new StringBuilder();
            //if (exc.InnerException != null)
            //{
            //    sb.Append("Inner Exception Type: ");
            //    sb.AppendLine(exc.InnerException.GetType().ToString());
            //    sb.Append("Inner Exception: ");
            //    sb.AppendLine(exc.InnerException.Message);
            //    sb.Append("Inner Source: ");
            //    sb.AppendLine(exc.InnerException.Source);
            //    if (exc.InnerException.StackTrace != null)
            //    {
            //        sb.AppendLine("Inner Stack Trace: ");
            //        sb.AppendLine(exc.InnerException.StackTrace);
            //    }
            //}
            //sb.Append("Exception Type: ");
            //sb.AppendLine(exc.GetType().ToString());
            //sb.AppendLine("Exception: " + exc.Message);
            //sb.AppendLine("Source: " + source);
            //sb.AppendLine("Stack Trace: ");
            //if (exc.StackTrace != null)
            //{
            //    sb.AppendLine(exc.StackTrace);
            //}
        }
        catch (Exception ex)
        { 
        
        }
    }



    // Notify System Operators about an exception
    public static void NotifySystemOps(Exception exc)
    {
        // Include code for notifying IT system operators
    }
}