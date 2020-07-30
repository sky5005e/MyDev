using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;

using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using Incentex.DAL.Common;
using System.Web.UI.HtmlControls;

public partial class admin_ArtWorkDecoratingLibrary_MainCompanyPartners : PageBase
{
    #region Properties
    Int64 SupplierID
    {
        get
        {
            if (ViewState["SupplierID"] == null)
                ViewState["SupplierID"] = 0;

            return Convert.ToInt64(ViewState["SupplierID"]);
        }
        set
        {
            ViewState["SupplierID"] = value;
        }
    }

    DAEnums.SortOrderType SortOrder
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
    SupplierRepository.SupplierSortExpType SortExp
    {
        get
        {
            if (ViewState["SortExp"] == null)
            {
                ViewState["SortExp"] = SupplierRepository.SupplierSortExpType.FirstName;
            }
            return (SupplierRepository.SupplierSortExpType)ViewState["SortExp"];
        }
        set
        {
            ViewState["SortExp"] = value;
        }
    }
    SupplierRepository ObjsupplierRepo = new SupplierRepository();
    SupplierRepository objRepo = new SupplierRepository();
    DecoratingSpecsRepository objDecoRepo = new DecoratingSpecsRepository();
    #endregion 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/ArtWorkDecoratingLibrary/MngDecoratingPartners.aspx";
            ((Label)Master.FindControl("lblPageHeading")).Text = "Company Details";

            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                this.SupplierID = Convert.ToInt64(Request.QueryString["Id"]);

