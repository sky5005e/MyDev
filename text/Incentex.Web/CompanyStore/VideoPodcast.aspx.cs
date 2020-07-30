using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class CompanyStore_VideoPodcast : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Redirect user to login page if session gets expires
            CheckLogin();

            //Assign Page Header and return URL 
            ((Label)Master.FindControl("lblPageHeading")).Text = "Video Podcast";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            if (IncentexGlobal.IsIEFromStore)
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            }
            else
            {
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";
            }

            bindVideoMenu();
        }
    }
    protected void dtVideoList_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "Play")
        {
            //get detail and play video based on commandargument
            HiddenField hdnVideoTitle;
            DataListItem row;
            row = (DataListItem)((LinkButton)e.CommandSource).Parent;
            hdnVideoTitle = ((HiddenField)dtVideoList.Items[row.ItemIndex].FindControl("hdnVideoTitle"));
            usercontrol_TrainingVideo ur = LoadControl("~/usercontrol/TrainingVideo.ascx") as usercontrol_TrainingVideo;
            ur.VideoName = e.CommandArgument.ToString();
            ur.VideoTitle = hdnVideoTitle.Value;
            ((Panel)Master.FindControl("pnlLoadVideoUserControl")).Controls.Add(ur);
        }
    }

    /// <summary>
    /// Binds the video menu.
    /// Get all general video list by current member
    /// </summary>
    public void bindVideoMenu()
    {
       List<VideoTraining> objVideoTraining = new List<VideoTraining>();
       objVideoTraining = new VideoTrainingRepository().GetAllVideoByStoreIDWorkgroupIDUserTypeAndUrl(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId), IncentexGlobal.CurrentMember.UserInfoID, IncentexGlobal.CurrentMember.Usertype, ConfigurationSettings.AppSettings["siteurl"] + "Index.aspx");

       dtVideoList.DataSource = objVideoTraining;
       dtVideoList.DataBind();

    }
}
