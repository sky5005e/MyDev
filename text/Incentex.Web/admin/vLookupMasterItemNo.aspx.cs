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
using System.Xml.Linq;
using Incentex.BE;
using Incentex.DA;
using System.Data.SqlClient;
using System.IO;

public partial class vLookupMasterItemNo : PageBase
{
    DataSet dsLookup;
    DataSet ds;
    LookupDA objLookup = new LookupDA();
    Common objcommon = new Common();
    string strMessgae = null;
    CountryDA objCountry = new CountryDA();
    LookupBE objBe = new LookupBE();
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
                    Session["iLookupCode"] = strEncrept.Trim();
                    BindDatlist();
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                    ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "dropdownmenus.aspx";
                }
                else
                {
                    Response.Redirect("dropdownmenus.aspx");
                }
            }
        }
    }
    public void BindDatlist()
    {
        try
        {
            objBe.SOperation = "Selectall";
            objBe.iLookupCode=Session["iLookupCode"].ToString();
            dsLookup = objLookup.LookUp(objBe);
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
    protected void dtLstLookup_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //string path = "../icons/" + ((HiddenField)e.Item.FindControl("hdnLookupIcon")).Value;
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

                objBe.iLookupID = Convert.ToInt64(e.CommandArgument.ToString());
                objBe.SOperation = "DeleteLookup";
                dsLookup = objLookup.LookUp(objBe);
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
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "@@@@MyPopUpScript", strMaessage, true);
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

                objBe.iLookupID = Convert.ToInt64(e.CommandArgument.ToString());
                objBe.SOperation = "SelectbyLookupid";
                DataSet ds = objLookup.LookUp(objBe);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        icondiv.Visible = true;
                        btnSubmit.Text = "Edit";
                        Session["id"] = e.CommandArgument.ToString();
                        Session["iconvalue"] = ds.Tables[0].Rows[0]["sLookupIcon"].ToString();
                        txtPriorityName.Text = ds.Tables[0].Rows[0]["sLookupName"].ToString();
                        if (ds.Tables[0].Rows[0]["Val1"] != "")
                        {
                            ddlGender.SelectedValue = ds.Tables[0].Rows[0]["Val1"].ToString();
                        }
                        else
                        {
                            ddlGender.SelectedIndex = 0;
                        }
                        // imgEdit.Src = "../icons/" + Session["iconvalue"];
                        imgEdit.Src = "../admin/Incentex_Used_Icons/" + Session["iconvalue"];


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
            dsLookup = objLookup.CheckDuplication(0, txtPriorityName.Text, btnSubmit.Text, Session["iLookupCode"].ToString());
            if (dsLookup.Tables[0].Rows.Count > 0)
            {
                if (dsLookup.Tables[0].Rows[0].ItemArray[0].ToString() == "1")
                {
                    lblMessage.Text = "Record already exist!";
                    modal.Show();
                }
                else
                {
                    objBe.SOperation = "AddLookup";
                    objBe.sLookupName = txtPriorityName.Text;
                    objBe.Val1 = ddlGender.SelectedValue;
                    objBe.iLookupCode = Session["iLookupCode"].ToString();
                    if (flFile.Value != null)
                        objBe.sLookupIcon = "MasterItemNo" + "_" + flFile.Value;
                    else
                        objBe.sLookupIcon = null;
                    sFilePath = Server.MapPath("../admin/Incentex_Used_Icons/") + "MasterItemNo" + "_" + flFile.Value;
                    //Delete Here if File already exist in Folder
                    objcommon.DeleteImageFromFolder("MasterItemNo" + "_" + flFile.Value, IncentexGlobal.dropdownIconPath);
                    Request.Files[0].SaveAs(sFilePath);
                    objLookup.LookUp(objBe);
                    //lblMessage.Text = "Record saved successfully!";
                }
            }



        }
        else
        {
            objBe.SOperation = "EditLookup";
            objBe.iLookupID = Convert.ToInt64(Session["id"].ToString());
            objBe.iLookupCode = Session["iLookupCode"].ToString();
            objBe.Val1 = ddlGender.SelectedValue;
            objBe.sLookupName = txtPriorityName.Text;
            if (flFile.Value != "")
            {
                //Delete Here if File already exist in Folder
                objBe.sLookupIcon = "MasterItemNo" + "_" + flFile.Value;
                //objcommon.DeleteImageFromFolder(Session["iconvalue"].ToString(), IncentexGlobal.dropdownIconPath);
            }
            else
            {
                objBe.sLookupIcon = Session["iconvalue"].ToString();
            }
            //save new icon
            //string sFilePath = Server.MapPath("../icons/") + objBe.sLookupIcon;
            if (((float)Request.Files[0].ContentLength / 1048576) > 2)
            {
                lblMessage.Text = "The file you are uploading is more than 2MB.";
                modal.Show();
                return;
            }
            //Check dulication here in edit mode
            dsLookup = objLookup.CheckDuplication(Convert.ToInt32(Session["id"].ToString()), txtPriorityName.Text, btnSubmit.Text,Session["iLookupCode"].ToString());
            if (dsLookup.Tables[0].Rows.Count > 0)
            {
                if (dsLookup.Tables[0].Rows[0].ItemArray[0].ToString() == "1")
                {
                    lblMessage.Text = "Record already exist!";
                    modal.Show();
                }
                else
                {
                    if (Session["iconvalue"] == null)
                    {
                        if (flFile.Value != null)
                        {
                            sFilePath = Server.MapPath("../admin/Incentex_Used_Icons/") + "MasterItemNo" + "_" + flFile.Value;
                        }
                        objcommon.DeleteImageFromFolder(Session["iconvalue"].ToString(), IncentexGlobal.dropdownIconPath);
                        Request.Files[0].SaveAs(sFilePath);
                        objLookup.LookUp(objBe);
                    }
                    else
                    {
                        if (flFile.Value != "")
                        {
                            sFilePath = Server.MapPath("../admin/Incentex_Used_Icons/") + "MasterItemNo" + "_" + flFile.Value;
                            objcommon.DeleteImageFromFolder(Session["iconvalue"].ToString(), IncentexGlobal.dropdownIconPath);
                            Request.Files[0].SaveAs(sFilePath);
                        }
                        else
                        {
                            sFilePath = Server.MapPath("../admin/Incentex_Used_Icons/") + Session["iconvalue"].ToString();
                            Request.Files[0].SaveAs(sFilePath);
                        }

                        objLookup.LookUp(objBe);
                    }

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
        icondiv.Visible = false;
        ddlGender.SelectedIndex = 0;
        lblMessage.Text = string.Empty;
        modal.Controls.Clear();
        modal.Show();
    }
}
