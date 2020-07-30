/// <summary>
/// Module Name : State Management
/// Description : This page is for add/edit/delete state from country.
/// Created : Mayur on 21-jan-2012
/// </summary>

using System;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_State : PageBase
{
    #region Data Member
    DataSet ds;
    CountryDA objCountry = new CountryDA();
    StateDA objState = new StateDA();
    PagedDataSource pds = new PagedDataSource();
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
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Add State";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "dropdownmenus.aspx";

            //bind country dropdown
            FillCountry();

            //bind state grid based on country selected
            BindStateGrid();
        }
    }
    
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindStateGrid();
    }
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        StateRepository objStateRepository = new StateRepository();
        INC_State objState = new INC_State();
        if (btnSubmit.Text == "Add State")
        {
            objState.iCountryID = Convert.ToInt64(ddlCountry.SelectedItem.Value);
            objState.sStatename= txtState.Text;
            objStateRepository.Insert(objState);
            objStateRepository.SubmitChanges();
            txtState.Text = string.Empty;
        }
        else
        {
            if (hfStateID.Value != "")
            {
                objState = objStateRepository.GetById(Convert.ToInt64(hfStateID.Value.ToString()));
                objState.sStatename = txtState.Text;
                objStateRepository.SubmitChanges();
            }
        }
        BindStateGrid();
    }

    protected void lnkAddNew_Click(object sender, EventArgs e)
    {

        lblErrorMessage.Text = string.Empty;
        btnSubmit.Text = "Add State";
        txtState.Text = string.Empty;
        modal.Controls.Clear();
        modal.Show();
    }

    protected void gvState_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteState")
            {
                try
                {
                    StateRepository objRep = new StateRepository();
                    INC_State objState = objRep.GetById(Convert.ToInt64(e.CommandArgument.ToString()));
                    objRep.Delete(objState);
                    objRep.SubmitChanges();
                    BindStateGrid();
                    lblErrorMessage.Text = "";
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex)).Number == 547)
                    {
                        lblErrorMessage.Text = "Unable to delete state as it is being used in other tables!!";
                    }
                }


            }
            else if (e.CommandName == "EditState")
            {
                txtState.Text = new StateRepository().GetById(Convert.ToInt64(e.CommandArgument.ToString())).sStatename;
                btnSubmit.Text = "Edit State";
                hfStateID.Value = e.CommandArgument.ToString();
                modal.Show();

            }
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region Methods

    /// <summary>
    /// Fills the country dropdown.
    /// </summary>
    public void FillCountry()
    {
        try
        {
            ds = objCountry.GetCountry();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCountry.DataSource = ds;
                ddlCountry.DataTextField = "sCountryName";
                ddlCountry.DataValueField = "iCountryID";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("-select country-", "0"));
                ddlCountry.SelectedValue = ddlCountry.Items.FindByText("United States").Value;
            }
        }
        catch (Exception ex)
        {
        }
        finally
        {
            ds.Dispose();
            objCountry = null;
        }
    }

    /// <summary>
    /// Binds the state gridview based on country selected.
    /// </summary>
    private void BindStateGrid()
    {
        if (ddlCountry.SelectedIndex > 0)
        {
            DataView myDataView = new DataView();
            ds = objState.GetStateByCountryID(Convert.ToInt32(ddlCountry.SelectedItem.Value));
            if (ds.Tables[0].Rows.Count == 0)
            {
                dvPaging.Visible = false;
            }
            else
            {
                dvPaging.Visible = true;
            }
            myDataView = ds.Tables[0].DefaultView;
            pds.DataSource = myDataView;
            pds.AllowPaging = true;
            pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
            pds.CurrentPageIndex = CurrentPage;
            lnkbtnNext.Enabled = !pds.IsLastPage;
            lnkbtnPrevious.Enabled = !pds.IsFirstPage;
            gvState.DataSource = pds;
            gvState.DataBind();

            doPaging();
        }
        else
        {
            gvState.DataSource = null;
            gvState.DataBind();
        }
    }

    #endregion
    
    #region Girdview Paging
    protected void dtlViewEmployee_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }
    protected void dtlViewEmployee_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt16(e.CommandArgument.ToString());
            BindStateGrid();
        }
    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        BindStateGrid();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        BindStateGrid();
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
            {
                ToPg = pds.PageCount;
            }

            for (int i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }



            dtlViewEmployee.DataSource = dt;
            dtlViewEmployee.DataBind();

        }
        catch (Exception)
        { }
    }
    #endregion
}
