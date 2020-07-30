using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;


public partial class admin_CompanyStore_CompanyPrograms_ViewAnniversaryPrograms : PageBase
{
    PagedDataSource pds = new PagedDataSource();
    Int64 CompanyStoreId
    {
        get
        {
            if (ViewState["CompanyStoreId"] == null)
            {
                ViewState["CompanyStoreId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {
            ViewState["CompanyStoreId"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString.Count > 0)
            {
                CheckLogin();

                this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));

                ((Label)Master.FindControl("lblPageHeading")).Text = "Company Anniversary Programs";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/CompanyStore/CompanyPrograms.aspx?id=" + this.CompanyStoreId;
                menuControl.PopulateMenu(4, 0, this.CompanyStoreId,0, true);
                bindgrid();
            }
        }
    }
    public void bindgrid()
    {
        DataView myDataView = new DataView();
        AnniversaryProgramRepository objStoreRep = new AnniversaryProgramRepository();
        List<Incentex.DAL.SqlRepository.AnniversaryProgramRepository.AnniversaryCreditProgramResults> objList = objStoreRep.GetCompanyProgramsByStoreId(this.CompanyStoreId);
        if (objList.Count == 0)
        {
            pagingtable.Visible = false;
            btnDelete.Visible = false;
        }
        else
        {
            pagingtable.Visible = true;
            btnDelete.Visible = true;
        }
        DataTable dataTable = ListToDataTable(objList);
        myDataView = dataTable.DefaultView;
        if (this.ViewState["SortExp"] != null)
        {
            myDataView.Sort = this.ViewState["SortExp"].ToString() + " " + this.ViewState["SortOrder"].ToString();
        }
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;

        gvCompanyProgram.DataSource = pds;
        gvCompanyProgram.DataBind();
        doPaging();
    }
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {
            dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null);
            }
            dt.Rows.Add(row);
        }
        return dt;
    }
    protected void btnAddAnniversaryProg_Click(object sender, EventArgs e)
    {
        Response.Redirect("AnniversaryProgram.aspx?id=" + this.CompanyStoreId + "&SubId=0");
    }
    protected void gvCompanyProgram_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Sort"))
        {
            if (this.ViewState["SortExp"] == null)
            {
                this.ViewState["SortExp"] = e.CommandArgument.ToString();
                this.ViewState["SortOrder"] = "ASC";
            }
            else
            {
                if (this.ViewState["SortExp"].ToString() == e.CommandArgument.ToString())
                {
                    if (this.ViewState["SortOrder"].ToString() == "ASC")
                        this.ViewState["SortOrder"] = "DESC";
                    else
                        this.ViewState["SortOrder"] = "ASC";

                }
                else
                {
                    this.ViewState["SortOrder"] = "ASC";
                    this.ViewState["SortExp"] = e.CommandArgument.ToString();
                }
            }
        }

        if (e.CommandName == "EditProgram")
        {
            GridViewRow row;
            row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            Response.Redirect("AnniversaryProgram.aspx?Id=" + ((Label)gvCompanyProgram.Rows[row.RowIndex].FindControl("lblStoreID")).Text + "&SubId=" + e.CommandArgument.ToString());

        }
        bindgrid();

        if (e.CommandName == "StatusVhange")
        {
            LinkButton lbtnAction;
            HiddenField hdnStatusID;
            Label lblAnniversaryCreditProgramID;

            GridViewRow row;
            row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            lbtnAction = (LinkButton)(gvCompanyProgram.Rows[row.RowIndex].FindControl("hypCategoryName"));
            lblAnniversaryCreditProgramID = (Label)(gvCompanyProgram.Rows[row.RowIndex].FindControl("lblAnniversaryCreditProgramID"));

            hdnStatusID = (HiddenField)(gvCompanyProgram.Rows[row.RowIndex].FindControl("hdnStatusID"));
            if (lblAnniversaryCreditProgramID.Text != "")
            {

                AnniversaryProgramRepository objStoreRep = new AnniversaryProgramRepository();
                objStoreRep.UpdateStatus(Convert.ToInt32(lblAnniversaryCreditProgramID.Text), Convert.ToInt32(hdnStatusID.Value));
                objStoreRep.SubmitChanges();
                    bindgrid();
                
            }

        }
        if (e.CommandName == "UpdateStatus")
        {
            CompanyStoreRepository objCompanyStore = new CompanyStoreRepository();
            AnniversaryCreditProgram objProgram = new AnniversaryCreditProgram();
            CompanyStore objStore = objCompanyStore.GetById(this.CompanyStoreId);
            objProgram = new AnniversaryProgramRepository().GetById(Convert.ToInt64(e.CommandArgument.ToString()));
            //Update CreditAmountToApplied from the company employee table
            UserInformationRepository objUserInfoRep = new UserInformationRepository();
            List<UserInformationRepository.SearchResults> objEmpl = new List<UserInformationRepository.SearchResults>();
            objEmpl = objUserInfoRep.getCompanyUsersForAnniversaryCredit("", "", "", "", "", objStore.CompanyID, objProgram.WorkgroupID, "EditAnniversary", Convert.ToDateTime(objProgram.CreatedDate));
            
            foreach (UserInformationRepository.SearchResults peremployee in objEmpl)
            {
                //Using Creaded SP Just For calculation Purpose.
                AnniversaryProgramRepository objUserData = new AnniversaryProgramRepository();
                SelectAnniversaryCreditProgramPerEmployeeResult objUserDataResult = objUserData.GetCompanyEmployeeAnniversaryCreditDetails(peremployee.UserInformationId);
                CompanyEmployeeRepository objCERep = new CompanyEmployeeRepository();
                CompanyEmployee objCE = objCERep.GetById(peremployee.CompanyEmployeeId);

                if (String.IsNullOrEmpty(objCE.CreditAmtToApplied.ToString()))
                {
                    if (objUserDataResult.IssueCreditOn <= System.DateTime.Now.Date)
                    {
                        objCE.CreditAmtToApplied = objProgram.StandardCreditAmount;
                        objCE.NextCreditAppliedOn = Convert.ToDateTime(objUserDataResult.IssueCreditOn).AddYears(1).ToShortDateString();
                        if (((DateTime)objUserDataResult.CreditExpireAfter).Year.ToString() == "9999")
                        {
                            objCE.CreditExpireOn = "---";
                            objCE.CreditAmtToExpired = 0;
                        }
                        else
                        {
                            objCE.CreditExpireOn = Convert.ToDateTime(objUserDataResult.CreditExpireAfter).AddYears(1).ToShortDateString();
                            objCE.CreditAmtToExpired = objProgram.StandardCreditAmount;
                        }
                        objCERep.SubmitChanges();
                    }
                    else if (objUserDataResult.IssueCreditOn > System.DateTime.Now.Date)
                    {
                        objCE.CreditAmtToApplied = objProgram.StandardCreditAmount;
                        objCE.NextCreditAppliedOn = Convert.ToDateTime(objUserDataResult.IssueCreditOn).ToShortDateString();
                        if (((DateTime)objUserDataResult.CreditExpireAfter).Year.ToString() == "9999")
                        {
                            objCE.CreditExpireOn = "---";
                            objCE.CreditAmtToExpired = 0;
                        }
                        else
                        {
                            objCE.CreditExpireOn = objUserDataResult.CreditExpireAfter.ToString();
                            objCE.CreditAmtToExpired = objProgram.StandardCreditAmount;
                        }
                       objCERep.SubmitChanges();
                    }
                }
            }
        }

    }
    protected void gvCompanyProgram_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            switch (this.ViewState["SortExp"].ToString())
            {
                case "Workgroup":
                    PlaceHolder placeholderWorkgroup = (PlaceHolder)e.Row.FindControl("placeholderWorkgroup");
                    break;
                case "Department":
                    PlaceHolder placeholderDepartment = (PlaceHolder)e.Row.FindControl("placeholderDepartment");
                    break;
                case "StandardCreditAmount":
                    PlaceHolder placeholderStandardCreditAmount = (PlaceHolder)e.Row.FindControl("placeholderStandardCreditAmount");
                    break;
                case "CreditExpiresIn":
                    PlaceHolder placeholderCreditExpiresIn = (PlaceHolder)e.Row.FindControl("placeholderCreditExpiresIn");
                    break;
                case "EmployeeStatus":
                    PlaceHolder placeholderProgramStatus = (PlaceHolder)e.Row.FindControl("placeholderProgramStatus");
                    //placeholdercontactnumber.Controls.Add(ImgSort);
                    break;

            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            string path = "../../../admin/Incentex_Used_Icons/" + ((HiddenField)e.Row.FindControl("hdnLookupIcon")).Value;
            ((HtmlImage)e.Row.FindControl("imgLookupIcon")).Src = path;
        }
    }
   
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            bool IsChecked = false;

            foreach (GridViewRow dsrow in gvCompanyProgram.Rows)
            {
                if (((CheckBox)dsrow.FindControl("chkStore")).Checked == true)
                {
                    IsChecked = true;
                    AnniversaryProgramRepository objStoreRep = new AnniversaryProgramRepository();
                    objStoreRep.DeleteProgram(Convert.ToInt64(((Label)dsrow.FindControl("lblAnniversaryCreditProgramID")).Text));
                }

            }

            if (IsChecked)
                lblmsg.Text = "Selected Records Deleted Successfully ...";
            else
                lblmsg.Text = "Please Select Record to delete ...";
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            if (((System.Data.SqlClient.SqlException)(ex)).Number == 547)
            {
                lblmsg.Text = "Unable to delete store as this record is used in other detail table";
            }
            ErrHandler.WriteError(ex);
        }
        bindgrid();
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
}
