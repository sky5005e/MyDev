using System;
using System.Data;
using System.Web.UI.WebControls;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_city : PageBase
{
    #region Data Members
    DataSet ds;
    CountryDA objCountry = new CountryDA();
    StateDA objState = new StateDA();
    CityDA objCity = new CityDA();
    string messagae = null;
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
            FillCountry();
            ((Label)Master.FindControl("lblPageHeading")).Text = "Add city";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "dropdownmenus.aspx";
        }
    }
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
            bindCity();
        }
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCountry.SelectedIndex <= 0)
            {
                ddlState.Enabled = false;
                ddlState.Items.Clear();
                ddlState.Items.Add(new ListItem("-select state-", "0"));

                ddlCity.Enabled = false;
                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-select city-", "0"));
            }
            else
            {
                ddlState.Enabled = true;
                ds = objState.GetStateByCountryID(Convert.ToInt32(ddlCountry.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlState.DataSource = ds;
                        ddlState.DataValueField = "iStateID";
                        ddlState.DataTextField = "sStateName";
                        ddlState.DataBind();
                        ddlState.Items.Insert(0, new ListItem("-select state-", "0"));

                        
                        /*ddlCity.Items.Clear();
                        ddlCity.Items.Add(new ListItem("-select city-", "0"));*/
                    }
                }

            }

        }
        catch (Exception ex)
        {
            messagae = ex.Message;
        }
        finally
        {
            //ds.Dispose();
            //objState = null;
        }
    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet dsCity = new DataSet();
        try
        {
            if (ddlState.SelectedIndex <= 0)
            {
                ddlCity.Enabled = false;
                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-select city-", "0"));
            }
            else
            {
                bindCity();
            }
        }
        catch (Exception ex)
        {
            messagae = ex.Message;
        }
        finally
        {
            dsCity.Dispose();
            //objCity = null;
        }
    }
    
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        CityRepository objCityRep = new CityRepository();
        INC_City objCity = new INC_City();
        if (btnSubmit.Text == "Add City")
        {
            objCity.iCountryID = Convert.ToInt64(ddlCountry.SelectedItem.Value);
            objCity.iStateID = Convert.ToInt64(ddlState.SelectedItem.Value);
            objCity.sCityName = txtCity.Text;
            objCityRep.Insert(objCity);
            objCityRep.SubmitChanges();
            txtCity.Text = string.Empty;
            
        }
        else
        {
            if (hfCityId.Value != "")
            {
                objCity = objCityRep.GetById(Convert.ToInt64(hfCityId.Value.ToString()));
                objCity.sCityName = txtCity.Text;
                objCityRep.SubmitChanges();
            }
        }
        bindCity();
    }
    protected void lnkAddNew_Click(object sender, EventArgs e)
    {

        lblErrorMessage.Text = string.Empty;
        btnSubmit.Text = "Add City";
        txtCity.Text = string.Empty;
        modal.Controls.Clear();
        modal.Show();
    }
    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        bindCity();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        bindCity();
    }

    
    protected void grvCity_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "delCity")
            {
                try
                {
                    CityRepository objRep = new CityRepository();
                    INC_City objCity = objRep.GetById(Convert.ToInt64(e.CommandArgument.ToString()));
                    objRep.Delete(objCity);
                    objRep.SubmitChanges();
                    bindCity();
                    lblErrorMessage.Text = "";
                }
                catch (Exception ex) 
                {
                    if (((System.Data.SqlClient.SqlException)(ex)).Number == 547)
                    {
                        lblErrorMessage.Text = "Unable to delete city as it is being used in other tables!!";
                    }
                    
                }
                
              
            }
            else if (e.CommandName == "editCity")
            {
                txtCity.Text = new CityRepository().GetById(Convert.ToInt64(e.CommandArgument.ToString())).sCityName;
                btnSubmit.Text = "Edit City";
                hfCityId.Value = e.CommandArgument.ToString();
                modal.Show();

            }
        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region Methods

    public void FillCountry()
    {
        DataSet dsCity = new DataSet();
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



                ddlState.Enabled = true;
                ds = objState.GetStateByCountryID(Convert.ToInt32(ddlCountry.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlState.DataSource = ds;
                        ddlState.DataValueField = "iStateID";
                        ddlState.DataTextField = "sStateName";
                        ddlState.DataBind();
                        ddlState.Items.Insert(0, new ListItem("-select state-", "0"));
                        ddlState.SelectedValue = ddlState.Items.FindByText("Alabama").Value;

                        dsCity = objCity.GetCityByStateID(Convert.ToInt32(ddlState.SelectedItem.Value));

                        if (dsCity != null)
                        {
                            bindCity();
                        }
                    }
                }

            }
        }
        catch (Exception ex)
        {
            messagae = ex.Message;
        }
        finally
        {
            ds.Dispose();
            dsCity.Dispose();
            objCountry = null;
        }
    }

    private void bindCity()
    {
        DataView myDataView = new DataView();
        DataSet dsCity = new DataSet();
        ddlCity.Enabled = true;
        dsCity = objCity.GetCityByStateID(Convert.ToInt32(ddlState.SelectedItem.Value));
        if (dsCity.Tables[0].Rows.Count == 0)
        {
            dvPaging.Visible = false;

        }
        else
        {
            dvPaging.Visible = true;

        }
        myDataView = dsCity.Tables[0].DefaultView;
        pds.DataSource = myDataView;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;
        grvCity.DataSource = pds;
        grvCity.DataBind();

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
