
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_CompanyStore_CompanyPrograms_UniformIssuanceStep1 : PageBase
{

    #region Properties
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
    Int64 UniformIssuancePolicyID
    {
        get
        {
            if (ViewState["UniformIssuancePolicyID"] == null)
            {
                ViewState["UniformIssuancePolicyID"] = 0;
            }
            return Convert.ToInt64(ViewState["UniformIssuancePolicyID"]);
        }
        set
        {
            ViewState["UniformIssuancePolicyID"] = value;
        }
    }
    Int64 workgroupid
    {
        get
        {
            if (ViewState["workgroupid"] == null)
            {
                ViewState["workgroupid"] = 0;
            }
            return Convert.ToInt64(ViewState["workgroupid"]);
        }
        set
        {
            ViewState["workgroupid"] = value;
        }
    }
    String IsGroupAssociation
    {
        get
        {
            if (ViewState["IsGroupAssociation"] == null)
            {
                ViewState["IsGroupAssociation"] = null;
            }
            return Convert.ToString(ViewState["IsGroupAssociation"]);
        }
        set
        {
            ViewState["IsGroupAssociation"] = value;
        }
    }
    String PaymentType
    {
        get
        {
            if (ViewState["PaymentType"] == null)
            {
                ViewState["PaymentType"] = null;
            }
            return Convert.ToString(ViewState["PaymentType"]);
        }
        set
        {
            ViewState["PaymentType"] = value;
        }
    }
    LookupRepository objLookupRepository = new LookupRepository();
    UniformIssuancePolicyRepository objUniformIssuancePolicyRepository = new UniformIssuancePolicyRepository();
    UniformIssuancePolicyItemRepository objUniformIssuancePolicyStyleRepository = new UniformIssuancePolicyItemRepository();
    StoreProductRepository objStoreProductRepository = new StoreProductRepository();
    protected string DisableRank
    {
        get
        {
            if (ViewState["DisableRank"] == null)
            {
                ViewState["DisableRank"] = "";
            }
            return ViewState["DisableRank"].ToString();
        }
        set
        {
            ViewState["DisableRank"] = value;
        }
    }
    #endregion
    DataSet ds;
    CountryDA objCountry = new CountryDA();
    StateDA objState = new StateDA();
    CityDA objCity = new CityDA();
    LookupDA objLookuoDA = new LookupDA();
    CompanyEmployeeContactInfoRepository objCmpEmpContRep = new CompanyEmployeeContactInfoRepository();
    UserInformationRepository objUsrInfoRep = new UserInformationRepository();
    CompanyEmployeeRepository objCompanyEmployeeRepository = new CompanyEmployeeRepository();
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        //txtBudgetAmt.Attributes.Add("onkeypress", "EnableKeys(event,document.getElementById('" + txtBudgetAmt.ClientID + "').value)");
        //  txtBudgetAmt.Attributes.Add("onkeypress", "return EnableKeys();");
        lblMsg.Text = "";
        if (!IsPostBack)
        {
            this.PaymentType = Request.QueryString.Get("PaymentType");
            this.CompanyStoreId = Convert.ToInt64(Request.QueryString.Get("Id"));
            ((Label)Master.FindControl("lblPageHeading")).Text = "Uniform Issuance Policy - Step 2";
            this.UniformIssuancePolicyID = Convert.ToInt64(Request.QueryString.Get("SubId"));
            ViewState["Id"] = this.UniformIssuancePolicyID;
            menuControl.PopulateMenu(4, 1, this.CompanyStoreId, this.UniformIssuancePolicyID, true);
            if (PaymentType == "CompanyPays" || PaymentType == "MOAS")
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/CompanyPrograms/IssuanceCompanyAddress.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CompanyStore/CompanyPrograms/UniformIssuanceStep1.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID;
            }
            
            imgPhoto.Visible = false;
            FillAssocationIssuancePolicy();
            BindMasterItems();
            BindDropDowns();
            if (Convert.ToString(ViewState["AssociationName"]) != "")
            {
                if (ViewState["AssociationName"].ToString() == "Create Group Association with Budget")
                {
                    divBudgetamt.Visible = true;
                    spanBAmount.Visible = true;
                    divbamountbottom.Visible = true;
                }
                else
                {
                    divBudgetamt.Visible = false;
                    spanBAmount.Visible = false;
                    divbamountbottom.Visible = false;
                }
            }

            BinData();
            gv.DataBind();
            


        }
    }
    #region Methods
    /// <summary>
    /// Bind master data
    /// </summary>
    void BinData()
    {
        //insert value in issuance
        ddlIssuance.Items.Insert(0, new ListItem("Issuance", "0"));
        for (int i = 1; i <= 12; i++)
        {
            ddlIssuance.Items.Insert(i, new ListItem(i.ToString() + " Pieces Issued", i.ToString()));
        }

        ddlIssuance.Attributes.Add("onchange", "pageLoad(this,value);");

        //insert values in rank
        List<INC_Lookup> objRankList = objLookupRepository.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.RANK);
        //Common.BindDDL(ddlRank, objRankList,"sLookupName", "iLookupID", "Rank");

        //bind Payment Option
        ListItem LiCompany = new ListItem(Incentex.DAL.Common.DAEnums.GetUniformIssuancePaymentOptionName(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.CompanyPays),
             Convert.ToString((int)Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.CompanyPays));

        ListItem LiEmp = new ListItem(Incentex.DAL.Common.DAEnums.GetUniformIssuancePaymentOptionName(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.EmployeePays),
             Convert.ToString((int)Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.EmployeePays));

        //check that moas user is available for this workgroup or not 
        List<UserInformation> lstCA = new CompanyEmployeeRepository().GetCAByWorkgroupId(this.workgroupid, new CompanyStoreRepository().GetById(this.CompanyStoreId).CompanyID);
        //Add by mayur got moas option 1-mar-2012
        ListItem LiMOAS = new ListItem(Incentex.DAL.Common.DAEnums.GetUniformIssuancePaymentOptionName(Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.MOAS),
             Convert.ToString((int)Incentex.DAL.Common.DAEnums.UniformIssuancePaymentOption.MOAS));
        
        //Comment  Nagmani 31-March-2012
        
        //if(lstCA.Count>0)//check that ca is available for this workgroup then only we have to display this option
        //    ddlPaymentOption.Items.Insert(0, LiMOAS);
        //ddlPaymentOption.Items.Insert(0, LiCompany);
        //ddlPaymentOption.Items.Insert(0, LiEmp);

        //Common.AddOnChangeAttribute(ddlPaymentOption);
        
        //End
    }

    //bind dropdown for the workgroup, department
    public void BindDropDowns()
    {

        LookupDA sLookup = new LookupDA();
        LookupBE sLookupBE = new LookupBE();
        //For Climate
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "StationAdditionalInfo";
        ddlClimate.DataSource = sLookup.LookUp(sLookupBE);
        ddlClimate.DataValueField = "iLookupID";
        ddlClimate.DataTextField = "sLookupName";
        ddlClimate.DataBind();
        ddlClimate.Items.Insert(0, new ListItem("-Select-", "0"));


        // For EmployeeType
        sLookupBE.SOperation = "selectall";
        sLookupBE.iLookupCode = "EmployeeType";
        ddlEmployeeType.DataSource = sLookup.LookUp(sLookupBE);
        ddlEmployeeType.DataValueField = "iLookupID";
        ddlEmployeeType.DataTextField = "sLookupName";
        ddlEmployeeType.DataBind();
        ddlEmployeeType.Items.Insert(0, new ListItem("-Select-", "0"));

        // For Department
        LookupRepository objLookupRepository = new LookupRepository();
        List<INC_Lookup> objList = objLookupRepository.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.RANK);
        ddlDepartment.DataSource = objList;
        ddlDepartment.DataValueField = "iLookupID";
        ddlDepartment.DataTextField = "sLookupName";
        ddlDepartment.DataBind();
        ddlDepartment.Items.Insert(0, new ListItem("-Select-", "0"));


    }

    /// <summary>
    /// bind Master items dropdown
    /// </summary>
    public void BindMasterItems()
    {
         UniformIssuancePolicy objPolicy=new UniformIssuancePolicy();
         if (this.UniformIssuancePolicyID != 0)
            {
               objPolicy= objUniformIssuancePolicyRepository.GetById(this.UniformIssuancePolicyID);
            if(objPolicy!=null)
            {
                this.workgroupid=objPolicy.WorkgroupID;
            }
            }
        DataSet dsGender = new DataSet();
        dsGender = objLookuoDA.GetGenderType(this.UniformIssuancePolicyID);
        string Val1 = null;
        if (dsGender.Tables[0].Rows.Count > 0)
        {
            Val1 = dsGender.Tables[0].Rows[0].ItemArray[0].ToString();
        }
        try
        {
            ds = objLookuoDA.GetMasterItemNo(Val1, Convert.ToInt32(this.CompanyStoreId), Convert.ToInt32(this.workgroupid));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlItems.DataSource = ds;
                ddlItems.DataTextField = "sLookupName";
                ddlItems.DataValueField = "storeproductid";
                ddlItems.DataBind();
                ddlItems.Items.Insert(0, new ListItem("-Select-", "0"));

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



        //remove added items from dropdown
        //foreach (GridViewRow gr in gv.Rows)
        //{
        //    Label lblMasterItemId = gr.FindControl("lblMasterItemId") as Label;
        //    ListItem li = ddlItems.Items.FindByValue(lblMasterItemId.Text);
        //    ddlItems.Items.Remove(li);
        //}

    }
    /// <summary>
    /// FillAssocationIssuancePolicy()
    /// Nagmani /05/01/2011
    /// </summary>
    public void FillAssocationIssuancePolicy()
    {
        try
        {
            LookupRepository objLookRep = new LookupRepository();
            string strStatus = "lnkIssuancePolicy";
            ddlAssociationPolicy.DataSource = objLookRep.GetByLookup(strStatus);
            ddlAssociationPolicy.DataValueField = "iLookupID";
            ddlAssociationPolicy.DataTextField = "sLookupName";
            ddlAssociationPolicy.DataBind();
            ddlAssociationPolicy.Items.Insert(0, new ListItem("-Select-", "0"));

        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    #endregion
    #region Events
    /// <summary>
    /// redirect to previous page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        if (PaymentType == "CompanyPays" || PaymentType == "MOAS")
        {
            Response.Redirect("IssuanceCompanyAddress.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType);
        }
        else
        {
            Response.Redirect("UniformIssuanceStep1.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID);
        }
    }
    /// <summary>
    /// redirect to next page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkNext_Click(object sender, EventArgs e)
    {
        Response.Redirect("UniformIssuanceStep3.aspx?Id=" + this.CompanyStoreId + "&SubId=" + this.UniformIssuancePolicyID + "&PaymentType=" + this.PaymentType);

    }
    /// <summary>
    /// Add item to grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkAddItem_Click(object sender, EventArgs e)
    {
        this.UniformIssuancePolicyID = Convert.ToInt64(Request.QueryString.Get("SubId"));
        Int64 masteritmeid = 0;
      
        if (ddlItems.SelectedIndex > 0)
        {
            masteritmeid = Convert.ToInt64(objLookupRepository.GetIdByLookupName(ddlItems.SelectedItem.Text));
        }

        //Check Here for duplication
        UniformIssuancePolicyItemRepository objrepos = new UniformIssuancePolicyItemRepository();
        int isduplicat = objrepos.CheckMasterItemDuplicate(ddlAssociationPolicy.SelectedItem.Text.Trim(), UniformIssuancePolicyID, txtNewGroup.Text.Trim(), Convert.ToInt32(masteritmeid), Convert.ToInt64(ddlItems.SelectedValue));
        if (isduplicat == 1)
        {
            lblMsg.Text = "This Master Item Already Exist In This Group";
            SetFocus(ddlItems);
            return;
        }

        if (ddlAssociationPolicy.SelectedItem.Text.Trim() != "Create Single Item Association")
        {
            int isduplicatGroupName = objrepos.CheckGroupName(UniformIssuancePolicyID, txtNewGroup.Text, Convert.ToInt32(ddlIssuance.SelectedValue));
          if (isduplicatGroupName > 0)
          {
              //ViewState["strisGroupAssociation"] = "N";
              IsGroupAssociation = "N";
          }
        }
        //end 
        lblMsg.Text = "";
        try
        {
            int intClimate=0;
            int intEmployeeType = 0;
            if (Convert.ToInt32(ddlClimate.SelectedValue) != 0)
            {
                intClimate = Convert.ToInt32(ddlClimate.SelectedValue);
            }


            if (Convert.ToInt32(ddlEmployeeType.SelectedValue) != 0)
            {
                intEmployeeType = Convert.ToInt32(ddlEmployeeType.SelectedValue);
            }
            
            
            Int64? RankId = null;
            int intIssuanceNoSelectd = 0;
            if (Convert.ToInt64(ddlDepartment.SelectedValue) != 0)
            {
                RankId = Convert.ToInt64(ddlDepartment.SelectedValue);
            }
            if (ddlIssuance.SelectedValue == "0")
            {
                lblMsg.Text = "Please Select Issuance";
                return;
            }
            else
            {
                intIssuanceNoSelectd = Convert.ToInt32(ddlIssuance.SelectedValue);
            }
            if (ddlItems.SelectedValue == "0")
            {
                lblMsg.Text = "Please Select Master Items";
                return;
            }
            long AssociationPolicy = 0;
            if (ddlAssociationPolicy.SelectedIndex > 0)
            {
                AssociationPolicy = Convert.ToInt64(ddlAssociationPolicy.SelectedValue);
            }
            else
            {
                AssociationPolicy = Convert.ToInt64(ViewState["AssociationValue"]);
            }
            string strNewGroup = null;
            if (txtNewGroup.Text != "")
            {
                strNewGroup = txtNewGroup.Text;
            }

            string strBudgetAmount = null;
            if (ddlAssociationPolicy.SelectedItem.Text == "Create Group Association with Budget")
            {
                if (txtBudgetAmt.Text != "")
                {
                    strBudgetAmount = txtBudgetAmt.Text;
                }
            }
            
            string strAssociationPolicyNote = "";
            if (divPolicyNote.Visible == true)
            {
                if (txtAssociationPolicyNote.Text == "")
                {
                    lblMsg.Text = "Enter policy note";
                    return;
                }
            }

            if (txtAssociationPolicyNote.Text != "")
            {
                strAssociationPolicyNote = txtAssociationPolicyNote.Text;
            }

           
            
            UniformIssuancePolicyItem objUniformIssuancePolicyItem = new UniformIssuancePolicyItem()
               {
                   UniformIssuancePolicyID = this.UniformIssuancePolicyID,
                   Issuance = Convert.ToInt32(ddlIssuance.SelectedValue),
                   MasterItemId = masteritmeid,//Convert.ToInt64(ddlItems.SelectedValue)
                   RankId = RankId,
                   CreatedDate = DateTime.Now,
                  // PaymentOption = Convert.ToInt32(ddlPaymentOption.SelectedValue),
                   AssociationIssuanceType = AssociationPolicy,
                   AssociationbudgetAmt = strBudgetAmount,
                   AssociationIssuancePolicyNote = strAssociationPolicyNote,
                   //Add nagmani 16-May2011
                   NEWGROUP =strNewGroup,
                   ISGROUPASSOCIATION = Convert.ToChar(IsGroupAssociation),
                   WeatherTypeid=intClimate,
                   EmployeeTypeid=intEmployeeType,
                   StoreProductid=Convert.ToInt32(ddlItems.SelectedValue),
                   //End Nagmani 16-May2011
                   
                  
               };

            objUniformIssuancePolicyStyleRepository.Insert(objUniformIssuancePolicyItem);
            objUniformIssuancePolicyStyleRepository.SubmitChanges();
            BindMasterItems();
            gv.DataBind();
            ClrData();
            divBudgetamt.Visible = false;
            divIssuanceBudget.Visible = false;
            divPolicyNote.Visible = false;
            divPolicyNoteBudgetamt.Visible = false;
           // txtBudgetAmt.Text = "";
            ViewState["strisGroupAssociation"] = "N";
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in adding record in list.";
            ErrHandler.WriteError(ex);
        }

    }
    /// <summary>
    /// handled events in grid control - delete
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gv_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "DeleteRec":
                Int64 UniformIssuancePolicyItemID = Convert.ToInt64(e.CommandArgument);

                try
                {
                    UniformIssuancePolicyItem objUniformIssuancePolicyItem = objUniformIssuancePolicyStyleRepository.GetById(UniformIssuancePolicyItemID);
                    objUniformIssuancePolicyStyleRepository.Delete(objUniformIssuancePolicyItem);
                    objUniformIssuancePolicyStyleRepository.SubmitChanges();
                    gv.DataBind();
                    List<CheckAssociationIssuancePolicyNoteResult> objResult = new List<CheckAssociationIssuancePolicyNoteResult>();
                    if (ddlAssociationPolicy.SelectedIndex > 0)
                    {
                        int intObj = objUniformIssuancePolicyStyleRepository.CheckDuplicate(this.UniformIssuancePolicyID, Convert.ToInt64(ddlAssociationPolicy.SelectedValue));

                        if (intObj == 0)
                        {
                            divPolicyNote.Visible = true;
                            divIssuanceBudget.Visible = true;
                            divBudgetamt.Visible = true;
                            txtBudgetAmt.Visible = true;

                        }
                        else
                        {
                            divPolicyNote.Visible = false;
                            divIssuanceBudget.Visible = false;
                            txtBudgetAmt.Visible = false;
                            divBudgetamt.Visible = false;

                        }
                    }
                    ddlAssociationPolicy.SelectedIndex = 0;
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 547)
                    {

                        lblMsg.Text = "Unable to delete record as this record is used in other detail table";
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Error in deleting record ...";
                    ErrHandler.WriteError(ex);
                }

                break;
        }
    }
    protected void gv_RowEditing(object sender, GridViewEditEventArgs e)
    {
        //07-Feb-2012
        gv.EditIndex = e.NewEditIndex;
        gv.DataBind();
        int index = gv.EditIndex;
        GridViewRow row = gv.Rows[index];
        DropDownList ddlEmployeeType = (DropDownList)row.FindControl("ddlEmployeeType");
        DropDownList ddlWeatherType = (DropDownList)row.FindControl("ddlWeatherType");
        HiddenField hdnWeatherType = (HiddenField)row.FindControl("hdnWeatherType");
        HiddenField hdnEmployeeType = (HiddenField)row.FindControl("hdnEmployeeType");

        LookupRepository objLookRep = new LookupRepository();
        string strStatus = "StationAdditionalInfo";
        ddlWeatherType.DataSource = objLookRep.GetByLookup(strStatus);
        ddlWeatherType.DataValueField = "iLookupID";
        ddlWeatherType.DataTextField = "sLookupName";
        ddlWeatherType.DataBind();
        ddlWeatherType.Items.Insert(0, new ListItem("-Select-", "0"));
        if (hdnWeatherType.Value != "")
        {
           
            ddlWeatherType.Items.FindByText(hdnWeatherType.Value).Selected = true;
        }
        LookupRepository objLookRep1 = new LookupRepository();
        string strStatus1 = "EmployeeType";
        ddlEmployeeType.DataSource = objLookRep1.GetByLookup(strStatus1);
        ddlEmployeeType.DataValueField = "iLookupID";
        ddlEmployeeType.DataTextField = "sLookupName";
        ddlEmployeeType.DataBind();
        ddlEmployeeType.Items.Insert(0, new ListItem("-Select-", "0"));
        if (hdnEmployeeType.Value != "")
        {
           
            ddlEmployeeType.Items.FindByText(hdnEmployeeType.Value).Selected = true;
        }       
        //End

       
    }
    /// <summary>
    /// get data source and set it to grid
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gv_DataBinding(object sender, EventArgs e)
    {
        //List<UniformIssuancePolicyItem> objList = objUniformIssuancePolicyStyleRepository.GetByUniformIssuancePolicyID(this.UniformIssuancePolicyID);
        List<SelectUniformIssuancePolicyProgramResult> objList = objUniformIssuancePolicyStyleRepository.GetCreditProgram(this.UniformIssuancePolicyID);
        UniformIssuancePolicy objUniformIssuancePolicy = objUniformIssuancePolicyRepository.GetById(this.UniformIssuancePolicyID);
        if (objUniformIssuancePolicy != null)
        {
            INC_Lookup obj = objLookupRepository.GetById(objUniformIssuancePolicy.WorkgroupID);
            if (obj.sLookupName.ToLower().Contains("pilot"))
            {
                pnlRank.Visible = true;
            }
            else
            {
                pnlRank.Visible = false;
            }

        }

        DataTable dt = Common.ListToDataTable(objList);

        dt.Columns.Add("ItemName");

        foreach (DataRow dr in dt.Rows)
        {
            Int64 UniformIssuancePolicyItemID = Convert.ToInt64(dr["UniformIssuancePolicyItemID"]);
            if (UniformIssuancePolicyItemID == 0)
            {
                dr["ItemName"] = "No Item";
            }
            else
            {
                dr["ItemName"] = objLookupRepository.GetById(Convert.ToInt64(dr["MasterItemId"])) == null ? "No Item" : objLookupRepository.GetById(Convert.ToInt64(dr["MasterItemId"])).sLookupName;
            }
        }
        gv.DataSource = dt;
    }
    /// <summary>
    /// event after grid is data bound
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gv_DataBound(object sender, EventArgs e)
    {
        //BindMasterItems();
        if (gv.Rows.Count == 0)
        {
            ddlIssuance.SelectedIndex = 0;
            ddlIssuance.Enabled = true;
            ddlAssociationPolicy.SelectedIndex = 0;
            ddlAssociationPolicy.Enabled = true;
            this.DisableRank = "";
            //txtAssociationPolicyNote.Visible = false;

        }
        else
        {

           
            this.DisableRank = "disabled";
            GridViewRow gr = gv.Rows[0];

            HiddenField lblRankId = gr.FindControl("lblRankId") as HiddenField;
            Label lnlAssociationType = gr.FindControl("lblsLookupName") as Label;
            Label lnlAssociationPolicyNote = gr.FindControl("lblPolicyNote") as Label;

            ddlDepartment.SelectedValue = lblRankId.Value;

            if (lnlAssociationType.Text == "Create Single Item Association" && lnlAssociationPolicyNote != null)
            {
                // txtAssociationPolicyNote.Visible = false; 
            }
            else if (lnlAssociationType.Text == "Create Group Association" && lnlAssociationPolicyNote != null)
            {
                //txtAssociationPolicyNote.Visible = false; 

            }
            else if (lnlAssociationType.Text == "Create Group Association with Budget" && lnlAssociationPolicyNote != null)
            {
                // txtAssociationPolicyNote.Visible = false;

            }
            else
            {
                //  txtAssociationPolicyNote.Visible = true;
            }
            if (Convert.ToString(ViewState["AssociationName"]) != "")
            {
                if (ViewState["AssociationName"].ToString() == "Create Single Item Association")
                {
                    ddlIssuance.Enabled = true;

                }
                else if (ViewState["AssociationName"].ToString() == "Create Group Association")
                {
                    ddlIssuance.Enabled = false;

                }
                else if (ViewState["AssociationName"].ToString() == "Create Group Association with Budget")
                {
                    ddlIssuance.Enabled = false;

                }
                else
                {
                    txtBudgetAmt.Enabled = true;
                    ddlIssuance.Enabled = true;
                    ddlAssociationPolicy.Enabled = true;

                    // ddlPaymentOption.Enabled = true;
                }

            }
        }
        BindMasterItems();

    }
    /// <summary>
    /// Display image if item is selected
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlItems_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

           

            if (ddlItems.SelectedIndex > 0)
            {
                Int64 masteritmeid = 0;
                if (ddlItems.SelectedIndex > 0)
                {
                    masteritmeid = Convert.ToInt64(objLookupRepository.GetIdByLookupName(ddlItems.SelectedItem.Text));
                    
                }

                StoreProductImageRepository objStoreProductImageRepository = new StoreProductImageRepository();
               
                //Start Update 16-Feb-2012 nagmani
               //  List<StoreProductImage> objList = objStoreProductImageRepository.GetStoreProductImage(this.CompanyStoreId, Convert.ToInt64(ddlItems.SelectedValue));
                List<StoreProductImage> objList = objStoreProductImageRepository.GetStoreProductImageNew(this.CompanyStoreId, masteritmeid, Convert.ToInt64(ddlItems.SelectedValue));
                //End Nagmani
                imgPhoto.Src = "";

                imgPhoto.Visible = false;
                if (objList.Count > 0)
                {

                    StoreProductImage obj = (from i in objList
                                             where i.ProductImageActive == 1
                                             select i).FirstOrDefault();

                    if (obj != null)
                    {
                        string FilePath = "~/UploadedImages/ProductImages/Thumbs/" + obj.ProductImage;

                        if (File.Exists(Server.MapPath(FilePath)))
                        {
                           
                            imgSplashImage.Src = FilePath;
                            prettyphotoDiv.HRef = "~/UploadedImages/ProductImages/" + obj.LargerProductImage;
                            
                        }
                        else
                        {
                            imgSplashImage.Src = "~/UploadedImages/employeePhoto/employee-photo.gif";
                        }
                    }

                }
                else
                {
                    imgSplashImage.Src = "~/UploadedImages/employeePhoto/employee-photo.gif";
                }

            }
            else
            {
                imgSplashImage.Src = "~/UploadedImages/employeePhoto/employee-photo.gif";
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// Empty message
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlIssuance_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = "";
    }
    /// <summary>
    ///  Empty message
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlRank_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = "";
    }
    #endregion
    protected void ddlAssociationPolicy_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        //Add Nagmani 13-May-2011
        txtNewGroup.Text = "";
        txtAssociationPolicyNote.Text = "";

        if (ddlAssociationPolicy.SelectedItem.Text.Trim() == "Create Single Item Association")
        {
            IsGroupAssociation = "N";
            pnlNewGroup.Visible = false;
        }
        else
        {
            pnlNewGroup.Visible = true;
        }
        //End Nagmani 13-May-2011

        int intObj = 0;
        List<CheckAssociationIssuancePolicyNoteResult> objResult = new List<CheckAssociationIssuancePolicyNoteResult>();
        if (ddlAssociationPolicy.SelectedIndex > 0)
        {
            intObj = objUniformIssuancePolicyStyleRepository.CheckDuplicate(this.UniformIssuancePolicyID, Convert.ToInt64(ddlAssociationPolicy.SelectedValue));

            if (intObj == 0)
            {
                divPolicyNote.Visible = true;
                divIssuanceBudget.Visible = true;
            }
            else
            {
                divPolicyNote.Visible = false;
                divIssuanceBudget.Visible = false;
            }
        }

        if (ddlAssociationPolicy.SelectedIndex > 0)
        {

            divbamountbottom.Visible = true;
            divNote.Visible = true;
            divPolicyNoteBudgetamt.Visible = false;
            divIssuanceBudget.Visible = true;

        }
        else
        {

            divbamountbottom.Visible = true;
            divNote.Visible = true;
            divIssuanceBudget.Visible = false;
            divPolicyNoteBudgetamt.Visible = false;
            divBudgetamt.Visible = false;
            divIssuanceBudget.Visible = false;
            divPolicyNote.Visible = false;

        }
        ViewState["AssociationValue"] = ddlAssociationPolicy.SelectedItem.Value;
        ViewState["AssociationName"] = ddlAssociationPolicy.SelectedItem.Text;
        ddlIssuance.Enabled = true;
        if (ddlAssociationPolicy.SelectedItem.Text.Trim() == "Create Group Association with Budget")
        {
            divBudgetamt.Visible = true;
            spanBAmount.Visible = true;
            divbamountbottom.Visible = true;

            divIssuanceBudget.Visible = true;
            divIssuanceAssociation.Visible = false;
            divIssuanceBudget.Visible = true;
            divPolicyNoteBudgetamt.Visible = true;
            if (divPolicyNote.Visible == false)
            {
                divPolicyNoteBudgetamt.Visible = false;

            }
            else
            {
                divPolicyNoteBudgetamt.Visible = true;
            }
            divNote.Visible = false;
            if (divbamountbottom.Visible == true && txtBudgetAmt.Text == "")
            {
                ddlIssuance.Enabled = true;
                txtBudgetAmt.Enabled = true;
                txtBudgetAmt.Visible = true;
            }
            else
            {
                ddlIssuance.Enabled = false;
                txtBudgetAmt.Enabled = false;
            }

            foreach (GridViewRow gr in gv.Rows)
            {
                Label lblAssociationName = gr.FindControl("lblsLookupName") as Label;
        
                Label lblPolicyNote = gr.FindControl("lblPolicyNote") as Label;
              
                if (lblAssociationName.Text.Trim() != null)
                {
                    if (lblAssociationName.Text.Trim() == "Create Group Association with Budget")
                    {
                      txtBudgetAmt.Enabled = true;
                        return;
                    }

                }


            }


        }
        else if (ddlAssociationPolicy.SelectedItem.Text.Trim() == "Create Group Association")
        {
            divBudgetamt.Visible = false;
            spanBAmount.Visible = false;
            divbamountbottom.Visible = false;
            divIssuanceAssociation.Visible = false;
            divPolicyNoteBudgetamt.Visible = false;

            if (intObj == 0)
            {
                divIssuanceBudget.Visible = true;
            }
            else
            {
                divIssuanceBudget.Visible = false;
            }
            foreach (GridViewRow gr in gv.Rows)
            {
                Label lblAssociationName = gr.FindControl("lblsLookupName") as Label;
              
                Label lblPolicyNote = gr.FindControl("lblPolicyNote") as Label;
                if (lblAssociationName.Text != null)
                {
                    if (lblAssociationName.Text.Trim() == "Create Group Association")
                    {

                        
                        ddlIssuance.Enabled = false;

                        return;
                    }

                }

            }
        }
        else if (ddlAssociationPolicy.SelectedItem.Text.Trim() == "Create Single Item Association")
        {
            divIssuanceBudget.Visible = false;
            divNote.Visible = false;
            divBudgetamt.Visible = false;
            divIssuanceAssociation.Visible = false;
            divPolicyNoteBudgetamt.Visible = false;
            if (intObj == 0)
            {
                divIssuanceBudget.Visible = true;
            }
            else
            {
                divIssuanceBudget.Visible = false;
            }
        }

        else
        {
            divBudgetamt.Visible = false;
            spanBAmount.Visible = false;
            divbamountbottom.Visible = false;
            ddlIssuance.Enabled = true;
            divIssuanceBudget.Visible = false;
        }



        BindMasterItems();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }

    private void ClrData()
    {
        imgSplashImage.Src = "~/UploadedImages/employeePhoto/employee-photo.gif";
        ddlEmployeeType.SelectedIndex = 0;
        ddlClimate.SelectedIndex = 0;

    }
    protected void txtNewGroup_TextChanged(object sender, EventArgs e)
    {
        ddlIssuance.Enabled = true;
        if (ddlAssociationPolicy.SelectedItem.Text == "Create Group Association with Budget")
        {
            divBudgetamt.Visible = true;
            spanBAmount.Visible = true;
            divbamountbottom.Visible = true;
            divIssuanceBudget.Visible = true;
            txtBudgetAmt.Enabled = true;
            divPolicyNote.Visible = true;
            divPolicyNoteBudgetamt.Visible = true;
            IsGroupAssociation = "N";
            
        }
        else if (ddlAssociationPolicy.SelectedItem.Text == "Create Group Association")
        {
          
            IsGroupAssociation = "Y";
            divPolicyNote.Visible = true;
            divPolicyNoteBudgetamt.Visible = true;
            divIssuanceBudget.Visible = false;
            
        }
        else
        {
            divBudgetamt.Visible = false;
            spanBAmount.Visible = false;
            divbamountbottom.Visible = false;
            IsGroupAssociation = "N";
            divPolicyNote.Visible = false;
            divPolicyNoteBudgetamt.Visible = false;
            
        }

       
    }
    protected void ddlEmployeeType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ddlWeatherType_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void gv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {try
        {
        gv.EditIndex = -1;
        gv.DataBind();
        }
    catch (Exception ex)
    {
        ErrHandler.WriteError(ex);
    }
    }
    protected void gv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)gv.Rows[e.RowIndex];
            int intUniformIssuancePolicyItemID = Int32.Parse(gv.DataKeys[e.RowIndex].Value.ToString());
            DropDownList ddlEmployeeType = (DropDownList)row.FindControl("ddlEmployeeType");
            DropDownList ddlWeatherType = (DropDownList)row.FindControl("ddlWeatherType");
            //TextBox tPageDesc = (TextBox)row.FindControl("txtPageDesc");
            UniformIssuancePolicyItem objnewitem = new UniformIssuancePolicyItem();
            objnewitem = objUniformIssuancePolicyStyleRepository.GetById(intUniformIssuancePolicyItemID);
            objnewitem.WeatherTypeid = Convert.ToInt32(ddlWeatherType.SelectedValue);
            objnewitem.EmployeeTypeid = Convert.ToInt32(ddlEmployeeType.SelectedValue);
            objUniformIssuancePolicyStyleRepository.SubmitChanges();
            lblMsg.Text = "Record Updated successfully.";
            // Refresh the data
            gv.EditIndex = -1;
            gv.DataBind();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}
