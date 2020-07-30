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
using System.Net;
using System.IO;
public partial class RND_ClickATel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            WebClient client = new WebClient();
            
            // Add a user agent header in case the requested URI contains a query.
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            client.QueryString.Add("user", "saurabh");
            client.QueryString.Add("password", "ARKGMRSPDZQTQD");
            client.QueryString.Add("api_id", "3394406");
            client.QueryString.Add("to", "918490012235");
            client.QueryString.Add("text", "This is an example message");
            string baseurl = "http://api.clickatell.com/http/sendmsg";
            Stream data = client.OpenRead(baseurl);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();
            //return (s);
        }
        catch (Exception)
        {

            throw;
        }
    }
}
