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
using System.Web.UI.WebControls.WebParts;
using Incentex.DAL;
using Incentex.DA;
using Incentex.DAL.SqlRepository;
using Incentex.DAL.Common;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

public partial class admin_GetDBValues : PageBase
{

    DataSet ds;
    StateDA objState = new StateDA();
    CityDA objCity = new CityDA();
    CountryDA objCountry = new CountryDA();
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckLogin();
        if (!IsPostBack)
        {
            ((Label)Master.FindControl("lblPageHeading")).Text = "Bind Database Values";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/GlobalSetting.aspx";
            BindGrid();
        }
    }
    private void BindGrid()
    {
        grvUserType.DataSource = new UserTypeRepository().GetUserTypes();
        grvUserType.DataBind();

        grvCompany.DataSource = new CompanyRepository().GetAllCompany();
        grvCompany.DataBind();

        grvWrokgroup.DataSource = new LookupRepository().GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Workgroup);
        grvWrokgroup.DataBind();

        grvGender.DataSource = new LookupRepository().GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Gender);
        grvGender.DataBind();

        grvDept.DataSource = new LookupRepository().GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Department);
        grvDept.DataBind();

        grvStatus.DataSource = new LookupRepository().GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Status);
        grvStatus.DataBind();

        grvRegion.DataSource = new LookupRepository().GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.Region);
        grvRegion.DataBind();


        grvBaseStation.DataSource = new CompanyRepository().GetAllBaseStation();
        grvBaseStation.DataBind();

        FillCountry();
    }
    public void FillCountry()
    {
        try
        {

           
            ds = objCountry.GetAllCountry();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCountry.DataSource = ds;
                ddlCountry.DataTextField = "NameAndId";
                ddlCountry.DataValueField = "iCountryID";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("-select country-", "0"));
                //ddlCountry.SelectedValue = ddlCountry.Items.FindByText("United States").Value;

                

                ddlState.Enabled = true;
                ds = objState.GetStateAndIdByCountryID(Convert.ToInt32(ddlCountry.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlState.DataSource = ds;
                        ddlState.DataValueField = "iStateID";
                        ddlState.DataTextField = "NameAndId";
                        ddlState.DataBind();
                        ddlState.Items.Insert(0, new ListItem("-select state-", "0"));

                        //ddlCity.Items.Clear();
                        //ddlCity.Items.Add(new ListItem("-select city-", "0"));
                    }
                }

            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        finally
        {
            ds.Dispose();
           
        }
    }

    protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCountry.SelectedIndex <= 0)
            {
                ddlState.Enabled = false;
                ddlState.Items.Clear();
                ddlState.Items.Add(new ListItem("-select state-", "0"));

               // ddlCity.Enabled = false;
              //  ddlCity.Items.Clear();
               // ddlCity.Items.Add(new ListItem("-select city-", "0"));
            }
            else
            {
                ddlState.Enabled = true;
                ds = objState.GetStateAndIdByCountryID(Convert.ToInt32(ddlCountry.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlState.DataSource = ds;
                        ddlState.DataValueField = "iStateID";
                        ddlState.DataTextField = "NameAndId";
                        ddlState.DataBind();
                        ddlState.Items.Insert(0, new ListItem("-select state-", "0"));

                        //ddlCity.Items.Clear();
                        //ddlCity.Items.Add(new ListItem("-select city-", "0"));
                    }
                }
                

            }

        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        finally
        {
            //ds.Dispose();
            //objState = null;
        }
    }
    protected void ddlState_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlState.SelectedIndex <= 0)
            {
                ddlCity.Enabled = false;
                ddlCity.Items.Clear();
                ddlCity.Items.Add(new ListItem("-select city-", "0"));
            }
            else
            {
                ddlCity.Enabled = true;
                ds = objCity.GetCityAndIdByStateID(Convert.ToInt32(ddlState.SelectedItem.Value));
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ddlCity.DataSource = ds;
                        ddlCity.DataValueField = "iCityID";
                        ddlCity.DataTextField = "NameAndId";
                        ddlCity.DataBind();
                        ddlCity.Items.Insert(0, new ListItem("-select city-", "0"));
                    }
                }
            }
        }
        catch (System.Threading.ThreadAbortException lException)
        {
            //do nothing
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        finally
        {
            //ds.Dispose();
            //objCity = null;
        }
    }
}