            BindDcrServices();
            DisplayData();
            BindGridView();
        }

    }

    private void BindDcrServices()
    {
        LookupRepository objLookupRepo = new LookupRepository();
        dtDecoratingServices.DataSource = objLookupRepo.GetByLookup("DecoratingServices");
        dtDecoratingServices.DataBind();
    }
    private void BindGridView()
    {
        //List<SupplierDetails> objList = objRepo.GetAllSupplierList(this.SortExp, this.SortOrder, this.SupplierID);
        //if (objList.Count > 0)
        //{
        //    gvEmpDetails.DataSource = objList;
        //    gvEmpDetails.DataBind();
        //}
        
    }
    private void BindGVPricing()
    {
        List<DecoratingPartner> objDPartners = objDecoRepo.GetAllDecoratingPartnersByID(this.SupplierID);
        if (objDPartners.Count > 0)
        {
            GridViewPricing.DataSource = objDPartners;
            GridViewPricing.DataBind();
        }

    }
    
    private void DisplayData()
    {
       
        List<SupplierDetails> objList = objRepo.GetAllSupplierList(this.SortExp, this.SortOrder,this.SupplierID);
        if (objList.Count > 0)
        {
            this.txtCompanyName.Text = Convert.ToString(objList[0].CompanyName);
            this.txtOwnerName.Text = Convert.ToString(objList[0].FullName);
            this.txtAddress1.Text = Convert.ToString(objList[0].Address1);
            this.txtAddress2.Text = Convert.ToString(objList[0].Address2);
            this.txtCity.Text = Convert.ToString(objList[0].City);
            this.txtState.Text = Convert.ToString(objList[0].State);
            this.txtTelephone.Text = Convert.ToString(objList[0].Telephone);
            this.txtFax.Text = Convert.ToString(objList[0].Fax);
            this.txtZip.Text = Convert.ToString(objList[0].ZipCode);

            // Decorating Services 
            if (objList[0].DecoratingServicesID != null)
            {
                String[] Ids = objList[0].DecoratingServicesID.Split(',');

                foreach (DataListItem DecoratingServices in dtDecoratingServices.Items)
                {
                    CheckBox chk = DecoratingServices.FindControl("chkDcrtServiceName") as CheckBox;
                    HiddenField lblId = DecoratingServices.FindControl("hdnDcrtServiceID") as HiddenField;
                    HtmlGenericControl dvChk = DecoratingServices.FindControl("DcrtServicespan") as HtmlGenericControl;

                    foreach (String i in Ids)
                    {
                        if (i.Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }
                    }
                }
            }

            DisplayNotesIE();
            BindGVPricing();
        }
        
    }
    public void DisplayNotesIE()
    {
        try
        {
            if (Request.QueryString["Id"] != null)
            {
                SupplierID = Convert.ToInt64(Request.QueryString["Id"].ToString());
            }
            else
            {
                SupplierID = 0;
            }
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            List<NoteDetail> objList = new List<NoteDetail>();
            objList = objRepo.GetNotesForIEPerOrderId(SupplierID, Incentex.DAL.Common.DAEnums.NoteForType.Supplier, "SupplierNotes", 0);
            if (objList.Count == 0)
            {
                txtNotes.Text = "";
            }
            else
            {
                txtNotes.Text = string.Empty;
                foreach (NoteDetail obj in objList)
                {
                    txtNotes.Text += obj.Notecontents;
                    txtNotes.Text += "\n\n";
                    UserInformationRepository objUserRepo = new UserInformationRepository();
                    UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);
                    if (objUser != null)
                    {
                        txtNotes.Text += objUser.FirstName + " " + objUser.LastName + "   ";
                    }
                    txtNotes.Text += Convert.ToDateTime(obj.CreateDate).ToString("MM/dd/yyyy");
                    txtNotes.Text += " @ " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n";
                    txtNotes.Text += "______________________________________________________________________________";
                    txtNotes.Text += "\n\n";



                }
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void lnkBtnSaveServices_Click(object sender, EventArgs e)
    { 
        try
        {
            String selectedID = String.Empty;
            Supplier supplier = ObjsupplierRepo.GetById(this.SupplierID);
            //Base stations
            foreach (DataListItem dt in dtDecoratingServices.Items)
            {
                if (((CheckBox)dt.FindControl("chkDcrtServiceName")).Checked == true)
                {
                    if (selectedID == "")
                        selectedID = ((HiddenField)dt.FindControl("hdnDcrtServiceID")).Value;
                    else
                        selectedID = selectedID + "," + ((HiddenField)dt.FindControl("hdnDcrtServiceID")).Value;
                }
            }

            supplier.DecoratingServicesID = selectedID;
            ObjsupplierRepo.SubmitChanges();
            DisplayData();
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void lnkNoteHis_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["Id"] != null)
            {
                SupplierID = Convert.ToInt64(Request.QueryString["Id"].ToString());
            }
            else
            {
                SupplierID = 0;
            }
            String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.Supplier);
            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();


            objComNot.Notecontents = txtCompanyNote.Text;
            objComNot.NoteFor = strNoteFor;
            objComNot.SpecificNoteFor = "SupplierNotes";
            objComNot.ForeignKey = SupplierID;
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objComNot.UpdateDate = System.DateTime.Now;
            objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

            if (!(string.IsNullOrEmpty(txtCompanyNote.Text)))
            {

                objCompNoteHistRepos.Insert(objComNot);
                objCompNoteHistRepos.SubmitChanges();
                int NoteId = (int)(objComNot.NoteID);

                txtCompanyNote.Text = "";
            }
            else
            {

            }

            DisplayNotesIE();

        }

        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);

        }
    }
    protected void lnkSavePricing_Click(object sender, EventArgs e)
    {
        try
        {
            String File = String.Empty;
            if (fileUp.HasFile)
            {
                File = "File_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + fileUp.FileName;
                String filePath = Server.MapPath("../../UploadedImages/DecoratingPartners/") + File;
                fileUp.SaveAs(filePath);
            }
            DecoratingPartner objDecoPrt = new DecoratingPartner();
            objDecoPrt.CustomerName = Convert.ToString(txtCustomerName.Text);
            objDecoPrt.CreatedDate = Convert.ToDateTime(txtDate.Text);
            objDecoPrt.SupplierID = Convert.ToInt64(this.SupplierID);
            objDecoPrt.CreatedDate = DateTime.Now;
            objDecoPrt.File = File;
            objDecoPrt.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID;
            objDecoPrt.Good_Until = Convert.ToString(txtGoodUntil.Text);
            objDecoRepo.Insert(objDecoPrt);
            objDecoRepo.SubmitChanges();

            
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        BindGVPricing();
    }

}
