/// <summary>
/// Module Name : Video training system
/// Description : this is the usercontrol created for display video on CA/CE based on url,storeid,workgroup,department,startdate and enddate.
///               If request is comming for general video the this popup is display only on home page when page load.
///               When user click for watch letter then add note to user note history.
/// Created : Mayur on 12-dec-2011
/// </summary>

using System;
using System.Configuration;
using System.Text;
using System.Web.UI;
using AjaxControlToolkit;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.IO;

public partial class usercontrol_TrainingVideo : System.Web.UI.UserControl
{
    #region Data Members
    public string VideoName
    {
        get
        {
            if (ViewState["VideoName"] == null)
            {
                ViewState["VideoName"] = "";
            }
            return ViewState["VideoName"].ToString();
        }
        set
        {
            ViewState["VideoName"] = value;
        }

    }
    public string VideoYouTubID
    {
        get
        {
            if (ViewState["VideoYouTubID"] == null)
            {
                ViewState["VideoYouTubID"] = "";
            }
            return ViewState["VideoYouTubID"].ToString();
        }
        set
        {
            ViewState["VideoYouTubID"] = value;
        }

    }
    public string VideoTitle
    {
        get
        {
            if (ViewState["VideoTitle"] == null)
            {
                ViewState["VideoTitle"] = "";
            }
            return ViewState["VideoTitle"].ToString();
        }
        set
        {
            ViewState["VideoTitle"] = value;
        }

    }
    public bool IsForGeneralVideo
    {
        get
        {
            if (ViewState["IsForGeneralVideo"] == null)
            {
                ViewState["IsForGeneralVideo"] = false;
            }
            return Convert.ToBoolean(ViewState["IsForGeneralVideo"]);
        }
        set
        {
            ViewState["IsForGeneralVideo"] = value;
        }

    }
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(this.VideoName))
        {
            EmbedObject();
            mpeATrainigVideo.X = Convert.ToInt32(Convert.ToDecimal(hfPopX.Value)) - 200;
            mpeATrainigVideo.Y = Convert.ToInt32(Convert.ToDecimal(hfPopY.Value)) - 250;
            mpeATrainigVideo.Show();
        }
        else if (!string.IsNullOrEmpty(this.VideoYouTubID))
        {
            EmbedYouTubVideoObject();
            mpeATrainigVideo.X = Convert.ToInt32(Convert.ToDecimal(hfPopX.Value)) - 200;
            mpeATrainigVideo.Y = Convert.ToInt32(Convert.ToDecimal(hfPopY.Value)) - 250;
            mpeATrainigVideo.Show();
        }
    }

    protected override void OnInit(EventArgs e)
    {
        Page.Init += delegate(object sender, EventArgs e_Init)
        {
            if (ToolkitScriptManager.GetCurrent(Page) == null && ScriptManager.GetCurrent(Page) == null)
            {
                ToolkitScriptManager sMgr = new ToolkitScriptManager();
                phScriptManager.Controls.AddAt(0, sMgr);
            }
        };
        base.OnInit(e);
    }

    protected void lnkbtnWatchLatter_Click(object sender, EventArgs e)
    {
        mpeATrainigVideo.Hide();
        AddUserNote("skip", this.VideoTitle);
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    #endregion

    #region Methods
    protected void EmbedObject()
    {
        StringBuilder strb = new StringBuilder();
        if (Path.GetExtension(this.VideoName).ToLower() == ".flv")
        {
            strb.Append("<a href='" + ConfigurationSettings.AppSettings["siteurl"] + "UploadedImages/TrainingVideo/" + this.VideoName + "' style=\"display: block; width: 800px; height: 400px; margin-top:25px;\" id=\"player\"></a>");
            strb.Append("<script>flowplayer('player', '../../JS/Flowplayer/flowplayer-3.2.15.swf');</script>");
        }
        else
        {
            strb.Append("<object id='objVideo' codetype='video/quicktime' classid='clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B' codebase='http://www.apple.com/qtactivex/qtplugin.cab' height='400' width='800'>");
            strb.Append("<param name='src' value='" + ConfigurationSettings.AppSettings["siteurl"] + "UploadedImages/TrainingVideo/" + this.VideoName + "'/>");
            strb.Append("<param name='autoplay' value='true'/>");
            strb.Append("<param name='wmode' value='transparent'/>");
            strb.Append("<param name='wmode' value='opaque'/>");
            strb.Append("<param name='showlogo' value='false'/>");
            strb.Append("<param name='targetcache' value='true'/>");
            strb.Append("<param name='scale' value='tofit'/>");
            strb.Append("<param name='controller' value='true'/>");
            strb.Append("<embed type='video/quicktime' src='" + ConfigurationSettings.AppSettings["siteurl"] + "UploadedImages/TrainingVideo/" + this.VideoName + "' pluginspage='http://www.apple.com/quicktime/' scale='tofit' targetcache='true' width='800' height='400' showlogo='false' autoplay='true' controller='true' wmode='transparent'>");
            strb.Append("</embed>");
            strb.Append("</object>");
        }
        dvVideoTag.InnerHtml = strb.ToString();
    }
    protected void EmbedYouTubVideoObject()
    {
        StringBuilder strb = new StringBuilder();
        strb.Append("<object width='800' height='400'>");
        strb.Append("<param name='movie' value='http://www.youtube.com/v/" + this.VideoYouTubID + "?fs=1&amp;hl=en_US&amp;rel=0'/>");
        strb.Append("<param name='allowFullScreen' value='true'/>");
        strb.Append("<param name='allowScriptAccess' value='always'/>");
        strb.Append("<param name='wmode' value='transparent'/>");
        strb.Append("<param name='showlogo' value='false'/>");
        strb.Append("<embed src='http://www.youtube.com/v/" + this.VideoYouTubID + "?fs=1&amp;hl=en_US&amp;rel=0' type='application/x-shockwave-flash' wmode='transparent' allowfullscreen='true' allowscriptaccess='always' width='800' height='400' showlogo='false'>");
        strb.Append("</embed>");
        strb.Append("</object>");
        dvVideoTag.InnerHtml = strb.ToString();
    }

    /// <summary>
    /// Adds the note for general video to user note section.
    /// </summary>
    public void AddUserNote(string action, string videoTitle)
    {
        NotesHistoryRepository objRepo = new NotesHistoryRepository();
        CompanyStoreDocumentRepository objRep = new CompanyStoreDocumentRepository();
        CompanyEmployee objCmpnyInfo = new CompanyEmployee();
        CompanyEmployeeRepository objCmpnyEmpRepo = new CompanyEmployeeRepository();

        objCmpnyInfo = objCmpnyEmpRepo.GetByUserInfoId(IncentexGlobal.CurrentMember.UserInfoID);
        string content = "";
        if (action == "skip")
            content = "General video skipped.";

        content += System.Environment.NewLine + "Video Title:" + videoTitle;
        NoteDetail obj = new NoteDetail()
        {
            ForeignKey = objCmpnyInfo.CompanyEmployeeID,
            Notecontents = content,
            NoteFor = Incentex.DAL.Common.DAEnums.GetNoteForTypeName(Incentex.DAL.Common.DAEnums.NoteForType.CompanyEmployee),
            CreatedBy = IncentexGlobal.CurrentMember.UserInfoID,
            CreateDate = DateTime.Now,
            UpdateDate = DateTime.Now,
            UpdatedBy = IncentexGlobal.CurrentMember.UserInfoID
        };

        objRepo.Insert(obj);
        objRepo.SubmitChanges();
    }
    #endregion
}
