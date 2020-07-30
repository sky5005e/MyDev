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
using Incentex.DAL;
using Incentex.DAL.SqlRepository;
using System.Collections.Generic;
public partial class AssetManagement_vLookupDropdown : PageBase
{
    DataSet dsLookup;
    LookupDA objLookup = new LookupDA();
    LookupBE objBe = new LookupBE();
    Common objcommon = new Common();
    AssetMgtRepository objAssetMgtRepository = new AssetMgtRepository();
    List<EquipmentLookup> objListLookup = new List<EquipmentLookup>();
    string strMessgae = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            base.MenuItem = "Drop Down Menu";
            base.ParentMenuID = 46;

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
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/AssetManagement/DropdownMenu.aspx";
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
            objListLookup = objAssetMgtRepository.GetItemFrmEquipmentLookup(objBe.iLookupCode);
            if (objListLookup.Count>0)
            {
                dtLstLookup.DataSource = objListLookup;
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
            //dsLookup.Dispose();

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
               objBe.iLookupID = Convert.ToInt64(e.CommandArgument.ToString());
                objBe.SOperation = "DeleteLookup";
                EquipmentLookup objDelLookup = new EquipmentLookup();
                objDelLookup = objAssetMgtRepository.GetLookupByID(objBe.iLookupID);
                if (objDelLookup != null)
                {
                    objAssetMgtRepository.Delete(objDelLookup);
                    objAssetMgtRepository.SubmitChanges();
                }
                else
                {
                    lblErrorMessage.Visible = true;
                    lblErrorMessage.Text = "Unable to delete record as this record exists in other detail table";
                    return;
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

                objBe.iLookupID = Convert.ToInt64(e.CommandArgument.ToString());
                objBe.SOperation = "SelectbyLookupid";
                btnSubmit.Text = "Edit";
                objListLookup = objAssetMgtRepository.CheckDuplicatLookup(txtPriorityName.Text, Session["iLookupCode"].ToString());
                if (objListLookup != null)
                {
                    EquipmentLookup objEdtLookup = new EquipmentLookup();
                    objEdtLookup = objAssetMgtRepository.GetLookupByID(objBe.iLookupID);
                    Session["id"] = objEdtLookup.iLookupID;
                    txtPriorityName.Text = objEdtLookup.sLookupName;
                    txtPriorityName.Focus();                   
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
            objListLookup = objAssetMgtRepository.CheckDuplicatLookup(txtPriorityName.Text, Session["iLookupCode"].ToString());
            if (objListLookup.Count==0)
            {
                EquipmentLookup objInsLookup = new EquipmentLookup();
                objInsLookup.iLookupCode = Session["iLookupCode"].ToString();
                objInsLookup.sLookupName = txtPriorityName.Text;
                objAssetMgtRepository.Insert(objInsLookup);
                objAssetMgtRepository.SubmitChanges();
            }
            else
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
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
            objListLookup = objAssetMgtRepository.CheckDuplicatLookup(txtPriorityName.Text, Session["iLookupCode"].ToString());
            if (objListLookup != null)
            {
                EquipmentLookup objEdtLookup = new EquipmentLookup();
                objEdtLookup = objAssetMgtRepository.GetLookupByID(objBe.iLookupID);
                objEdtLookup.iLookupCode = Session["iLookupCode"].ToString();
                objEdtLookup.sLookupName = txtPriorityName.Text;                
                objAssetMgtRepository.SubmitChanges();
            }
            else
            {
                lblMessage.Text = "Record already exist!";
                modal.Show();
            }           

        }

        BindDatlist(Session["iLookupCode"].ToString());
    }
    protected void lnkAddNew_Click(object sender, EventArgs e)
    {

        btnSubmit.Text = "Add";
        txtPriorityName.Text = string.Empty;

        lblMessage.Text = string.Empty;
        modal.Controls.Clear();
        Page.SetFocus(txtPriorityName);
        modal.Show();

    }
}
