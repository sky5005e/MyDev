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

public partial class DropdownMenu_Priority : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        PriorityBE objPriorityBe = new PriorityBE();
        PriorityDA objPriorityDa = new PriorityDA();
        objPriorityBe.SOperation = "addpriority";
        objPriorityBe.sPriorityName = txtPriorityName.Text;
        objPriorityBe.sPriorityIcon = flFile.Value;
        string sFilePath = Server.MapPath("../icons/") + flFile.Value;;
        Request.Files[0].SaveAs(sFilePath);
        

        //objPriorityBe.sPriorityIcon;
        DataSet ds = objPriorityDa.Priority(objPriorityBe);
        
    }
}
