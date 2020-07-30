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
using Incentex.DAL.Common;
using System.Collections.Generic;

public partial class admin_ManagedShipAddress : PageBase
{
    ManagedShipAddressRepository objRepo = new ManagedShipAddressRepository();
    MangedShipAddress obj = new MangedShipAddress();
    Int64 ManagedShipId
    {
        get
        {
            if (ViewState["ManagedShipId"] == null)
            {
                ViewState["ManagedShipId"] = 0;
            }
            return Convert.ToInt64(ViewState["ManagedShipId"]);
        }
        set
        {
            ViewState["ManagedShipId"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {   
        if (!IsPostBack)
        {
            base.MenuItem = "Manage Ship To";
            base.ParentMenuID = 10;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            ((Label)Master.FindControl("lblPageHeading")).Text = "Manage Ship To";

            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/UserManagement.aspx";

            Common.BindCountry(ddlCountry);
            ddlCountry.SelectedValue = ddlCountry.Items.FindByText("United States").Value;

            ddlCountry_SelectedIndexChanged(sender, e);
            DisplayData();

        }
    }
    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindState(ddlState, Convert.ToInt64(ddlCountry.SelectedValue));
        ddlState_SelectedIndexChanged(sender, e);

    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        Common.BindCity(ddlCity, Convert.ToInt64(ddlState.SelectedValue));
    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!base.CanEdit)
            {
                base.RedirectToUnauthorised();
            }
            
            if (this.ManagedShipId != 0)
            {
                obj = objRepo.GetById(this.ManagedShipId);
            }
            obj.CompanyName = txtCompanyName.Text;
            obj.FirstName = txtFirstName.Text;
            obj.LastName = txtLastName.Text;
            obj.Title = txtTitle.Text;
            obj.Address = txtAddress.Text;
            obj.CountryId = Convert.ToInt64(ddlCountry.SelectedValue);
            obj.StateId = Convert.ToInt64(ddlState.SelectedValue);
            obj.CityId = Convert.ToInt64(ddlCity.SelectedValue);
            obj.Zipcode =  txtZip.Text;
            obj.Telephone = txtTelephone.Text;
            obj.Extension = txtExtension.Text;
            if (this.ManagedShipId == 0)
            {
                objRepo.Insert(obj);
            }
            objRepo.SubmitChanges();

            this.ManagedShipId = obj.ManagedShipId;
            lblMsg.Text = "Record Seved Successfully !....";
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
    void DisplayData()
    {

        obj = objRepo.GetAllRecord();
        if (obj != null)
        {
            ManagedShipId = obj.ManagedShipId;
            txtCompanyName.Text = obj.CompanyName;
            txtFirstName.Text = obj.FirstName;
            txtLastName.Text = obj.LastName;
            txtTitle.Text = obj.Title;
            txtAddress.Text = obj.Address;
            ddlCountry.SelectedValue = obj.CountryId.ToString();
            Common.BindState(ddlState, Convert.ToInt64(obj.CountryId));
            ddlState.SelectedValue = obj.StateId.ToString();
            Common.BindCity(ddlCity, Convert.ToInt64(obj.StateId));
            ddlCity.SelectedValue = obj.CityId.ToString(); ;
            txtZip.Text = obj.Zipcode;
            txtTelephone.Text = obj.Telephone;
            txtExtension.Text = obj.Extension;
        }

    }
}