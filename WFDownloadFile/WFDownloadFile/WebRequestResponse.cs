using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Itstep.Jobmachine.Crawler.Core
{
    public class WebRequestResponse
    {
        public static string DoRequest(Uri uri, NameValueCollection toPostNameValues)
        {
            using (WebClient client = new WebClient())
            {
                //System.Collections.Specialized.NameValueCollection reqparm = new System.Collections.Specialized.NameValueCollection();
                //reqparm.Add("jobspage", "5");
                //reqparm.Add("3.9", "WONoSelectionString");
                byte[] responsebytes = client.UploadValues(uri, "POST", toPostNameValues);
                string responsebody = Encoding.UTF8.GetString(responsebytes);
                return responsebody;
            }
        }

        public static string PostMessageToURL(string url, string parameters)
        {
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(url, "POST", parameters);
                return HtmlResult;
            }
        }

        public static string DoRequest(Uri uri)
        {

            //// Create a request for the URL. 		
            //WebRequest request = WebRequest.Create(uri);
            ////InitTor();
            ////request.Proxy = new WebProxy("127.0.0.1:8118");
            ////request.KeepAlive = false;

            //// If required by the server, set the credentials.
            //request.Credentials = CredentialCache.DefaultCredentials;
            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //using (Stream dataStream = response.GetResponseStream())
            //using (StreamReader reader = new StreamReader(dataStream))
            //{
            //    string responseFromServer = reader.ReadToEnd();
            //    return responseFromServer;
            //}

            // http://social.msdn.microsoft.com/Forums/vstudio/en-US/ef333419-1573-4ed1-8c4b-1993ddd8e0da/webclient-webexception-too-many-automatic-redirections-were-attempted?forum=netfxbcl
            string html = string.Empty;
            using (WebClient client = new WebClient())
            {
                //Uri innUri = null;
                //Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out innUri);

                try
                {
                    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    using (StreamReader str = new StreamReader(client.OpenRead(uri)))
                    {
                        html = str.ReadToEnd();
                    }
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                return html;
            }
        }

        public static string DoRequestWithCookies(Uri uri, NameValueCollection cookies)
        {
            #region old cookie sample
            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(@"http://www.zurich.com/careers/jobsearchresults.htm?resultsperpage=2147483647&sort=Title&sortdir=Asc");
            //request.CookieContainer = new CookieContainer();
            //request.CookieContainer.Add(new Uri("http://www.zurich.com"),
            //    new Cookie("JobSearchCriteria", "location=Switzerland|activityarea=ALL|positiontype=ALL|positionlevel=ALL|keyword="));

            //HttpWebResponse response = (HttpWebResponse)request.GetResponse();           
            ////int cookieCount = cookieJar.Count;
            //StreamReader sr = new StreamReader(response.GetResponseStream());
            //string result = sr.ReadToEnd();
            //sr.Close();
            //Console.WriteLine(result);
            #endregion

            //// Create a request for the URL. 		
            //WebRequest request = WebRequest.Create(uri);
            ////InitTor();
            ////request.Proxy = new WebProxy("127.0.0.1:8118");
            ////request.KeepAlive = false;

            //// If required by the server, set the credentials.
            //request.Credentials = CredentialCache.DefaultCredentials;
            //using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            //using (Stream dataStream = response.GetResponseStream())
            //using (StreamReader reader = new StreamReader(dataStream))
            //{
            //    string responseFromServer = reader.ReadToEnd();
            //    return responseFromServer;
            //}

            // http://social.msdn.microsoft.com/Forums/vstudio/en-US/ef333419-1573-4ed1-8c4b-1993ddd8e0da/webclient-webexception-too-many-automatic-redirections-were-attempted?forum=netfxbcl
            string html = string.Empty;
            using (WebClient client = new WebClient())
            {
                if (cookies != null)
                {
                    foreach (string key in cookies)
                    {
                        client.Headers.Add(HttpRequestHeader.Cookie, string.Format("{0}={1}", key, cookies[key]));
                        //Console.WriteLine("{0} {1}", key, cookies[key]);
                    }
                }

                try
                {
                    client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    using (StreamReader str = new StreamReader(client.OpenRead(uri)))
                    {
                        html = str.ReadToEnd();
                    }
                }
                catch (WebException ex)
                {
                    throw ex;
                }
                return html;
            }
        }

        public static string DoJsonRequest(Uri uri, string requestJson)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Accept = "application/json, text/javascript, */*; q=0.01";
            httpWebRequest.Method = "POST";


            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(requestJson);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                return result;
            }
        }

        public static string XmlHttpRequest(string urlString, string xmlContent)
        {
            string response = null;
            HttpWebRequest httpWebRequest = null;//Declare an HTTP-specific implementation of the WebRequest class.
            HttpWebResponse httpWebResponse = null;//Declare an HTTP-specific implementation of the WebResponse class

            //Creates an HttpWebRequest for the specified URL.
            httpWebRequest = (HttpWebRequest)WebRequest.Create(urlString);

            try
            {
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(xmlContent);
                //Set HttpWebRequest properties
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = bytes.Length;
                httpWebRequest.ContentType = "text/xml; encoding='utf-8'";

                using (Stream requestStream = httpWebRequest.GetRequestStream())
                {
                    //Writes a sequence of bytes to the current stream 
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();//Close stream
                }

                //Sends the HttpWebRequest, and waits for a response.
                httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    //Get response stream into StreamReader
                    using (Stream responseStream = httpWebResponse.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                            response = reader.ReadToEnd();
                    }
                }
                httpWebResponse.Close();//Close HttpWebResponse
            }
            catch (WebException we)
            {   //TODO: Add custom exception handling
                throw new Exception(we.Message);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            finally
            {
                httpWebResponse.Close();
                //Release objects
                httpWebResponse = null;
                httpWebRequest = null;
            }
            return response;

        }
    }
}
