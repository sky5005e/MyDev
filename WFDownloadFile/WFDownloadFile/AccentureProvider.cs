using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using System.IO;

namespace WFDownloadFile
{
    public class AccentureProvider
    {
        public void GETWSDL()
        {
            String _url = @"http://careers.accenture.com/ch-de/_vti_bin/Accenture/JobSearchWebService.asmx?op=GetJobs";
//@"http://careers.accenture.com/ch-de/_vti_bin/Accenture/JobSearchWebService.asmx/GetJobs";
            HttpWebRequest hwrequest = (HttpWebRequest)WebRequest.Create(_url);

            HttpWebResponse httpResponse = (HttpWebResponse)hwrequest.GetResponse();


            String Result = string.Empty;
            using (StreamReader rd = new StreamReader(httpResponse.GetResponseStream()))
            {
                Result = rd.ReadToEnd();
            }
            XDocument Value = XDocument.Parse(Result);
            //redirectURL = Convert.ToString(Value.Root.Value);
        
        
        }
        /*
        private static bool completed = false;
        private static WebBrowser wb;
        [STAThread]
        static void Main(string[] args)
        {
            wb = new WebBrowser();
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);
            wb.Navigate("http://www.google.com");
            while (!completed)
            {
                Application.DoEvents();
                Thread.Sleep(100);
            }
            Console.Write("\n\nDone with it!\n\n");
            Console.ReadLine();
        }


        static void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Console.WriteLine(wb.Document.Body.InnerHtml);
            completed = true;
        }*/
    }
}
