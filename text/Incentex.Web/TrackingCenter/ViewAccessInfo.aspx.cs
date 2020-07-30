using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using Artem.Google.UI;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class TrackingCenter_ViewAccessInfo : PageBase
{
    #region Page Properties

    Int32 CurrentPage
    {
        get
        {
            if (this.ViewState["CurrentPage"] == null)
                return 0;
            else
                return Convert.ToInt32(this.ViewState["CurrentPage"].ToString());
        }
        set
        {
            this.ViewState["CurrentPage"] = value;
        }
    }

    Int32 PagerSize
    {
        get
        {
            if (this.ViewState["PagerSize"] == null)
                return Convert.ToInt32(Application["GRIDPAGERSIZE"]);
            else
                return Convert.ToInt32(this.ViewState["PagerSize"].ToString());
        }
        set
        {
            this.ViewState["PagerSize"] = value;
        }
    }

    Int32 FrmPg
    {
        get
        {
            if (this.ViewState["FrmPg"] == null)
                return 1;
            else
                return Convert.ToInt32(this.ViewState["FrmPg"].ToString());
        }
        set
        {
            this.ViewState["FrmPg"] = value;
        }
    }

    Int32 ToPg
    {
        get
        {
            if (this.ViewState["ToPg"] == null)
                return PagerSize;
            else
                return Convert.ToInt32(this.ViewState["ToPg"].ToString());
        }
        set
        {
            this.ViewState["ToPg"] = value;
        }
    }

    Int32 CountGrid = 0;

    Int32 uid
    {
        get
        {
            if (this.ViewState["uid"] == null)
                return 0;
            else
                return Convert.ToInt32(this.ViewState["uid"].ToString());
        }
        set
        {
            this.ViewState["uid"] = value;
        }
    }

    String sd
    {
        get
        {
            if (this.ViewState["sd"] == null)
                return null;
            else
                return (this.ViewState["sd"].ToString());
        }
        set
        {
            this.ViewState["sd"] = value;
        }
    }

    String ed
    {
        get
        {
            if (this.ViewState["ed"] == null)
                return null;
            else
                return (this.ViewState["ed"].ToString());
        }
        set
        {
            this.ViewState["ed"] = value;
        }
    }

    UserTrackingRepo ObjUserTrackRepo = new UserTrackingRepo();
    PagedDataSource pds = new PagedDataSource();

    #endregion

    #region Page Events

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Tracking Center";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            this.uid = Convert.ToInt32(Request.QueryString["uid"]);
            this.sd = (Request.QueryString["sdate"]);
            this.ed = (Request.QueryString["edate"]);
            ((Label)Master.FindControl("lblPageHeading")).Text = "List of Users";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/TrackingCenter/UserSystemAccessRpt.aspx?uid=" + this.uid + "&sdate=" + sd + "&edate=" + ed;
          
            BindGird(Convert.ToInt32(this.uid), Convert.ToDateTime(this.sd), Convert.ToDateTime(this.ed));
            lblmsg.Visible = false;
        }
    }

    #endregion

    #region Control Events

    /// <summary>
    /// from the ip address it will take latitude and longitude and show on map.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void gvmenu_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "MapLocation")
        {
            String ipaddress = e.CommandArgument.ToString();
            if (!String.IsNullOrEmpty(ipaddress))
            {
                GeoDetailsByIP latlong = Common.GetLatLong(ipaddress);

                if (latlong.statusCode == "OK")
                {
                    GoogleMarker objMarker;
                    objMarker = new GoogleMarker(latlong.latitude, latlong.longitude);
                    GoogleMap1.Markers.Add(objMarker);

                    StringBuilder Info = new StringBuilder();
                    Info.Append("<b style='font-size:mthis.edium'>Address:</b>: ");
                    Info.AppendLine("<br/>");
                    Info.AppendLine("<br/>");

                    Info.Append("<b>Country</b>: " + latlong.countryName);
                    Info.AppendLine("<br/>");
                    Info.Append("<b>Region</b>: " + latlong.regionName);
                    Info.AppendLine("<br/>");
                    Info.Append("<b>City</b>: " + latlong.cityName);
                    Info.AppendLine("<br/>");
                    if (latlong.zipCode != "-")
                    {
                        Info.Append("<b>ZipCode</b>: " + latlong.zipCode);
                    }
                    Label lbl = new Label();
                    lbl.Text = Info.ToString();

                    //objMarker.InfoContent.Attributes.Add("style", "foreColor:#1155CC");
                    //objMarker.Shadow=
                    objMarker.InfoContent.ForeColor = System.Drawing.Color.FromArgb(17, 85, 204);
                    objMarker.InfoContent.Controls.Add(lbl);
                    objMarker.OpenInfoBehaviour = OpenInfoBehaviour.Click;
                    GoogleMap1.Center = new LatLng(latlong.latitude, latlong.longitude);
                }
            }
            modal.Show();

        }
        if (e.CommandName == "BindHistory")
        {
            GridViewRow row;
            row = (GridViewRow)((ImageButton)e.CommandSource).Parent.Parent;
            if (((ImageButton)gvmenu.Rows[row.RowIndex].FindControl("btnAddemp")).Visible == true)
            {
                BindGird(this.uid, Convert.ToDateTime(this.sd), Convert.ToDateTime(this.ed));

                ((ImageButton)gvmenu.Rows[row.RowIndex].FindControl("btnAddemp")).Visible = false;
                ((ImageButton)gvmenu.Rows[row.RowIndex].FindControl("btnminusemp")).Visible = true;
                GridView gvChPageHistory = ((GridView)gvmenu.Rows[row.RowIndex].FindControl("gvChPageHistory"));
                List<UserPageHistoryTracking> objUserPageHistoryTracking = new UserTrackingRepo().GetHistoryByUserTrackingID(Convert.ToInt32(e.CommandArgument));
                if (objUserPageHistoryTracking.Count == 0)
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "No Page History available";
                }
                else
                {
                    gvChPageHistory.DataSource = objUserPageHistoryTracking;
                    gvChPageHistory.DataBind();
                    gvChPageHistory.Visible = true;
                    lblmsg.Visible = false;
                }
            }
            else
            {
                BindGird(this.uid, Convert.ToDateTime(this.sd), Convert.ToDateTime(this.ed));
            }
        }
    }

    protected void gvmenu_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (((HiddenField)e.Row.FindControl("hdnBrowserName")).Value == "Firefox")
            {
                ((ImageButton)e.Row.FindControl("imgFirefox")).Visible = true;
                ((ImageButton)e.Row.FindControl("imgIE")).Visible = false;
                ((ImageButton)e.Row.FindControl("imgOpera")).Visible = false;
                ((ImageButton)e.Row.FindControl("imgSafari")).Visible = false;
            }
            else if (((HiddenField)e.Row.FindControl("hdnBrowserName")).Value == "IE")
            {
                ((ImageButton)e.Row.FindControl("imgFirefox")).Visible = false;
                ((ImageButton)e.Row.FindControl("imgIE")).Visible = true;
                ((ImageButton)e.Row.FindControl("imgOpera")).Visible = false;
                ((ImageButton)e.Row.FindControl("imgSafari")).Visible = false;
            }
            else if (((HiddenField)e.Row.FindControl("hdnBrowserName")).Value == "Opera")
            {
                ((ImageButton)e.Row.FindControl("imgFirefox")).Visible = false;
                ((ImageButton)e.Row.FindControl("imgIE")).Visible = false;
                ((ImageButton)e.Row.FindControl("imgOpera")).Visible = true;
                ((ImageButton)e.Row.FindControl("imgSafari")).Visible = false;
            }
            else if (((HiddenField)e.Row.FindControl("hdnBrowserName")).Value.Contains("Safari"))
            {
                ((ImageButton)e.Row.FindControl("imgFirefox")).Visible = false;
                ((ImageButton)e.Row.FindControl("imgIE")).Visible = false;
                ((ImageButton)e.Row.FindControl("imgOpera")).Visible = false;
                ((ImageButton)e.Row.FindControl("imgSafari")).Visible = true;
            }
            else 
            {
                ((ImageButton)e.Row.FindControl("imgFirefox")).Visible = false;
                ((ImageButton)e.Row.FindControl("imgIE")).Visible = false;
                ((ImageButton)e.Row.FindControl("imgOpera")).Visible = false;
                ((ImageButton)e.Row.FindControl("imgSafari")).Visible = false;
                ((Label)e.Row.FindControl("mapid")).Text = "---";
            }

            CountGrid--;
            ((Label)e.Row.FindControl("SrNo")).Text = CountGrid.ToString();

            // code for take ipaddress as input and return location            
            String ipaddress = ((HiddenField)e.Row.FindControl("hdnIpadd")).Value;

            if (!String.IsNullOrEmpty(ipaddress))
            {
                GeoDetailsByIP latlong = Common.GetLatLong(ipaddress);

                if (latlong.statusCode == "OK")
                {
                    GoogleMarker objMarker;
                    objMarker = new GoogleMarker(latlong.latitude, latlong.longitude);
                    GoogleMap1.Markers.Add(objMarker);

                    StringBuilder Info = new StringBuilder();
                    Info.Append(latlong.countryName);
                    Info.Append(", " + latlong.regionName);

                    ((Label)e.Row.FindControl("imgLocation")).Text = Info.ToString();
                }
            }
        }
    }

    protected void gvmenu_DataBound(object sender, EventArgs e)
    {
        if (gvmenu.Rows.Count == 0)
        {
            lstPaging.Visible = false;
            lnkbtnNext.Visible = false;
            lnkbtnPrevious.Visible = false;
        }
        else
        {
            lstPaging.Visible = true;
            lnkbtnNext.Visible = true;
            lnkbtnPrevious.Visible = true;
        }
    }    

    protected void lstPaging_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName.Equals("lnkbtnPaging"))
        {
            CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
            BindGird(Convert.ToInt32(this.uid), Convert.ToDateTime(this.sd), Convert.ToDateTime(this.ed));
        }
    }

    protected void lstPaging_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        LinkButton lnkbtnPage = (LinkButton)e.Item.FindControl("lnkbtnPaging");
        if (lnkbtnPage.Text == Convert.ToString(CurrentPage + 1))
        {
            lnkbtnPage.Enabled = false;
            lnkbtnPage.Font.Bold = true;
        }
    }

    protected void lnkbtnPrevious_Click(object sender, EventArgs e)
    {
        CurrentPage -= 1;
        gvmenu.PageIndex = CurrentPage;
        BindGird(Convert.ToInt32(this.uid), Convert.ToDateTime(this.sd), Convert.ToDateTime(this.ed));
    }

    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        CurrentPage += 1;
        gvmenu.PageIndex = CurrentPage;
        BindGird(Convert.ToInt32(this.uid), Convert.ToDateTime(this.sd), Convert.ToDateTime(this.ed));
    }    

    #endregion

    #region Page Methods

    private void BindGird(Int32 uid, DateTime sdate, DateTime edate)
    {
        UserTrackingRepo ObjRepo = new UserTrackingRepo();        

        List<UserTracking> objlist = ObjRepo.GetUserByUserinfoid(this.uid, Convert.ToDateTime(this.sd), Convert.ToDateTime(this.ed));
        
        pds.DataSource = objlist;
        pds.AllowPaging = true;
        pds.PageSize = Convert.ToInt32(Application["GRIDPAGING"]);
        pds.CurrentPageIndex = CurrentPage;
        lnkbtnNext.Enabled = !pds.IsLastPage;
        lnkbtnPrevious.Enabled = !pds.IsFirstPage;

        CountGrid = objlist.Count + 1;

        gvmenu.DataSource = pds;
        gvmenu.DataBind();

        doPaging();
    }

    private void doPaging()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");
            Int32 CurrentPg = pds.CurrentPageIndex + 1;

            if (CurrentPg > ToPg)
            {
                FrmPg = ToPg + 1;
                ToPg = ToPg + PagerSize;
            }

            if (CurrentPg < FrmPg)
            {
                ToPg = FrmPg - 1;
                FrmPg = FrmPg - PagerSize;
            }

            if (pds.PageCount < ToPg)
                ToPg = pds.PageCount;

            for (Int32 i = FrmPg - 1; i < ToPg; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i + 1;
                dt.Rows.Add(dr);
            }

            lstPaging.DataSource = dt;
            lstPaging.DataBind();
        }
        catch
        { }
    }

    /// <summary>
    /// when user click on the menu in tooltip it will display browser and version
    /// </summary>
    /// <param name="Lname"></param>
    /// <param name="Pass"></param>
    /// <returns></returns>
    public String GetTooltip(Object Bname, Object Version)
    {
        return "Browser name : " + Bname + "  ,  " + "Version :" + Version;
    }

    #endregion
}