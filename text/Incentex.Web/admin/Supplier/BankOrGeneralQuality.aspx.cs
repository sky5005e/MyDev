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
using Incentex.DAL.Common;
using System.Collections.Generic;
using System.IO;

public partial class admin_Supplier_BankOrGeneralQuality : PageBase
{

    #region Properties

    Int64 SupplierId
    {
        get
        {
            if (ViewState["SupplierId"] == null)
            {
                ViewState["SupplierId"] = 0;
            }
            return Convert.ToInt64(ViewState["SupplierId"]);
        }
        set
        {
            ViewState["SupplierId"] = value;
        }
    }

    public Int64 DocTypeId_SupplierQualityCertificates
    {
        get
        {
            if (ViewState["DocTypeId_SupplierQualityCertificates"] == null)
            {
                ViewState["DocTypeId_SupplierQualityCertificates"] = 0;
            }
            return Convert.ToInt64(ViewState["DocTypeId_SupplierQualityCertificates"]);
        }
        set
        {
            ViewState["DocTypeId_SupplierQualityCertificates"] = value;
        }
    }

#endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Bank/Quality Info";
            base.ParentMenuID = 18;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Bank / Quality Info";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to Supplier listing</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Supplier/ViewSupplier.aspx";
          

            if (Request.QueryString.Count > 0)
            {
                this.SupplierId = Convert.ToInt64(Request.QueryString.Get("Id"));
                manuControl.PopulateMenu(1, 0, this.SupplierId, 0, false);
        
            }
            else
            {
                Response.Redirect("~/admin/Supplier/ViewSupplier.aspx");
            }

            if(this.SupplierId == 0)
            {
                Response.Redirect("~/admin/Supplier/MainCompanyContact.aspx?Id=0");
            }

