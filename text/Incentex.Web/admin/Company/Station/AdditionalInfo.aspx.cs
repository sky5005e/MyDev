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

public partial class admin_Company_Station_AddAdditionalInfo : PageBase
{
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

    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
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

            ((Label)Master.FindControl("lblPageHeading")).Text = "Additional Information";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to company listing</span>";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Admin/Company/ViewCompany.aspx";
           
            if (Request.QueryString.Count > 0)
            {
                try
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

                }
                catch (Exception ex)
                {
                    Response.Redirect("~/admin/Company/Station/ViewStation.aspx?Id=" + this.CompanyId);
                }

                if (this.StationId == 0)
                {
                    Response.Redirect("~/admin/Company/Station/MainStationInfo.aspx?Id=" + this.CompanyId, true);
                }

                manuControl.PopulateMenu(3, 4, this.CompanyId, this.StationId,true);
                gv.DataBind();
                DisplayData();
            }
            else
            {
                Response.Redirect("~/admin/Company/Station/ViewStation.aspx");
            }

        }
    }

    void DisplayData()
    {
        CompanyStationRepository objRepo = new CompanyStationRepository();

        CompanyStation objCompanyStation = objRepo.GetById(this.StationId);
        if (string.IsNullOrEmpty(objCompanyStation.SeasonalWeather))
        {
            return;
        }

        string[] Ids =  objCompanyStation.SeasonalWeather.Split(',');
        foreach (GridViewRow gr in gv.Rows)
        {
            CheckBox chk = gr.FindControl("chk") as CheckBox;
            Label lblId = gr.FindControl("lblId") as Label;
            HtmlGenericControl dvChk = gr.FindControl("dvChk") as HtmlGenericControl;

            foreach(string i in Ids)
            {
                if(i.Equals(lblId.Text))
                {
                    chk.Checked = true;
                    dvChk.Attributes.Add("class", "wheather_checked");
                    break;
                }
            }
        }

    }

    protected void gv_DataBinding(object sender, EventArgs e)
    {
        LookupRepository objRepo = new LookupRepository();
        List<INC_Lookup> objList = objRepo.GetByLookupCode(DAEnums.LookupCodeType.StationAdditionalInfo);

        gv.DataSource = objList;
    }

    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.StationId == 0)
            {
                if (!base.CanAdd)
                {
                    base.RedirectToUnauthorised();
                }
            }
            else
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }
            }

            CompanyStationRepository objRepo = new CompanyStationRepository();

            CompanyStation objCompanyStation = objRepo.GetById(this.StationId);

            string LookUpIdList = "";
            bool IsAnyChacked = false;

            foreach(GridViewRow gr in gv.Rows)
            {
                CheckBox chk = gr.FindControl("chk") as CheckBox;
                Label lblId = gr.FindControl("lblId") as Label;

                if(chk.Checked)
                {
                    LookUpIdList += lblId.Text + ",";
                    IsAnyChacked = true;
                }
            }

            if (!IsAnyChacked)
            {
                lblMsg.Text = "Please select any one weather condition ... ";
                return;
            }

            objCompanyStation.SeasonalWeather = LookUpIdList;
            objRepo.SubmitChanges();
            
            lblMsg.Text = "Record Saved Successfully.";
            Response.Redirect("ViewStation.aspx?Id=" + this.CompanyId,false);
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch(Exception ex)
        {
            lblMsg.Text = "Error in saving record ...";
            ErrHandler.WriteError(ex);
        }
    }
}