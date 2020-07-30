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
using System.Xml.Linq;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
//using Incentex.DA;
//using Incentex.BE;
using Incentex.DAL.Common;

public partial class admin_Company_Station_ViewStation : PageBase
{
    #region Properties

    const int PageSize = 10;

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
    CompanyStationRepository.StationSortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = SupplierRepository.SupplierSortExpType.FirstName;
            }
            return (CompanyStationRepository.StationSortExpType)ViewState["SortExp"];
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
                ViewState["SortOrder"] = DAEnums.SortOrderType.Asc;
            }
            return (DAEnums.SortOrderType)ViewState["SortOrder"];
        }
        set
        {
            ViewState["SortOrder"] = value;
        }
    }

    Int64 CompanyId
    {
        get
        {
            if (ViewState["CompanyId"] == null)
            {
                ViewState["CompanyId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyId"]);
        }
        set
        {
            ViewState["CompanyId"] = value;
        }
    }

    Int64 StationId
    {
        get
        {
            if (ViewState["StationId"] == null)
            {
                ViewState["StationId"] = 0;
            }
            return Convert.ToInt64(ViewState["StationId"]);
        }
        set
        {
            ViewState["StationId"] = value;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            if (Request.QueryString.Count > 0)
            {
                
              
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                Company objCompany = new CompanyRepository().GetById(this.CompanyId);
                if (objCompany == null)
                {
                    Response.Redirect("~/MyAccount/UserManagement.aspx");
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = objCompany.CompanyName + " Stations";
                //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to company listing</span>";
               // ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/UserManagement.aspx";
           
                lnkAddStation.NavigateUrl = "MainStationInfo.aspx?Id=" + this.CompanyId + "&SubId=0";
                gv.DataBind();
                //manuControl.PopulateMenu(3, 0, this.CompanyId, this.StationId,false);
            }
            else
            {
                Response.Redirect("~/MyAccount/UserManagement.aspx");
            }
                        
        }
    }

    PagedDataSource pds = new PagedDataSource();


    protected void gv_DataBinding(object sender, EventArgs e)
    {
        /*
        DataTable dt = new DataTable();
        dt.Columns.Add("StationID");
        dt.Columns.Add("StationCode");
        dt.Columns.Add("Country");
        dt.Columns.Add("StationManager");
        dt.Columns.Add("StationAdmin");

        for (int i = 1; i <= 4; i++)
        {
            DataRow dr = dt.NewRow();

            dr["ID"] = i;
            dr["StationName"] = "StationName" + i;
            dr["Country"] = "Country" + i;
            dr["StationManager"] = "StationManager" + i;
            dr["StationAdmin"] = "StationAdmin" + i;

            dt.Rows.Add(dr);
        }

        gv.DataSource = dt;
         */


        CompanyStationRepository objCompanyStationRepo = new CompanyStationRepository();
         var objList = objCompanyStationRepo.GetByCompanyId(this.CompanyId,this.SortExp,this.SortOrder);

         pds.DataSource = objList;
         pds.AllowPaging = true;
         pds.PageSize = PageSize;
         pds.CurrentPageIndex = CurrentPage;
         lnkbtnNext.Enabled = !pds.IsLastPage;
         lnkbtnPrevious.Enabled = !pds.IsFirstPage;
         gv.DataSource = pds;

         doPaging(); 

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

            lstPaging.DataSource = dt;
            lstPaging.DataBind();

        }
        catch (Exception)
        { }
    }


    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if(e.Row.RowType == DataControlRowType.DataRow  )
        {
            Label lblID = e.Row.FindControl("lblID") as Label;

            Int64 StationId = Convert.ToInt64(lblID.Text);

            //Label lblCountry = e.Row.FindControl("lblCountry") as Label;
            Label lblStationManager = e.Row.FindControl("lblStationManager") as Label;
            Label lblStationAdmin = e.Row.FindControl("lblStationAdmin") as Label;

            HyperLink lnkEdit = e.Row.FindControl("lnkEdit") as HyperLink;

            CompanyStationUserInformationRepository objStationUserRepo = new CompanyStationUserInformationRepository();
            CompanyStationUserInformation objUser =  objStationUserRepo.GetByStationId( Convert.ToInt64(lblID.Text), "manager");
            if (objUser != null)
            {
                lblStationManager.Text = objUser.FirstName + " " + objUser.LastName;
            }

            objUser = objStationUserRepo.GetByStationId(Convert.ToInt64(lblID.Text), "admin");
            if (objUser != null)
            {
                lblStationAdmin.Text = objUser.FirstName + " " + objUser.LastName;
            }


            lnkEdit.NavigateUrl = "~/MyAccount/Station/MainStationInfo.aspx?Id=" + this.CompanyId + "&SubId=" + lblID.Text;

        }
    }


    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        bool IsChecked = false;

        try
        {

            CompanyStationRepository objRepo = new CompanyStationRepository();
            
            foreach (GridViewRow gr in gv.Rows)
            {
                CheckBox chkDelete = gr.FindControl("chkDelete") as CheckBox;
                Label lblID = gr.FindControl("lblID") as Label;

                if (chkDelete.Checked)
                {
                    //CompanyStation obj = objRepo.GetById ( Convert.ToInt64(lblID.Text));
                    //objRepo.Delete(obj);
                    objRepo.Delete(Convert.ToInt64(lblID.Text));
                    IsChecked = true;
                }

            }
            gv.DataBind();
            if (IsChecked)
            {
                objRepo.SubmitChanges();
                lblMsg.Text = "Selected Records Deleted Successfully ...";
            }
            else
            {
                lblMsg.Text = "Please Select Record to delete ...";
            }
        }
        catch(Exception ex)
        {
            lblMsg.Text = "Error in deleting record ...";
            ErrHandler.WriteError(ex);
        }

    }
    protected void gv_DataBound(object sender, EventArgs e)
    {
        if (gv.Rows.Count == 0)
        {
            lnkDelete.Visible = false;
            lstPaging.Visible = false;
            lnkbtnNext.Visible = false;
            lnkbtnPrevious.Visible = false;
        }
        else
        {
            lnkDelete.Visible = true;
            lstPaging.Visible = true;
            lnkbtnNext.Visible = true;
            lnkbtnPrevious.Visible = true;
        }
    }

    protected void lstPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            gv.DataBind();
            //bindGridView();
        }
    }
    protected void lstPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        gv.PageIndex = CurrentPage;
        gv.DataBind();
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        gv.PageIndex = CurrentPage;
        gv.DataBind();
    }

    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblMsg.Text = "";
        switch (e.CommandName)
        {

            case "Sorting":

                if (this.SortExp.ToString() == e.CommandArgument.ToString())
                {
                    if (this.SortOrder == DAEnums.SortOrderType.Asc)
                        this.SortOrder = DAEnums.SortOrderType.Desc;
                    else
                        this.SortOrder = DAEnums.SortOrderType.Asc;
                }
                else
                {
                    this.SortOrder = DAEnums.SortOrderType.Asc;
                    this.SortExp = (CompanyStationRepository.StationSortExpType)Enum.Parse(typeof(CompanyStationRepository.StationSortExpType), e.CommandArgument.ToString());
                }
                gv.DataBind();
                break;
        }
    }
}
