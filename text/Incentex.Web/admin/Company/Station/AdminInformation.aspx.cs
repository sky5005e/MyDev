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

public partial class admin_Company_Station_AddAdminInformation : PageBase
{
    #region Properties

    Int64 CompanyId
    {
        get
        {
            if (ViewState["CompanyId"] == null)
            {
                ViewState["CompanyId"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyId"]);
        }
        set
        {
            ViewState["CompanyId"] = value;
        }
    }

    Int64 StationId
    {
        get
        {
            if (ViewState["StationId"] == null)
            {
                ViewState["StationId"] = 0;
            }
            return Convert.ToInt64(ViewState["StationId"]);
        }
        set
        {
            ViewState["StationId"] = value;
        }
    }

    Int64 CompanyStationUserInformationID
    {
        get
        {
            if (ViewState["CompanyStationUserInformationID"] == null)
            {
                ViewState["CompanyStationUserInformationID"] = 0;
            }
            return Convert.ToInt64(ViewState["CompanyStationUserInformationID"]);
        }
        set
        {
            ViewState["CompanyStationUserInformationID"] = value;
        }
    }

    string UserType = "admin";

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Stations";
            base.ParentMenuID = 11;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Admin Information";
           // ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to company listing</span>";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Company/ViewCompany.aspx";
           
            if (Request.QueryString.Count > 0)
            {
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.StationId = Convert.ToInt64(Request.QueryString.Get("SubId"));

                if (this.StationId == 0)
                {
                    Response.Redirect("~/admin/Company/Station/MainStationInfo.aspx?Id=" + this.CompanyId);
                }
                else
                {
                    if (!base.CanEdit)
                    {
                        base.RedirectToUnauthorised();
                    }
                }

                manuControl.PopulateMenu(3, 2, this.CompanyId, this.StationId,true);
            }
            else
            {
                Response.Redirect("~/admin/Company/Station/ViewStation.aspx");
            }

            DisplayData();

        }
    }
    void DisplayData()
    {
        CompanyStationUserInformation objUser = new CompanyStationUserInformationRepository().GetByStationId(this.StationId, UserType);

        if (objUser != null)
        {
            this.CompanyStationUserInformationID = objUser.CompanyStationUserInformationID;
            txtFirstName.Text = objUser.FirstName;
            txtLastName.Text = objUser.LastName;
            txtTitle.Text = objUser.Title;
            txtTel.Text = objUser.TelephoneNumber;
            txtMobile.Text = objUser.Mobile;
            txtEmail.Text = objUser.Email;
            txtEstimatedDate.Text = Common.GetDateString(objUser.HiredDate);
            txtSpouseName.Text = objUser.SpouseName;
            txtChildrensName.Text = objUser.ChildrenName;
            txtPreviousEmployer.Text = objUser.PreviousEmployer;
            txtBirthdate.Text = Common.GetDateString(objUser.BirthDate);
            hdPriPhoto.Value = objUser.UploadedImage;

        }

    }

    
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            //insert or update record
            CompanyStationUserInformationRepository objRepo = new CompanyStationUserInformationRepository();
            CompanyStationUserInformation objUser = new CompanyStationUserInformation();

            if (this.CompanyStationUserInformationID != 0)
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                // get user to update
                objUser = objRepo.GetById(this.CompanyStationUserInformationID);
            }
            else
            {
                if (!base.CanAdd)
                {
                    base.RedirectToUnauthorised();
                }
            }

            objUser.StationID = this.StationId;
            objUser.FirstName = txtFirstName.Text;
            objUser.LastName = txtLastName.Text;
            objUser.Title = txtTitle.Text;
            objUser.TelephoneNumber = txtTel.Text;
            objUser.Mobile = txtMobile.Text;
            objUser.Email = txtEmail.Text;
            objUser.HiredDate = Common.GetDate(txtEstimatedDate);
            objUser.SpouseName = txtSpouseName.Text;
            objUser.ChildrenName = txtChildrensName.Text;
            objUser.PreviousEmployer = txtPreviousEmployer.Text;
            objUser.BirthDate = Common.GetDate(txtBirthdate);
            objUser.StationUserType = UserType;
            objUser.UploadedImage = hdPriPhoto.Value;

            if (this.CompanyStationUserInformationID == 0)
            {
                //insert
                objRepo.Insert(objUser);
            }

            objRepo.SubmitChanges();

            Response.Redirect("Service.aspx?Id=" + this.CompanyId + "&SubId=" + this.StationId, false);

            lblMsg.Text = "Record Saved Successfully";
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


}
