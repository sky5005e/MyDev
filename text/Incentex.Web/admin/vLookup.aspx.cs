using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;

public partial class admin_vLookup : PageBase
{
    DataSet dsLookup;
    LookupDA objLookup = new LookupDA();
    LookupBE objBe = new LookupBE();
    Common objcommon = new Common();
    string strMessgae = null;
    Boolean IsIconeDisplay
    {
        get
        {
            if (ViewState["IsIcone"] != null)
                return Convert.ToBoolean(ViewState["IsIcone"].ToString());
            else
                return true;
        }
        set
        {
            ViewState["IsIcone"] = value;
        }
    }


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
                    Session["iLookupCode"] = strEncrept;
                    // string strLookCode = Common.Decryption(strEncrept.Replace(" ", "+"));

                    if (Request.QueryString["IsDisplayIcon"] !=null)
                    {
                        uploadicondiv.Visible = Convert.ToBoolean(Request.QueryString["IsIconeDisplay"]);
                        IsIconeDisplay = Convert.ToBoolean(Request.QueryString["IsIconeDisplay"]);
                    }

                    BindDatlist(strEncrept);
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
    /// <summary>
    /// BindDatlist()
    /// This method is used to bind the datalist
    /// on the basis of using parameter ILookupcode from which is coming
    /// from the dropdown menus page.
    /// </summary>
    /// <param name="strQurystring"></param>
    public void BindDatlist(string strQurystring)
    {
        try
        {
            objBe.SOperation = "Selectall";
            objBe.iLookupCode = Session["iLookupCode"].ToString();
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
    /// <summary>
    /// Bind the image on dtlstlookup_itemDataBound.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dtLstLookup_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //string path = "../icons/" + ((HiddenField)e.Item.FindControl("hdnLookupIcon")).Value;
            string path = "~/admin/Incentex_Used_Icons/" + ((HiddenField)e.Item.FindControl("hdnLookupIcon")).Value;
            ((HtmlImage)e.Item.FindControl("imgLookupIcon")).Src = path;

            ((HtmlImage)e.Item.FindControl("imgLookupIcon")).Visible = IsIconeDisplay;
        }
    }
    /// <summary>
    /// Delete record from vlookup table and image form folder
    /// And edit the record.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
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

                //objBe.iLookupID = Convert.ToInt64(e.CommandArgument.ToString());
                //string strMaessage = objLookup.DeleteRecordByLookupID(Convert.ToInt32(e.CommandArgument.ToString()));
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
                        icondiv.Visible = IsIconeDisplay;
                        btnSubmit.Text = "Edit";
                        Session["id"] = e.CommandArgument.ToString();
                        Session["iconvalue"] = ds.Tables[0].Rows[0]["sLookupIcon"].ToString();
                        txtPriorityName.Text = ds.Tables[0].Rows[0]["sLookupName"].ToString();
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

        BindDatlist(Session["iLookupCode"].ToString());
    }
    /// <summary>
    /// Add new record and Update record in vLookup Table
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string sFilePath = null;
        if (btnSubmit.Text == "Add")
        {

            if (IsIconeDisplay && ((float)Request.Files[0].ContentLength / 1048576) > 2)
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
                    objBe.iLookupCode = Session["iLookupCode"].ToString();
                    if (IsIconeDisplay && flFile.Value != null)
                        objBe.sLookupIcon = Session["iLookupCode"].ToString() + "_" + flFile.Value;
                    else
                        objBe.sLookupIcon = null;

                    if (IsIconeDisplay)
                    {
                        sFilePath = Server.MapPath("../admin/Incentex_Used_Icons/") + Session["iLookupCode"].ToString() + "_" + flFile.Value;
                        //Delete Here if File already exist in Folder
                        objcommon.DeleteImageFromFolder(Session["iLookupCode"].ToString() + "_" + flFile.Value, IncentexGlobal.dropdownIconPath);
                        Request.Files[0].SaveAs(sFilePath);
                    }

                    objLookup.LookUp(objBe);
                    //lblMessage.Text = "Record saved successfully!";
                }
            }



            //  objBe.SOperation = "AddLookup";
            // objBe.sLookupName = txtPriorityName.Text;
            // objBe.iLookupCode = Session["iLookupCode"].ToString();
            // objBe.sLookupIcon= flFile.Value;
            //// string sFilePath = Server.MapPath("../icons/") + flFile.Value;
            //  sFilePath = Server.MapPath("../admin/Incentex_Used_Icons/") + flFile.Value;
            // Request.Files[0].SaveAs(sFilePath);
            // objLookup.LookUp(objBe);

        }
        else
        {
            objBe.SOperation = "EditLookup";
            objBe.iLookupID = Convert.ToInt64(Session["id"].ToString());
            objBe.iLookupCode = Session["iLookupCode"].ToString();
            objBe.sLookupName = txtPriorityName.Text;
           
            if (IsIconeDisplay && flFile.Value != "")
            {
                //Delete Here if File already exist in Folder
                objBe.sLookupIcon = Session["iLookupCode"].ToString() + "_" + flFile.Value;
                //objcommon.DeleteImageFromFolder(Session["iconvalue"].ToString(), IncentexGlobal.dropdownIconPath);
            }
            else
            {
                objBe.sLookupIcon = Session["iconvalue"].ToString();
            }
            //save new icon
            //string sFilePath = Server.MapPath("../icons/") + objBe.sLookupIcon;
            if (IsIconeDisplay && ((float)Request.Files[0].ContentLength / 1048576) > 2)
            {
                lblMessage.Text = "The file you are uploading is more than 2MB.";
                modal.Show();
                return;
            }
            //Check dulication here in edit mode
            dsLookup = objLookup.CheckDuplication(Convert.ToInt32(Session["id"].ToString()), txtPriorityName.Text, btnSubmit.Text, Session["iLookupCode"].ToString());
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
                        if (IsIconeDisplay)
                        {
                            if (flFile.Value != null)
                            {
                                sFilePath = Server.MapPath("../admin/Incentex_Used_Icons/") + Session["iLookupCode"].ToString() + "_" + flFile.Value;
                            }
                            objcommon.DeleteImageFromFolder(Session["iconvalue"].ToString(), IncentexGlobal.dropdownIconPath);
                            Request.Files[0].SaveAs(sFilePath);
                        }
                        objLookup.LookUp(objBe);
                    }
                    else
                    {
                        if (IsIconeDisplay)
                        {
                            if (flFile.Value != "")
                            {
                                sFilePath = Server.MapPath("../admin/Incentex_Used_Icons/") + Session["iLookupCode"].ToString() + "_" + flFile.Value;
                                objcommon.DeleteImageFromFolder(Session["iconvalue"].ToString(), IncentexGlobal.dropdownIconPath);
                                Request.Files[0].SaveAs(sFilePath);
                            }
                            else
                            {
                                sFilePath = Server.MapPath("../admin/Incentex_Used_Icons/") + Session["iconvalue"].ToString();
                                Request.Files[0].SaveAs(sFilePath);
                            }
                        }

                        objLookup.LookUp(objBe);
                    }

                    //lblMessage.Text = "Record updated successfully!";
                }
            }

        }

        BindDatlist(Session["iLookupCode"].ToString());
    }
    /// <summary>
    /// After Add or edit record, clear the field of popup.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
        lblMessage.Text = string.Empty;
        modal.Controls.Clear();
        modal.Show();
    }
}
