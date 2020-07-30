using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;

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
                return 5;
            else
                return Convert.ToInt16(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }
    #endregion
    CompanyStationRepository objCompanyStationRepo = new CompanyStationRepository();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Stations";
            base.ParentMenuID = 11;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            if (Request.QueryString.Count > 0)
            {
                ((HtmlGenericControl)manuControl.FindControl("dvSubMenu")).Visible = false;
              
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                Company objCompany = new CompanyRepository().GetById(this.CompanyId);
                if (objCompany == null)
                {
                    Response.Redirect("~/admin/Company/ViewCompany.aspx");
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = objCompany.CompanyName + " Stations";
                //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to company listing</span>";
               // ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Company/ViewCompany.aspx";
           
                lnkAddStation.NavigateUrl = "MainStationInfo.aspx?Id=" + this.CompanyId + "&SubId=0";
                gv.DataBind();
                manuControl.PopulateMenu(3, 0, this.CompanyId, this.StationId,false);
            }
            else
            {
                Response.Redirect("~/admin/Company/ViewCompany.aspx");
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
                ToPg = ToPg + 5;
            }
            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - 5;

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

            CompanyStationUserInformationRepository objStationUserRepo = new CompanyStationUserInformationRepository();
            CompanyStationUserInformation objUser;
            
            List<SelectCompanyStationStatusResult> objlistStatus;
            Label lblID = e.Row.FindControl("lblID") as Label;

            Int64 StationId = Convert.ToInt64(lblID.Text);

            //Label lblCountry = e.Row.FindControl("lblCountry") as Label;
            Label lblStationManager = e.Row.FindControl("lblStationManager") as Label;
            Label lblStationAdmin = e.Row.FindControl("lblStationAdmin") as Label;

            HyperLink lnkEdit = e.Row.FindControl("lnkEdit") as HyperLink;
             objUser =  objStationUserRepo.GetByStationId( Convert.ToInt64(lblID.Text), "manager");
             if (objUser != null)
             {
                 lblStationManager.Text = objUser.FirstName + " " + objUser.LastName;
             }
             else
             {
                 lblStationManager.Text = "&nbsp";
             }

            objUser = objStationUserRepo.GetByStationId(Convert.ToInt64(lblID.Text), "admin");
            if (objUser != null)
            {
                lblStationAdmin.Text = objUser.FirstName + " " + objUser.LastName;
            }
            else
            {
                lblStationAdmin.Text = "&nbsp";
            }
            HiddenField hdnActice=e.Row.FindControl("hdnStatusID") as HiddenField;
            HiddenField hdnLookupIcon = e.Row.FindControl("hdnLookupIcon") as HiddenField;
            if (hdnActice.Value != "")
            {
                objlistStatus = objCompanyStationRepo.SelectStationStatus(Convert.ToInt32(hdnActice.Value));
                hdnLookupIcon.Value = objlistStatus[0].sLookupIcon.ToString();
            }
           
            lnkEdit.NavigateUrl = "~/admin/Company/Station/MainStationInfo.aspx?Id=" + this.CompanyId + "&SubId=" + lblID.Text ;

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            string path = "../../../admin/Incentex_Used_Icons/" + ((HiddenField)e.Row.FindControl("hdnLookupIcon")).Value;
            ((HtmlImage)e.Row.FindControl("imgLookupIcon")).Src = path;
        }
    }


    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        bool IsChecked = false;

        try
        {
            if (!base.CanDelete)
            {
                base.RedirectToUnauthorised();
            }

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
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
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
