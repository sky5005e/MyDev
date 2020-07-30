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

public partial class admin_CommunicationCenter_CreateCampaignStep3_1 : PageBase
{
    EmailTemplete ObjTbl = new EmailTemplete();
    CampignRepo ObjRepo = new CampignRepo();
    Campaign ObjCampTable = new Campaign();
    int active = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            ((Label)Master.FindControl("lblPageHeading")).Text = "Select or Upload Email Template";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CreateCampaignStep3.aspx";

            dvTemp.Attributes.Add("style", "display: none; visibility: collapse;");
            lblMsgGrid.Text = "";
            lblErrorMessage.Text = "";
        }
    }
    protected void lnkViewTemplates_Click(object sender, EventArgs e)
    {
        dvTemp.Attributes.Add("style", "display: block; visibility: visible;");
        lblMsgGrid.Text = "";
        bindGridView();
    }
    public void bindGridView()
    {
        gvTemplatesList.DataSource = ObjRepo.GetAllTemp();
        gvTemplatesList.DataBind();
    }
    protected void lnkUploadTemplates_Click(object sender, EventArgs e)
    {
        dvTemp.Attributes.Add("style", "display: none; visibility: collapse;");
        lblMsgGrid.Text = "";
        lblErrorMessage.Text = "";
        txtTempName.Text = "";
        modal.Controls.Clear();
        modal.Show();
    }

    // it is popup's submit button
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {

            EmailTemplete lst = ObjRepo.FindDuplicate(Convert.ToString(flFile.Value));
            if (lst != null)
            {
                lblMessage.Text = "Duplicate file is not allow";
                modal.Controls.Clear();
                modal.Show();

            }
            else
            {
                string sFilePath = null;

                sFilePath = Server.MapPath("../../UploadedImages/EmailTempletes/") + flFile.Value;
                Request.Files[0].SaveAs(sFilePath);
                ObjTbl.TempFile = flFile.Value;
                ObjTbl.TempName = txtTempName.Text;
                ObjTbl.CreatedDate = DateTime.Now;
                ObjRepo.Insert(ObjTbl);
                ObjRepo.SubmitChanges();
            }

        }

        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }

    }

    protected void lnkPrev_Click(object sender, EventArgs e)
    {
        Response.Redirect("CreateCampaignStep3.aspx?cid=" + Session["cid"].ToString());
    }

    protected void lnkNext_Click(object sender, EventArgs e)
    {
        if (Session["TempId"] != null)
        {
            Response.Redirect("CreateCampaignStep4.aspx");
        }
        else
        {
            dvTemp.Attributes.Add("style", "display: block; visibility: visible;");
            lblMsgGrid.Text = "";
            bindGridView();
            lblErrorMessage.Text = "Please Select atleast one Template";
        }

    }
    protected void chkTemp_CheckedChanged(object sender, EventArgs e)
    {       
        string str = ((RadioButton)(sender)).GroupName;

        foreach (GridViewRow gv in gvTemplatesList.Rows)
        {
            RadioButton RBtn = (RadioButton)gv.FindControl("chkTemp");
            HiddenField hdnTempvalue = (HiddenField)gv.FindControl("hdnTempvalue");
            string str1 = RBtn.GroupName;
            if (str == str1)
            {
                RBtn.Checked = true;
            }
            else
            {
                RBtn.Checked = false;
            }
            active = 1;
            if (RBtn.Checked == true)
            {
                string TempId = ((HiddenField)gv.FindControl("hdnTempvalue")).Value;
                Session["TempId"] = TempId;
                Session["TemplateID"] = ((HiddenField)gv.FindControl("hdnTempID")).Value;
            }
        }

    }

    protected void gvTemplatesList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header && this.ViewState["SortExp"] != null)
        {
            Image ImgSort = new Image();
            if (this.ViewState["SortOrder"].ToString() == "ASC")
                ImgSort.ImageUrl = "";
            else
                ImgSort.ImageUrl = "";
            switch (this.ViewState["SortExp"].ToString())
            {
                case "DateAdded":
                    PlaceHolder placeholderDateAdded = (PlaceHolder)e.Row.FindControl("placeholderDateAdded");
                    break;

                case "TempName":
                    PlaceHolder placeholderTempName = (PlaceHolder)e.Row.FindControl("placeholderTempName");
                    break;

                case "ViewTemp":
                    PlaceHolder placeholderViewTemp = (PlaceHolder)e.Row.FindControl("placeholderViewTemp");
                    break;
            }

        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Now Want to open Popup window from hyper link
            Int32 tid = Convert.ToInt32(((HiddenField)e.Row.FindControl("hdnTempID")).Value);
            String urlWithParams = "ViewTemplates.aspx?tid=" + tid;
            LinkButton hypViewTemp = (LinkButton)e.Row.FindControl("hypViewTemp");
            hypViewTemp.Attributes.Add("OnClick", "window.open('" + urlWithParams + "','PopupWindow','width=650,height=650, scrollbars=yes')");

            RadioButton rbchk = (RadioButton)e.Row.FindControl("chkTemp");
            if (active == 1)
            {
                rbchk.Checked = true;
                string TempId = ((HiddenField)e.Row.FindControl("hdnTempvalue")).Value;
                Session["TempName"] = TempId;
            }
            else
            {
                rbchk.Checked = false;
            }
        }
    }
    protected void gvTemplatesList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "del")
        {
            ObjRepo.DeleteTempItem(Convert.ToInt32(e.CommandArgument.ToString()));
            ObjRepo.SubmitChanges();
            dvTemp.Attributes.Add("style", "display: block; visibility: visible;");
            lblMsgGrid.Text = "Record deleted successfully";
            bindGridView();
        }
    }
}
