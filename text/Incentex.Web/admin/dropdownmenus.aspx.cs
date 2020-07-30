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

public partial class admin_dropdownmenus : PageBase
{
    string strIDValue = null;
    string strText = null;
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

            ((Label)Master.FindControl("lblPageHeading")).Text = "World-Link System Menu";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).Text = "<span><< Go Back</span>";
            ((HyperLink)Master.FindControl("lnkLoginUrl")).NavigateUrl = "~/admin/index.aspx";

        }
    }
    /// <summary>
    /// When Priority button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void Priority_Click(object sender, EventArgs e)
    {


        strText = Common.Encryption(Priority.Text);
        strIDValue = Priority.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When Department button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void Department_Click(object sender, EventArgs e)
    {

        strText = Common.Encryption(Department.Text);
        strIDValue = Department.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");

        //Response.Redirect("vLookup.aspx?strID=" + strIDValue + "");
    }

    /// <summary>
    /// When Number of Month button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void NumberOfMonths_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(NumberOfMonths.Text);
        strIDValue = NumberOfMonths.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");

    }
    /// <summary>
    /// When ProductionStatus button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void ProductionStatus_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ProductionStatus.Text);
        strIDValue = ProductionStatus.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When EmployeeRankPilotsOnly button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void EmployeeRankPilotsOnly_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(EmployeeRankPilotsOnly.Text);
        strIDValue = EmployeeRankPilotsOnly.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When lnkFirstReminder button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void FirstReminder_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(FirstReminder.Text);
        strIDValue = FirstReminder.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When GeneralStatus button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void GeneralStatus_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(GeneralStatus.Text);
        strIDValue = GeneralStatus.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When EmploymentStatus button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void EmploymentStatus_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(EmploymentStatus.Text);
        strIDValue = EmploymentStatus.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When SecondReminders button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void SecondReminders_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(SecondReminders.Text);
        strIDValue = SecondReminders.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When ProofStatus button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void ProofStatus_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ProofStatus.Text);
        strIDValue = ProofStatus.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When ShippingMethod button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void ShippingMethod_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ShippingMethod.Text);
        strIDValue = ShippingMethod.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When ThirdReminder button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void ThirdReminder_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ThirdReminder.Text);
        strIDValue = ThirdReminder.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    
    
    /// <summary>
    /// When ItemsToBePolybagged button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void ItemsToBePolybagged_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ItemsToBePolybagged.Text);
        strIDValue = ItemsToBePolybagged.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When ExpirationByMonths button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void ExpirationByMonths_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ExpirationByMonths.Text);
        strIDValue = ExpirationByMonths.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When ManagingShipmentBy button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void ManagingShipmentBy_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ManagingShipmentBy.Text);
        strIDValue = ManagingShipmentBy.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When ItemsToHaveSizeStickers button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void ItemsToHaveSizeStickers_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ItemsToHaveSizeStickers.Text);
        strIDValue = ItemsToHaveSizeStickers.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When ExpirationByDate button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void ExpirationByDate_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ExpirationByDate.Text);
        strIDValue = ExpirationByDate.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When ConsolidatedShipment button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void ConsolidatedShipment_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ConsolidatedShipment.Text);
        strIDValue = ConsolidatedShipment.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When ItemToBePackagedUsingCardboardInsert button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void ItemToBePackagedUsingCardboardInsert_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ItemToBePackagedUsingCardboardInsert.Text);
        strIDValue = ItemToBePackagedUsingCardboardInsert.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When AssociateWithFileCategory button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void AssociateWithFileCategory_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(AssociateWithFileCategory.Text);
        strIDValue = AssociateWithFileCategory.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When Workgroup button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void Workgroup_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(Workgroup.Text);
        //strIDValue = Common.Encryption(Workgroup.ID);
        strIDValue = Workgroup.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
        //Response.Redirect("vLookup.aspx?strID=" + strIDValue + "");

    }
    /// <summary>
    /// When ItemToBePackagedUsingPlasticClips button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void ItemToBePackagedUsingPlasticClips_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ItemToBePackagedUsingPlasticClips.Text);
        strIDValue = ItemToBePackagedUsingPlasticClips.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// When ProductCategory button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:29-july-2010
    /// ******************************************************
    protected void ProductCategory_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ProductCategory.Text);
        strIDValue = ProductCategory.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }

    /// <summary>
    /// When BaseStation button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Nagmani Date:31-july-2010
    /// ******************************************************
    protected void BaseStation_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(BaseStation.Text);
        strIDValue = BaseStation.ID;
        Response.Redirect("vLookupBaseStation.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }

    protected void Gender_Click(object sender, EventArgs e)
    {

        strText = Common.Encryption(Gender.Text);
        strIDValue = Gender.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");

    }
    protected void Rank_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(Rank.Text);
        strIDValue = Rank.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");

    }
    protected void MasterItemNumber_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(MasterItemNumber.Text);
        strIDValue = MasterItemNumber.ID;
        Response.Redirect("vLookupMasterItemNo.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");

    }
    protected void ItemColor_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ItemColor.Text);
        strIDValue = ItemColor.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    protected void ItemSize_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ItemSize.Text);
        strIDValue = ItemSize.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    protected void SoldBy_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(SoldBy.Text);
        strIDValue = SoldBy.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    protected void StyleNo_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(StyleNo.Text);
        strIDValue = StyleNo.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    protected void GarmentType_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(GarmentType.Text);
        strIDValue = GarmentType.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    protected void lnkIssuancePolicy_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(lnkIssuancePolicy.Text);
        strIDValue = lnkIssuancePolicy.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    protected void lnkRegion_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(Region.Text);
        strIDValue = Region.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }

    protected void EmployeeTitle_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(EmployeeTitle.Text);
        strIDValue = EmployeeTitle.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    protected void lnkCity_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(lnkCity.Text);
        strIDValue = lnkCity.ID;
        Response.Redirect("city.aspx");
    }
    protected void lnkFileCategory_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(lnkFileCategory.Text);
        strIDValue = lnkFileCategory.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }

    /// <summary>
    /// When Employee Type button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender">strID</param>
    /// <param name="e"></param>
    /// Mayur Date:30th-December-2011
    /// ******************************************************
    protected void EmployeeType_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(EmployeeType.Text);
        strIDValue = EmployeeType.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }


    /// <summary>
    /// When Support Issue button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// Devraj Gadhavi Date:20th-January-2012
    /// ******************************************************
    protected void ServiceSupportIssue_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ServiceSupportIssue.Text);
        strIDValue = ServiceSupportIssue.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }

    protected void lnkState_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(lnkState.Text);
        strIDValue = lnkState.ID;
        Response.Redirect("State.aspx");
    }

    /// <summary>
    /// When Support Ticket Status button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// Devraj Gadhavi Date:25th-January-2012
    /// ******************************************************
    protected void ServiceTicketStatus_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ServiceTicketStatus.Text);
        strIDValue = ServiceTicketStatus.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }

    /// <summary>
    /// Asset Management System
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// Saurabh Mathur Date:5th-April-2012
    /// ******************************************************
    protected void EquipmentType_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(EquipmentType.Text);
        strIDValue = EquipmentType.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// Asset Management System
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// Saurabh Mathur Date:5th-April-2012
    /// ******************************************************
    protected void Brand_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(Brand.Text);
        strIDValue = Brand.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// Asset Management System
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// Saurabh Mathur Date:5th-April-2012
    /// ******************************************************
    protected void FuelType_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(FuelType.Text);
        strIDValue = FuelType.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// Asset Management System
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// Saurabh Mathur Date:5th-April-2012
    /// ******************************************************
    protected void PurchasedFrom_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(PurchasedFrom.Text);
        strIDValue = PurchasedFrom.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// Asset Management System
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// Saurabh Mathur Date:5th-April-2012
    /// ******************************************************
    protected void GSEDepartment_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(GSEDepartment.Text);
        strIDValue = GSEDepartment.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// Asset Management System
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// Saurabh Mathur Date:5th-April-2012
    /// ******************************************************
    protected void ChargeTo_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ChargeTo.Text);
        strIDValue = ChargeTo.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// Asset Management System
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// Saurabh Mathur Date:5th-April-2012
    /// ******************************************************
    protected void EquipmentStatus_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(EquipmentStatus.Text);
        strIDValue = EquipmentStatus.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
   }
    /// <summary>
    /// When Type Of Request button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// Devraj Gadhavi Date:25th-January-2012
    /// ******************************************************
    protected void TypeOfRequest_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(TypeOfRequest.Text);
        strIDValue = TypeOfRequest.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }

    protected void lnkLookupEntry_Click(object sender, EventArgs e)
    {
        Response.Redirect("vLookupEntry.aspx");
    }

    protected void Supplies_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(Supplies.Text);
        strIDValue = Supplies.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    /// <summary>
    /// This button for Material 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Material_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(Material.Text);
        strIDValue = Material.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }
    protected void OrderFor_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(OrderFor.Text);
        strIDValue = OrderFor.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    /// <summary>
    /// When Support Ticket reason button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// Devraj Gadhavi Date:10th-September-2012
    /// ******************************************************
    protected void lnkSupportTicketReason_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(SupportTicketReason.Text);
        strIDValue = SupportTicketReason.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    /// <summary>
    /// When Reason for Replacement button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>
    /// Devraj Gadhavi Date:10th-October-2012
    /// ******************************************************
    protected void ReasonForReplacement_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ReasonForReplacement.Text);
        strIDValue = ReasonForReplacement.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    /// <summary>
    /// Handles the Click event of the ReportTag control.
    /// This dropdown is used in product -> general tab
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    /// Mayur Rathod Date:10th-Oct-2012
    protected void ReportTag_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ReportTag.Text);
        strIDValue = ReportTag.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    protected void ItemType_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ItemType.Text);
        strIDValue = ItemType.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    /// <summary>
    /// When Additional Information button is clicked then we passed the Text type
    /// of this button through query string
    /// </summary>    
    /// Devraj Gadhavi Date:07th-November-2012
    /// ******************************************************
    protected void lnkAdditionalInfo_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(lnkAdditionalInfo.ToolTip);
        strIDValue = lnkAdditionalInfo.ToolTip;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }


    protected void lnkHeadsetBrand_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(HeadsetBrand.Text);
        strIDValue = HeadsetBrand.ID;
        Response.Redirect("vLookup.aspx?IsDisplayIcon=false" + " " + "&strID=" + strIDValue + " &strValue=" + strText + " ");
    }

    protected void lnkHeadsetStatus_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(HeadsetStatus.Text);
        strIDValue = HeadsetStatus.ID;
        Response.Redirect("vLookup.aspx?IsDisplayIcon=false"+ " " +"&strID=" + strIDValue + " &strValue=" + strText + " ");
    }

    protected void lnkReasonForReturn_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(lnkReasonForReturn.ToolTip);
        strIDValue = lnkReasonForReturn.ToolTip;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    protected void lnkBusinessType_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(lnkBusinessType.ToolTip);
        strIDValue = lnkBusinessType.ToolTip;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    protected void lnkOrderfor_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(lnkOrderfor.ToolTip);
        strIDValue = lnkOrderfor.ToolTip;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }
    protected void lnkOverseasVendor_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(lnkOverseasVendor.ToolTip);
        strIDValue = lnkOverseasVendor.ToolTip;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }
    protected void lnkOrderSentby_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(lnkOrderSentby.ToolTip);
        strIDValue = lnkOrderSentby.ToolTip;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }
    protected void lnkOrderStatus_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(lnkOrderStatus.ToolTip);
        strIDValue = lnkOrderStatus.ToolTip;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }
    
    protected void ClimateSetting_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ClimateSetting.ToolTip);
        strIDValue = ClimateSetting.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    protected void BackOrderManagement_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(BackOrderManagement.ToolTip);
        strIDValue = BackOrderManagement.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    #region for Purchase Order System

    protected void ArtworkCreatedBy_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ArtworkCreatedBy.Text);
        strIDValue = ArtworkCreatedBy.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    protected void GarmentSizeApply_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(GarmentSizeApply.Text);
        strIDValue = GarmentSizeApply.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    /// <summary>
    ///  
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DecoratingMethod_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(DecoratingMethod.Text);
        strIDValue = DecoratingMethod.ID;
        Response.Redirect("vLookup.aspx?strID=" + strIDValue + " &strValue=" + strText + " ");
    }

    protected void ProductType_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ProductType.Text);
        strIDValue = ProductType.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    protected void Decorator_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(Decorator.Text);
        strIDValue = Decorator.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    protected void ImprintLocations_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ImprintLocations.Text);
        strIDValue = ImprintLocations.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    protected void ProductGLCode_Click(object sender, EventArgs e)
    {
        strText = Common.Encryption(ProductGLCode.Text);
        strIDValue = ProductGLCode.ID;
        Response.Redirect("vLookupSimple.aspx?strID=" + strIDValue + "&strValue=" + strText + "");
    }

    #endregion 
}