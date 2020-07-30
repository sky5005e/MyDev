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
using System.Globalization;

public partial class SmartCustomErrorPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Get exception details
       
        Exception ex = HttpContext.Current.Server.GetLastError();

        if (ex is HttpUnhandledException && ex.InnerException != null)
        {
            ex = ex.InnerException;
          
        }

        if (ex == null)
        {
            ErrorDetails.Text = "Holy exceptions! There seems to be no error!";
        }
        else
        {
            //Determine what type of error we're dealing with
            if (ex is System.Data.SqlClient.SqlException)
            {
                //A database-related exception...
                ErrorDetails.Text = "The database is having problems... let's hope your data has not been irrevocably lost!";
              
            }
            else if (ex is ApplicationException)
            {
                //An ApplicationException has occurred
                ErrorDetails.Text = string.Concat("You did something to upset the app: ", ex.Message);
               
            }
            else
            {
                //For all others, display the StackTrace
                ErrorDetails.Text = string.Format("An unknown error occurred. I hope the following dump of exception information doesn't frighten you too much...<br /><br /><b>Exception:</b> {0}<br /><br /><b>Message:</b> {1}<br /><br /><b>Stack Trace:</b><br />{2}", ex.GetType().ToString(), ex.Message, ex.StackTrace.Replace(System.Environment.NewLine, "<br />"));

            }
        }

    }

   
}
