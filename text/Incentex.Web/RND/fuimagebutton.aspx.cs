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
using System.IO;

public partial class fuimagebutton : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btntest_Click(object sender, EventArgs e)
    {
        /*Path.GetFileName(Request.Files[0].FileName);
        string a = test.Value;
        string a1 = test1.Value;*/
        string sFilePath = @"D:\Ankit\" + Path.GetFileName(Request.Files[0].FileName);
        string sFilePath1 = @"D:\Ankit\" + Path.GetFileName(Request.Files[1].FileName); ;
        /*Response.Write(a + "<br/>");
        Response.Write(a1 + "<br/>");*/
        int size = Request.Files[0].ContentLength;
        int size1 = Request.Files[1].ContentLength;
        Request.Files[0].SaveAs(sFilePath);
        Request.Files[1].SaveAs(sFilePath1);
        Response.Write(size);
    }
}
