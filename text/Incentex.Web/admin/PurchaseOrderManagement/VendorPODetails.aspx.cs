using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Incentex.DAL;
using Incentex.DAL.SqlRepository;


public partial class admin_PurchaseOrderManagement_VendorPODetails : PageBase
{
    #region Properties 
    Int64 POID
    {
        get
        {
            if (ViewState["POID"] == null)
                ViewState["POID"] = 0;

            return Convert.ToInt64(ViewState["POID"]);
        }
        set
        {
            ViewState["POID"] = value;
        }
    }

    PurchaseOrderManagmentRepository objPOMRepo = new PurchaseOrderManagmentRepository();
    UserInformationRepository objUserInfoRepo = new UserInformationRepository();
    #endregion 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Incentex Order Management";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }
            if (!String.IsNullOrEmpty(Request.QueryString["POID"]))
                this.POID = Convert.ToInt64(Request.QueryString["POID"]);

            ((Label)Master.FindControl("lblPageHeading")).Text = "PO Details";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/PurchaseOrderManagement/VendorAccessIncOrderManegement.aspx";
            BindDropDown();
            BindGridView();
            
            
            
        }
    }
    private void DisplayData(List<SearchPurchaseOrderVendor> objList)
    {
        if (objList.Count > 0)
        {
            lblExpDelivery.Text = String.Format("{0:MM/dd/yyyy}",objList[0].ExpDeliveryDate);
            lblStartPro.Text = String.Format("{0:MM/dd/yyyy}",objList[0].StartProductionDate);
            lblConDelivery.Text = String.Format("{0:MM/dd/yyyy}",objList[0].ConFirmedDeliveryDate);
            //lblViewPO.Text = objList[0].
            ddlStatus.SelectedValue = objList[0].StatusID.ToString();

            DisplayNotesIE();
        }
    }
    private void BindGridView()
    {
        SupplierEmployee objSup = new SupplierEmployeeRepository().GetEmployeeByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        if (objSup != null)
        {
            List<SearchPurchaseOrderVendor> objList = objPOMRepo.GetPurchaseOrderDetailsByPOID(this.POID, objSup.SupplierID);
            UserInformation obj = objUserInfoRepo.GetById(IncentexGlobal.CurrentMember.UserInfoID);
            if (obj != null)
                lblVendorName.Text = obj.FirstName + " " + obj.LastName;
            DisplayData(objList);
            gvItemDetails.DataSource = objList;
            gvItemDetails.DataBind();
        }

    }

    private void BindDropDown()
    {
        ddlStatus.DataSource = new LookupRepository().GetByLookup("Purchase Order Status");
        ddlStatus.DataValueField = "iLookupID";
        ddlStatus.DataTextField = "sLookupName";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new ListItem("-Select-", "0"));
    }
    public void DisplayNotesIE()
    {
        try
        {
            if (Request.QueryString["POID"] != null)
            {
                POID = Convert.ToInt64(Request.QueryString["POID"].ToString());
            }
            else
            {
                POID = 0;
            }
            NotesHistoryRepository objRepo = new NotesHistoryRepository();
            List<NoteDetail> objList = new List<NoteDetail>();
            objList = objRepo.GetNotesForIEPerOrderId(POID, Incentex.DAL.Common.DAEnums.NoteForType.SupplierPurchaseOrder, "POIEInternalNotes",0);
            if (objList.Count == 0)
            {
                txtOrderNotesForIE.Text = "";
            }
            else
            {
                txtOrderNotesForIE.Text = string.Empty;
                foreach (NoteDetail obj in objList)
                {
                    txtOrderNotesForIE.Text += obj.Notecontents;
                    txtOrderNotesForIE.Text += "\n\n";
                    UserInformationRepository objUserRepo = new UserInformationRepository();
                    UserInformation objUser = objUserRepo.GetById(obj.CreatedBy);
                    if (objUser != null)
                    {
                        txtOrderNotesForIE.Text += objUser.FirstName + " " + objUser.LastName + "   ";
                    }
                    txtOrderNotesForIE.Text += Convert.ToDateTime(obj.CreateDate).ToString("MM/dd/yyyy");
                    txtOrderNotesForIE.Text += " @ " + Convert.ToDateTime(obj.CreateDate).ToShortTimeString() + "\n";
                    txtOrderNotesForIE.Text += "______________________________________________________________________________";
                    txtOrderNotesForIE.Text += "\n\n";



                }
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void lnkSubmit_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow gvItem in gvItemDetails.Rows)
        {
            CheckBox chkItem = (CheckBox)gvItem.FindControl("chkItem");
            TextBox txtfinalamount = (TextBox)gvItem.FindControl("txtfinalamount");
            Label lblProductItemID = (Label)gvItem.FindControl("lblProductItemID");
            if (chkItem != null)
            {
                if (chkItem.Checked)
                {
                    PurchaseOrderDetail objPurchase = new PurchaseOrderDetail();
                    objPurchase = objPOMRepo.GetPurchaseOrderDetailByProductItemID(Convert.ToInt64(lblProductItemID.Text));
                    if (objPurchase != null && !String.IsNullOrEmpty(txtfinalamount.Text))
                    {
                        objPurchase.FinalQty = Convert.ToInt32(txtfinalamount.Text);
                        objPOMRepo.SubmitChanges();
                    }
                    chkItem.Checked = false;
                }
            }
        }
        BindGridView();
    }
    protected void lnkNoteHisForIE_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["POID"] != null)
            {
                POID = Convert.ToInt64(Request.QueryString["POID"].ToString());
            }
            else
            {
                POID = 0;
            }
            String strNoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.SupplierPurchaseOrder);
            NoteDetail objComNot = new NoteDetail();
            NotesHistoryRepository objCompNoteHistRepos = new NotesHistoryRepository();


            objComNot.Notecontents = txtNoteIE.Text;
            objComNot.NoteFor = strNoteFor;
            objComNot.SpecificNoteFor = Convert.ToString(POID);
            objComNot.ForeignKey = IncentexGlobal.CurrentMember.UserInfoID;
            objComNot.CreateDate = System.DateTime.Now;
            objComNot.CreatedBy = IncentexGlobal.CurrentMember.UserInfoID; 
            objComNot.UpdateDate = System.DateTime.Now;
            objComNot.UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID;

            if (!(string.IsNullOrEmpty(txtNoteIE.Text)))
            {

                objCompNoteHistRepos.Insert(objComNot);
                objCompNoteHistRepos.SubmitChanges();
                int NoteId = (int)(objComNot.NoteID);

                txtNoteIE.Text = "";
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
}
