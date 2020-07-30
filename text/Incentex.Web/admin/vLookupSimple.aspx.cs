using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.BE;
using Incentex.DA;

public partial class admin_vLookupSimple :PageBase
{
    DataSet dsLookup;
    LookupDA objLookup = new LookupDA();
    LookupBE objBe = new LookupBE();
    Common objcommon = new Common();
    string strMessgae = null;
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

            if (Convert.ToString(Request.QueryString["strID"]) != null && Convert.ToString(Request.QueryString["strValue"]) != null)
            {
                string strText = Request.QueryString["strValue"].ToString();
                string strHeaderText = Common.Decryption(strText.Replace(" ", "+"));
                ((Label)Master.FindControl("lblPageHeading")).Text = strHeaderText;
                string strEncrept = Request.QueryString["strID"].ToString();
                Session["iLookupCode"] = strEncrept;
                // string strLookCode = Common.Decryption(strEncrept.Replace(" ", "+"));
                BindDatlist(strEncrept);

                ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/dropdownmenus.aspx";
            }
            else
            {
                Response.Redirect("dropdownmenus.aspx");
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
    protected void dtLstLookup_ItemDataBound(object sender, DataListItemEventArgs e)
    {

        
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
                        
                        btnSubmit.Text = "Edit";
                        Session["id"] = e.CommandArgument.ToString();
                        Session["iconvalue"] = ds.Tables[0].Rows[0]["sLookupIcon"].ToString();
                        txtPriorityName.Text = ds.Tables[0].Rows[0]["sLookupName"].ToString();
                        txtPriorityName.Focus();


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
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
     
        if (btnSubmit.Text == "Add")
        {

            
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
                        objBe.sLookupIcon = null;
                        objLookup.LookUp(objBe);
                        //lblMessage.Text = "Record saved successfully!";
                    }
                }

            //}
        
        }
        else
        {
            objBe.SOperation = "EditLookup";
            objBe.iLookupID = Convert.ToInt64(Session["id"].ToString());
            objBe.iLookupCode = Session["iLookupCode"].ToString();
            objBe.sLookupName = txtPriorityName.Text;
           
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
                    
                    objLookup.LookUp(objBe);
                    lblMessage.Text = "Record updated successfully!";
                }
            }

        }

        BindDatlist(Session["iLookupCode"].ToString());
    }
    protected void lnkAddNew_Click(object sender, EventArgs e)
    {
        if (!base.CanAdd)
        {
            base.RedirectToUnauthorised();
        }
      
        btnSubmit.Text = "Add";
        txtPriorityName.Text = string.Empty;
        
        lblMessage.Text = string.Empty;
        modal.Controls.Clear();
        Page.SetFocus(txtPriorityName);
        modal.Show();
       
    }
}
