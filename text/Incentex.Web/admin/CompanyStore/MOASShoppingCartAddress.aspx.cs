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
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Collections.Generic;
using Incentex.DA;

public partial class admin_CompanyStore_MOASShoppingCartAddress : PageBase
{
    #region Local Property
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
    Int64 MOASShoppingCartAddID
    {
        get
        {
            if (ViewState["MOASShoppingCartAddID"] == null)
            {
                ViewState["MOASShoppingCartAddID"] = 0;
            }
            return Convert.ToInt64(ViewState["MOASShoppingCartAddID"]);
        }
        set
        {
            ViewState["MOASShoppingCartAddID"] = value;
        }
    }
    int iDuplicate;
    Int64 intchekworkgroup
    {
        get
        {
            if (ViewState["intchekworkgroup"] == null)
            {
                ViewState["intchekworkgroup"] = 0;
            }
            return Convert.ToInt64(ViewState["intchekworkgroup"]);
        }
        set
        {
            ViewState["intchekworkgroup"] = value;
        }
    }
   
    #endregion
    DataSet ds;
    CountryDA objCountry = new CountryDA();
    StateDA objState = new StateDA();
    CityDA objCity = new CityDA();
    MOASShoppingCartAddressRepository objNewAddressRepos = new MOASShoppingCartAddressRepository();
    Company objcom = new Company();
    CompanyStoreRepository objcomrepos = new CompanyStoreRepository();
    MOASShoppingCartAddress objnewaddress = new MOASShoppingCartAddress();
    protected void Page_Load(object sender, EventArgs e)
    {   
        if (!IsPostBack)
        {   
            base.MenuItem = "Store Management Console";
            base.ParentMenuID = 0;

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
                this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));

                if (this.CompanyStoreId == 0)
                {
                    Response.Redirect("~/admin/CompanyStore/GeneralStoreSetup.aspx?Id=" + this.CompanyStoreId);
                }

                if (this.CompanyStoreId > 0 && !base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                } 

                //Assign Page Header and return URL 
                ((Label)Master.FindControl("lblPageHeading")).Text = "MOAS Shopping Cart Address";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/CompanyStore/ViewCompanyStore.aspx";

                Session["ManageID"] = 5;
                menuControl.PopulateMenu(9, 0, this.CompanyStoreId, 0, false);

