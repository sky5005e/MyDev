using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;
using System.Text;
using Incentex.DAL;
using System.Diagnostics;
using System.IO;

public partial class admin_EmployeeTrainingCenter_AddEmployeeTrainingCenter : PageBase
{
    #region Data Member's
    EmployeeTrainingCenterRepository objEmployeeTrainingRepo = new EmployeeTrainingCenterRepository();
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Employee Training Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView && !base.CanAdd)
            {
                base.RedirectToUnauthorised();
            }

            BindDropDowns();

            ((Label)Master.FindControl("lblPageHeading")).Text = "Add Emlployee Training";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/EmployeeTrainingCenter/EmployeeTrainingCenter.aspx";
        }
    }

    protected void lnkBtnSaveInfo_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkUploadToWL.Checked == false && chkUploadToYouTube.Checked == false)
            {
                lblMsg.Text = "Please select one of the video upload option.";
                return;
            }

            if (chkUploadToWL.Checked == true) //This is for upload video to world-link
            {
                if (((float)Request.Files[0].ContentLength / 1048576) > 150)
                {
                    lblMsg.Text = "The file you are uploading is more than 150MB.";
                    return;
                }
            }

            EmployeeTrainingCenter objEmployeeTraining = new EmployeeTrainingCenter();
            String SavedFileName = string.Empty;
            decimal FileSize = 0;
            if (chkUploadToWL.Checked == true) //This is for upload video to world-link
            {
                #region Saving Attachments
                if (Request.Files.Count > 0)
                {
                    if (((float)Request.Files[0].ContentLength / 1048576) > 150)
                    {
                        lblMsg.Text = "The file you are uploading is more than 150MB.";
                        return;
                    }

                    HttpFileCollection Attachments = Request.Files;

                    HttpPostedFile Attachment = Attachments[0];
                    if (Attachment.ContentLength > 0 && !String.IsNullOrEmpty(Attachment.FileName))
                    {
                        SavedFileName = ddlEmployeeTrainingType.SelectedItem + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Attachment.FileName;
                        SavedFileName = Common.MakeValidFileName(SavedFileName).Trim().Replace(" ", "");
                        String filePath = Server.MapPath("../../UploadedImages/TrainingVideo/") + SavedFileName;
                        Attachment.SaveAs(filePath);
                        FileSize = Attachment.ContentLength;
                        this.ConvertTo_FLV(SavedFileName);
                    }
                }
                #endregion

                objEmployeeTraining.OriginalFileName = SavedFileName;
                objEmployeeTraining.FileSize = FileSize;
            }
            else
            {
                objEmployeeTraining.YouTubeVideoID = txtYouTubeVideoID.Text.Trim();
                objEmployeeTraining.OriginalFileName = null;
            }

            objEmployeeTraining.FileName = txtFileName.Text;
            objEmployeeTraining.Searchkeyword = txtSearchKeyword.Text;
            objEmployeeTraining.EmployeeTrainingType = Convert.ToInt64(ddlEmployeeTrainingType.SelectedValue);
            objEmployeeTraining.UplodedDate = System.DateTime.Now;
            objEmployeeTraining.UplodedBy = IncentexGlobal.CurrentMember.UserInfoID;

            objEmployeeTrainingRepo.Insert(objEmployeeTraining);
            objEmployeeTrainingRepo.SubmitChanges();

            ResetControls();
            Response.Redirect("EmployeeTrainingCenter.aspx?req=1");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void chkUploadToWL_CheckedChanged(object sender, EventArgs e)
    {
        if (chkUploadToWL.Checked == true)
        {
            chkUploadToWL.Checked = true;
            spnUploadToWL.Attributes.Add("class", "wheather_checked");
            trUploadToWL.Visible = true;

            chkUploadToYouTube.Checked = false;
            spnUploadToYouTube.Attributes.Add("class", "wheather_check");
            trUploadToYoutube.Visible = false;
        }
        else
        {
            chkUploadToWL.Checked = false;
            spnUploadToWL.Attributes.Add("class", "wheather_check");
            trUploadToWL.Visible = false;
        }
    }
    protected void chkUploadToYouTube_CheckedChanged(object sender, EventArgs e)
    {
        if (chkUploadToYouTube.Checked == true)
        {
            chkUploadToYouTube.Checked = true;
            spnUploadToYouTube.Attributes.Add("class", "wheather_checked");
            trUploadToYoutube.Visible = true;

            chkUploadToWL.Checked = false;
            spnUploadToWL.Attributes.Add("class", "wheather_check");
            trUploadToWL.Visible = false;
        }
        else
        {
            chkUploadToYouTube.Checked = false;
            trUploadToYoutube.Visible = false;
            spnUploadToYouTube.Attributes.Add("class", "wheather_check");
        }

    }

    public void ConvertTo_FLV(string FileName)
    {
        string SavePath = Server.MapPath("../../UploadedImages/TrainingVideo/");
        string NewFName = FileName;


        //Start Converting
        string inputfile, outputfile, filargs;
        string withoutext;

        //Get the file name without Extension
        withoutext = Path.GetFileNameWithoutExtension(NewFName);

        //Input file path of uploaded image
        inputfile = SavePath + NewFName;

        //output file format in swf
        outputfile = (SavePath + withoutext + ".flv").Trim().Replace(" ", "");

        //file orguments for FFMEPG
        //filargs = string.Format("-i {0} -ar 22050 -qscale 1 {1}", inputfile, outputfile);
        filargs = "-i " + inputfile + " -ab 64k -ac 2 -ar 44100 -f flv -deinterlace -nr 500 -croptop 4 -cropbottom 8 -cropleft 8 -cropright 8 -s 640x480 -aspect 4:3 -r 25 -b 650k -me_range 25 -i_qfactor 0.71 -g 500 " + outputfile;

        Process proc = new Process();
        try
        {
            proc.StartInfo.FileName = HttpContext.Current.Server.MapPath("~\\ffmpeg\\ffmpeg.exe");
            proc.StartInfo.Arguments = filargs;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;


            proc.Start();
            string StdOutVideo = proc.StandardOutput.ReadToEnd();
            string StdErrVideo = proc.StandardError.ReadToEnd();
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }
        finally
        {
            proc.WaitForExit();
            proc.Close();
        }
    }
    #endregion

    #region Methods
    /// <summary>
    /// To generate Random of 7 Digit.
    /// </summary>
    /// <returns></returns>
    private Int64 GenerateRandomNumber()
    {
        Random random = new Random();
        return random.Next(1000000, 9999999);
    }

    /// <summary>
    /// To Bind Drop Downs
    /// </summary>
    private void BindDropDowns()
    {
        LookupRepository objLookRep = new LookupRepository();
        ddlEmployeeTrainingType.DataSource = objLookRep.GetByLookup("EmployeeTrainingType");
        ddlEmployeeTrainingType.DataValueField = "iLookupID";
        ddlEmployeeTrainingType.DataTextField = "sLookupName";
        ddlEmployeeTrainingType.DataBind();
        ddlEmployeeTrainingType.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    private void ResetControls()
    {
        ddlEmployeeTrainingType.SelectedIndex = 0;
        txtFileName.Text = "";
        txtSearchKeyword.Text = "";
    }
    #endregion
}
