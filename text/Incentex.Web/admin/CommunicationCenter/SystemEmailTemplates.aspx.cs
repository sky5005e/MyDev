using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.IO;
using System.Text;

public partial class admin_CommunicationCenter_SystemEmailTemplates : PageBase
{
    #region Data Members
    TodayEmailsRepository objEmailRepo = new TodayEmailsRepository();
    INC_EmailTemplate objEmail = new INC_EmailTemplate();
    /// <summary>
    /// Set FilePath 
    /// </summary>
    private String FilePath
    {
        get
        {
            if (ViewState["FilePath"] == null)
            {
                ViewState["FilePath"] = null;
            }
            return ViewState["FilePath"].ToString();

        }
        set
        {
            ViewState["FilePath"] = value;
        }
    }

    /// <summary>
    /// Set File Temp ID
    /// </summary>
    private Int64 TemplateID
    {
        get
        {
            if (ViewState["TemplateID"] == null)
            {
                ViewState["TemplateID"] = 0;
            }
            return Convert.ToInt64(ViewState["TemplateID"]);
        }
        set
        {
            ViewState["TemplateID"] = value;
        }
    }
    #endregion

    #region Event Handlers

    protected void Page_Load(object sender, EventArgs e)
    {
        base.MenuItem = "System Email Templates";
        base.ParentMenuID = 29;

        if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
            base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
        else
            base.SetAccessRights(true, true, true, true);

        if (!base.CanView)
        {
            base.RedirectToUnauthorised();
        }

        ((Label)Master.FindControl("lblPageHeading")).Text = "System Email Templates";
        ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
        ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/CommunicationCenter/CampaignSelection.aspx";

        if (!IsPostBack)
        {
            bindGridView();
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
            Int64 tempID = Convert.ToInt32(((Label)e.Row.FindControl("lblID")).Text);
            String TemplatesName = ((Label)e.Row.FindControl("lblTempName")).Text;
            String urlWithParams = String.Empty;
            if (TemplatesName.EndsWith(".htm"))
            {
                urlWithParams = "../../emailtemplate/" + TemplatesName;
            }
            else
            {
                urlWithParams = "ViewTemplates.aspx?templateID=" + tempID;
            }
            //Now Want to open Popup window from hyper link
            LinkButton hypViewTemp = (LinkButton)e.Row.FindControl("hypViewTemp");
            hypViewTemp.Attributes.Add("OnClick", "window.open('" + urlWithParams + "','PopupWindow','width=650,height=650, scrollbars=yes')");
        }
    }

    protected void gvTemplatesList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Modify")
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }

            FilePath = Server.MapPath("~/emailtemplate/" + e.CommandArgument.ToString());
            String body = String.Empty;
            if (e.CommandArgument.ToString().EndsWith(".htm"))
            {
                body = File.ReadAllText(FilePath);
            }
            else
            {
                INC_EmailTemplate objTemp = objEmailRepo.GetEmailTemplatesByTempName(e.CommandArgument.ToString());
                body = objTemp.sTemplateContent;
                TemplateID = objTemp.iTemplateID;
            }
            TxtEmailText.Text = body.ToString();
            modalPopup.Show();
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (FilePath.EndsWith(".htm"))
            File.WriteAllText(FilePath, TxtEmailText.Text);
        else
        {
            if (TemplateID != 0)
                objEmail = objEmailRepo.GetTemplatesDetailsByID(TemplateID);
            if (objEmail != null)
            {
                objEmail.sTemplateContent = TxtEmailText.Text;
                objEmailRepo.SubmitChanges();
            }
        }
    }
    #endregion

    #region  Method's

    private void bindGridView()
    {
        gvTemplatesList.DataSource = GetAllEmailTemplateList();
        gvTemplatesList.DataBind();
    }

    private Dictionary<Int64, String> GetEmailTemplateListfromFolder(string folderLoc)
    {
        Dictionary<Int64, String> dictioanytable = new Dictionary<Int64, String>();
        System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Server.MapPath(folderLoc));
        int i = 0;// this is only for key.
        foreach (System.IO.FileInfo file in directory.GetFiles())
        {
            string fileName = file.Name;
            dictioanytable.Add(i++, fileName);
        }

        return dictioanytable;
    }
    private ArrayList GetAllEmailTemplateList()
    {
        Dictionary<Int64, String> dictioanytable = new Dictionary<Int64, String>();
        dictioanytable = objEmailRepo.GetAllTemplatesFromDB();
        ArrayList arrList = new ArrayList();
        arrList.AddRange(dictioanytable);
        arrList.AddRange(GetEmailTemplateListfromFolder("~/emailtemplate"));
        return arrList;
    }
    #endregion
}
