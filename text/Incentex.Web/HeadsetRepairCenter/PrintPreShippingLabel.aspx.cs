using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Incentex.DAL.SqlRepository;

public partial class ProductReturnManagement_PrintPreShippingLabel : System.Web.UI.Page
{
    #region Data Member's
    HeadsetRepairCenterRepository objHeadsetRepair = new HeadsetRepairCenterRepository();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //if (!string.IsNullOrEmpty(Request.QueryString["Repairnumber"]))
        //{
        //    long repairNumber = Convert.ToInt64(Request.QueryString["Repairnumber"]);
        //    Incentex.DAL.SqlRepository.HeadsetRepairCenterCustom objHeadsetDetails = objHeadsetRepair.GetHeadsetRepaircustomByRepairNumber(repairNumber);

        //    if (objHeadsetDetails != null && objHeadsetDetails.HeadsetRepairID > 0)
        //    {
        //        lblcompany.Text = objHeadsetDetails.CompanyName;
        //        lblcontact.Text = objHeadsetDetails.ContactName;
        //        lblrepairquantity.Text = Convert.ToString(objHeadsetDetails.TotalHeadset);
        //        lblheadsetbrand.Text = objHeadsetDetails.HeadsetBrandName;
        //        lblrepairnumber.Text = "HR"+ objHeadsetDetails.RepairNumber;
        //        lblsubmitdate.Text = Convert.ToString(objHeadsetDetails.Date);
        //        lblrequestquote.Text = objHeadsetDetails.Requestquotebeforerepair ? "Yes" : "No";   
        //    }
        //}
    }
}
