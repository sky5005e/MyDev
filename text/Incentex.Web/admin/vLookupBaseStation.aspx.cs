using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class admin_vLookupBaseStation : PageBase
{
    #region Data Members
    BaseStationDA objLookup = new BaseStationDA();
    Common objcommon = new Common();
    CountryDA objCountry = new CountryDA();
    BasedStationBE objBe = new BasedStationBE();
    DataSet dsLookup;
    DataSet ds;
    string strMessgae = null;
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Drop-Down Management";
            base.ParentMenuID = 0;

            if (IncentexGlobal.CurrentMember.Usertype == Convert.ToInt64(Incentex.DAL.Common.DAEnums.UserTypes.IncentexAdmin))
                base.CheckAccessRights(IncentexGlobal.CurrentMember.UserInfoID, base.MenuItem, base.ParentMenuID);
            else
                base.SetAccessRights(true, true, true, true);

            if (!base.CanView)
            {
                base.RedirectToUnauthorised();
            }

            FillSCountry();
            if (Convert.ToString(Session["iLookupCode"]) == null)
            {
                Response.Redirect("dropdownmenus.aspx");
            }
            else
            {
                if (Convert.ToString(Request.QueryString["strID"]) != null && Convert.ToString(Request.QueryString["strValue"]) != null)
                {
                    string strText = Request.QueryString["strValue"].ToString();
                    string strHeaderText = Common.Decryption(strText.Replace(" ", "+"));
                    ((Label)Master.FindControl("lblPageHeading")).Text = strHeaderText;
                    string strEncrept = Request.QueryString["strID"].ToString();

                    BindDatlist();
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/dropdownmenus.aspx";
                }
                else
                {
                    Response.Redirect("dropdownmenus.aspx");
                }
            }
        }
    }

    #region DataList Events
    protected void dtLstLookup_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            string path = "../admin/Incentex_Used_Icons/" + ((HiddenField)e.Item.FindControl("hdnLookupIcon")).Value;
            ((HtmlImage)e.Item.FindControl("imgLookupIcon")).Src = path;
        }
    }
    protected void dtLstLookup_ItemCommand(object source, DataListCommandEventArgs e)
    {

        if (e.CommandName == "deletevalue")
        {
            try
            {
                if (!base.CanDelete)
                {
                    base.RedirectToUnauthorised();
                }

                objBe = new BasedStationBE();
                objBe.iBaseStationId = Convert.ToInt64(e.CommandArgument.ToString());
                objBe.SOperation = "DeleteLookup";
                dsLookup = objLookup.LookUpBaseStation(objBe);
                if (dsLookup.Tables[0].Rows.Count > 0)
                {

                    if (dsLookup.Tables[0].Rows[0]["errormessage"].ToString() == "547")
                    {
                        lblErrorMessage.Visible = true;
                        lblErrorMessage.Text = "Unable to delete record as this record exists in other detail table";
                        return;
                    }
                    else
                    {
                        lblErrorMessage.Visible = false;
                        objcommon.DeleteImageFromFolder(((HiddenField)e.Item.FindControl("hdnLookupIcon")).Value, IncentexGlobal.dropdownIconPath);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {


            }
        }
        else if (e.CommandName == "editvalue")
        {
            try
            {
                if (!base.CanEdit)
                {
                    base.RedirectToUnauthorised();
                }

                objBe = new BasedStationBE();
                objBe.iBaseStationId = Convert.ToInt64(e.CommandArgument.ToString());
                objBe.SOperation = "SelectbyLookupid";
                DataSet ds = objLookup.LookUpBaseStation(objBe);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        imgEdit.Visible = true;
                        btnSubmit.Text = "Edit";
                        Session["id"] = e.CommandArgument.ToString();
                        Session["iconvalue"] = ds.Tables[0].Rows[0]["sBaseStationIcon"].ToString();
                        txtPriorityName.Text = ds.Tables[0].Rows[0]["sBaseStation"].ToString();
                        imgEdit.Src = "../admin/Incentex_Used_Icons/" + Session["iconvalue"];
                        ddlCountry.SelectedValue = ds.Tables[0].Rows[0]["iCountryID"].ToString();
                        txtLongitude.Text = ds.Tables[0].Rows[0]["fLongitude"].ToString();
                        txtLatitude.Text = ds.Tables[0].Rows[0]["fLatitude"].ToString();
                        BindWeatherGrid();
                        DisplayData(Convert.ToInt32(Session["id"]));
                    }

                }
                modal.Show();
            }
            catch (Exception ex)
            {
            }
        }

        BindDatlist();
    }
    #endregion

    #region Link Button Events
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string sFilePath = null;
        if (btnSubmit.Text == "Add")
        {

            if (((float)Request.Files[0].ContentLength / 1048576) > 2)
            {
                lblMessage.Text = "The file you are uploading is more than 2MB.";
                modal.Show();
                return;
            }

            //Check dulication here when add new record.
            dsLookup = objLookup.CheckBaseDuplication(0, txtPriorityName.Text, btnSubmit.Text);
            if (dsLookup.Tables[0].Rows.Count > 0)
            {
                if (dsLookup.Tables[0].Rows[0].ItemArray[0].ToString() == "1")
                {
                    lblMessage.Text = "Record already exist!";
                    modal.Show();
                }
                else
                {
                    //Start Select Weater Condition From Grid

                    string LookUpIdList = "";
                    bool IsAnyChacked = false;

                    foreach (GridViewRow gr in gv.Rows)
                    {
                        CheckBox chk = gr.FindControl("chk") as CheckBox;
                        Label lblId = gr.FindControl("lblId") as Label;

                        if (chk.Checked)
                        {
                            LookUpIdList += lblId.Text + ",";
                            IsAnyChacked = true;
                        }
                    }

                    if (!IsAnyChacked)
                    {
                        lblMessage.Text = "Please select any one weather condition ... ";
                        modal.Show();
                        return;
                    }

                    //  End Weater Condition

                    objBe = new BasedStationBE();
                    objBe.SOperation = "AddLookup";
                    objBe.sBaseStation = txtPriorityName.Text;
                    objBe.iCountryID = Convert.ToInt64(ddlCountry.SelectedValue);
                    objBe.iLatitude = (txtLatitude.Text);
                    objBe.iLongitude = (txtLongitude.Text);
                    objBe.sWeatherType = LookUpIdList;
                    if (flFile.Value != null)
                    {
                        objBe.sBaseStationIcon = "BaseStation" + "_" + flFile.Value;
                        sFilePath = IncentexGlobal.dropdownIconPath + "BaseStation" + "_" + flFile.Value;
                        //Delete Here if File already exist in Folder
                        objcommon.DeleteImageFromFolder("BaseStation" + "_" + flFile.Value, IncentexGlobal.dropdownIconPath);
                        Request.Files[0].SaveAs(sFilePath);
                    }
                    else
                        objBe.sBaseStationIcon = null;

                    objLookup.LookUpBaseStation(objBe);
                    //lblMessage.Text = "Record saved successfully!";
                }
            }
        }
        else
        {
            objBe = new BasedStationBE();
            objBe.SOperation = "EditLookup";

            //Start Select Weater Condition From Grid
            string LookUpIdList = "";
            bool IsAnyChacked = false;

            foreach (GridViewRow gr in gv.Rows)
            {
                CheckBox chk = gr.FindControl("chk") as CheckBox;
                Label lblId = gr.FindControl("lblId") as Label;

                if (chk.Checked)
                {
                    LookUpIdList += lblId.Text + ",";
                    IsAnyChacked = true;
                }
            }

            if (!IsAnyChacked)
            {
                lblMessage.Text = "Please select any one weather condition ... ";
                modal.Show();
                return;
            }
            //  End Weater Condition


            objBe.iBaseStationId = Convert.ToInt64(Session["id"].ToString());
            objBe.iCountryID = Convert.ToInt64(ddlCountry.SelectedValue);
            objBe.sBaseStation = txtPriorityName.Text;
            objBe.iLatitude = (txtLatitude.Text);
            objBe.iLongitude = (txtLongitude.Text);
            objBe.sWeatherType = LookUpIdList;

            if (flFile.Value != "")
                objBe.sBaseStationIcon = "BaseStation" + "_" + flFile.Value;
            else
                objBe.sBaseStationIcon = Session["iconvalue"].ToString();

            if (((float)Request.Files[0].ContentLength / 1048576) > 2)
            {
                lblMessage.Text = "The file you are uploading is more than 2MB.";
                modal.Show();
                return;
            }
            //Check dulication here in edit mode
            dsLookup = objLookup.CheckBaseDuplication(Convert.ToInt32(Session["id"].ToString()), txtPriorityName.Text, btnSubmit.Text);
            if (dsLookup.Tables[0].Rows.Count > 0)
            {
                if (dsLookup.Tables[0].Rows[0].ItemArray[0].ToString() == "1")
                {
                    lblMessage.Text = "Record already exist!";
                    modal.Show();
                }
                else
                {
                    if (!String.IsNullOrEmpty(flFile.Value))
                    {
                        sFilePath = IncentexGlobal.dropdownIconPath + "BaseStation" + "_" + flFile.Value;
                        objcommon.DeleteImageFromFolder("BaseStation" + "_" + flFile.Value, IncentexGlobal.dropdownIconPath);
                        Request.Files[0].SaveAs(sFilePath);
                    }

                    objLookup.LookUpBaseStation(objBe);

                    //lblMessage.Text = "Record updated successfully!";
                }
            }

        }

        BindDatlist();
    }
    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }

        lblErrorMessage.Text = string.Empty;
        btnSubmit.Text = "Add";
        txtPriorityName.Text = string.Empty;
        txtLatitude.Text = string.Empty;
        txtLongitude.Text = string.Empty;
        imgEdit.Visible = false;
        lblMessage.Text = string.Empty;
        BindWeatherGrid();
        modal.Controls.Clear();
        modal.Show();
    }
    #endregion

    #endregion

    #region Methods

    /// <summary>
    /// Fill the country droupdown
    /// </summary>
    private void FillSCountry()
    {
        try
        {
            ds = objCountry.GetCountry();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlCountry.DataSource = ds;
                ddlCountry.DataTextField = "sCountryName";
                ddlCountry.DataValueField = "iCountryID";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("-Select-", "0"));
                ddlCountry.SelectedValue = ddlCountry.Items.FindByText("United States").Value;
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        finally
        {
            ds.Dispose();
            objCountry = null;
        }
    }

    private void BindDatlist()
    {
        try
        {
            objBe = new BasedStationBE();
            objBe.SOperation = "Selectall";

            dsLookup = new BaseStationDA().LookUpBaseStation(objBe);
            if (dsLookup.Tables.Count > 0 && dsLookup.Tables[0].Rows.Count > 0)
            {
                dtLstLookup.DataSource = dsLookup;
                dtLstLookup.DataBind();
            }
            else
            {
                dtLstLookup.DataSource = null;
                dtLstLookup.DataBind();
            }
        }
        catch (Exception ex)
        {
            strMessgae = ex.Message.ToString();
        }
        finally
        {

            objLookup = null;
            dsLookup.Dispose();

        }
    }

    protected void BindWeatherGrid()
    {
        LookupRepository objRepo = new LookupRepository();
        List<INC_Lookup> objList = objRepo.GetByLookupCode(Incentex.DAL.Common.DAEnums.LookupCodeType.StationAdditionalInfo);
        gv.DataSource = objList;
        gv.DataBind();
    }

    void DisplayData(int id)
    {
        BaseStationRepository objRepo = new BaseStationRepository();

        INC_BasedStation objbaseStation = objRepo.GetById(id);
        if (string.IsNullOrEmpty(objbaseStation.WeatherType))
        {

            return;
        }

        string[] Ids = objbaseStation.WeatherType.Split(',');
        foreach (GridViewRow gr in gv.Rows)
        {
            CheckBox chk = gr.FindControl("chk") as CheckBox;
            Label lblId = gr.FindControl("lblId") as Label;
            HtmlGenericControl dvChk = gr.FindControl("dvChk") as HtmlGenericControl;
            foreach (string i in Ids)
            {
                if (i.Equals(lblId.Text))
                {
                    chk.Checked = true;
                    dvChk.Attributes.Add("class", "custom-checkbox_checked");
                    break;
                }
            }
        }

    }

    #endregion
}
