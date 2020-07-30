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
using System.Net.Mail;
using System.Net;

public partial class ProductReturnManagement_ReturnProductIA : PageBase
{
    
    public DateTime? ToDate = null;
    public DateTime? FromDate = null;
    String StoreId = null;
    String Email = null;
    String OrderNumber = null;
    String OrdeStatus = null;
    ProductReturnRepository objRepos = new ProductReturnRepository();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            if (Request.QueryString["StoreId"] != "")
            {
                StoreId = Request.QueryString["StoreId"].ToString();
            }
            if (Request.QueryString["ToDate"] != "")
            {
                ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            }
            if (Request.QueryString["FromDate"] != "")
            {
                FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            }
            if (Request.QueryString["Email"] != "")
            {
                Email = Request.QueryString["Email"].ToString();
            }
            if (Request.QueryString["OrderNumber"] != "")
            {
                OrderNumber = Request.QueryString["OrderNumber"].ToString();
            }
            if (Request.QueryString["OrdeStatus"] != "")
            {
                OrdeStatus = Request.QueryString["OrdeStatus"].ToString();
            }
            ((System.Web.UI.WebControls.Label)Master.FindControl("lblPageHeading")).Text = "Product Return View";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/ProductReturnManagement/ProductReturnLookupSearch.aspx";
            //((HtmlImage)Master.FindControl("imgShoppingCart")).Visible = true;
            Session["ReturnMgtBack"] = Request.Url.AbsoluteUri;
            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin) || IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.SuperAdmin))
            {
                BindGridview();
              
            }

        }
    }
    protected void gvProductReturn_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlStatus = (DropDownList)e.Row.FindControl("ddlStatus");
            LookupRepository objLookRep = new LookupRepository();
            String strPayment = "ProductReturn";
            ddlStatus.DataSource = objLookRep.GetByLookup(strPayment);
            ddlStatus.DataValueField = "iLookupID";
            ddlStatus.DataTextField = "sLookupName";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new ListItem("-Select-", "0"));
            HiddenField hdnStatus = (HiddenField)e.Row.FindControl("hdnStatus");
            if (!(String.IsNullOrEmpty(hdnStatus.Value)))
            {
                ddlStatus.SelectedIndex = ddlStatus.Items.IndexOf(ddlStatus.Items.FindByText(hdnStatus.Value));
            }
            else
            {
                ddlStatus.SelectedIndex = 0;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            ((HyperLink)e.Row.FindControl("hypOrderNumber")).NavigateUrl = "~/ProductReturnManagement/ReturnProductIncenteEmployee.aspx?OrderId=" + ((HiddenField)e.Row.FindControl("hdnOrderID")).Value.ToString() + "&Status=" + ((HiddenField)e.Row.FindControl("hdnStatus")).Value.ToString();
        }

        
    }
    private void BindGridview()
    {
        try
        {
                List<SelectReturnProductOnCompWiseResult> objList = new List<SelectReturnProductOnCompWiseResult>();
                objList = objRepos.GetProductReturnProduct(ToDate,FromDate,StoreId,Email,OrderNumber,OrdeStatus);
                if (objList.Count > 0)
                {
                   
                    DataView myDataView = new DataView();
                    DataTable dataTable = ListToDataTable(objList);
                    myDataView = RemoveDuplicate(dataTable).DefaultView;
                    gvProductReturn.DataSource = myDataView;
                    gvProductReturn.DataBind();
                    btnSaveStatus.Visible = true;
                }
                else
                {
                    String myStringVariable = String.Empty;
                    myStringVariable = "No Record Found!";
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
                    btnSaveStatus.Visible = false;
                }
          
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    protected void btnSaveStatus_Click(object sender, EventArgs e)
    {
        
        foreach (GridViewRow t in gvProductReturn.Rows)
        {
            CheckBox chkDelete = (CheckBox)t.FindControl("CheckBox1");
            HiddenField hdnOrderid = (HiddenField)t.FindControl("hdnOrderID");
            HiddenField hdnOrderNumber = (HiddenField)t.FindControl("hdnOrderNumber");
            DropDownList ddlStatus = (DropDownList)t.FindControl("ddlStatus");
            if (chkDelete != null)
            {
                if (chkDelete.Checked)
                {
                    objRepos.UpdateStatus(Convert.ToInt64(hdnOrderid.Value), ddlStatus.SelectedItem.Text);
                    chkDelete.Checked = false;
                    //Send Email After Order Status Changed
                    sendVerificationEmail(ddlStatus.SelectedItem.Text, Convert.ToInt64(hdnOrderid.Value));
                }
            }
        }
        
        String myStringVariable = String.Empty;
        myStringVariable = "Status Have Changed";
        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + myStringVariable + "');", true);
        BindGridview();         
    }
    private void sendVerificationEmail(String strStatus, Int64 OrderID)
    {
        try
        {
            EmailTemplateBE objEmailBE = new EmailTemplateBE();
            EmailTemplateDA objEmailDA = new EmailTemplateDA();
            DataSet dsEmailTemplate;
            //Get Email Content
            objEmailBE.SOperation = "SELECTTEMPLATEBYNAME";
            objEmailBE.STemplateName = "ReturnStatus";
            dsEmailTemplate = objEmailDA.EmailTemplate(objEmailBE);
            if (dsEmailTemplate != null)
            {
                //Find UserName who had order purchased
                Order objOrder = new OrderConfirmationRepository().GetByOrderID(OrderID);
                UserInformation objUserInformation = new UserInformationRepository().GetById(objOrder.UserId);

                String sFrmadd = dsEmailTemplate.Tables[0].Rows[0]["sFromAddress"].ToString();
                String sToadd = objUserInformation.LoginEmail;
                String sSubject = dsEmailTemplate.Tables[0].Rows[0]["sSubject"].ToString().Replace("(Return #)",OrderNumber);
                String sFrmname = dsEmailTemplate.Tables[0].Rows[0]["sFromName"].ToString();


                StringBuilder messagebody = new StringBuilder(dsEmailTemplate.Tables[0].Rows[0]["sTemplateContent"].ToString()); // From Template table
                messagebody.Replace("{ReturnNo}", objOrder.OrderNumber);
                messagebody.Replace("{status}", strStatus);
                messagebody.Replace("{siteurl}", ConfigurationSettings.AppSettings["siteurl"]);
                messagebody.Replace("{fullname}", objUserInformation.FirstName + " " + objUserInformation.LastName);

                String smtphost = Application["SMTPHOST"].ToString();
                Int32 smtpport = Convert.ToInt32(Application["SMTPPORT"]);
                String smtpUserID = Application["SMTPUSERID"].ToString();
                String smtppassword = Application["SMTPPASSWORD"].ToString();

                new CommonMails().SendMail(objUserInformation.UserInfoID, "Return Status", sFrmadd, sToadd, sSubject, messagebody.ToString(), sFrmname, smtphost, smtpport, false, true, objOrder.OrderID);                
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
    public static DataTable ListToDataTable<T>(IEnumerable<T> list)
    {
        var dt = new DataTable();
        foreach (var info in typeof(T).GetProperties())
        {

            dt.Columns.Add(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType);
        }
        foreach (var t in list)
        {
            var row = dt.NewRow();
            foreach (var info in typeof(T).GetProperties())
            {
                row[info.Name] = info.GetValue(t, null) ?? DBNull.Value;
            }
            dt.Rows.Add(row);
        }
        return dt;
    }
    private DataTable RemoveDuplicate(DataTable dtInput)
    {

        DataTable dtTemp = dtInput.Clone();

        foreach (DataRow drInput in dtInput.Rows)
        {

            if (dtTemp.Rows.Count == 0)
            {

                DataRow dr = dtTemp.NewRow();

                dr["Contact"] = drInput["Contact"].ToString();

                dr["OrderId"] = drInput["OrderId"].ToString();

                dr["OrderNumber"] = drInput["OrderNumber"].ToString();

                dr["PaymentOption"] = drInput["PaymentOption"].ToString();

                dr["ProductReturnId"] = drInput["ProductReturnId"].ToString();
                if (drInput["ShipingDate"].ToString() != "")
                {
                    dr["ShipingDate"] = drInput["ShipingDate"].ToString();
                }
                dr["ShippID"] = drInput["ShippID"].ToString();
                dr["Status"] = drInput["Status"].ToString();
                dr["CompanyName"] = drInput["CompanyName"].ToString();
                dr["SubmitDate"] = drInput["SubmitDate"].ToString();
                dr["WorkgroupId"] = drInput["WorkgroupId"].ToString();
                dr["WorkgroupName"] = drInput["WorkgroupName"].ToString();
                dtTemp.Rows.Add(dr);

            }

            else
            {

                Boolean xIsExist = false;

                for (Int32 idxT = 0; idxT < dtTemp.Rows.Count; idxT++)
                {

                    if ( drInput["OrderId"].ToString().Equals(dtTemp.Rows[idxT]["OrderId"].ToString()))
                    {

                        xIsExist = true;

                    }

                }



                if (!xIsExist)
                {

                    DataRow dr = dtTemp.NewRow();

                    dr["Contact"] = drInput["Contact"].ToString();

                    dr["OrderId"] = drInput["OrderId"].ToString();

                    dr["OrderNumber"] = drInput["OrderNumber"].ToString();

                    dr["PaymentOption"] = drInput["PaymentOption"].ToString();

                    dr["ProductReturnId"] = drInput["ProductReturnId"].ToString();
                    if (drInput["ShipingDate"].ToString() != "")
                    {
                        dr["ShipingDate"] = drInput["ShipingDate"].ToString();
                    }

                    dr["ShippID"] = drInput["ShippID"].ToString();
                    dr["Status"] = drInput["Status"].ToString();
                    dr["CompanyName"] = drInput["CompanyName"].ToString();
                    dr["SubmitDate"] = drInput["SubmitDate"].ToString();
                    dr["WorkgroupId"] = drInput["WorkgroupId"].ToString();
                    dr["WorkgroupName"] = drInput["WorkgroupName"].ToString();

                    dtTemp.Rows.Add(dr);

                }

            }

        }



        return dtTemp;



    }

}
