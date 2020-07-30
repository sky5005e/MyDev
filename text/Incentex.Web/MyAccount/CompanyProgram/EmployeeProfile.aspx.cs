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
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;

public partial class MyAccount_CompanyProgram_EmployeeProfile : PageBase
{
    PagedDataSource pds = new PagedDataSource();
    Int64 EmployeeId
    {
        get
        {
            if (ViewState["EmployeeId"] == null)
            {
                ViewState["EmployeeId"] = 0;
            }
            return Convert.ToInt64(ViewState["EmployeeId"]);
        }
        set
        {
            ViewState["EmployeeId"] = value;
        }
    }
    Int64 EmployeeUserId
    {
        get
        {
            if (ViewState["EmployeeUserId"] == null)
            {
                ViewState["EmployeeUserId"] = 0;
            }
            return Convert.ToInt64(ViewState["EmployeeUserId"]);
        }
        set
        {
            ViewState["EmployeeUserId"] = value;
        }
    }

    public int CurrentPage
    {
        get
        {
            if (this.ViewState["CurrentPage"] == null)
                return 0;
            else
                return Convert.ToInt16(this.ViewState["CurrentPage"].ToString());
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }
    public int PagerSize
    {
        get
        {
            if (this.ViewState["PagerSize"] == null)
                return Convert.ToInt32(Application["GRIDPAGERSIZE"]);
            else
                return Convert.ToInt16(this.ViewState["PagerSize"].ToString());
        }
        set
        {
            this.ViewState["PagerSize"] = value;
        }
    }
    public int FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt16(this.ViewState["FrmPg"].ToString());
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }
    public int ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return PagerSize;
            else
                return Convert.ToInt16(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }
    AnniversaryProgramRepository.CompanyAnniversarySortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = AnniversaryProgramRepository.CompanyAnniversarySortExpType.OrderNumber;
            }
            return (AnniversaryProgramRepository.CompanyAnniversarySortExpType)ViewState["SortExp"];
        }
        set
        {
            ViewState["SortExp"] = value;
        }
    }

    Incentex.DAL.Common.DAEnums.SortOrderType SortOrder
    {
        get
        {
            if (ViewState["SortOrder"] == null)
            {
                ViewState["SortOrder"] = Incentex.DAL.Common.DAEnums.SortOrderType.Asc;
            }
            return (Incentex.DAL.Common.DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            if (Request.QueryString.Count > 0)
            {
                this.EmployeeId = Convert.ToInt64(Request.QueryString.Get("Id"));
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = Session["backlink"].ToString();
                ((Label)Master.FindControl("lblPageHeading")).Text = "View Profile";
                DisplayData(sender, e);
            }
            else
            {
                Response.Redirect("~/index.aspx");
            }
        }
    }
    void DisplayData(object sender, EventArgs e)
    {

        if (this.EmployeeId != 0)
        {
            
            CompanyEmployee objCompanyEmployee = new CompanyEmployeeRepository().GetById(this.EmployeeId);
            
            if (objCompanyEmployee == null)
            {
                return;
            }
            //Get and set data from userinformation table
            UserInformation objUserInfo = new UserInformationRepository().GetById(objCompanyEmployee.UserInfoID);
            //lblFullName.Text = objUserInfo.FirstName +" "+objUserInfo.LastName;
            lblEmployeeName.Text = objUserInfo.FirstName + " " + objUserInfo.LastName;
            lblAddress.Text = objUserInfo.Address1 + " " + objUserInfo.Address2;
            lblCurrentCreditBalance.Text = new AnniversaryProgramRepository().GetCompanyEmployeeAnniversaryCreditDetails(objUserInfo.UserInfoID).CurrentBalance.ToString();
            lblZip.Text = objUserInfo.ZipCode;
            lblTelephone.Text = objUserInfo.Telephone;
            lblMobile.Text = objUserInfo.Mobile;
            lblDOH.Text = Convert.ToDateTime(objCompanyEmployee.HirerdDate).ToString("MM/dd/yyyy");
            lblEmployeeID.Text = objCompanyEmployee.EmployeeID;


            INC_Country objCountry = new CountryRepository().GetById((objUserInfo.CountryId.Value == null ? -1 :objUserInfo.CountryId.Value) );
            lblCountry.Text = objCountry.sCountryName;
            
            INC_State objState = new StateRepository().GetById((objUserInfo.StateId.Value == null ? -1 : objUserInfo.StateId.Value));
            lblState.Text = objState.sStatename;
            
            INC_City objCity = new CityRepository().GetById((objUserInfo.CityId.Value == null ? -1 : objUserInfo.CityId.Value));
            lblCity.Text = objCity.sCityName;

            INC_Lookup objLookup = new LookupRepository().GetById((long)objUserInfo.WLSStatusId);
            lblStatus.Text = objLookup.sLookupName;
            this.EmployeeUserId = objCompanyEmployee.UserInfoID; 

            bindgrid();

        }

    }
    public void bindgrid()
    {
        DataView myDataView = new DataView();
        //Bind Order History
        List<SelectOrderHistoryPerEmployeeResult> objList = new AnniversaryProgramRepository().GetOrderHistoryByEmployee(this.EmployeeUserId, this.SortExp, this.SortOrder, "Anniversary");
        
        if (objList.Count == 0)
        {
            pagingtable.Visible = false;
            lblMessage.Visible = true;
            
        }
        else
        {
            pagingtable.Visible = true;
            lblMessage.Visible = false;
        }
        DataTable dataTable = ListToDataTable(objList);
        myDataView = dataTable.DefaultView;
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;

        gvEmployeeOrderAnniversary.DataSource = pds;
        gvEmployeeOrderAnniversary.DataBind();
        doPaging();
        
        
    }

    //Changed on 3feb to Allow Nullble Property type in the DataTable..
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            //table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);

            dt.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);
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

    protected void gvEmployeeOrderAnniversary_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("OrderDetail"))
        {
            Response.Redirect("OrderDetail.aspx?id=" + e.CommandArgument.ToString() +"&from=ep");
        }
    }

    #region Paging

    protected void dtlPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            bindgrid();
        }
    }
    protected void dtlPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
    
    private void doPaging()
    {
        try
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            int CurrentPg = pds.CurrentPageIndex + 1;

            if (CurrentPg > ToPg)
            {
                FrmPg = ToPg + 1;
                ToPg = ToPg + PagerSize;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - PagerSize;

            }

            if (pds.PageCount < ToPg)
                ToPg = pds.PageCount;

            for (int i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            dtlPaging.DataSource = dt;
            dtlPaging.DataBind();

        }
        catch (Exception)
        { }
    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        bindgrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        bindgrid();
    }
    #endregion

    protected void gvEmployeeOrderAnniversary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((Label)e.Row.FindControl("lblCreditUsed")).Text != null || ((Label)e.Row.FindControl("lblCreditUsed")).Text != "")
            {
                if (((Label)e.Row.FindControl("lblCreditUsed")).Text == "Previous")
                {
                    ((Label)e.Row.FindControl("lblCreditUsed")).Text = "Starting Credits";
                }
                else if (((Label)e.Row.FindControl("lblCreditUsed")).Text == "Anniversary")
                {
                    ((Label)e.Row.FindControl("lblCreditUsed")).Text = "Anniversary Credit";
                }
                else
                {
                    ((Label)e.Row.FindControl("lblCreditUsed")).Text = "---";
                }
            }
            else
            {
                ((Label)e.Row.FindControl("lblCreditUsed")).Text = "---";
            }

        }
    }
}
