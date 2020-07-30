using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using Incentex.DAL;
using Incentex.DAL.SqlRepository;

public partial class usercontrol_ReplacementUniformsCA : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (IncentexGlobal.CurrentMember != null)
            {
                FillEmployees();
                mpReplacementUniformCA.Show();
            }
        }
    }

    protected override void OnInit(EventArgs e)
    {
        Page.Init += delegate(object sender, EventArgs e_Init)
        {
            if (ToolkitScriptManager.GetCurrent(Page) == null && ScriptManager.GetCurrent(Page) == null)
            {
                ToolkitScriptManager sMgr = new ToolkitScriptManager();
                phScriptManager.Controls.AddAt(0, sMgr);
            }
        };
        base.OnInit(e);
    }

    protected void btnPublishRU2CE_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToBoolean(Session["ruCAControl"]))
            {
                if (IncentexGlobal.CurrentMember != null && !String.IsNullOrEmpty(ddlEmployees.SelectedValue))
                {
                    Int64 EmployeeID = Convert.ToInt64(ddlEmployees.SelectedValue);
                    Int64 ReplacementUniformsID = Convert.ToInt64(new LookupRepository().GetIdByLookupNameNLookUpCode("Replacement Uniforms", "WLS Payment Option"));

                    if (EmployeeID > 0 && ReplacementUniformsID > 0)
                    {
                        CompanyEmployeeRepository objRepo = new CompanyEmployeeRepository();
                        CompanyEmployee objEmployee = objRepo.GetByUserInfoId(EmployeeID);

                        if (objEmployee != null)
                        {
                            Boolean IsAlreadyGiven = false;

                            if (!String.IsNullOrEmpty(objEmployee.Paymentoption))
                            {
                                foreach (String option in objEmployee.Paymentoption.Split(','))
                                {
                                    if (option == ReplacementUniformsID.ToString())
                                    {
                                        IsAlreadyGiven = true;
                                    }
                                }
                            }
                            else
                            {
                                IsAlreadyGiven = false;
                            }

                            if (!IsAlreadyGiven)
                            {
                                objEmployee.Paymentoption = objEmployee.Paymentoption + "," + ReplacementUniformsID;
                                objRepo.SubmitChanges();
                            }
                        }
                    }                    
                }

                Session.Remove("ruCAControl");
            }
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
        Response.Redirect("~/index.aspx");
    }

    /// <summary>
    /// For filling the employees related to the CA
    /// </summary>
    private void FillEmployees()
    {
        try
        {
            CompanyEmployeeRepository objRepo = new CompanyEmployeeRepository();
            var lstServiceTicketOwners = objRepo.GetEmployeesByBaseStationAndWorkGroupOfCA(IncentexGlobal.CurrentMember.UserInfoID).Select(le => new { UserInfoID = le.UserInfoID, EmployeeName = le.FirstName + " " + le.LastName }).OrderBy(le => le.EmployeeName).ToList();

            ddlEmployees.DataSource = lstServiceTicketOwners;
            ddlEmployees.DataValueField = "UserInfoID";
            ddlEmployees.DataTextField = "EmployeeName";
            ddlEmployees.DataBind();
            ddlEmployees.Items.Insert(0, new ListItem("-Select Employee-", "0"));            
        }
        catch (Exception ex)
        {
            ErrHandler.WriteError(ex);
        }
    }
}