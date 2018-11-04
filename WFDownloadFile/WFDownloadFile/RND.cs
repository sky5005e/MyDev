using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Net;

namespace WFDownloadFile
{
    public class ChiropractorCls
    {
        public String Name { get; set; }
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String City { get; set; }
        public String ZipCode { get; set; }
        public String State { get; set; }
        public String ContactNo { get; set; }

    }
    public class RND
    {
        //public string ReplaceParagraph(HtmlDocument doc)
        //{
        //    doc.DocumentNode.SelectNodes("p")
        //        .ToList()
        //        .ForEach(pNode => pNode.InnerHtml = HtmlNode.CreateNode(pNode.InnerText + "\r").InnerHtml);
        //    return doc.DocumentNode.SelectNodes("//text()")
        //        .Aggregate("", (current, node) => current + (" " + node.InnerText)).TrimStart();
        //}

        //label3.Text = "Tr@171748";
            //webBrowser1.Url = new Uri("https://dealers.asiacell.net");
            //https://dealers.asiacell.net/ActivationReport.aspx;
            //DownloadHTMLData("http://localhost:51206/SendEmail/Upload%20Batch.htm");
         
        //DownloadHTMLData("https://dealers.asiacell.net/ActivationReport.aspx");

         //swWriterL = new StreamWriter(System.AppDomain.CurrentDomain.BaseDirectory + "log.log", true); // Inizializate log
         //   swWriterL.WriteLine("---------------------------------");
         //   swWriterL.WriteLine("Start working session " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
         //   //webBrowser1.Navigate("http://localhost:51206/SendEmail/DefaultDemo.aspx");
         //   // webBrowser1.Url = new Uri("http://localhost:51206/SendEmail/DefaultDemo.aspx");

        // http://localhost:51206/SendEmail/folderfiles/report_dealerinfo.csv
        //if (webBrowser1.Document.GetElementById("ContentPlaceHolder1_btSendtoExcel").GetAttribute("value").Contains("Export"))
        // {
        //webbrowser1.documentcompleted += new webbrowserdocumentcompletedeventhandler(webbrowser1_documentcompleted);
        //canceldialog = true;

        // webBrowser1.Document.GetElementById("ContentPlaceHolder1_btSendtoExcel").InvokeMember("click");


        //dwnld();
        //webBrowser1.Url =  new Uri("http://localhost:51206/SendEmail/folderfiles/report_dealerinfo.csv");

        //IntPtr iptrHWndControl = GetDlgItem(FileDialogHandle, 1148);
        //HandleRef hrefHWndTarget = new HandleRef(null, iptrHWndControl);
        //SendMessage(hrefHWndTarget, WM_SETTEXT, IntPtr.Zero, "your file path");

        //HtmlDocument doc = webBrowser1.Document;
        //HtmlElement head = doc.GetElementsByTagName("head")[0];
        //HtmlElement s = doc.CreateElement("script");
        //s.SetAttribute("text", "alert('hello');");
        //head.AppendChild(s);

        // break;

        // return true;

//        HtmlElement head = webBrowser1.Document.GetElementsByTagName("head")[0];
//HtmlElement scriptEl = webBrowser1.Document.CreateElement("script");
//IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;
//element.text = "function sayHello() { alert('hello') }";
//head.AppendChild(scriptEl);
//webBrowser1.Document.InvokeScript("sayHello");

        //}
        StreamWriter swWriterL;

        string sWebPath = "http://localhost:51206/SendEmail/"; //change this path with WebApplicationTest's virtual path
        string sLocalPath = @"c:\"; //change this path, physical path of a file downloading  (write permission)
        string sMyName = "erika";
        string sPageLogin = "login.aspx";
        string sPageDw = "DefaultDemo.aspx";
        //barq cell
        //Tr@171748
        //dealers.asiacell.net

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref uint pcchCookieData, int dwFlags, IntPtr lpReserved);

        const int INTERNET_COOKIE_HTTPONLY = 0x00002000;

        private string GetGlobalCookies(string uri)
        {
            uint uiDataSize = 2048;
            StringBuilder sbCookieData = new StringBuilder((int)uiDataSize);
            if (InternetGetCookieEx(uri, null, sbCookieData, ref uiDataSize, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero) && sbCookieData.Length > 0)
            {
                return sbCookieData.ToString().Replace(";", ",");
            }
            else
            {
                return null;
            }
        }

