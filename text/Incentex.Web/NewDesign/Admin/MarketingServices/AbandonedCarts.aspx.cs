using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
using Incentex.DAL.Common;
using System.Globalization;
using System.IO;
using System.Text;

public partial class Admin_AbandonedCarts : PageBase
{

    #region DataMembers
    PagedDataSource pds = new PagedDataSource();
    #endregion

    #region Page Level Variables
    string CurrModule = "Marketing Services";
    string CurrSubMenu = "Abandoned Carts";
    #endregion

    #region Properties & Fields

    public Int64? CompanyID
    {
        get
        {
            if (this.ViewState["CompanyID"] == null || Convert.ToString(this.ViewState["CompanyID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["CompanyID"]);
        }
        set
        {
            this.ViewState["CompanyID"] = value;
        }
    }

    public Int64? UserID
    {
        get
        {
            if (this.ViewState["UserID"] == null || Convert.ToString(this.ViewState["UserID"]) == "0")
                return null;
            else
                return Convert.ToInt64(this.ViewState["UserID"]);
        }
        set
        {
            this.ViewState["UserID"] = value;
        }
    }

    public String FromDate
    {
        get
        {
            if (Convert.ToString(this.ViewState["FromDate"]) == string.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["FromDate"]);
        }
        set
        {
            this.ViewState["FromDate"] = value;
        }
    }

    public String ToDate
    {
        get
        {
            if (Convert.ToString(this.ViewState["ToDate"]) == string.Empty)
                return null;
            else
                return Convert.ToString(this.ViewState["ToDate"]);
        }
        set
        {
            this.ViewState["ToDate"] = value;
        }
    }

    public String KeyWord
    {
        get
        {
            return Convert.ToString(this.ViewState["KeyWord"]);
        }
        set
        {
            this.ViewState["KeyWord"] = value;
        }
    }
        

    public Int32 NoOfRecordsToDisplay
    {
        get
        {
            if (Convert.ToString(this.ViewState["NoOfRecordsToDisplay"]) == "")
                return Convert.ToInt32(Application["NEWPAGESIZE"]);
            else
                return Convert.ToInt32(this.ViewState["NoOfRecordsToDisplay"]);
        }
        set
        {
            this.ViewState["NoOfRecordsToDisplay"] = value;
        }
    }

    public Int32 PageIndex
    {
        get
        {
            if (Convert.ToString(this.ViewState["PageIndex"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["PageIndex"]);
        }
        set
        {
            this.ViewState["PageIndex"] = value;
        }
    }

    public String SortColumn
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortColumn"]) == String.Empty)
                return "FirstName";
            else
                return Convert.ToString(this.ViewState["SortColumn"]);
        }
        set
        {
            this.ViewState["SortColumn"] = value;
        }
    }

    public String SortDirection
    {
        get
        {
            if (Convert.ToString(this.ViewState["SortDirection"]) == String.Empty)
                return "ASC";
            else
                return Convert.ToString(this.ViewState["SortDirection"]);
        }
        set
        {
            this.ViewState["SortDirection"] = value;
        }
    }

    public Int32 TotalPages
    {
        get
        {
            if (Convert.ToString(this.ViewState["TotalPages"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["TotalPages"]);
        }
        set
        {
            this.ViewState["TotalPages"] = value;
        }
    }

    public Int32 NoOfPagesToDisplay
    {
        get
        {
            if (Convert.ToString(this.ViewState["NoOfPagesToDisplay"]) == "")
                return Convert.ToInt32(Application["NEWPAGERSIZE"]);
            else
                return Convert.ToInt32(this.ViewState["NoOfPagesToDisplay"]);
        }
        set
        {
            this.ViewState["NoOfPagesToDisplay"] = value;
        }
    }

    public Int32 FromPage
    {
        get
        {
            if (Convert.ToString(this.ViewState["FromPage"]) == "")
                return 1;
            else
                return Convert.ToInt32(this.ViewState["FromPage"]);
        }
        set
        {
            this.ViewState["FromPage"] = value;
        }
    }

    public Int32 ToPage
    {
        get
        {
            if (Convert.ToString(this.ViewState["ToPage"]) == "")
                return this.NoOfPagesToDisplay;
            else
                return Convert.ToInt32(this.ViewState["ToPage"]);
        }
        set
        {
            this.ViewState["ToPage"] = value;
        }
    }

    public Int32? MyShoppingCartID
    {
        get
        {
            if (Convert.ToString(this.ViewState["PageErrorID"]) != "")
                return Convert.ToInt32(this.ViewState["PageErrorID"]);
            else
                return null;
        }
        set
        {
            this.ViewState["PageErrorID"] = value;
        }
    }

    #endregion

    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                base.StartStopwatch();

                BindSearchDropDowns();

                base.EndStopwatch("Page Load", CurrModule, CurrSubMenu);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    /// <summary>
    /// Row Command Event of Grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvAbandonedCart_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Sort")
            {
                if (this.SortColumn == null)
                {
                    this.SortColumn = Convert.ToString(e.CommandArgument);
                    this.SortDirection = "ASC";
                }
                else
                {
                    if (this.SortColumn == Convert.ToString(e.CommandArgument))
                    {
                        if (this.SortDirection == "ASC")
                            this.SortDirection = "DESC";
                        else
                            this.SortDirection = "ASC";
                    }
                    else
                    {
                        this.SortDirection = "ASC";
                        this.SortColumn = Convert.ToString(e.CommandArgument);
                    }
                }                

                BindAbandonedCartGrid(true);
            }
            else if (e.CommandName == "CustomerName")
            {
                //this.MyShoppingCartID = Convert.ToInt32(e.CommandArgument);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "MySript", "ShowPopUp('system-data-popup', 'warranty-content');", true);   
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }    

    /// <summary>
    /// Search event 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearchAbdnCartDate_Click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            this.CompanyID = Convert.ToInt64(ddlCompany.SelectedValue);
            this.UserID = Convert.ToInt64(ddlCustomerName.SelectedValue);
            
            if (ddlDate.SelectedValue == "0")
            {
                this.FromDate = this.ToDate = string.Empty;
            }
            else if (ddlDate.SelectedValue == "1")
            {
                this.FromDate = this.ToDate = DateTime.Now.ToString("MM/dd/yyyy");
            }
            else if (ddlDate.SelectedValue == "2")
            {
                this.FromDate = this.ToDate = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy");
            }
            else if (ddlDate.SelectedValue == "3")
            {
                this.FromDate = DateTime.Now.AddDays(-7).ToString("MM/dd/yyyy");
                this.ToDate = DateTime.Now.ToString("MM/dd/yyyy");
            }
            else if (ddlDate.SelectedValue == "4")
            {
                this.FromDate = txtFromDate.Text;
                this.ToDate = txtToDate.Text;
            }

            this.KeyWord = String.Empty;
            this.NoOfRecordsToDisplay = Convert.ToInt32(Application["NEWPAGESIZE"]);
            this.PageIndex = 1;
            this.ToPage = this.NoOfPagesToDisplay;
            this.FromPage = 1;

            btnSearchGrid.Enabled = true;
            btnSearchGrid.CssClass += " postback";
            txtSearchGrid.Text = String.Empty;

            BindAbandonedCartGrid(false);

            base.EndStopwatch("Search", CurrModule, CurrSubMenu);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnSend_click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            string SelectedRows = string.Empty;
            string Email, UserName, MyShoppCartID;
            long UserInfoId;
            for (int i = 0; i < gvAbandonedCart.Rows.Count; i++)
            {
                if ((gvAbandonedCart.Rows[i].Cells[0].FindControl("chkSelect") as CheckBox).Checked)
                {
                    Email = (gvAbandonedCart.Rows[i].Cells[0].FindControl("hdnEmail") as HiddenField).Value;
                    UserInfoId = Convert.ToInt32((gvAbandonedCart.Rows[i].Cells[0].FindControl("hdnUserInfoID") as HiddenField).Value);
                    MyShoppCartID = (gvAbandonedCart.Rows[i].Cells[0].FindControl("hdnMyShoppingCartID") as HiddenField).Value;
                    //UserName = (gvAbandonedCart.Rows[i].Cells[5].FindControl("lblCustomerName") as Label).Text;
                    UserName = (gvAbandonedCart.Rows[i].Cells[5].FindControl("lblFirstName") as Label).Text;
                    //sendVerificationEmail(UserInfoId, "navin.valera@indianic.com", UserName, MyShoppCartID);
                    sendVerificationEmail(UserInfoId, Email, UserName, MyShoppCartID, CurrModule, CurrSubMenu);
                }
            }
            base.EndStopwatch("Send Email", CurrModule, CurrSubMenu);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void btnSearchGrid_Click(object sender, EventArgs e)
    {
        try
        {
            base.StartStopwatch();

            this.KeyWord = txtSearchGrid.Text;
            BindAbandonedCartGrid(false);

            base.EndStopwatch("Search by Keyword", CurrModule, CurrSubMenu);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkPrevious_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex -= 1;
            BindAbandonedCartGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkNext_Click(Object sender, EventArgs e)
    {
        try
        {
            this.PageIndex += 1;
            BindAbandonedCartGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }    


    protected void lnkViewAll_Click(object sender, EventArgs e)
    {
        try
        {
            this.PageIndex = 1;
            this.NoOfRecordsToDisplay = 0;
            BindAbandonedCartGrid(false);
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Methods

    protected void BindSearchDropDowns()
    {
        try
        {
            IncentexBEDataContext db = new IncentexBEDataContext();

            //Bind Company Names
            CompanyRepository objCompanyRepository = new CompanyRepository();
            List<Company> objCompanyList = new List<Company>();

            objCompanyList = objCompanyRepository.GetAllCompany();
            Common.BindDropDown(ddlCompany, objCompanyList, "CompanyName", "CompanyID", "- Company -");

            LookupRepository objLookupRepository = new LookupRepository();
            List<INC_Lookup> objList = new List<INC_Lookup>();
            String LookUpCode = string.Empty;

            //Bind User Names
            var objUsers = (from usr in db.UserInformations where usr.IsDeleted == false select new { FirstName = usr.FirstName + " " + usr.LastName, usr.UserInfoID }).OrderBy(x => x.FirstName).ToList();
            ddlCustomerName.DataSource = objUsers;
            ddlCustomerName.DataTextField = "FirstName";
            ddlCustomerName.DataValueField = "UserInfoID";
            ddlCustomerName.DataBind();
            ddlCustomerName.Items.Insert(0, new ListItem("- Customer -", "0"));
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void BindAbandonedCartGrid(Boolean isForSorting)
    {
        try
        {
            AbandonedCartRepository ObjAbnCartRepo = new AbandonedCartRepository();
            List<GetAbandonedCartsResult> lstAbndCartResult = ObjAbnCartRepo.GetAbandonedCart(this.CompanyID, string.Empty, this.UserID,this.FromDate, this.ToDate, this.KeyWord, this.NoOfRecordsToDisplay, this.PageIndex, this.SortColumn, this.SortDirection);

            if (lstAbndCartResult != null && lstAbndCartResult.Count > 0)
            {
                if (this.NoOfRecordsToDisplay > 0)
                    this.TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(lstAbndCartResult[0].TotalRecords) / Convert.ToDecimal(this.NoOfRecordsToDisplay)));
                else
                    this.TotalPages = 1;

                lstAbndCartResult = lstAbndCartResult.Select(x => new GetAbandonedCartsResult
                {
                    TotalRecords = x.TotalRecords.Value,
                    MyShoppingCartID = x.MyShoppingCartID,
                    CompanyName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.CompanyName),
                    FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.FirstName.Split(' ')[0]) + " " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(x.FirstName.Split(' ')[1]),
                    CreatedDate = x.CreatedDate,
                    Location = x.Location,
                    RowNum = x.RowNum.Value,
                    Email = x.Email,
                    UnitPrice = x.UnitPrice,
                    Total = x.Total,
                    UserInfoID = x.UserInfoID
                }).ToList();
            }

            lstAbndCartResult.Add(new GetAbandonedCartsResult() { });

            gvAbandonedCart.DataSource = lstAbndCartResult;
            gvAbandonedCart.DataBind();

            if (gvAbandonedCart.Rows.Count > 0)
            {
                gvAbandonedCart.Rows[gvAbandonedCart.Rows.Count - 1].Cells[0].Text = "";
                gvAbandonedCart.Rows[gvAbandonedCart.Rows.Count - 1].Cells[gvAbandonedCart.Rows[0].Cells.Count - 1].Text = lstAbndCartResult[0].Total.Value.ToString("00.00");
            }

            if (!isForSorting)
            {
                GeneratePaging();
                lnkPrevious.Enabled = this.PageIndex > 1;
                lnkNext.Enabled = this.PageIndex < this.TotalPages;
            }

            if (gvAbandonedCart.Rows.Count > 0)
            {
                totalcount_em.InnerText = lstAbndCartResult[0].TotalRecords + " results";
                pagingtable.Visible = true;
                btnSend.Visible = true;
            }
            else
            {
                totalcount_em.InnerText = "no results";                
                pagingtable.Visible = false;
                btnSend.Visible = false;
            }

            totalcount_em.Visible = true;
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    private void sendVerificationEmail(Int64 UserInfoID, String UserEmail, String UserName, String myShoppingCartID, String ModuleName, String MenuName)
    {
        try
        {
            string sFrmadd = "support@world-link.us.com";
            string sToadd = UserEmail.Trim();
            string sSubject = "Incentex - Pending Items in Shopping Cart";
            string sFrmname = "Incentex Order Processing Team";
            string smtphost = Application["SMTPHOST"].ToString();
            int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
            string smtpUserID = Application["SMTPUSERID"].ToString();
            string smtppassword = Application["SMTPPASSWORD"].ToString();

            String eMailTemplate = String.Empty;

            StreamReader _StreamReader;
            _StreamReader = System.IO.File.OpenText(Server.MapPath("~/emailtemplate/SimpleMailFormat.htm"));
            eMailTemplate = _StreamReader.ReadToEnd();
            _StreamReader.Close();
            _StreamReader.Dispose();

            string body = null;

            body = "You have items in your cart.  Please complete your order or delete items from your cart. <br />  If you need further assistance please contact Customer Service at 772-453-2759 or support@incentex.com.";

            //body += "If you would like to delete the items in your cart please click on the button \"Delete Items\" and it will clear out your shopping cart.<br/>";

            ////Set Conformation Button
            //string buttonText = "<br/>";
            //buttonText += "<table style='font-size: 12px; font-family: Arial, Helvetica, sans-serif; line-height: 17px;color: #666' width='100%' border='0'>";
            //buttonText += "<tr>";
            //buttonText += "<td>";
            //buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "My Cart/MyShoppinCart.aspx'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/viewcart_order_btn.png' alt='View Cart' border='0'/></a>";
            //buttonText += "<td>";
            //buttonText += "<td>";
            //buttonText += "<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "admin/CommunicationCenter/DelMyShoppingCart.aspx?Id=" + myShoppingCartID + "&UserinfoID=" + UserInfoID + "'><img src='" + ConfigurationSettings.AppSettings["siteurl"] + "Images/dlt_items_btn.png' alt='Delete items' border='0'/></a>";
            //buttonText += "<td>";
            //buttonText += "</tr>";
            //buttonText += "</table><br/>";
            //body += buttonText;

            //body += "Thank you for your business!";
            StringBuilder MessageBody = new StringBuilder(eMailTemplate);
            MessageBody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
            MessageBody.Replace("{FullName}", String.IsNullOrEmpty(UserName.Trim()) ? "User" : UserName);
            MessageBody.Replace("{MessageBody}", body);
            MessageBody.Replace("{Sender}", sFrmname);

            if (HttpContext.Current.Request.IsLocal)
            {
                new CommonMails().SendEmail4Local(1092, "Testing", "navin.valera@indianic.com", sSubject, MessageBody.ToString(), "incentextest6@gmail.com", "test6incentex", true, CurrModule, CurrSubMenu);
            }
            else
                new CommonMails().SendMail(UserInfoID, "Pending Shopping Cart", sFrmadd, sToadd, sSubject, MessageBody.ToString(), sFrmname, smtphost, smtpport, false, true);


        }
        catch (Exception ex)
        {
        }

    }    
    #endregion

    #region Paging in the System Access GridView

    private void GeneratePaging()
    {
        try
        {
            Int32 tCurrentPg = this.PageIndex;
            Int32 tToPg = this.ToPage;
            Int32 tFrmPg = this.FromPage;

            if (tCurrentPg > tToPg)
            {
                tFrmPg = tToPg + 1;
                tToPg += this.NoOfPagesToDisplay;
            }

            if (tCurrentPg < tFrmPg)
            {
                tToPg = tFrmPg - 1;
                tFrmPg -= this.NoOfPagesToDisplay;
            }

            if (tToPg > this.TotalPages)
                tToPg = this.TotalPages;

            this.ToPage = tToPg;
            this.FromPage = tFrmPg;

            DataTable dt = new DataTable();
            dt.Columns.Add("Index");

            for (Int32 i = tFrmPg; i <= tToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr["Index"] = i;
                dt.Rows.Add(dr);
            }

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }    
    

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "ChangePage")
            {
                this.PageIndex = Convert.ToInt32(e.CommandArgument);
                BindAbandonedCartGrid(false);                
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion    
}
