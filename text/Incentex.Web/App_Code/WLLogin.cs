using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.IO;
using System.Collections.Specialized;
using Incentex.DAL.SqlRepository;
using Incentex.DAL;
/// <summary>
/// Summary description for WLLogin
/// </summary>
//[WebService(Namespace = "http://tempuri.org/")]
[WebService(Namespace = "https://www.world-link.us.com/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class WLLogin : System.Web.Services.WebService
{

    public WLLogin()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public XmlDocument Login()
    {
        String xml = string.Empty;
        XmlDocument xmlDoc = new XmlDocument();
        String cookie = string.Empty;
        String url = string.Empty;
        String UserEmail = string.Empty;
        String pyid = string.Empty;
        String tymstp = string.Empty;
        Int64 ID = 0;
        try
        {
            // Initialize soap request XML
            XmlDocument xDoc = new XmlDocument();
            // Get raw request body
            Stream receiveStream = HttpContext.Current.Request.InputStream;
            // Move to begining of input stream and read
            receiveStream.Position = 0;
            using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
            {
                // Load into XML document
                xDoc.Load(readStream);
            }
            //extract item from node
            XmlNodeList nodeTm = xDoc.SelectNodes("/cXML");
            if (nodeTm.Count > 0)
            {
                pyid = "\"" + nodeTm[0].Attributes.Item(1).Value + "\"";
                tymstp = "\"" + nodeTm[0].Attributes.Item(2).Value + "\"";
            }
            XmlNodeList nodeList = xDoc.SelectNodes("/cXML/Request/PunchOutSetupRequest");
            XmlNodeList PostURL = xDoc.SelectNodes("/cXML/Request/PunchOutSetupRequest/BrowserFormPost");
            if (PostURL.Count > 0)
                url = PostURL[0].InnerText;
            if (nodeList.Count > 0)
            {
                cookie = nodeList[0].ChildNodes[0].InnerText;
                UserEmail = nodeList[0].ChildNodes[4].InnerText;
                ID = InsertIntoCoupaPunchOutDetails(url, UserEmail, cookie, pyid);
            }
            //Check for Buyer Cooki
            if (string.IsNullOrEmpty(cookie))
                xml = "<Response><Status code=\"400\" text=\"unsuccess\"></Status></Response>";
            else
            {
                //Create string for response
                xml = "<cXML xml:lang=\"en-US\" payloadID=" + pyid + " timestamp=" + tymstp + @" >" +
                   "<Response><Status code=\"200\" text=\"success\"></Status><PunchOutSetupResponse>" +
                    //Live Server
                    // "<StartPage><URL>https://www.world-link.us.com/login.aspx?key=cp&amp;ID=" + ID.ToString() + @"</URL></StartPage></PunchOutSetupResponse></Response></cXML>";
                    // Test Server 
                "<StartPage><URL>http://216.157.31.116:82/login.aspx?key=cp&amp;ID=" + ID.ToString() + @"</URL></StartPage></PunchOutSetupResponse></Response></cXML>";
                //"<StartPage><URL>http://localhost:2166/Incentex.Web/login.aspx?key=cp&amp;ID=" + ID.ToString() + @"</URL></StartPage></PunchOutSetupResponse></Response></cXML>";
            }
            //Load string in xml doc and return
            xmlDoc.LoadXml(xml);
            return xmlDoc;
        }
        catch (Exception ex)
        {

            ErrHandler.WriteError(ex, false);

            xml = "<Response><Status code=\"400\" text=\"unsuccess\">Internal Exception</Status></Response>";
            xmlDoc.LoadXml(xml);
            return xmlDoc;

        }
    }

    /// <summary>
    /// Insert Details of Coupa from WebService WLLogin
    /// </summary>
    /// <param name="UrlString"></param>
    /// <param name="userEmail"></param>
    /// <returns>Max Coupa ID</returns>
    public Int64 InsertIntoCoupaPunchOutDetails(String UrlString, String userEmail, String cookie, String payID)
    {
        try
        {
            UserInformationRepository objUserRepo = new UserInformationRepository();
            CoupaPunchOutDetail objCPO = new CoupaPunchOutDetail();
            objCPO.PunchOutURL = UrlString;
            objCPO.CreatedDate = DateTime.Now;
            objCPO.UserEmail = userEmail;
            objCPO.BuyerCookie = cookie;
            objCPO.PayLoadID = payID;
            objUserRepo.Insert(objCPO);
            objUserRepo.SubmitChanges();
            return objCPO.PunchOutID;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }
}

