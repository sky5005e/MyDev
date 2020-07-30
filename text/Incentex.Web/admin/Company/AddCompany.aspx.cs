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
using Incentex.BE;
using Incentex.DA;
public partial class admin_Company_AddCompany : PageBase
{
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
    Int64 CompanyContactInfoID
    {
        get
        {
            if (ViewState["CompanyContactInfoId"] == null)
            {
                ViewState["CompanyContactInfoId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyContactInfoId"]);
        }
        set
        {
            ViewState["CompanyContactInfoId"] = value;
        }
    }
    Int64 NoteId
    {
        get
        {
            if (ViewState["NoteId"] == null)
            {
                ViewState["NoteId"] = 0;
            }
            return Convert.ToInt64(ViewState["NoteId"]);
        }
        set
        {
            ViewState["NoteId"] = value;
        }
    }
    CountryRepository obj = new CountryRepository();
    StateRepository objState = new StateRepository();
    CityRepository objCity = new CityRepository();
    Company objCompany = new Company();
    CompanyRepository objComRep = new CompanyRepository();
    LookupRepository objLookRep = new LookupRepository();
    CompanyContactInfo objCompanyInfo = new CompanyContactInfo();
    CompanyContactInfoRepository objCompContactRepository = new CompanyContactInfoRepository();
    NoteDetail objNote = new NoteDetail();
    NotesHistoryRepository objNoteHistRepos = new NotesHistoryRepository();
    int iDuplicate;
    bool bolflag = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Customer HQ Data";
            base.ParentMenuID = 11;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            FillCompCountry();
            FillAccountCountry();
            FillPaymentterm();
            txtCompanyName.Focus();
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Customer HQ Data";
            ((HtmlGenericControl)manuControl.FindControl("dvSubMenu")).Visible = false;
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Company/ViewCompany.aspx";
            if (Request.QueryString["id"] != "0")
            {
                this.CompanyId = Convert.ToInt64(Request.QueryString["id"]);                

                PopulateControl(sender, e);
                lnkAddNew.Enabled = true;
                txtNoteHistory.Enabled = false;
            }
            else
            {
                lnkAddNew.Enabled = false;
                txtNoteHistory.Enabled = true;
                ClearData();
            }
            manuControl.PopulateMenu(0, 0, this.CompanyId, 0, false);
        }
    }
    public void FillPaymentterm()
    {
        string strPayment = "PaymentTerms";
        ddlPaymentTerms.DataSource = objLookRep.GetByLookup(strPayment);
        ddlPaymentTerms.DataValueField = "iLookupID";
        ddlPaymentTerms.DataTextField = "sLookupName";
        ddlPaymentTerms.DataBind();
        ddlPaymentTerms.Items.Insert(0, new ListItem("-select payment terms-", "0"));
    }
    /// <summary>
    /// FillCompCountry()
    /// Nagmani 04/09/2010
    /// </summary>
    public void FillCompCountry()
    {
        try
        {
            //Company
            ddlCountry.DataSource = obj.GetAll();
            ddlCountry.DataTextField = "sCountryName";
            ddlCountry.DataValueField = "iCountryID";
            ddlCountry.DataBind();
            ddlCountry.Items.Insert(0, new ListItem("-select country-", "0"));
            ddlCountry.SelectedValue = ddlCountry.Items.FindByText("United States").Value;


            ddlState.DataSource = objState.GetByCountryId(Convert.ToInt32(ddlCountry.SelectedItem.Value));
            ddlState.DataValueField = "iStateID";
            ddlState.DataTextField = "sStateName";
            ddlState.DataBind();
            ddlState.Items.Insert(0, new ListItem("-select state-", "0"));

            ddlCity.Items.Clear();
            ddlCity.Items.Add(new ListItem("-select city-", "0"));
           



        }
        catch (System.Threading.ThreadAbortException lException)
        {

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }
    /// <summary>
    /// FillAccountCountry()()
    /// Nagmani 04/09/2010
    /// </summary>
    public void FillAccountCountry()
    {
        try
        {
            //Accounting
            ddldCountry.DataSource = obj.GetAll();
            ddldCountry.DataTextField = "sCountryName";
            ddldCountry.DataValueField = "iCountryID";
            ddldCountry.DataBind();
            ddldCountry.Items.Insert(0, new ListItem("-select country-", "0"));
            ddldCountry.SelectedValue = ddldCountry.Items.FindByText("United States").Value;


            ddldState.DataSource = objState.GetByCountryId(Convert.ToInt32(ddldCountry.SelectedItem.Value));
            ddldState.DataValueField = "iStateID";
            ddldState.DataTextField = "sStateName";
            ddldState.DataBind();
            ddldState.Items.Insert(0, new ListItem("-select state-", "0"));

            ddldCity.Items.Clear();
            ddldCity.Items.Add(new ListItem("-select city-", "0"));

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
    /// <summary>
    /// Nagmani 04/09/2010
    /// On Country Selection changed State dropdownlist fill.
    /// </summary>
    /// <param name="sender">icountryid</param>
    /// <param name="e"></param>
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
                ddlState.DataSource = objState.GetByCountryId(Convert.ToInt32(ddlCountry.SelectedValue));
                ddlState.DataTextField = "sStateName";
                ddlState.DataValueField = "iStateID";
                ddlState.DataBind();
                ddlState.Items.Insert(0, new ListItem("-select state-", "0"));

                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-select city-", "0"));


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


    }
    /// <summary>
    /// Nagmani 04/09/2010
    /// On State Selection changed City dropdownlist fill.
    /// </summary>
    /// <param name="sender">istateid</param>
    /// <param name="e"></param>
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
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
                ddlCity.Enabled = true;

                ddlCity.DataSource = objCity.GetByStateId(Convert.ToInt32(ddlState.SelectedItem.Value));
                ddlCity.DataValueField = "iCityID";
                ddlCity.DataTextField = "sCityName";
                ddlCity.DataBind();
                ddlCity.Items.Insert(0, new ListItem("-select city-", "0"));

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
    }
    /// <summary>
    /// Nagmani 04/09/2010
    /// On Country Selection changed State dropdownlist fill.
    /// </summary>
    /// <param name="sender">icountryid</param>
    /// <param name="e"></param>
    protected void ddldCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddldCountry.SelectedIndex <= 0)
            {
                ddldState.Enabled = false;
                ddldState.Items.Clear();
                ddldState.Items.Add(new ListItem("-select state-", "0"));

                ddldCity.Enabled = false;
                ddldCity.Items.Clear();
                ddldCity.Items.Add(new ListItem("-select city-", "0"));
            }
            else
            {
                ddldState.Enabled = true;
                ddldState.DataSource = objState.GetByCountryId(Convert.ToInt32(ddldCountry.SelectedValue));
                ddldState.DataTextField = "sStateName";
                ddldState.DataValueField = "iStateID";
                ddldState.DataBind();
                ddldState.Items.Insert(0, new ListItem("-select state-", "0"));

                ddldCity.Items.Clear();
                ddldCity.Items.Add(new ListItem("-select city-", "0"));


            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    /// <summary>
    /// Nagmani 04/09/2010
    /// On State Selection changed City dropdownlist fill.
    /// </summary>
    /// <param name="sender">istateid</param>
    /// <param name="e"></param>
    protected void ddldState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddldState.SelectedIndex <= 0)
            {
                ddldCity.Enabled = false;
                ddldCity.Items.Clear();
                ddldCity.Items.Add(new ListItem("-select city-", "0"));
            }
            else
            {
                ddldCity.Enabled = true;

                ddldCity.DataSource = objCity.GetByStateId(Convert.ToInt32(ddldState.SelectedItem.Value));
                ddldCity.DataValueField = "iCityID";
                ddldCity.DataTextField = "sCityName";
                ddldCity.DataBind();
                ddldCity.Items.Insert(0, new ListItem("-select city-", "0"));

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
    }
    /// <summary>
    ///Insert Record.
    /// SaveData()
    /// Nagmani 04/09/2010
    /// </summary>
    public void SaveData()
    {
        try
        {
            //Insert into Company tabel
            this.CompanyId = Convert.ToInt64(Request.QueryString["id"]);
            if (this.CompanyId != 0)
            {
                objCompany = objComRep.GetById(this.CompanyId);
            }
            objCompany.CompanyName = txtCompanyName.Text;
            objCompany.CompanyAdddress = txtAddress.Value;
            objCompany.CountryId = Convert.ToInt32(ddlCountry.SelectedValue);
            objCompany.StateId = Convert.ToInt32(ddlState.SelectedValue);
            objCompany.CityId = Convert.ToInt32(ddlCity.SelectedValue);
            objCompany.Zip = txtZip.Text;
            objCompany.Telephone = txtTelephone.Text;
            objCompany.Website = txtWebSite.Text;
            objCompany.StatusID = 214;
            objCompany.PaymentTermsID = Convert.ToInt32(ddlPaymentTerms.SelectedValue);
            //objCompany.PaymentTermsID = Convert.ToInt64(selectedPaymentTerm.ToString());
            objCompany.CreateDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
            objCompany.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;// 10; //Session["CurrentUser"].ToString();
            objCompany.UpdateDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
            objCompany.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;// 10;
            if (chkCentralized.Checked == true)
            {
                objCompany.isCentralized = true;
            }
            else
            {
                objCompany.isCentralized = false;
            }
            if (this.CompanyId == 0)
            {

                string modeAdd = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.Add);
                iDuplicate = objComRep.CheckDuplicate(txtCompanyName.Text, Convert.ToInt32(Request.QueryString.Get("id")), modeAdd);
                if (iDuplicate == 0)
                {
                    objComRep.Insert(objCompany);
                    objComRep.SubmitChanges();
                    this.CompanyId = objCompany.CompanyID;
                    Session["CompanyId"] = objCompany.CompanyID;

                }
                else
                {
                    // Page.SetFocus(txtCompanyName);
                    bolflag = true;
                    return;
                }
            }
            else
            {

                string modeEdit = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.Edit);
                iDuplicate = objComRep.CheckDuplicate(txtCompanyName.Text, Convert.ToInt32(Request.QueryString.Get("id")), modeEdit);

                if (iDuplicate == 0)
                {
                    objComRep.SubmitChanges();
                    this.CompanyId = objCompany.CompanyID;
                    Session["CompanyId"] = objCompany.CompanyID;

                }
                else
                {
                    //Page.SetFocus(txtCompanyName);
                    bolflag = true;
                    return;
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
    }
    /// <summary>
    /// Save the record in the comapny and companycontactifno
    /// table.
    /// Nagmani 06/09/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            SaveData();
            SaveAccountData();
            NoteSave();
            if (bolflag == false)
            {
                Response.Redirect("ViewUploadDocument.aspx?id=" + Session["CompanyId"]);
            }
            else
            {
                txtCompanyName.Focus();
                lblmsg.Text = "Record already exist!";
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
    }
    /// <summary>
    /// ClearData()
    /// Celear Record in New Mode of screen when Open.
    /// Nagmani 06/09/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void ClearData()
    {
        txtCompanyName.Text = "";
        txtdAddress.Value = "";
        txtdEmail.Text = "";
        txtdExtension.Text = "";
        txtdFax.Text = "";
        txtdFirstName.Text = "";
        txtdLastName.Text = "";
        txtdMobile.Text = "";
        txtdTitle.Text = "";
        txtZip.Text = "";
        txtdTitle.Text = "";
        txtWebSite.Text = "";
        chkCentralized.Checked = false;
        txtAddress.Value = "";
        //txtNoteHistory.Text = "";
    }
    /// <summary>
    /// PopulateControl()()
    /// Reterieve the record from Table Company , Noticedetail and
    /// CompanyContactinfo table on the companyid.
    /// Nagmani 07/09/2010
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void SaveAccountData()
    {
        try
        {

            //Insert into CompanyContactinfo
            this.CompanyId = Convert.ToInt32(Session["CompanyId"]);
            string strAccoutType = "AccountType";
            if (this.CompanyId != 0)
            {
                objCompanyInfo = objCompContactRepository.GetById(this.CompanyId, strAccoutType);
                this.CompanyContactInfoID = objCompanyInfo.CompanyContactInfoID;
            }

            objCompanyInfo.FirstName = txtdFirstName.Text;
            objCompanyInfo.CompanyID = CompanyId;
            objCompanyInfo.LastName = txtdLastName.Text;
            objCompanyInfo.Address = txtdAddress.Value;
            objCompanyInfo.Mobile = txtdMobile.Text;
            objCompanyInfo.Telephone = txtdTelphone.Text;
            objCompanyInfo.Title = txtdTitle.Text;
            objCompanyInfo.StateId = Convert.ToInt32(ddldState.SelectedValue);
            objCompanyInfo.CountryId = Convert.ToInt32(ddldCountry.SelectedValue);
            objCompanyInfo.CityId = Convert.ToInt32(ddldCity.SelectedValue);
            objCompanyInfo.Extension = txtdExtension.Text;
            objCompanyInfo.Email = txtdEmail.Text;
            objCompanyInfo.Fax = txtdFax.Text;
            objCompanyInfo.Zip = txtdZip.Text;
            objCompanyInfo.ContactType = strAccoutType;
            // Object objContactInfo = null;
            //if (this.CompanyId != 0)
            // {
            // objContactInfo = objCompContactRepository.GetById(this.CompanyId, strAccoutType);
            //}

            if (this.CompanyContactInfoID == 0)
            {
                objCompContactRepository.Insert(objCompanyInfo);
                objCompContactRepository.SubmitChanges();
                this.CompanyContactInfoID = objCompanyInfo.CompanyContactInfoID;
            }
            else
            {

                objCompContactRepository.SubmitChanges();
                this.CompanyContactInfoID = objCompanyInfo.CompanyContactInfoID;
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
    }
    public void PopulateControl(object sender, EventArgs e)
    {
        try
        {

            if (Request.QueryString["id"] != "0")
            {

                int iCompanyID = Convert.ToInt32(Request.QueryString["id"].ToString());
                // int iCompanyID = Convert.ToInt32(Session["CompanyID"].ToString());
                //Reterieve Data from company table here
                String strAccountType = "AccountType";
                objCompany = objComRep.GetSingleCompany(iCompanyID);
                objCompanyInfo = objCompContactRepository.GetSingleCompanyContactInfo(iCompanyID, strAccountType);
                //Start Company
                if (objCompany.CompanyName != null)
                {
                    txtCompanyName.Text = objCompany.CompanyName;
                }
                if (objCompany.CompanyAdddress != null)
                {
                    txtAddress.Value = objCompany.CompanyAdddress;
                }
                if (objCompany.Zip != null)
                {
                    txtZip.Text = objCompany.Zip;
                }
                if (objCompany.Telephone != null)
                {
                    txtTelephone.Text = objCompany.Telephone;
                }
                if (objCompany.Website != null)
                {
                    txtWebSite.Text = objCompany.Website;
                }
                FillPaymentterm();
                if (objCompany.PaymentTermsID != null)
                {
                    ddlPaymentTerms.SelectedValue = objCompany.PaymentTermsID.ToString();
                }
                if (objCompany.isCentralized == true)
                {
                    //spanisCentralised.Attributes.Add("class", "custom-checkbox_checked");
                    chkCentralized.Checked = true;
                }
                else
                {
                    chkCentralized.Checked = false;
                }

                ddlCountry.SelectedValue = objCompany.CountryId.ToString();
                ddlCountry_SelectedIndexChanged(sender, e);
                ddlState.SelectedValue = objCompany.StateId.ToString();
                ddlState_SelectedIndexChanged(sender, e);
                ddlCity.SelectedValue = objCompany.CityId.ToString();

                //End Company

                //Satart Accountinf Of Company
                if (objCompanyInfo != null)
                {
                    if (objCompanyInfo.FirstName != null)
                    {
                        txtdFirstName.Text = objCompanyInfo.FirstName;
                    }
                    if (objCompanyInfo.LastName != null)
                    {
                        txtdLastName.Text = objCompanyInfo.LastName;
                    }
                    if (objCompanyInfo.Title != null)
                    {
                        txtdTitle.Text = objCompanyInfo.Title;
                    }

                    if (objCompanyInfo.Address != null)
                    {
                        txtdAddress.Value = objCompanyInfo.Address;
                    }
                    if (objCompanyInfo.Zip != null)
                    {
                        txtdZip.Text = objCompanyInfo.Zip;
                    }
                    if (objCompanyInfo.Telephone != null)
                    {
                        txtdTelphone.Text = objCompanyInfo.Telephone;
                    }
                    if (objCompanyInfo.Fax != null)
                    {
                        txtdFax.Text = objCompanyInfo.Fax;
                    }
                    if (objCompanyInfo.Mobile != null)
                    {
                        txtdMobile.Text = objCompanyInfo.Mobile;
                    }
                    if (objCompanyInfo.Email != null)
                    {
                        txtdEmail.Text = objCompanyInfo.Email;
                    }
                    if (objCompanyInfo.Extension != null)
                    {
                        txtdExtension.Text = objCompanyInfo.Extension;
                    }

                    ddldCountry.SelectedValue = objCompanyInfo.CountryId.ToString();
                    ddldCountry_SelectedIndexChanged(sender, e);
                    ddldState.SelectedValue = objCompanyInfo.StateId.ToString();
                    ddldState_SelectedIndexChanged(sender, e);
                    ddldCity.SelectedValue = objCompanyInfo.CityId.ToString();
                }
                //End Accountinf Of Company
                //Start history Notes
                if (objNote != null)
                {
                    DisplayNotes();
                    
                }
                //End
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

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            this.CompanyId = Convert.ToInt64(Request.QueryString["id"]);
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            NoteDetail obj = new NoteDetail()
            {
                ForeignKey = this.CompanyId,
                Notecontents = txtNote.Text
                ,
                NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.CompanyAccount)
                ,
                CreatedBy = IncentexGlobal.CurrentMember.UserInfoID
                ,
                CreateDate = DateTime.Now
                ,
                UpdateDate = DateTime.Now
                ,
                UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID
            };

            objRepo.Insert(obj);
            objRepo.SubmitChanges();
            DisplayNotes();
            txtNote.Text = string.Empty;
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
            //lblMsg.Text = "Error in saving record ...";
        }
    }
    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        modalAddnotes.Show();
    }
    public void NoteSave()
    {
        try
        {
            //NoteHistory Information for Company
            string strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.CompanyAccount);
            object objNoteCheckCompany = null;
            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();
            if (this.CompanyId != 0)
            {
                objComNot.Notecontents = txtNoteHistory.Text;
                objComNot.NoteFor = strNoteFor;
                objComNot.ForeignKey = CompanyId;
                objComNot.CreateDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
                objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID; // 10; //Session["CurrentUser"].ToString();
                objComNot.UpdateDate = Convert.ToDateTime(System.DateTime.Now.ToShortDateString());
                objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;// 10;
                if (this.CompanyId != 0)
                {
                    objNoteCheckCompany = objCompNoteHistRepos.GetById(this.CompanyId, strNoteFor);
                }
                if (objNoteCheckCompany == null)
                {
                    objCompNoteHistRepos.Insert(objComNot);
                    objCompNoteHistRepos.SubmitChanges();
                    this.NoteId = objComNot.NoteID;
                }
                else
                {

                }


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
    }
    public void DisplayNotes()
    {
        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        List<NoteDetail> objList = objRepo.GetByForeignKeyId(Convert.ToInt64(Request.QueryString.Get("id")), Incentex.DAL.Common.DAEnums.NoteForType.CompanyAccount);
        txtNoteHistory.Text = string.Empty;
        foreach (NoteDetail obj in objList)
        {
            txtNoteHistory.Text += "Created Date : " + Convert.ToDateTime(obj.CreateDate).ToString("dd-MMM-yyyy") + "\n";
            txtNoteHistory.Text += "Created Time : " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n";
            UserInformationRepository objUserRepo = new UserInformationRepository();
            UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);

            if (objUser != null)
            {
                txtNoteHistory.Text += "Created By : " + objUser.FirstName + " " + objUser.LastName + "\n";
            }
            txtNoteHistory.Text += "Note : " + obj.Notecontents + "\n";
            txtNoteHistory.Text += "---------------------------------------------";
            txtNoteHistory.Text += "\n";


        }
    }
    }