                BindDropDowns();
                FillBCountry();
                bindgrid();
                List<SelectCompanyNameCompanyIDResult> query = new List<SelectCompanyNameCompanyIDResult>();
                query = objcomrepos.GetBYStoreId(Convert.ToInt32(CompanyStoreId));
                lblCompanyName.Text = query[0].CompanyName;
            }
        }
    }
    private void ClrData()
    {
       
        DrpBillingCountry.SelectedIndex = 0;
        DrpBillingCity.SelectedIndex = 0;
        DrpBillingState.SelectedIndex = 0;
        txtMOASAddressPriority.Text = "";
        ddlDepartment.SelectedIndex = 0;
        ddlWorkgroup.SelectedIndex = 0;
        TxtBillingAddress1.Text = "";
        TxtBillingAddress2.Text = "";
        TxtBillingZip.Text = "";
        TxtEmail.Text = "";
        TxtFirstName.Text = "";
        TxtLastName.Text = "";
        TxtMobile.Text = "";
        TxtPhone.Text = "";
        
        TxtBillingCompanyName.Text = "";
        
        
    }
    #region Drop downs
    //bind dropdown for the workgroup, department
    public void BindDropDowns()
    {
        LookupRepository objLookRep = new LookupRepository();

        // For Department
        ddlDepartment.DataSource = objLookRep.GetByLookup("Department");
        ddlDepartment.DataValueField = "iLookupID";
        ddlDepartment.DataTextField = "sLookupName";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Workgroup
        ddlWorkgroup.DataSource = objLookRep.GetByLookup("Workgroup");
        ddlWorkgroup.DataValueField = "iLookupID";
        ddlWorkgroup.DataTextField = "sLookupName";
        ddlWorkgroup.DataBind();
        ddlWorkgroup.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    public void FillBCountry()
    {
        try
        {
            ds = objCountry.GetCountry();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DrpBillingCountry.DataSource = ds;
                DrpBillingCountry.DataTextField = "sCountryName";
                DrpBillingCountry.DataValueField = "iCountryID";
                DrpBillingCountry.DataBind();
                DrpBillingCountry.Items.Insert(0, new ListItem("-Select-", "0"));
                // DrpBillingCountry.SelectedValue = DrpBillingCountry.Items.FindByText("United States").Value;
                // DrpBillingCountry.Enabled = true;
                ds = objState.GetStateByCountryID(Convert.ToInt32(DrpBillingCountry.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DrpBillingState.DataSource = ds;
                        DrpBillingState.DataValueField = "iStateID";
                        DrpBillingState.DataTextField = "sStateName";
                        DrpBillingState.DataBind();
                        DrpBillingState.Items.Insert(0, new ListItem("-Select-", "0"));

                        DrpBillingCity.Items.Clear();
                        DrpBillingCity.Items.Add(new ListItem("-Select-", "0"));
                    }
                    else
                    {
                        DrpBillingState.Items.Insert(0, new ListItem("-Select-", "0"));
                        DrpBillingCity.Items.Add(new ListItem("-Select-", "0"));
                    }
                }
                else
                {
                    DrpBillingState.Items.Insert(0, new ListItem("-Select-", "0"));
                    DrpBillingCity.Items.Add(new ListItem("-Select-", "0"));
                }


            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        finally
        {

        }
    }
    protected void DrpBillingCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        StateRepository objStateRepo = new StateRepository();
        List<INC_State> objStateList = objStateRepo.GetByCountryId(Convert.ToInt64(DrpBillingCountry.SelectedValue));

        DrpBillingState.Items.Clear();
        DrpBillingCity.Items.Clear();
        DrpBillingCity.Items.Insert(0, new ListItem("-Select-", "0"));
        Common.BindDDL(DrpBillingState, objStateList, "sStatename", "iStateID", "-Select-");
    }
    protected void DrpBillingState_SelectedIndexChanged(object sender, EventArgs e)
    {
        CityRepository objCityRepo = new CityRepository();
        List<INC_City> objCityList = objCityRepo.GetByStateId(Convert.ToInt64(DrpBillingState.SelectedValue));
        DrpBillingCity.Items.Clear();
        Common.BindDDL(DrpBillingCity, objCityList, "sCityName", "iCityID", "-Select-");
    }
    #endregion
    #region Event
    protected void lnkAddItem_Click(object sender, EventArgs e)
    {

        //end 
        lblFor.Text = "";
        //Billing
        string strFName = null;
        string strLName = null;
        string strAddress1 = null;
        string strAddress2 = null;
        string strTelePhone = null;
        string strMobile = null;
        string strZipCode = null;
        string strEmail = null;
        long intCityId = 0;
        long intCountryId = 0;
        long intStateId = 0;
        string strBCompanyName = null;
        try
        {
            
                //Billing
                if (TxtFirstName.Text != "")
                {
                    strFName = TxtFirstName.Text;
                }
                if (TxtLastName.Text != "")
                {
                    strLName = TxtLastName.Text;
                }
                if (TxtBillingAddress1.Text != "")
                {
                    strAddress1 = TxtBillingAddress1.Text;
                }
                if (TxtBillingAddress2.Text != "")
                {
                    strAddress2 = TxtBillingAddress2.Text;
                }
                if (TxtBillingCompanyName.Text != null)
                {

                    strBCompanyName = TxtBillingCompanyName.Text;

                }
                if (TxtBillingZip.Text != null)
                {
                    strZipCode = TxtBillingZip.Text;
                }
                if (TxtEmail.Text != "")
                {
                    strEmail = TxtEmail.Text;
                }
                if (TxtPhone.Text != "")
                {
                    strTelePhone = TxtPhone.Text;
                }
                if (TxtMobile.Text != "")
                {
                    strMobile = TxtMobile.Text;
                }
                if (DrpBillingCountry.SelectedIndex > 0)
                {
                    intCountryId = Convert.ToInt64(DrpBillingCountry.SelectedValue);
                }
                if (DrpBillingState.SelectedIndex > 0)
                {
                    intStateId = Convert.ToInt64(DrpBillingState.SelectedValue);
                }
                if (DrpBillingCity.SelectedIndex > 0)
                {
                    intCityId = Convert.ToInt64(DrpBillingCity.SelectedValue);
                }
                
               
            if (this.MOASShoppingCartAddID != 0)
            {
            objnewaddress = objNewAddressRepos.GetById(this.MOASShoppingCartAddID);
            }

            if (objnewaddress != null)
            {
                //Billing
                objnewaddress.WorkgroupId = Convert.ToInt64(ddlWorkgroup.SelectedValue);
                objnewaddress.DepartmentId = Convert.ToInt64(ddlDepartment.SelectedValue);
                objnewaddress.StoreID = this.CompanyStoreId;
                objnewaddress.MOASAddressPriority = txtMOASAddressPriority.Text.ToUpper();
                objnewaddress.FirstBName = strFName;
                objnewaddress.LastBName = strLName;
                objnewaddress.BAddress1 = strAddress1;
                objnewaddress.BAddress2 = strAddress2;
                objnewaddress.BEmail = strEmail;
                objnewaddress.BTelephone = strTelePhone;
                objnewaddress.BMobile = strMobile;
                objnewaddress.BCountryId = intCountryId;
                objnewaddress.BStateId = intStateId;
                objnewaddress.BCityId = intCityId;
                objnewaddress.BZipCode = strZipCode;
                objnewaddress.BCompanyName = strBCompanyName;
            }

            if (this.MOASShoppingCartAddID == 0)
            {
                string modeAdd = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.Add);
                iDuplicate = objNewAddressRepos.CheckDuplicate(this.CompanyStoreId, Convert.ToInt64(ddlWorkgroup.SelectedValue));
                if (iDuplicate == 0)
                {

                    objNewAddressRepos.Insert(objnewaddress);
                    objNewAddressRepos.SubmitChanges();
                    this.MOASShoppingCartAddID = objnewaddress.MOASShoppingCartAddID;
                    lblFor.Text = "Record saved successfully!";
                }
                else
                {
                    txtMOASAddressPriority.Focus();
                    lblFor.Text = "You can not add more than one address for this store of workgroup!";
                }
            }
            else
            {
                string modeAdd = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.Add);
                iDuplicate = objNewAddressRepos.CheckDuplicate(this.CompanyStoreId, Convert.ToInt64(ddlWorkgroup.SelectedValue));
                if (iDuplicate == 0)
                {
                    objNewAddressRepos.SubmitChanges();
                    this.MOASShoppingCartAddID = objnewaddress.MOASShoppingCartAddID;
                    lblFor.Text = "Record updated successfully!";
                }
                else
                {
                    if (this.intchekworkgroup == Convert.ToInt32(ddlWorkgroup.SelectedValue))
                    {
                        objNewAddressRepos.SubmitChanges();
                        this.MOASShoppingCartAddID = objnewaddress.MOASShoppingCartAddID;
                        lblFor.Text = "Record updated successfully!";
                    }
                    else
                    {
                        lblFor.Text = "You can not update this address.Address for this store of workgroup already exists!";
                    }
                }

            }
            objnewaddress = null;
            this.MOASShoppingCartAddID = 0;
            ClrData();
            bindgrid();
            
            
        }
        catch (Exception ex)
        {
            lblFor.Text = "Error in adding record in list.";
            ErrHandler.WriteError(ex);
        }

    }
    public void bindgrid()
    {
        objNewAddressRepos = new MOASShoppingCartAddressRepository(); 
        List<MOASShoppingCartAddress> objList = new List<MOASShoppingCartAddress>();
        objList = objNewAddressRepos.GetAllAddress(Convert.ToInt32(this.CompanyStoreId));
        gvShippingBilling.DataSource = objList;
        gvShippingBilling.DataBind();
       
    }
    protected void gvShippingBilling_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblFor.Text = "";
        MOASShoppingCartAddress objShipInfo = new MOASShoppingCartAddress();
        switch (e.CommandName)
        {
            case "EditRec":
               
                //display billing /record to edit
                this.MOASShoppingCartAddID = Convert.ToInt64(e.CommandArgument);
                objShipInfo = objNewAddressRepos.GetById(MOASShoppingCartAddID);

                HiddenField hdnWorkgroupiD;
                GridViewRow row;
                row = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
                hdnWorkgroupiD = (HiddenField)(gvShippingBilling.Rows[row.RowIndex].FindControl("hdnWorkgroupiD"));
                this.intchekworkgroup=Convert.ToInt32(hdnWorkgroupiD.Value);
                if (objShipInfo != null)
                {
                    List<SelectCompanyNameCompanyIDResult> query = new List<SelectCompanyNameCompanyIDResult>();
                    query = objcomrepos.GetBYStoreId(Convert.ToInt32(CompanyStoreId));
                    lblCompanyName.Text = query[0].CompanyName;
                    TxtBillingCompanyName.Text = objShipInfo.BCompanyName;
                   
                    txtMOASAddressPriority.Text = objShipInfo.MOASAddressPriority;
                    
                    ddlWorkgroup.SelectedValue = objShipInfo.WorkgroupId != null ? objShipInfo.WorkgroupId.ToString() : "0";
                    ddlDepartment.SelectedValue = objShipInfo.DepartmentId != null ? objShipInfo.DepartmentId.ToString() : "0";
                    
                    TxtFirstName.Text = objShipInfo.FirstBName;
                    TxtLastName.Text = objShipInfo.LastBName;
                   
                   
                    
                    DrpBillingCountry.SelectedValue = Common.GetLongString(objShipInfo.BCountryId);
                    DrpBillingCountry_SelectedIndexChanged(sender, e);
                    DrpBillingState.SelectedValue = Common.GetLongString(objShipInfo.BStateId);
                    DrpBillingState_SelectedIndexChanged(sender, e);
                    DrpBillingCity.SelectedValue = Common.GetLongString(objShipInfo.BCityId);

                 
                    TxtBillingAddress1.Text = objShipInfo.BAddress1;
                    TxtBillingAddress2.Text = objShipInfo.BAddress2;
                    TxtBillingZip.Text = objShipInfo.BZipCode;
                    TxtMobile.Text = objShipInfo.BMobile;
                    TxtEmail.Text = objShipInfo.BEmail;
                    TxtPhone.Text = objShipInfo.BTelephone;
                    intchekworkgroup = Convert.ToInt32(ddlWorkgroup.SelectedValue);
                    TxtBillingCompanyName.Focus();
                    objShipInfo = null; 
                }

                break;
            case "DeleteRec":

                if (!base.CanDelete)
                {
                    base.RedirectToUnauthorised();
                }

                try
                {
                    MOASShoppingCartAddID = Convert.ToInt64(e.CommandArgument);
                    objShipInfo = objNewAddressRepos.GetById(MOASShoppingCartAddID);
                    objNewAddressRepos.Delete(objShipInfo);
                    objNewAddressRepos.SubmitChanges();
                    lblFor.Text = "Record deleted successfully ...";
                    MOASShoppingCartAddID = 0;
                    ClrData();
                    bindgrid();
                   
                }
                catch (Exception ex)
                {
                    if (((System.Data.SqlClient.SqlException)(ex)).Number == 547)
                    {
                        lblFor.Text = "Unable to delete Shipping information as it is being used in orders!!";
                    }
                    else
                    {
                        ErrHandler.WriteError(ex);
                        lblFor.Text = "Error in deleting record ...";
                    }

                }

                break;

                
        }
    }
    #endregion
    protected void gvShippingBilling_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton hypCompanyname = (LinkButton)e.Row.FindControl("hypCompany");
            List<SelectCompanyNameCompanyIDResult> query = new List<SelectCompanyNameCompanyIDResult>();
            query = objcomrepos.GetBYStoreId(Convert.ToInt32(CompanyStoreId));
            hypCompanyname.Text = query[0].CompanyName;
            Label lblWorkgroupName = (Label)e.Row.FindControl("lblWorkgroupName");
            LookupRepository objLokrepos = new LookupRepository();
            INC_Lookup objlok = new INC_Lookup();
            HiddenField hdnWorkgroupiD = (HiddenField)e.Row.FindControl("hdnWorkgroupiD");
            objlok = objLokrepos.GetById(Convert.ToInt32(hdnWorkgroupiD.Value));
            if (objlok != null)
            {
                lblWorkgroupName.Text = objlok.sLookupName;
            }

        }
    }
}
