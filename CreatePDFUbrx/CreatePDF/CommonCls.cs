using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OfficeOpenXml;
using System.Data;

public class CommonCls
{
    public static String FTPInfoPath = @"D:\CRM\CRM\WWWRoot\CustomPages\OrderTracking\";
    public static String rootDir = @"D:\CRM\CRM\WWWRoot\CustomPages\OrderTracking\CatalystXML\Sent\";
    public static String subDir = DateTime.Now.ToString("yyyy-MM-dd");

    /// <summary>
    /// Get Dir path
    /// </summary>
    public static String MainDir
    {
        get
        {
            String newDirPath = rootDir + subDir + @"\";
            bool isExists = System.IO.Directory.Exists(newDirPath);

            if (!isExists)
                System.IO.Directory.CreateDirectory(newDirPath);

            return newDirPath;

        }
    }




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

}

public static class ExcelPackageExtensions
{

    public static System.Data.DataTable ToDataTable(this ExcelPackage package)
    {
        ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
        System.Data.DataTable table = new System.Data.DataTable();
        foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
        {
            table.Columns.Add(firstRowCell.Text == null ? "" : firstRowCell.Text.Trim());
        }

        for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
        {
            var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
            var newRow = table.NewRow();
            foreach (var cell in row)
            {
                newRow[cell.Start.Column - 1] = (cell.Text == null ? "" : cell.Text.Trim());
            }
            table.Rows.Add(newRow);
        }
        return table;
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

public static class DataTableExtensions
{
    public static List<T> DataTableToList<T>(this DataTable table) where T : new()
    {
        List<T> list = new List<T>();
        var typeProperties = typeof(T).GetProperties().Select(propertyInfo => new
        {
            PropertyInfo = propertyInfo,
            Type = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType
        }).ToList();

        foreach (var row in table.Rows.Cast<DataRow>())
        {
            T obj = new T();
            foreach (var typeProperty in typeProperties)
            {
                object value = row[typeProperty.PropertyInfo.Name];
                object safeValue = value == null || DBNull.Value.Equals(value)
                    ? null
                    : Convert.ChangeType(value, typeProperty.Type);

                typeProperty.PropertyInfo.SetValue(obj, safeValue, null);
            }
            list.Add(obj);
        }
        return list;
    }
}

