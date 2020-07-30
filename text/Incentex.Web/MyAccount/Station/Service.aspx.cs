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


public partial class admin_Company_Station_AddService : PageBase
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

 
    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();

        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Admin Information";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to company listing</span>";
            //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            

           
            if (Request.QueryString.Count > 0)
            {
                this.CompanyId = Convert.ToInt64(Request.QueryString.Get("Id"));
                this.StationId = Convert.ToInt64(Request.QueryString.Get("SubId"));

                if (this.StationId == 0)
                {
                    Response.Redirect("~/MyAccount/Station/MainStationInfo.aspx?Id=" + this.CompanyId);
                }


                IncentexGlobal.ManageID = 8;
                manuControl.PopulateMenu(0, 3, this.CompanyId, this.StationId, true);
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/MyAccount/Station/ViewStation.aspx?id=" + this.CompanyId;
        
            }
            else
            {
                Response.Redirect("~/MyAccount/Station/ViewStation.aspx");
            }
            
            lst.DataBind();
            DisplayData();
            
        }
    }

    void DisplayData()
    {
        CompanyStationsServiceMapRepository objRepo = new CompanyStationsServiceMapRepository();

        List<CompanyStationsServiceMap> objList = objRepo.GetByStationId(this.StationId);

        foreach (DataListItem li in lst.Items)
        {
            Label lblId = li.FindControl("lblId") as Label;
            TextBox txtVal = li.FindControl("txtVal") as TextBox;
            CheckBox chk = li.FindControl("chk") as CheckBox;
            HtmlGenericControl divChk = li.FindControl("divChk") as HtmlGenericControl;


            foreach(CompanyStationsServiceMap obj in objList)
            {
                if(obj.LookupID == Convert.ToInt64(lblId.Text))
                {
                    chk.Checked = true;
                    //chk.CssClass = "station_checkbox_checked";
                    
                    txtVal.Text = obj.MapValue.ToString();
                    divChk.Attributes.Add("class", "alignright station_checkbox_checked");
                }
            }
        }
    }



    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            CompanyStationsServiceMapRepository objRepo = new CompanyStationsServiceMapRepository();
            //delete from map table

            List<CompanyStationsServiceMap> objList = objRepo.GetByStationId(this.StationId);
            foreach(CompanyStationsServiceMap s in objList)
            {
                objRepo.Delete(s);
            }
            objRepo.SubmitChanges();
                
            // insert in map table

            foreach(DataListItem li in lst.Items)
            {
                Label lblId = li.FindControl("lblId") as Label;
                TextBox txtVal = li.FindControl("txtVal") as TextBox;
                CheckBox chk = li.FindControl("chk") as CheckBox;

                if(chk.Checked)
                {
                    CompanyStationsServiceMap obj = new CompanyStationsServiceMap()
                    {
                        LookupID = Convert.ToInt64(lblId.Text),
                        MapValue = txtVal.Text,
                        StationID = this.StationId
                    };

                    objRepo.Insert(obj);
                }

            }
            objRepo.SubmitChanges();
            
            Response.Redirect("AdditionalInfo.aspx?Id=" + this.CompanyId + "&SubId=" + this.StationId, false);

            lblMsg.Text = "Record Saved Successfully";
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

      


    protected void lst_DataBinding(object sender, EventArgs e)
    {
        LookupRepository objRepo = new LookupRepository();
        List<INC_Lookup> objList = objRepo.GetByLookupCode(DAEnums.LookupCodeType.StationService);
        lst.DataSource = objList;
    }
}

