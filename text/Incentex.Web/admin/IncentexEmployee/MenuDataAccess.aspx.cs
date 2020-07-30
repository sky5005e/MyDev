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
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
using System.Collections.Generic;
using Incentex.DA;
using Incentex.BE;

public partial class admin_IncentexEmployee_MenuDataAccess : PageBase
{
    #region Properties

    IncentexEmployeeRepository objRepo = new IncentexEmployeeRepository();
    IncentexEmployee obj = new IncentexEmployee();

    Int64 IncentexEmployeeID
    {
        get
        {
            if (ViewState["IncentexEmployeeID"] == null)
            {
                ViewState["IncentexEmployeeID"] = 0;
            }
            return Convert.ToInt64(ViewState["IncentexEmployeeID"]);
        }
        set
        {
            ViewState["IncentexEmployeeID"] = value;
        }
    }

    Int64 UserInfoID
    {
        get
        {
            if (ViewState["UserInfoID"] == null)
            {
                ViewState["UserInfoID"] = 0;
            }
            return Convert.ToInt64(ViewState["UserInfoID"]);
        }
        set
        {
            ViewState["UserInfoID"] = value;
        }
    }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();
            
            if (Request.QueryString.Count > 0)
            {
                this.IncentexEmployeeID = Convert.ToInt64(Request.QueryString.Get("Id"));
                if (this.IncentexEmployeeID == 0)
                {
                    Response.Redirect("~/admin/IncentexEmployee/BasicInformation.aspx?Id=" + this.IncentexEmployeeID);
                }

                ((Label)Master.FindControl("lblPageHeading")).Text = "Menu/Data Access";
                //((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span>Return to  Manage Incentex Employee</span>";
                ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/IncentexEmployee/ViewIncentexEmployee.aspx";
                
                menucontrol.PopulateMenu(3, 0, this.IncentexEmployeeID, 0,false);
            }
            else
            {
                Response.Redirect("ViewIncentexEmployee.aspx");
            }

            
            bindMenus();
            DisplayData();
        }
    }
    
    void DisplayData()
    {
        CheckBox chk;
        HiddenField lblId;
        if (this.IncentexEmployeeID != 0)
        {
            objRepo = new IncentexEmployeeRepository();
            obj = objRepo.GetById(this.IncentexEmployeeID);

            if (obj != null)
            {
                //Get menu from empmenuaccess table
                List<IncEmpMenuAccess> lstMenuAccess = new IncentexEmpMenuAccessRepository().GetMenusByEmployeeId(this.IncentexEmployeeID);

                foreach (DataListItem dtM in dtlMenus.Items)
                {
                    chk = dtM.FindControl("chkdtlMenus") as CheckBox;
                    lblId = dtM.FindControl("hdnMenuAccess") as HiddenField;
                    HtmlGenericControl dvChk = dtM.FindControl("menuspan") as HtmlGenericControl;

                    foreach (IncEmpMenuAccess objMenu in lstMenuAccess)
                    {

                        if (objMenu.MenuPrivilegeID.ToString().Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChk.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }

                    }

                }


                //Get Data from dataaccess


                List<IncEmpDataAccess> lstdataAccess = new IncentexEmpDataAccessRepository().GetDataByEmployeeId(this.IncentexEmployeeID);

                foreach (DataListItem dtD in dtlDataAccess.Items)
                {
                    chk = dtD.FindControl("chkdtlDataAccess") as CheckBox;
                    lblId = dtD.FindControl("hdnDataBaseAccess") as HiddenField;
                    HtmlGenericControl dvChkData = dtD.FindControl("menuspanData") as HtmlGenericControl;

                    foreach (IncEmpDataAccess objMenu in lstdataAccess)
                    {

                        if (objMenu.DataPrivilegeID.ToString().Equals(lblId.Value))
                        {
                            chk.Checked = true;
                            dvChkData.Attributes.Add("class", "custom-checkbox_checked");
                            break;
                        }

                    }

                }
            }
        }
    }

    public void bindMenus()
    {
        MenuPrivilegeRepository objMenu = new MenuPrivilegeRepository();
        List<INC_MenuPrivilege> objList = objMenu.GetFrontMenu(DAEnums.MenuTypes.BackEnd.ToString(), "Incentex Employee").OrderBy(le => le.MenuOrder).ToList();
        dtlMenus.DataSource = objList;
        dtlMenus.DataBind();

        DataAccessRepository objData = new DataAccessRepository();
        List<INC_DataPrivilege> objDataList =  objData.GetDataAccess();
        dtlDataAccess.DataSource = objDataList;
        dtlDataAccess.DataBind();
    }
    protected void lnkSave_Click(object sender, EventArgs e)
    {
        try
        {
            //Delete from IncentexEmpMenuAccess
            IncentexEmpMenuAccessRepository objCmpMenuAccesRepos = new IncentexEmpMenuAccessRepository();
            IncEmpMenuAccess objCmpMenuAccessfordel = new IncEmpMenuAccess();

            List<IncEmpMenuAccess> lst = objCmpMenuAccesRepos.GetMenusByEmployeeId(this.IncentexEmployeeID);

            foreach (IncEmpMenuAccess l in lst)
            {
                objCmpMenuAccesRepos.Delete(l);
                objCmpMenuAccesRepos.SubmitChanges();
            }

            //delete from incentexdataaccess

            IncentexEmpDataAccessRepository objCmpDataAccesRepos = new IncentexEmpDataAccessRepository();
            IncEmpDataAccess objCmpDataAccessfordel = new IncEmpDataAccess();

            List<IncEmpDataAccess> lstData = objCmpDataAccesRepos.GetDataByEmployeeId(this.IncentexEmployeeID);

            foreach (IncEmpDataAccess l in lstData)
            {
                objCmpDataAccesRepos.Delete(l);
                objCmpDataAccesRepos.SubmitChanges();
            }


            //Insert in incempmenuaccess

            //Menu Access
            foreach (DataListItem dt in dtlMenus.Items)
            {
                if (((CheckBox)dt.FindControl("chkdtlMenus")).Checked == true)
                {
                    IncentexEmpMenuAccessRepository objCmpMenuAccesRep = new IncentexEmpMenuAccessRepository();
                    IncEmpMenuAccess objCmpMenuAccess = new IncEmpMenuAccess();
                    objCmpMenuAccess.MenuPrivilegeID = Convert.ToInt64(((HiddenField)dt.FindControl("hdnMenuAccess")).Value);
                    objCmpMenuAccess.IncentexEmployeeID= this.IncentexEmployeeID;
                    objCmpMenuAccesRep.Insert(objCmpMenuAccess);
                    objCmpMenuAccesRep.SubmitChanges();
                }

            }

            //Insert in incempdataaccess
            foreach (DataListItem dtData in dtlDataAccess.Items)
            {
                if (((CheckBox)dtData.FindControl("chkdtlDataAccess")).Checked == true)
                {
                    IncentexEmpDataAccessRepository objCmpDataAccesRep = new IncentexEmpDataAccessRepository();
                    IncEmpDataAccess objEmpMenuAccess = new IncEmpDataAccess();
                    objEmpMenuAccess.DataPrivilegeID = Convert.ToInt64(((HiddenField)dtData.FindControl("hdnDataBaseAccess")).Value);
                    objEmpMenuAccess.IncentexEmployeeID = this.IncentexEmployeeID;
                    objCmpDataAccesRep.Insert(objEmpMenuAccess);
                    objCmpDataAccesRep.SubmitChanges();
                }

            }
            Response.Redirect("PersonalInformation.aspx?id="+this.IncentexEmployeeID);

        }
        catch (Exception ex)
        {

        }
    }
}