        //private void InjectAlertBlocker()
        //{
        //    HtmlElement head = webBrowser1.Document.GetElementsByTagName("head")[0];
        //    HtmlElement scriptEl = webBrowser1.Document.CreateElement("script");
        //    IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;
        //    string alertBlocker = "window.alert = function () { }";
        //    element.text = alertBlocker;
        //    head.AppendChild(scriptEl);
        //}

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        private const int WM_SETTEXT = 0x000C;

        IntPtr FileDialogHandle = FindWindow("#32770", "Choose File To Upload");
        /*
        private void dwnld()
        {
            string filepath = @"C:\Users\Sky\Desktop\ProFinal\FTP\test.csv";
            WebClient webClient = new WebClient();

            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri(""), filepath);
        }

        /// <summary>
        /// Fires before navigation occurs in the given object (on either a window or frameset element).
        /// </summary>
        /// <param name="pDisp">Object that evaluates to the top level or frame WebBrowser object corresponding to the navigation.</param>
        /// <param name="url">String expression that evaluates to the URL to which the browser is navigating.</param>
        /// <param name="Flags">Reserved. Set to zero.</param>
        /// <param name="TargetFrameName">String expression that evaluates to the name of the frame in which the resource will be displayed, or Null if no named frame is targeted for the resource.</param>
        /// <param name="PostData">Data to send to the server if the HTTP POST transaction is being used.</param>
        /// <param name="Headers">Value that specifies the additional HTTP headers to send to the server (HTTP URLs only). The headers can specify such things as the action required of the server, the type of data being passed to the server, or a status code.</param>
        /// <param name="Cancel">Boolean value that the container can set to True to cancel the navigation operation, or to False to allow it to proceed.</param>
        private delegate void BeforeNavigate2(object pDisp, ref dynamic url, ref dynamic Flags, ref dynamic TargetFrameName, ref dynamic PostData, ref dynamic Headers, ref bool Cancel);

        /// <summary>
        /// Fires to indicate that a file download is about to occur. If a file download dialog box can be displayed, this event fires prior to the appearance of the dialog box.
        /// </summary>
        /// <param name="bActiveDocument">A Boolean that specifies whether the file is an Active Document.</param>
        /// <param name="bCancel">A Boolean that specifies whether to continue the download process and display the download dialog box.</param>
        private delegate void FileDownload(bool bActiveDocument, ref bool bCancel);

        //protected override void OnLoad(EventArgs e)
        //{
        //    try
        //    {
        //        dynamic d = webBrowser1.ActiveXInstance;
        //        string uri = string.Empty;

        //        d.BeforeNavigate2 += new BeforeNavigate2((object pDisp,
        //            ref dynamic url,
        //            ref dynamic Flags,
        //            ref dynamic TargetFrameName,
        //            ref dynamic PostData,
        //            ref dynamic Headers,
        //            ref bool Cancel) =>
        //            {

        //                uri = url.ToString();
        //                Trace.WriteLine(uri);
        //            });
        //        webBrowser1.Document.GetElementById("btnDownload").InvokeMember("Click");

        //        d.FileDownload += new FileDownload((bool bActiveDocument, ref bool bCancel) =>
        //        {
        //            bool isFile = true;//uri.EndsWith("exe");

        //            if (isFile)
        //            {
        //                bCancel = true;
        //                Trace.Write("Canceled a file download from the DLR.");
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("File downloaded");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            swWriterL.WriteLine("Error timeout application");
            swWriterL.Close();
            timer1.Stop();
            this.Close();
            Application.Exit();

        }
        //extract __VIEWSTATE __EVENTVALIDATION from input type hidden
        private string ExtractInputHidden(string str, string name)
        {
            string sRit = "";
            string sPattern = "(?<=" + name + "\" value=\")(?<val>.*?)(?=\")";
            Match match = Regex.Match(str, sPattern);
            if (match.Success)
            {
                sRit = match.Groups["val"].Value;
                // sRit = HttpUtility.UrlEncodeUnicode(sRit);
            }
            return sRit;
        }


        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.ToString().Contains("Demo.aspx") == true)
            {
                try
                {
                    //first request to extract __VIEWSTATE and __EVENTVALIDATION and cookie use get

                    string sPageData;
                    string PathRemote = sWebPath + sPageDw;
                    string sTmpCookieString = GetGlobalCookies(webBrowser1.Url.AbsoluteUri);
                    HttpWebRequest fstRequest = (HttpWebRequest)WebRequest.Create(PathRemote);
                    fstRequest.Method = "GET";
                    fstRequest.CookieContainer = new System.Net.CookieContainer();
                    fstRequest.CookieContainer.SetCookies(webBrowser1.Document.Url, sTmpCookieString);
                    HttpWebResponse fstResponse = (HttpWebResponse)fstRequest.GetResponse();
                    StreamReader sr = new StreamReader(fstResponse.GetResponseStream());
                    sPageData = sr.ReadToEnd();
                    sr.Close();

                    string sViewState = ExtractInputHidden(sPageData, "__VIEWSTATE");
                    string sEventValidation = this.ExtractInputHidden(sPageData, "__EVENTVALIDATION");

                    //second request post all values and save generated file
                    string sUrl = sWebPath + sPageDw;
                    HttpWebRequest hwrRequest = (HttpWebRequest)WebRequest.Create(sUrl);
                    hwrRequest.Method = "POST";
                    hwrRequest.CookieContainer = new System.Net.CookieContainer();

                    string sPostData = "__EVENTTARGET=&__EVENTARGUMENT=&__VIEWSTATE=" + sViewState + "&__EVENTVALIDATION=" + sEventValidation + "&Name=" + sMyName + "&Button1=Button";

                    byte[] bByteArray = Encoding.UTF8.GetBytes(sPostData);
                    hwrRequest.ContentType = "application/x-www-form-urlencoded";
                    hwrRequest.CookieContainer.SetCookies(webBrowser1.Document.Url, sTmpCookieString);
                    hwrRequest.ContentLength = bByteArray.Length;
                    Stream sDataStream = hwrRequest.GetRequestStream();
                    sDataStream.Write(bByteArray, 0, bByteArray.Length);
                    sDataStream.Close();
                    using (WebResponse response = hwrRequest.GetResponse())
                    {
                        using (sDataStream = response.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(sDataStream);
                            {
                                //get original file name
                                string sHeader = response.Headers["Content-Disposition"];
                                int iFileNameStartIndex = sHeader.IndexOf("filename=") + "filename=".ToString().Length;
                                int iFileNameLength = sHeader.Length - iFileNameStartIndex;
                                string sFileName = sHeader.Substring(iFileNameStartIndex, iFileNameLength);
                                string sResponseFromServer = reader.ReadToEnd();
                                //save the file
                                FileStream fs = File.Open(sLocalPath + sFileName, FileMode.OpenOrCreate, FileAccess.Write);
                                Byte[] info = new System.Text.UTF8Encoding(true).GetBytes(sResponseFromServer);
                                fs.Write(info, 0, info.Length);
                                fs.Close();

                                swWriterL.WriteLine("saved");
                                reader.Close();
                                sDataStream.Close();
                                response.Close();
                            } //end StreamReader
                        } //end dataStream
                    }//end WebResponse
                }//end try
                catch (Exception eccezione)
                {
                    swWriterL.WriteLine("Error: " + eccezione.Message);
                    swWriterL.Close();
                }
                swWriterL.Close();
                this.Close(); //close form
                Application.Exit();  //close app
            }
        }
         * 
         *  private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Download completed!");
        }
        bool canceldialog = false;
        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if(e.Url.ToString().Contains("ActivationReport.aspx"))
            {
                string pagingnumber = String.Empty;
                var divs = webBrowser1.Document.GetElementsByTagName("div");
                foreach (HtmlElement link in divs)
                {
                    
                }
                foreach (HtmlElement link in divs)
                {
                    if (link.GetAttribute("class") == "rgWrap")
                    {
                        //do something
                        pagingnumber += pagingnumber + link.Children.Count;
                    }
                }
                webBrowser1.Document.GetElementsByTagName("div");
                label1.Text = pagingnumber;
            }
        }

         */
    }
}
