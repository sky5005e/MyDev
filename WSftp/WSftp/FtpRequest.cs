using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;

namespace WSftp
{
    class FtpRequest
    {
        private const int BUF_SIZE = 10240;
        private const string PASSWORD = "MwE7YUdt";
        private const string USERNAME = "adeeva";
        private const string SERVER = "ftp.catalystbiz.com";
        private string path;
        private String remoteDir = @"for_adeeva";

        public FtpRequest()
        {
            Cancel = false;
            path = @"C:\Users\Sky\Desktop\ProFinal\FTP";//Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        }

        public bool Cancel { get; set; }

        //public bool Complete { get; set; }

        //public Thread Thread1 { get; set; }

        //public int Timeout { get; set; }

        //public int ReadWriteTimeout { get; set; }







        private bool UploadFile(FileInfo fileInfo)
        {
            FtpWebRequest request = null;
            try
            {
                string ftpPath = "ftp://www.tt.com/" + fileInfo.Name;
                request = (FtpWebRequest)WebRequest.Create(ftpPath);
                request.Credentials = new NetworkCredential("ftptest", "ftptest");  // username , password
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.KeepAlive = false;
                request.Timeout = 60000; // 1 minute time out
                request.ServicePoint.ConnectionLimit = 15; // Connection limit 15 with the ftp., By default 2, -1 means infinite.

                byte[] buffer = new byte[1024];
                using (FileStream fs = new FileStream(fileInfo.FullName, FileMode.Open))
                {
                    int dataLength = (int)fs.Length;
                    int bytesRead = 0;
                    int bytesDownloaded = 0;
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        while (bytesRead < dataLength)
                        {
                            bytesDownloaded = fs.Read(buffer, 0, buffer.Length);
                            bytesRead = bytesRead + bytesDownloaded;
                            requestStream.Write(buffer, 0, bytesDownloaded);
                        }
                        requestStream.Close();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;                
            }
            finally
            {
                request = null;
            }
            return false;
        }// UploadFile



        public bool IsDirectory(string directory)
        {
            if (directory == null)
            {
                throw new ArgumentNullException(); // or however you want to handle null values
            }

            // GetExtension(string) returns string.Empty when no extension found
            return System.IO.Path.GetExtension(directory) == string.Empty;
        }

        public string[] GetFileList()
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + SERVER + "/" + remoteDir + "/"));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(USERNAME, PASSWORD);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                reqFTP.Proxy = null;
                reqFTP.KeepAlive = false;
                reqFTP.UsePassive = false;
                response = reqFTP.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Contains("."))
                    {
                        result.Append(line);
                        result.Append("\n");
                    }
                        line = reader.ReadLine();
                    
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (response != null)
                {
                    response.Close();
                }                
                downloadFiles = null;
                return downloadFiles;
            }
        }

        public  bool MakeNewDirectory(String url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.MakeDirectory;
            request.Credentials = new NetworkCredential(USERNAME, PASSWORD);
            using (var resp = (FtpWebResponse)request.GetResponse())
            {
                return true;
            }
        }

        public bool FtpDirectoryExists(string directoryPath)
        {
            bool IsExists = true;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(directoryPath);
                request.Credentials = new NetworkCredential(USERNAME, PASSWORD);
                request.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                IsExists = false;
            }
            return IsExists;
        }

        public string SendFile(string filename, byte[] datatosend)
        {
            string subPath = DateTime.Now.ToString("yyyy-MM-dd");

            string uri = "ftp://" + SERVER + "/" + remoteDir;
            bool isExists = FtpDirectoryExists(uri);

            if (!isExists)
                MakeNewDirectory(uri + "/" + subPath);

            if (uri.Substring(uri.Length - 1) != "/")
            {
                uri += "/";
            }
            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(uri + subPath + filename);
            ftp.Method = WebRequestMethods.Ftp.UploadFile;
            ftp.Credentials = new NetworkCredential(USERNAME, PASSWORD);
            ftp.UsePassive = true;
            ftp.ContentLength = datatosend.Length;
            Stream requestStream = ftp.GetRequestStream();
            requestStream.Write(datatosend, 0, datatosend.Length);
            requestStream.Close();

            FtpWebResponse ftpresponse = (FtpWebResponse)ftp.GetResponse();

            return ftpresponse.StatusDescription;
        }
        public string DeleteFile(string filename)
        {
            string ftpuri = "ftp://" + SERVER + "/" + remoteDir + "/";
            if (ftpuri.Substring(ftpuri.Length - 1) != "/")
            {
                ftpuri += "/";
            }
            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpuri + filename);
            ftp.Method = WebRequestMethods.Ftp.DeleteFile;
            ftp.Credentials = new NetworkCredential(USERNAME, PASSWORD);
            ftp.UsePassive = true;

            FtpWebResponse response = (FtpWebResponse)ftp.GetResponse();

            Stream responseStream = response.GetResponseStream();

            StreamReader reader = new StreamReader(responseStream);

            return reader.ReadToEnd();
        }
        public string MoveFile(string filename)
        {
            string retval = string.Empty;
            byte[] filecontents = GetFile(filename);
            retval += SendFile(filename, filecontents);
            retval += DeleteFile(filename);
            return retval;
        }

        public void MoveFileMethod(string movefilename)
        {
            try
            {
                string subPath = DateTime.Now.ToString("yyyy-MM-dd");

                string uri = "ftp://" + SERVER + "/" + remoteDir;
                bool isExists = FtpDirectoryExists(uri);
                
                if (!isExists)
                    MakeNewDirectory(uri + "/" + subPath);

                NetworkCredential User = new NetworkCredential(USERNAME, PASSWORD);
                FtpWebRequest Wr = (FtpWebRequest)FtpWebRequest.Create(uri + "/" + movefilename);
                Wr.UseBinary = true;
                Wr.Method = WebRequestMethods.Ftp.Rename;
                Wr.Credentials = User;
                Wr.RenameTo = movefilename;
                Wr.GetResponse().Close();
                // MessageBox.Show("Move File Successfully");
            }
           catch (Exception ex)
            {

            }

        }
        public byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
        public byte[] GetFile(string filename)
        {
            byte[] bytes = null;
            try
            {
                string uri = "ftp://" + SERVER + "/" + remoteDir + "/" + filename;
                Uri serverUri = new Uri(uri);
                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    bytes = null;
                    return bytes;
                }
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + SERVER + "/" + remoteDir + "/" + filename));
                reqFTP.Credentials = new NetworkCredential(USERNAME, PASSWORD);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;
                reqFTP.UsePassive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream responseStream = response.GetResponseStream();
                bytes =  ReadFully(responseStream);

                //FileStream writeStream = new FileStream(path + "/" + file, FileMode.Create);
                //int Length = 2048;
                //Byte[] buffer = new Byte[Length];
                //int bytesRead = responseStream.Read(buffer, 0, Length);
                //while (bytesRead > 0)
                //{
                //    writeStream.Write(buffer, 0, bytesRead);
                //    bytesRead = responseStream.Read(buffer, 0, Length);
                //}
                //writeStream.Close();
                //response.Close();
                return bytes;
            }
            catch (WebException wEx)
            {
                Console.WriteLine(wEx.Message, "Download Error");
                return bytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Download Error");
                return bytes;
            }
        }

        public void Download(string file)
        {                       
            try
            {                
                string uri = "ftp://" + SERVER + "/" + remoteDir + "/" + file;
                Uri serverUri = new Uri(uri);
                if (serverUri.Scheme != Uri.UriSchemeFtp)
                {
                    return;
                }       
                FtpWebRequest reqFTP;                
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + SERVER + "/" + remoteDir + "/" + file));                                
                reqFTP.Credentials = new NetworkCredential(USERNAME, PASSWORD);                
                reqFTP.KeepAlive = false;                
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;                                
                reqFTP.UseBinary = true;
                reqFTP.Proxy = null;                 
                reqFTP.UsePassive = false;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream responseStream = response.GetResponseStream();
                FileStream writeStream = new FileStream(path + "/" + file, FileMode.Create);                
                int Length = 2048;
                Byte[] buffer = new Byte[Length];
                int bytesRead = responseStream.Read(buffer, 0, Length);               
                while (bytesRead > 0)
                {
                    writeStream.Write(buffer, 0, bytesRead);
                    bytesRead = responseStream.Read(buffer, 0, Length);
                }                
                writeStream.Close();
                response.Close(); 
            }
            catch (WebException wEx)
            {
                Console.WriteLine(wEx.Message, "Download Error");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, "Download Error");
            }
        }


        //public void StartFtpDownload(string download, string file)
        //{
        //    string objString = string.Format("{0};{1}", download, file);
        //    Thread1 = new Thread(startFtpThread);
        //    Thread1.Name = string.Format("{0} download", file);
        //    Thread1.IsBackground = true;
        //    Thread1.Start(objString);
        //}

        //public  void Main()
        //{
        //    // Get the object used to communicate with the server.
        //    FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://ftp.catalystbiz.com/");
        //    request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

        //    // This example assumes the FTP site uses anonymous logon.
        //    request.Credentials = new NetworkCredential(USERNAME, PASSWORD);

        //    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        //    Stream responseStream = response.GetResponseStream();
        //    StreamReader reader = new StreamReader(responseStream);
        //    Console.WriteLine(reader.ReadToEnd());

        //    Console.WriteLine("Directory List Complete, status {0}", response.StatusDescription);

        //    reader.Close();
        //    response.Close();
        //}

        //public static bool AppendFileOnServer(string fileName, Uri serverUri)
        //{
        //    // The URI described by serverUri should use the ftp:// scheme.
        //    // It contains the name of the file on the server.
        //    // Example: ftp://contoso.com/someFile.txt.
        //    // The fileName parameter identifies the file containing
        //    // the data to be appended to the file on the server.

        //    if (serverUri.Scheme != Uri.UriSchemeFtp)
        //    {
        //        return false;
        //    }
        //    // Get the object used to communicate with the server.
        //    FtpWebRequest request = (FtpWebRequest)WebRequest.Create(serverUri);
        //    request.Method = WebRequestMethods.Ftp.AppendFile;

        //    StreamReader sourceStream = new StreamReader(fileName);
        //    byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
        //    sourceStream.Close();
        //    request.ContentLength = fileContents.Length;

        //    // This example assumes the FTP site uses anonymous logon.
        //    request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");
        //    Stream requestStream = request.GetRequestStream();
        //    requestStream.Write(fileContents, 0, fileContents.Length);
        //    requestStream.Close();
        //    FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        //    Console.WriteLine("Append status: {0}", response.StatusDescription);

        //    response.Close();
        //    return true;
        //}

        //private void startFtpThread(object obj)
        //{
        //    Complete = false;
        //    string objString = obj.ToString();
        //    string[] split = objString.Split(';');
        //    string download = split[0];
        //    string file = split[1];
        //    do
        //    {
        //        try
        //        {
        //            string uri = String.Format("ftp://{0}/{1}/{2}", SERVER, download, file);
        //            Uri serverUri = new Uri(uri);
        //            if (serverUri.Scheme != Uri.UriSchemeFtp)
        //            {
        //                Cancel = true;
        //                return;
        //            }
        //            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
        //            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

        //            FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
        //            reqFTP.Credentials = new NetworkCredential(USERNAME, PASSWORD);
        //            reqFTP.KeepAlive = true;
        //            reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
        //            reqFTP.EnableSsl = false;
        //            reqFTP.Proxy = null;
        //            reqFTP.UsePassive = true;
        //            reqFTP.Timeout = Timeout;
        //            reqFTP.ReadWriteTimeout = ReadWriteTimeout;
        //            using (FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse())
        //            {
        //                using (Stream responseStream = response.GetResponseStream())
        //                {
        //                    using (FileStream writeStream = new FileStream(path + file, FileMode.Create))
        //                    {
        //                        Byte[] buffer = new Byte[BUF_SIZE];
        //                        int bytesRead = responseStream.Read(buffer, 0, BUF_SIZE);
        //                        while (0 < bytesRead)
        //                        {
        //                            writeStream.Write(buffer, 0, bytesRead);
        //                            bytesRead = responseStream.Read(buffer, 0, BUF_SIZE);
        //                        }
        //                    }
        //                    responseStream.Close();
        //                }
        //                response.Close();
        //                Complete = true;
        //            }
        //        }
        //        catch (WebException wEx)
        //        {
        //            //LogDatabase.WriteLog("Download File", wEx.ToString(), "Download File");
        //        }
        //    } while (!Cancel && !Complete);
        //}
    }
}
