using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_TrackingCenter : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Tracking Center";

            if (IncentexGlobal.IsIEFromStore)
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = IncentexGlobal.IndexPageLink;
            else
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/index.aspx";

            if (Session["CoupaID"] == null)
                BindTrackingCenterOptions();
        }
    }

    public void BindTrackingCenterOptions()
    {
        try
        {
            List<INC_Lookup> objlist = new List<INC_Lookup>();
            LookupRepository objLookRep = new LookupRepository();
            objlist = objLookRep.GetByLookup("TrackingCenter");

            if (objlist.Count > 0)
            {
                dtIndex.DataSource = objlist;
                dtIndex.DataBind();
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }

    protected void dtIndex_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "Redirect")
        {
            if (e.CommandArgument.ToString() == "Track Order")
            {
                Response.Redirect("MyTrackingCenter.aspx");
            }
            else if (e.CommandArgument.ToString() == "Track Support Ticket")
            {
                if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.CompanyEmployee))
                    Response.Redirect("TrackServiceTicket.aspx");
                else
                    Response.Redirect("SearchTicketCA.aspx");
            }
            else if (e.CommandArgument.ToString() == "Track Headset Repair")
            {
                Response.Redirect("~/HeadsetRepairCenter/SearchHeadsetRepairCenter.aspx");
            }
        }
    }
}