            Common.BindCountry(ddlCountry);
            lst.DataBind();
            // lstSupplierType.DataBind();
            BindSupplierType();
            DisplayData();
            SetDocumentTypeId();
            gvDocumnet.DataBind();
            
        }
    }


    /// <summary>
    /// Display Existing record
    /// </summary>
    void DisplayData()
    {

        SupplierRepository objSupplierRepository = new SupplierRepository();
        Supplier obj = objSupplierRepository.GetById(this.SupplierId);

        if(obj != null)
        {
            string GeneralQualitySystemCompliances = "";


            txtBankName.Text = obj.BankName;
            txtBankContactPerson.Text = obj.BannkContactPerson;
            txtAddress.Text = obj.BankAddress;

            if (obj.BankCountryID != null)
            {
                ddlCountry.SelectedValue = obj.BankCountryID.ToString();
            }

            if (obj.BankCountryID != null)
            {
                Common.BindState(ddlState, (long)obj.BankCountryID);

                ddlState.SelectedValue = obj.BankStateID.ToString();
            }

            if (obj.BankStateID != null)
            {
                Common.BindCity(ddlCity, (long)obj.BankStateID);
                ddlCity.SelectedValue = obj.BankCityID.ToString();
            }

            txtZip.Text = obj.BankZip;
            txtFax.Text = obj.BankFax;
            txtEmail.Text = obj.BankEmail;
            txtTelephone.Text = obj.BankTelephone;
            txtMobile.Text = obj.BankMobile;
            txtAccountNumber.Text = obj.BankAccountNumber;
            txtAccountName.Text = obj.BankAccountName;
            txtRoutingNumber.Text = obj.BankRoutingNumber;
            txtEmaiABA.Text = obj.BankEmailABA;
            GeneralQualitySystemCompliances = obj.GeneralQualitySystemCompliances;
            if (Convert.ToString(obj.SupplierTypeID) != null)
            {
                ddlSupplierType.SelectedValue = obj.SupplierTypeID.ToString();
            }

            if (!string.IsNullOrEmpty(GeneralQualitySystemCompliances))
            {

                string[] Ids = GeneralQualitySystemCompliances.Split(',');

                foreach (string id in Ids)
                {
                    foreach (DataListItem li in lst.Items)
                    {
                        CheckBox chk = li.FindControl("chk") as CheckBox;
                        Label lblId = li.FindControl("lblId") as Label;
                        HtmlGenericControl spchk = li.FindControl("spchk") as HtmlGenericControl;

                        if (lblId.Text.Equals(id))
                        {
                            chk.Checked = true;
                            spchk.Attributes.Add("class", "custom-checkbox_checked alignleft");
                        }
                    }
                }
            }
        }

    }

    void BindSupplierType()
    {
        LookupRepository objRepo = new LookupRepository();
        List<INC_Lookup> objList = objRepo.GetByLookupCode(DAEnums.LookupCodeType.SupplierType);
        Common.BindDDL(ddlSupplierType, objList, "sLookupName", "iLookupID", "-select type-");
    }


    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindState(ddlState, Convert.ToInt64(ddlCountry.SelectedValue));
    }

    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindCity(ddlCity, Convert.ToInt64(ddlState.SelectedValue));
    }

    protected void lst_DataBinding(object sender, EventArgs e)
    {
        LookupRepository objRepo = new LookupRepository();
        List<INC_Lookup> objList = objRepo.GetByLookupCode(DAEnums.LookupCodeType.SupplierGeneralQualitySystemCompliances);
        lst.DataSource = objList;
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            SupplierRepository objSupplierRepository = new SupplierRepository();

            Supplier obj = objSupplierRepository.GetById(this.SupplierId);

            string GeneralQualitySystemCompliances = "";

            foreach (DataListItem li in lst.Items)
            {
                CheckBox chk = li.FindControl("chk") as CheckBox;
                if (chk.Checked)
                {
                    Label lblId = li.FindControl("lblId") as Label;
                    GeneralQualitySystemCompliances += lblId.Text + ",";
                }
            }

            obj.BankName = txtBankName.Text;
            obj.BannkContactPerson = txtBankContactPerson.Text;
            obj.BankAddress = txtAddress.Text;
            obj.BankCountryID = Convert.ToInt64(ddlCountry.SelectedValue);
            obj.BankStateID = Convert.ToInt64(ddlState.SelectedValue);
            obj.BankCityID = Convert.ToInt64(ddlCity.SelectedValue);
            obj.BankZip = txtZip.Text;
            obj.BankFax = txtFax.Text;
            obj.BankEmail = txtEmail.Text;
            obj.BankTelephone = txtTelephone.Text;
            obj.BankMobile = txtMobile.Text;
            obj.BankAccountNumber = txtAccountNumber.Text;
            obj.BankAccountName = txtAccountName.Text;
            obj.BankRoutingNumber = txtRoutingNumber.Text;
            obj.BankEmailABA = txtEmaiABA.Text;
            obj.GeneralQualitySystemCompliances = GeneralQualitySystemCompliances;
            if (ddlSupplierType.SelectedIndex > 0)
            {
                obj.SupplierTypeID = Convert.ToInt64(ddlSupplierType.SelectedValue);
            }
            else
            {
                obj.SupplierTypeID = null;
            }

            objSupplierRepository.SubmitChanges();
            lblMsg.Text = "Record Saved Successfully ...";
            Response.Redirect("FactoryOrPersonalInfo.aspx?Id=" + this.SupplierId ,false);
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in saving record ...";
            ErrHandler.WriteError(ex);
        }
    }

    #region Document

    void SetDocumentTypeId()
    {
        LookupRepository objLookupRepository = new LookupRepository();

        // get DocumentTypeId from lookup list
        DocTypeId_SupplierQualityCertificates = objLookupRepository.GetSupplierDocumentLookUpId(LookupRepository.SupplierDocumentsType.SupplierQualityCertificates); 
        

    }

    /// <summary>
    /// Event for document grid databinding
    /// Amit 17Sep10
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDocumnet_DataBinding(object sender, EventArgs e)
    {

        Int64 DocumentTypeId = this.DocTypeId_SupplierQualityCertificates;

        //get document List
        DocumentRepository objRepo = new DocumentRepository();

        List<Document> objList = objRepo.GetByDocumentTypeId(DocumentTypeId,this.SupplierId);
        gvDocumnet.DataSource = objList;

    }

    protected void gvDocumnet_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    /// <summary>
    /// habdle grid row command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvDocumnet_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch(e.CommandName)
        {
            case "del":
                try
                {
                    HiddenField hdnFileName = ((HiddenField)((DataControlFieldCell)((LinkButton)e.CommandSource).Parent).FindControl("hdnFileName"));
                    string PathAndName = Common.SupplierDocumentUploadPath + hdnFileName.Value;

                    //delete file
                    Common.DeleteFile(PathAndName);

                    Int64 DocumentId = Convert.ToInt64(e.CommandArgument);
                    DocumentRepository objRepo = new DocumentRepository();
                    objRepo.Delete(DocumentId);
                    gvDocumnet.DataBind();
                }
                catch(Exception ex)
                {
                    ErrHandler.WriteError(ex);
                }

                break;

        }
    }

    /// <summary>
    /// save document
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (fluDocument.HasFile == true || fluDocument.FileName != string.Empty)
        {

            try
            {

                DocumentRepository objRepo = new DocumentRepository();
                string OriginalFileName = "";
                string FileName = "";

                FileInfo objFileInfo = new FileInfo(fluDocument.FileName);
                OriginalFileName = objFileInfo.Name;

                FileName = OriginalFileName.Split('.')[0].Replace(" ", "_") + "_" + this.SupplierId.ToString() + "_" + this.DocTypeId_SupplierQualityCertificates.ToString() + "_" + LookupRepository.SupplierDocumentsType.SupplierQualityCertificates.ToString() + "_" + DateTime.Now.ToString("ddMMMyyyyhhss") + objFileInfo.Extension;

                Document objExist = objRepo.GetByTypeAndName(this.DocTypeId_SupplierQualityCertificates, OriginalFileName);

                if(objExist != null )
                {
                    lblMessage.Text = "Document already exist ...";
                    modal.Show();
                    return;
                }

                Document obj = new Document();
                obj.CreatedDate = DateTime.Now;
                obj.DocumentFor = DAEnums.DocumentForType.Supplier.ToString();
                obj.DocumentTypeID = this.DocTypeId_SupplierQualityCertificates;
                obj.OriginalFileName = OriginalFileName;
                obj.FileName = FileName;
                obj.ForeignKey = this.SupplierId;

                string UploadPathAndName = Common.SupplierDocumentUploadPath + FileName;
                fluDocument.SaveAs(UploadPathAndName);

                objRepo.Insert(obj);
                objRepo.SubmitChanges();

                gvDocumnet.DataBind();
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
        }

        foreach (DataListItem li in lst.Items)
        {
            CheckBox chk = li.FindControl("chk") as CheckBox;
            Label lblId = li.FindControl("lblId") as Label;
            HtmlGenericControl spchk = li.FindControl("spchk") as HtmlGenericControl;

            if (chk.Checked)
            {
                spchk.Attributes.Add("class", "custom-checkbox_checked alignleft");
            }
        }
    }


    /// <summary>
    /// Display file upload popup
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lnkUploadQualityCertificates_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        modal.Show();
    }

    #endregion


   
}
