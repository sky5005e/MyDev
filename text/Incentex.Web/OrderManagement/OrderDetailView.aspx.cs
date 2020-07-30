using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class OrderManagement_OrderDetailView : PageBase
{
    CompanyRepository objCompRepos = new CompanyRepository();
    OrderConfirmationRepository objOrdRepos = new OrderConfirmationRepository();
    List<SelectCompanyOrderDetailsResult> ObjCompanyOrderList = new List<SelectCompanyOrderDetailsResult>();

    Int64 SupplierId
    {
        get
        {
            return Convert.ToInt64(ViewState["SupplierId"]);
        }
        set
        {
            ViewState["SupplierId"] = value;
        }
    }
    List<SelectCompanyOrderDetailsResult> objSupplierORderList = new List<SelectCompanyOrderDetailsResult>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Order Detail View";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                bindOrderRepeaterHeaderForSupplier();
            else
                bindOrderRepeaterHeader();
        }
    }

    public void bindOrderRepeaterHeader()
    {
        try
        {
            List<Company> ObjList = new List<Company>();
            List<Company> ObjMainList = new List<Company>();
            ObjList = objCompRepos.GetAllCompany();

            //Logic: if company has a orders then and then only it should bind to the repeater..
            //Created By Ankit on 14 Mar 11
            foreach (Company c in ObjList)
            {
                ObjCompanyOrderList = objOrdRepos.GetCompanyOrderDetail(Convert.ToInt64(c.CompanyID));
                if (ObjCompanyOrderList.Count > 0)
                {
                    ObjMainList.Add(c);
                }
            }
            //End Logic
            if (ObjMainList.Count > 0)
            {
                rptOrderHeader.DataSource = ObjMainList;
                rptOrderHeader.DataBind();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    public void bindOrderRepeaterHeaderForSupplier()
    {
        try
        {
            //long SupplierId =  new SupplierRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID).SupplierID;
            Supplier objSupplier = new SupplierRepository().GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
            this.SupplierId = objSupplier.SupplierID;
            List<Company> ObjList = new List<Company>();
            List<Company> ObjMainList = new List<Company>();
            //ObjList = objCompRepos.GetByCompanyId(objSupplier.CompanyId);
            ObjList = objCompRepos.GetAllCompany();

            //Logic: if company has a orders then and then only it should bind to the repeater..
            //Created By Ankit on 14 Mar 11
            foreach (Company c in ObjList)
            {
                ObjCompanyOrderList = objOrdRepos.GetCompanyOrderDetail(Convert.ToInt64(c.CompanyID));
                if (ObjCompanyOrderList.Count > 0)
                {
                    ObjMainList.Add(c);
                }
            }
            //End Logic
            if (ObjMainList.Count > 0)
            {
                rptOrderHeader.DataSource = ObjMainList;
                rptOrderHeader.DataBind();
            }

        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    //Created By Ankit on 4-Mar-11
    protected void rptOrderHeader_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            GridView gridview = ((GridView)(e.Item.FindControl("gvOrderDetail")));
            HiddenField hiddenCompanyID = (HiddenField)e.Item.FindControl("hiddenCompanyID");
            ObjCompanyOrderList = objOrdRepos.GetCompanyOrderDetail(Convert.ToInt64(hiddenCompanyID.Value));
            if (ObjCompanyOrderList.Count > 0)
            {
                objSupplierORderList = ObjCompanyOrderList;
                gridview.DataSource = ObjCompanyOrderList;
                gridview.DataBind();
                //Assign For supplier Order Sorting
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.Supplier))
                {
                    //Hide the status coloumn..added on 28 Mar
                    gridview.Columns[5].Visible = false;
                    //End

                    List<GetSupplierOrdersResult> obj = new List<GetSupplierOrdersResult>();
                    foreach (GridViewRow grv in gridview.Rows)
                    {
                        List<GetSupplierOrdersResult> objSuppOrderCount = new SupplierRepository().GetSupplierOrder(Convert.ToInt64(((HiddenField)grv.FindControl("hdnOrderNumber")).Value), this.SupplierId);
                        if (objSuppOrderCount.Count == 0)
                        {
                            SelectCompanyOrderDetailsResult a = new SelectCompanyOrderDetailsResult();
                            objSupplierORderList.RemoveAll(delegate(SelectCompanyOrderDetailsResult p) { return p.OrderID == Convert.ToInt64(((HiddenField)grv.FindControl("hdnOrderNumber")).Value); });
                        }
                    }
                    //If User is supplier then againg bind the Sorted list.
                    gridview.DataSource = objSupplierORderList;
                    gridview.DataBind();

                }
            }

            try
            {
                foreach (GridViewRow t in gridview.Rows)
                {
                    DropDownList ddlStatus = (DropDownList)t.FindControl("ddlStatus");
                    LookupRepository objLookRep = new LookupRepository();
                    string strPayment = "StatusOptionOne";
                    ddlStatus.DataSource = objLookRep.GetByLookup(strPayment);
                    ddlStatus.DataValueField = "iLookupID";
                    ddlStatus.DataTextField = "sLookupName";
                    ddlStatus.DataBind();
                    ddlStatus.Items.Insert(0, new ListItem("-Select-", "0"));
                    HiddenField hdnOrderStatus = (HiddenField)t.FindControl("hdnOrderStatus");
                    if (!(string.IsNullOrEmpty(hdnOrderStatus.Value)))
                        ddlStatus.Items.FindByText(hdnOrderStatus.Value).Selected = true;
                    else
                        ddlStatus.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                ErrHandler.WriteError(ex);
            }
        }
    }
    //Created By Ankit on 4-Mar-11
    protected void gvOrderDetail_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //((HyperLink)e.Row.FindControl("hypOrderNumber")).NavigateUrl = "~/OrderManagement/OrderDetail.aspx?Id=" + ((HiddenField)e.Row.FindControl("hdnOrderNumber")).Value.ToString() + "&OrderStatusID=" + ((HiddenField)e.Row.FindControl("hdnOrderStatus")).Value + "&SubmitDate=" + ((Label)e.Row.FindControl("lblOrderDate")).Text;

            ((HyperLink)e.Row.FindControl("hypOrderNumber")).NavigateUrl = "~/OrderManagement/OrderDetail.aspx?id=" + ((HiddenField)e.Row.FindControl("hdnOrderNumber")).Value.ToString();
        }
    }

    protected void gvOrderDetailSupplier_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void rptOrderHeader_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string strStatus = "";
        string strOrderNo = "";
        if (e.CommandName == "Status")
        {
            GridView gvOrderDetail = (GridView)e.Item.FindControl("gvOrderDetail");
            foreach (GridViewRow t in gvOrderDetail.Rows)
            {
                CheckBox chkDelete = (CheckBox)t.FindControl("CheckBox1");
                HiddenField hdnOrderid = (HiddenField)t.FindControl("hdnOrderNumber");
                DropDownList ddlStatus = (DropDownList)t.FindControl("ddlStatus");
                if (chkDelete != null)
                {
                    if (chkDelete.Checked)
                    {
                        if (ddlStatus.SelectedItem.Text != "Deleted")
                            objOrdRepos.UpdateStatus(Convert.ToInt64(hdnOrderid.Value), ddlStatus.SelectedItem.Text);
                        else
                            objOrdRepos.UpdateStatus(Convert.ToInt64(hdnOrderid.Value), ddlStatus.SelectedItem.Text, IncentexGlobal.CurrentMember.UserInfoID, DateTime.Now);

                        strStatus += ddlStatus.SelectedItem.Text + ",";
                        strOrderNo += hdnOrderid.Value + ",";
                        chkDelete.Checked = false;
                    }
                }
            }
            //Send Email After Order Status Changed
            sendVerificationEmail(strStatus, strOrderNo);
        }
    }
    private void sendVerificationEmail(string strStatus, string OrderNumber)
    {
        try
        {

            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "OrderStatus";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                string sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                string sToadd = "knelson@Incentex.com";
                string sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString();
                string sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();

                //if (dsEmaiUser.Tables[0].Rows.Count > 0)
                //{
                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{OrderNo}", OrderNumber);
                messagebody.Replace("{OrderStatus}", strStatus);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);

                string smtphost = Application["SMTPHOST"].ToString();
                int smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                string smtpUserID = Application["SMTPUSERID"].ToString();
                string smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(3, "Order Status", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true);
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}
