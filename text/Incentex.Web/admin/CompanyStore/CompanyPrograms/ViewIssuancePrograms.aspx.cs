/* Project Name : Incentex 
 * Module Name : Uniform Isuance Program 
 * Description : This page displays the list of Uniform Isuance Program 
 * ----------------------------------------------------------------------------------------- 
 * DATE | ID/ISSUE| AUTHOR | REMARKS 
 * ----------------------------------------------------------------------------------------- 
 * 23-Oct-2010 | 1 | Amit Trivedi | Design and Coding
 * ----------------------------------------------------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.Common;
using Incentex.DAL.SqlRepository;


public partial class admin_CompanyStore_CompanyPrograms_ViewIssuancePrograms : PageBase
{
    #region Properties

    Int64 CompanyStoreId
    {
        get
        {
            return Convert.ToInt64(ViewState["CompanyStoreId"]);
        }
        set
        {
            ViewState["CompanyStoreId"] = value;
        }
    }

    public int CurrentPage
    {
        get
        {
            
            //if (this.ViewState["CurrentPage"] == null)
            //    return 0;
            //else
                return Convert.ToInt32(this.ViewState["CurrentPage"]);
        }
        set
        {
            ViewState["CurrentPage"] = value;
        }
    }
    public int FrmPg
    {
        get
        {
            if (ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt32(ViewState["FrmPg"]);
        }
        set
        {
            ViewState["FrmPg"] = value;
        }
    }
    public int ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return 5;
            else
                return Convert.ToInt32(this.ViewState["ToPg"]);
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }
    UniformIssuancePolicyRepository objUniformIssuancePolicyRepository = new UniformIssuancePolicyRepository();
    LookupRepository objLookupRepository = new LookupRepository();
    PagedDataSource pds = new PagedDataSource();

    public string SortExp
    {
        get
        {
            if (this.ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = "WorkGroup";
            }

            return Convert.ToString(ViewState["SortExp"]);
        }
        set
        {
            ViewState["SortExp"] = value;
        }
    }

    public string SortDir
    {
        get
        {
            if (this.ViewState["SortDir"] == null)
            {
                ViewState["SortDir"] = " ASC";
            }

            return ViewState["SortDir"].ToString();
        }
        set
        {
            ViewState["SortDir"] = value;
        }
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Company Issuance Programs";
            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                this.CompanyStoreId = Convert.ToInt64(Request.QueryString["Id"]);
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/CompanyPrograms.aspx?Id=" + this.CompanyStoreId;
            menuControl.PopulateMenu(4, 1, this.CompanyStoreId, 0, true);
            gv.DataBind();
        }
    }

    #region Events

    /// <summary>
    /// click event Add new program
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAddUniformIssuanceProg_Click(object sender, EventArgs e)
    {
        Response.Redirect("UniformIssuanceStep1.aspx?Id=" + this.CompanyStoreId + "&SubId=0");
    }

    #region Paging
    /// <summary>
    /// change page to previous grid page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        gv.PageIndex = CurrentPage;
        gv.DataBind();
    }

    /// <summary>
    /// change event to next grid page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        gv.PageIndex = CurrentPage;
        gv.DataBind();
    }

    /// <summary>
    /// event for pager item databound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lstPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    /// <summary>
    /// handle events fired from pager control
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void lstPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
        gv.DataBind();
    }

    #endregion

    /// <summary>
    /// handle grid events
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        Int64 UniformIssuancePolicyID = 0;

           lblMsg.Text = "";
           switch (e.CommandName)
           {
               case "Sorting":
                  
                       if (this.SortExp.ToString().Equals(e.CommandArgument.ToString()))
                       {
                           if (this.SortDir == " ASC")
                           {
                               this.SortDir = " DESC";
                           }
                           else
                           {
                               this.SortDir = " ASC";
                           }
                       }
                       else
                       {
                           this.SortDir = " ASC";
                           this.SortExp = e.CommandArgument.ToString();
                       }
                       gv.DataBind();
                   break;
               case "EditRec":
                   UniformIssuancePolicyID = Convert.ToInt64(e.CommandArgument);
                   Response.Redirect("UniformIssuanceStep1.aspx?Id=" + this.CompanyStoreId + "&SubId=" + UniformIssuancePolicyID);
                   break;
               case "ChangeStatus":
                   UniformIssuancePolicyID = Convert.ToInt64(e.CommandArgument);

                   try
                   {
                      UniformIssuancePolicy objUniformIssuancePolicy = objUniformIssuancePolicyRepository.GetById(UniformIssuancePolicyID);
                      List<INC_Lookup> objLookupList = objLookupRepository.GetByLookupCode(DAEnums.LookupCodeType.Status);
                                              
                       
                       if(objLookupList.Count > 0)
                       {
                     
                     objUniformIssuancePolicy.Status = (from l in objLookupList
                                                               where l.iLookupID != (Int64)objUniformIssuancePolicy.Status
                                                            select l.iLookupID
                                                            ).FirstOrDefault();

                     objUniformIssuancePolicyRepository.SubmitChanges();
                     gv.DataBind();
                       }
                       
                   }
                   catch (System.Threading.ThreadAbortException lException)
                   {
                       //do nothing
                   }
                   catch(Exception ex)
                   {
                       ErrHandler.WriteError(ex);
                   }
                    
                   break;
           }
    }



    /// <summary>
    /// Bind Data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gv_DataBinding(object sender, EventArgs e)
    {
        List<UniformIssuancePolicy> objList = objUniformIssuancePolicyRepository.GetByStoreId(this.CompanyStoreId);

        DataTable dt = Common.ListToDataTable(objList);

        dt.Columns.Add("WorkGroup");
        //dt.Columns.Add("Department");
        dt.Columns.Add("EligibleDateDisp");
        dt.Columns.Add("CreditExpireDateDisp");
     
        foreach(DataRow dr in dt.Rows)
        {
            //objList[0].
            // Int64 DepartmentID = Convert.ToInt64(dr["DepartmentID"]);
            Int64 WorkgroupID = Convert.ToInt64(dr["WorkgroupID"]);

            dr["WorkGroup"] = objLookupRepository.GetById(WorkgroupID).sLookupName;
           // dr["Department"] = objLookupRepository.GetById(DepartmentID).sLookupName;
            if (!string.IsNullOrEmpty(dr["EligibleDate"].ToString()))
            {
                dr["EligibleDateDisp"] = (Convert.ToDateTime(dr["EligibleDate"])).ToShortDateString();
            }

            if (!string.IsNullOrEmpty(dr["CreditExpireDate"].ToString()))
            {
                dr["CreditExpireDateDisp"] = (Convert.ToDateTime(dr["CreditExpireDate"])).ToShortDateString();
            }

        }

        dt.DefaultView.Sort = this.SortExp + this.SortDir;
        pds.DataSource = dt.DefaultView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        gv.DataSource = pds;
        doPaging();
    }


    /// <summary>
    /// grid data bound event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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

    /// <summary>
    /// grid row command event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       if(e.Row.RowType == DataControlRowType.DataRow)
       {
           DataRowView objUniformIssuancePolicy = e.Row.DataItem as DataRowView;

           //Image imgStatus = e.Row.FindControl("imgStatus") as Image;
           ImageButton imgbtnStatus = e.Row.FindControl("imgbtnStatus") as ImageButton;

           Label lblExpiresAfter = e.Row.FindControl("lblExpiresAfter") as Label;

           Label lblEligibleDate = e.Row.FindControl("lblEligibleDate") as Label;

           if ( string.IsNullOrEmpty(objUniformIssuancePolicy["EligibleDate"].ToString()))
           {
               lblEligibleDate.Text = objUniformIssuancePolicy["NumberOfMonths"].ToString();
           }

           if (string.IsNullOrEmpty(objUniformIssuancePolicy["CreditExpireDate"].ToString()) )
           {
               lblExpiresAfter.Text = objUniformIssuancePolicy["CreditExpireNumberOfMonths"].ToString();
           }

           HiddenField hdnStatus = e.Row.FindControl("hdnStatus") as HiddenField;
           if (hdnStatus.Value != "")
           {
               INC_Lookup objLookup = objLookupRepository.GetById(Convert.ToInt64(hdnStatus.Value));

               if (objLookup != null)
               {
                   imgbtnStatus.ImageUrl = "~/admin/Incentex_Used_Icons/" + objLookup.sLookupIcon;
                   
               }
           }
           CheckBox chk = (CheckBox)e.Row.FindControl("chkDelete");
           TextBox txtDprt = (TextBox)e.Row.FindControl("txtDprt");
           String JSChecked = "SetTextboxEnable('" + txtDprt.ClientID + "','" + chk.ClientID + "');";
           chk.Attributes.Add("onclick", JSChecked);

       }
    }

    protected void LnkSaveGroupName_Click(object sender, EventArgs e)
    {
        Boolean IsSaveRecords = false;
        try
        {
            foreach (GridViewRow gr in gv.Rows)
            {
                CheckBox chkDelete = (CheckBox)gr.FindControl("chkDelete");
                Label lblID = (Label) gr.FindControl("lblID");
                TextBox txtDprt = (TextBox)gr.FindControl("txtDprt");
                if (chkDelete.Checked)
                {
                    objUniformIssuancePolicyRepository.UpdateUniformIssuancePolicyGroupNamebyID(Convert.ToInt64(lblID.Text), Convert.ToString(txtDprt.Text));
                    IsSaveRecords = true;
                }

            }
            gv.DataBind();
            if (IsSaveRecords)
                lblMsg.Text = "Selected records updated successfully ...";
            else
                lblMsg.Text = "Please select record to save group name ...";
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in records updated updattions ...";
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// delete record
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkDelete_Click(object sender, EventArgs e)
    {
        bool IsChecked = false;

        try
        {
            
            foreach (GridViewRow gr in gv.Rows)
            {
                CheckBox chkDelete = gr.FindControl("chkDelete") as CheckBox;
                Label lblID = gr.FindControl("lblID") as Label;

                if (chkDelete.Checked)
                {
                    
                    objUniformIssuancePolicyRepository.Delete(Convert.ToInt64(lblID.Text));
                    IsChecked = true;
                }

            }
            gv.DataBind();
            if (IsChecked)
            {
               // objUniformIssuancePolicyRepository.SubmitChanges();
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
        catch (Exception ex)
        {
            lblMsg.Text = "Error in deleting record ...";
            ErrHandler.WriteError(ex);
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// bind pager control
    /// </summary>
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


    #endregion


}
