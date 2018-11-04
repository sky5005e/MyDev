using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


public class CommonCls
{
    public static String FTPInfoPath = @"D:\CRM\CRM\WWWRoot\CustomPages\OrderTracking\";
    public static String rootDir = @"D:\CRM\CRM\WWWRoot\CustomPages\OrderTracking\CatalystXML\Sent\";
    

    

   
    /// Return Decimal value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static String DecimalValue(String value)
    {
        String _numb = String.Empty;
        if (!String.IsNullOrEmpty(value))
            _numb = Convert.ToDecimal(value).ToString("0.00");
        else
            _numb = value;
        return _numb;
    }

    /// Return Decimal value
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static String IntValue(String value)
    {
        String _numb = String.Empty;
        if (!String.IsNullOrEmpty(value))
            _numb = Convert.ToInt32(Convert.ToDouble(value)).ToString();
        else
            _numb = value;
        return _numb;
    }

    /// <summary>
    /// Get Bytes of Memory Stream
    /// </summary>
    /// <param name="inputStream"></param>
    /// <returns></returns>
    private static byte[] ToByteArray(Stream inputStream)
    {

        using (MemoryStream ms = new MemoryStream())
        {

            inputStream.CopyTo(ms);

            return ms.ToArray();

        }

    }

    public static String HtmlEncode(string text)
    {
       return System.Net.WebUtility.HtmlEncode(text);
    }

    /// <summary>
    /// WriteError()
    /// Create file in Error folder when error comes in applcation page.
    /// </summary>
    public static void ErrorMessage(Exception ex, String filepath)
    {

        string err = null;
        try
        {
            //To log out user on error

            string path = filepath;
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            using (StreamWriter w = File.AppendText(path))
            {

                System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(ex, true);
                w.WriteLine("\r\nLog Entry : ");
                w.WriteLine("{0}", DateTime.Now.ToString());
                err = "Error in: " + Environment.NewLine +
                           "File Name : " + trace.GetFrame(0).GetFileName() + Environment.NewLine +
                           "Line : " + trace.GetFrame(0).GetFileLineNumber() + Environment.NewLine +
                           "Column: " + trace.GetFrame(0).GetFileColumnNumber() + Environment.NewLine +
                           "Error Message: " + ex.Message + Environment.NewLine +
                           "Source: " + ex.Source + Environment.NewLine +
                           "StackTrace: " + ex.StackTrace + Environment.NewLine +
                           "TargetSite:" + ex.TargetSite.ToString();

                w.WriteLine(err);
                w.WriteLine("__________________________");
                w.Flush();
                w.Close();


            }
        }
        catch (Exception EX)
        {
            throw null;
        }

    }

    public static Boolean IsWeekend()
    {
        Boolean _isWeekend = false;
        DateTime date = DateTime.Now;//DateTime.ParseExact("2014-03-30 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff",System.Globalization.CultureInfo.InvariantCulture);
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            _isWeekend = true;
        }
        return _isWeekend;
    }

    public static Boolean IsWorkingHours()
    {
        Boolean _isWorkingHours = false;
        DateTime date = DateTime.Now;
        if (date.Hour > 8 && date.Hour < 21)
        {
            _isWorkingHours = true;
        }
        return _isWorkingHours;
    }
}

public class DirFiles
{
    public String OrderNumber { get; set; }
    public String CustomerXMLPath { get; set; }
    public String OrderXMLPath { get; set; }
    public String ShipToXMLPath { get; set; }
    public String InvoicePDFPath { get; set; }
}


