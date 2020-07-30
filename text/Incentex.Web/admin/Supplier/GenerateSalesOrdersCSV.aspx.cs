using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using Ionic.Utils.Zip;

public partial class admin_Supplier_GenerateSalesOrdersCSV : PageBase
{
    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Generate Sales Orders(CSV)";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";
            ((Label)Master.FindControl("lblPageHeading")).Text = "Generate Sales Orders (CSV)";
        }
    }

    #endregion

    #region Control Events

    protected void lnkDownloadCSV_Click(object sender, EventArgs e)
    {
        base.MenuItem = "Download Sales Orders";
        base.ParentMenuID = 60;

        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
            base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
        else
            base.SetAccessRights(true, true, true, true);

        if (!base.CanView)
        {
            base.RedirectToUnauthorised();
        }

        DownloadCSVFile();
    }

    protected void lnkPastDownloads_Click(object sender, EventArgs e)
    {
        base.MenuItem = "View Past Downloads";
        base.ParentMenuID = 60;

        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
            base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
        else
            base.SetAccessRights(true, true, true, true);

        if (!base.CanView)
        {
            base.RedirectToUnauthorised();
        }

        Response.Redirect("~/admin/Supplier/PastDownloads.aspx");
    }   

    #endregion

    #region Page Methods

    /// <summary>
    /// Generates the contents of the log file.
    /// </summary>
    /// <returns>The contents of the log file.</returns>
    private String[] GenerateCSVFields()
    {
        StringBuilder csvHeader = new StringBuilder();
        csvHeader.AppendLine("\"WebOrderNumber\",\"CompanyName\",\"OrderDate\",\"ShipToAddr1\",\"ShipToAddr2\",\"ShipToAddr3\",\"ShipToSuiteApt\",\"ShipToCity\",\"ShipToState\",\"ShipToZipCD\",\"ShipToCountry\",\"ContactPhone\",\"ContactEmail\",\"Ref1\",\"SpecInst\"");

        StringBuilder csvItem = new StringBuilder();
        csvItem.AppendLine("\"WebOrderNumber\",\"LineNo\",\"Prodno\",\"QtyOrd\",\"UOM\",\"ProdDesc\"");

        StringBuilder OrderIDs = new StringBuilder();

        try
        {
            PAASRepository objPAAS = new PAASRepository();
            List<SelectPAASOrderCSVFieldsResult> CSVLines = objPAAS.SelectPAASOrderCSVLines();

            Int32 LineNo = 0;
            String OrderNumber = String.Empty;
            String PreviousOrderNumber = String.Empty;
            foreach (SelectPAASOrderCSVFieldsResult Line in CSVLines)
            {
                OrderNumber = Line.WebOrderNumber;
                if (OrderNumber != PreviousOrderNumber)
                {
                    PreviousOrderNumber = OrderNumber;
                    LineNo = 1;

                    csvHeader.AppendLine(
                    String.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\"",
                    Convert.ToString(Line.WebOrderNumber), Convert.ToString(Line.CompanyName), Convert.ToString(Line.OrderDate),
                    Convert.ToString(Line.ShipToAddr1 ?? String.Empty).Replace("\"", "\"\""), Convert.ToString(Line.ShipToAddr2 ?? String.Empty).Replace("\"", "\"\""),
                    Convert.ToString(Line.ShipToAddr3 ?? String.Empty).Replace("\"", "\"\""), Convert.ToString(Line.ShipToStreet ?? String.Empty).Replace("\"", "\"\""),
                    Convert.ToString(Line.ShipToCity), Convert.ToString(Line.ShipToState),
                    Convert.ToString(Line.ShipToZipCD), Convert.ToString(Line.ShipToCountry),
                    Convert.ToString(Line.ContactPhone), Convert.ToString(Line.ContactEmail),
                    Convert.ToString(Line.Ref1 ?? String.Empty).Replace("\"", "\"\""), Convert.ToString(Line.SpecialInstructions ?? String.Empty).Replace("\"", "\"\"")));

                    if (OrderIDs.Length == 0)
                        OrderIDs.Append(Convert.ToString(Line.OrderID));
                    else
                        OrderIDs.Append("," + Convert.ToString(Line.OrderID));
                }

                csvItem.AppendLine(
                    String.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"",
                    Convert.ToString(Line.WebOrderNumber), LineNo, Convert.ToString(Line.Prodno).Replace("\"", "\"\""),
                    Convert.ToString(Line.QtyOrd), Convert.ToString(Line.UOM).Replace("\"", "\"\""), Convert.ToString(Line.ProdDesc).Replace("\"", "\"\"")));

                LineNo++;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

        String[] Orders = new String[3] { csvHeader.ToString(), csvItem.ToString(), OrderIDs.ToString() };

        return Orders;
    }

    /// <summary>
    /// Adds the CSV file to the response.
    /// </summary>
    /// <param name="csvExportContents">The contents of the CSV file.</param>
    private void DownloadCSVFile()
    {
        String[] Orders = GenerateCSVFields();
        String Headers = Orders[0];
        String Items = Orders[1];
        String OrderIDs = Orders[2];

        Byte[] data = new ASCIIEncoding().GetBytes(Headers + Items);

        String strTime = DateTime.Now.ToString("MM-dd-yyyy_hh-mm_tt");
        String DisplayName = "Incentex_Orders_" + strTime + ".zip";
        String OriginalFile = "Incentex_Orders_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".zip";

        ZipFile zip = new ZipFile();
        zip.AddStringAsFile(Headers, "Incentex_Orders_Header_" + strTime + ".CSV", null);
        zip.AddStringAsFile(Items, "Incentex_Orders_Detail_" + strTime + ".CSV", null);
        zip.Save(Common.DownloadedPAASOrderPath + OriginalFile);

        DownloadedPAASOrder objDownloadedOrders = new DownloadedPAASOrder();
        objDownloadedOrders.DownloadedDate = DateTime.Now;
        objDownloadedOrders.DisplayName = DisplayName;
        objDownloadedOrders.OriginalFile = OriginalFile;
        objDownloadedOrders.UserInfoID = IncentexGlobal.CurrentMember.UserInfoID;

        PAASRepository objRepo = new PAASRepository();
        objRepo.Insert(objDownloadedOrders);
        objRepo.SubmitChanges();

        PAASRepository objPAAS = new PAASRepository();
        Int32 OrdersSent = objPAAS.UpdatePAASOrdersAsSent(OrderIDs, DisplayName);

        DownloadFile(Common.DownloadedPAASOrderPath + OriginalFile, DisplayName);
    }

    private void DownloadFile(String filePath, String displayName)
    {
        Stream iStream = null;

        try
        {
            // Open the file.
            iStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);

            // Total bytes to read:
            Byte[] data = new ASCIIEncoding().GetBytes(Convert.ToString(iStream));

            Response.AppendHeader("Content-Disposition", "attachment; filename=" + displayName + "");
            Response.ContentType = "application/zip";
            Response.OutputStream.Write(data, 0, data.Length);
            Response.WriteFile(filePath);
            Response.End();
        }
        catch (Exception ex)
        {
            // Trap the error, if any.
            Response.Write("Error : " + ex.Message);
        }
        finally
        {
            if (iStream != null)
            {
                //Close the file.
                iStream.Close();
            }
        }
    }

    private static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }
        return dt;
    }

    #endregion   
}