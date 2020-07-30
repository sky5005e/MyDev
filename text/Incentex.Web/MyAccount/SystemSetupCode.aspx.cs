/// <summary>
/// Module Name : System Setup Code 
/// Description : This page display setup code for store,department and workgroup wise. So CA can give this code to new register user.
/// Created : Mayur on 07-May-2012
/// </summary>
/// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Incentex.DA;
using Incentex.BE;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class MyAccount_SystemSetupCode : PageBase
{
    #region Data Members
    LookupDA sEU = new LookupDA();
    LookupBE sEUBE = new LookupBE();
    #endregion

    #region Event Handlers
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CheckLogin();

            ((Label)Master.FindControl("lblPageHeading")).Text = "System Setup Code";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/Index.aspx";

            DisplayData();
        }
    }
    #endregion

    #region Methods
    private void DisplayData()
    {
        try
        {
            Company objCompany = new CompanyRepository().GetById(Convert.ToInt64(IncentexGlobal.CurrentMember.CompanyId));
            lblCompanyName.Text = objCompany.CompanyName + " : " + objCompany.CompanyID;            
            spnCompanyName2.InnerText = objCompany.CompanyName;
            spnCompanyCode2.InnerText = objCompany.CompanyID.ToString();            

            Int64 StoreID = new CompanyStoreRepository().GetByCompanyId(objCompany.CompanyID).StoreID;

            CompanyStoreRepository objComStoRepo = new CompanyStoreRepository();

            List<GetStoreDepartmentsResult> lstDepartments = new List<GetStoreDepartmentsResult>();
            lstDepartments = objComStoRepo.GetStoreDepartments(StoreID).Where(le => le.Existing == 1).OrderBy(le => le.Department).ToList();

            List<GetStoreWorkGroupsResult> lstWorkGroups = new List<GetStoreWorkGroupsResult>();
            lstWorkGroups = objComStoRepo.GetStoreWorkGroups(StoreID).Where(le => le.Existing == 1).OrderBy(le => le.WorkGroup).ToList();

            //sEUBE.SOperation = "selectall";
            //sEUBE.iLookupCode = "Department";
            //DataSet dsDept = sEU.LookUp(sEUBE);
            dtlDepartments.DataSource = lstDepartments;
            dtlDepartments.DataBind();

            //sEUBE.SOperation = "selectall";
            //sEUBE.iLookupCode = "Workgroup";
            //DataSet dsWork = sEU.LookUp(sEUBE);
            dtlWorkgroups.DataSource = lstWorkGroups;
            dtlWorkgroups.DataBind();
        }
        catch { }
    }
    #endregion
}
