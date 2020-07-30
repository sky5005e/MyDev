using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

/// <summary>
/// Summary description for POP3Class
/// </summary>
public class POP3Client :IDisposable
{
    public POP3Client()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public string Host { get; protected set; }
    public int Port { get; protected set; }
    public string Email { get; protected set; }
    public string Password { get; protected set; }
    public bool IsSecure { get; protected set; }
    public TcpClient Client { get; protected set; }
    public Stream ClientStream { get; protected set; }
    public StreamWriter Writer { get; protected set; }
    public StreamReader Reader { get; protected set; }
    private bool disposed = false;
    public POP3Client(string host, int port, string email, string password) : this(host, port, email, password, false) { }
    public POP3Client(string host, int port, string email, string password, bool secure)
    {
        Host = host;
        Port = port;
        Email = email;
        Password = password;
        IsSecure = secure;
    }
    public void Connect()
    {
        if (Client == null)
            Client = new TcpClient();
        if (!Client.Connected)
            Client.Connect(Host, Port);
        if (IsSecure)
        {
            SslStream secureStream = new SslStream(Client.GetStream());
            secureStream.AuthenticateAsClient(Host);
            ClientStream = secureStream;
            secureStream = null;
        }
        else
            ClientStream = Client.GetStream();
        Writer = new StreamWriter(ClientStream);
        Reader = new StreamReader(ClientStream);
        ReadLine();
        Login();
    }
    public int GetEmailCount()
    {
        int count = 0;
        string response = SendCommand("STAT");
        if (IsResponseOk(response))
        {
            string[] arr = response.Substring(4).Split(' ');
            count = Convert.ToInt32(arr[0]);
        }
        else
            count = -1;
        return count;
    }
    public Email FetchEmail(int emailId)
    {
        if (IsResponseOk(SendCommand("TOP " + emailId + " 100")))
            return new Email(ReadLines());
        else
            return null;
    }
    public List<Email> FetchEmailList(int start, int count)
    {
        DateTime dt = Convert.ToDateTime((DateTime.UtcNow.Date.AddDays(-7)));
        List<Email> emails = new List<Email>(count);
        for (int i = start; i <= count; i++)
        {
            Email email = FetchEmail(i);
            if (email != null)
                emails.Add(email);
        }
        //return emails.OrderBy(s=>s.UtcDateTime).ToList();
        return emails.ToList();
    }
    public List<MessagePart> FetchMessageParts(int emailId)
    {
        if (IsResponseOk(SendCommand("RETR " + emailId)))
            return Util.ParseMessageParts(ReadLines());

        return null;
    }
    public void Close()
    {
        if (Client != null)
        {
            if (Client.Connected)
                Logout();
            Client.Close();
            Client = null;
        }
        if (ClientStream != null)
        {
            ClientStream.Close();
            ClientStream = null;
        }
        if (Writer != null)
        {
            Writer.Close();
            Writer = null;
        }
        if (Reader != null)
        {
            Reader.Close();
            Reader = null;
        }
        disposed = true;
    }
    public void Dispose()
    {
        if (!disposed)
            Close();
    }
    protected void Login()
    {
        if (!IsResponseOk(SendCommand("USER " + Email)) || !IsResponseOk(SendCommand("PASS " + Password)))
            throw new Exception("User/password not accepted");
    }
    protected void Logout()
    {
        SendCommand("RSET");
    }
    protected string SendCommand(string cmdtext)
    {
        Writer.WriteLine(cmdtext);
        Writer.Flush();
        return ReadLine();
    }
    protected string ReadLine()
    {
        return Reader.ReadLine() + "\r\n";
    }
    protected string ReadLines()
    {
        StringBuilder b = new StringBuilder();
        while (true)
        {
            string temp = ReadLine();
            if (temp == ".\r\n" || temp.IndexOf("-ERR") != -1)
                break;
            b.Append(temp);
        }
        return b.ToString();
    }
    protected static bool IsResponseOk(string response)
    {
        if (response.StartsWith("+OK"))
            return true;
        if (response.StartsWith("-ERR"))
            return false;
        throw new Exception("Cannot understand server response: " + response);
    }

    public List<Incentex.DAL.SqlRepository.CampignRepo.SearchCampUser> GetCampaignBouncedEmailDetails(Int64 companyID,int page, int NoOfEmailsPerPage)
    {
        List<Incentex.DAL.SqlRepository.CampignRepo.SearchCampUser> ResultList = new List<Incentex.DAL.SqlRepository.CampignRepo.SearchCampUser>();
        List<Email> emails = new List<Email>();
        using (POP3Client client = new POP3Client("pop.gmail.com", 995, "incentexbounce@gmail.com", "worldlink", true))
        {

            client.Connect();
            int totalEmails = client.GetEmailCount();
            int start = 1;
            if(totalEmails > 100)
                start = totalEmails - 50;
            //emails = client.FetchEmailList(((page - 1) * NoOfEmailsPerPage) + 1, NoOfEmailsPerPage).OrderBy(s => s.UtcDateTime).ToList().Where(f => f.From == "\"Delivery Subsystem\" <POSTMASTER@world-link.us.com>").ToList();
            // Here it always shows the latest top 50 bounce emails from inbox of incentexbounce@gmail.com
            emails = client.FetchEmailList(start, totalEmails).OrderBy(s => s.UtcDateTime).ToList().Where(f => f.From == "\"Delivery Subsystem\" <POSTMASTER@world-link.us.com>").ToList();
        }

       Incentex.DAL.SqlRepository.CampignRepo  objRepo = new Incentex.DAL.SqlRepository.CampignRepo();
       List<Incentex.DAL.SqlRepository.CampignRepo.SearchCampUser> list = objRepo.GetBounceDetails(companyID).ToList();

       ResultList = (from e in emails
                     join l in list on e.ToEmailAddress equals l.LoginEmail
                     select l).ToList();


        return ResultList;
    }

}
    public class Email
    {
        public NameValueCollection Headers { get; protected set; }
        public List<MessagePart> Messages { get; protected set; }
        public string ToEmailAddress { get; protected set; }
        public string ContentType { get; protected set; }
        public DateTime UtcDateTime { get; protected set; }
        public string From { get; protected set; }
        public string To { get; protected set; }
        public string Subject { get; protected set; }
        public Email(string emailText)
        {
            Headers = Util.ParseHeaders(emailText, false);
            ContentType = Headers["Content-Type"];
            From = Headers["From"];
            To = Headers["To"];
            Subject = Headers["Subject"];
            Messages = Util.ParseMessageParts(emailText); ;
            ToEmailAddress = Messages[0].ToAddress;
            if (Headers["Date"] != null)
                try
                {
                    UtcDateTime =Util.ConvertStrToUtcDateTime(Headers["Date"]);
                }
                catch (FormatException)
                {
                    UtcDateTime = DateTime.MinValue;
                }
            else
                UtcDateTime = DateTime.MaxValue;
        }
    }
    public class MessagePart
    {
        public NameValueCollection Headers { get; protected set; }
        public string ContentType { get; protected set; }
        public string MessageText { get; protected set; }
        public string ToAddress { get; protected set; }
        public string UserId { get; protected set; }
        public MessagePart(NameValueCollection headers, string messageText,string toAddress)
        {

            Headers = headers;
            ContentType = Headers["Content-Type"];
            ToAddress = toAddress;
            MessageText = messageText;
        }
    }
    public class Util
    {
        protected static Regex BoundaryRegex = new Regex("Content-Type: multipart(?:/\\S+;)" + "\\s+[^\r\n]*boundary=\"?(?<boundary>" + "[^\"\r\n]+)\"?\r\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        protected static Regex UtcDateTimeRegex = new Regex(@"^(?:\w+,\s+)?(?<day>\d+)\s+(?<month>\w+)\s+(?<year>\d+)\s+(?<hour>\d{1,2})" + @":(?<minute>\d{1,2}):(?<second>\d{1,2})\s+(?<offsetsign>\-|\+)(?<offsethours>" + @"\d{2,2})(?<offsetminutes>\d{2,2})(?:.*)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        public static NameValueCollection ParseHeaders(string headerText, Boolean isMsg)
        {
            NameValueCollection headers = new NameValueCollection();
            StringReader reader = new StringReader(headerText);
            string line;
            string headerName = null, headerValue;
            int colonIndx;
            while ((line = reader.ReadLine()) != null)
            {
                if (line == "" && !isMsg)
                    break;
                if (line != "" && Char.IsLetterOrDigit(line[0]) && (colonIndx = line.IndexOf(':')) != -1)
                {
                    headerName = line.Substring(0, colonIndx);
                    headerValue = line.Substring(colonIndx + 1).Trim();
                    headers.Add(headerName, headerValue);
                }
                else if (headerName != null)
                    headers[headerName] += " " + line.Trim();
                else
                    throw new FormatException("Could not parse headers");
            }
            return headers;
        }
        public static List<MessagePart> ParseMessageParts(string emailText)
        {
            List<MessagePart> messageParts = new List<MessagePart>();
            int newLinesIndx = emailText.IndexOf("\r\n\r\n");
            Match m = BoundaryRegex.Match(emailText);
            if (m.Index < emailText.IndexOf("\r\n\r\n") && m.Success)
            {
                string boundary = m.Groups["boundary"].Value;
                string startingBoundary = "\r\n--" + boundary;
                int startingBoundaryIndx = -1;
                while (true)
                {
                    if (startingBoundaryIndx == -1)
                        startingBoundaryIndx = emailText.IndexOf(startingBoundary);
                    if (startingBoundaryIndx != -1)
                    {
                        int nextBoundaryIndx = emailText.IndexOf(startingBoundary, startingBoundaryIndx + startingBoundary.Length);
                        if (nextBoundaryIndx != -1 && nextBoundaryIndx != startingBoundaryIndx)
                        {
                            string multipartMsg = emailText.Substring(startingBoundaryIndx + startingBoundary.Length, (nextBoundaryIndx - startingBoundaryIndx - startingBoundary.Length));
                            int headersIndx = multipartMsg.IndexOf("\r\n\r\n");
                            if (headersIndx == -1)
                                throw new FormatException("Incompatible multipart message format");
                            string bodyText = multipartMsg.Substring(headersIndx).Trim();
                            NameValueCollection headers = Util.ParseHeaders(multipartMsg.Trim(), true);
                            messageParts.Add(new MessagePart(headers, bodyText, headers["To"]));
                        }
                        else
                            break;
                        startingBoundaryIndx = nextBoundaryIndx;
                    }
                    else
                        break;
                }
                if (newLinesIndx != -1)
                {
                    string emailBodyText = emailText.Substring(newLinesIndx + 1);
                }
            }
            else
            {
                int headersIndx = emailText.IndexOf("\r\n\r\n");
                if (headersIndx == -1)
                    throw new FormatException("Incompatible multipart message format");
                string bodyText = emailText.Substring(headersIndx).Trim();
                NameValueCollection headers = Util.ParseHeaders(bodyText, true);
                // To get User Id if plus addressing concept is used in to emailaddress
                //if (headers["To"].Contains('+'))
                //{
                //    string[] id = headers["To"].Split('+');
                //    if (id.Count() > 0)
                //    {
                //        String [] uid = id[1].Split('@');
                //        Int64? userid = Convert.ToInt64(uid[0]);
                //    }
                //}
                messageParts.Add(new MessagePart(headers, bodyText, headers["To"]));
            }
            return messageParts;
        }
        public static DateTime ConvertStrToUtcDateTime(string str)
        {
            Match m = UtcDateTimeRegex.Match(str);
            int day, month, year, hour, min, sec;
            if (m.Success)
            {
                day = Convert.ToInt32(m.Groups["day"].Value);
                year = Convert.ToInt32(m.Groups["year"].Value);
                hour = Convert.ToInt32(m.Groups["hour"].Value);
                min = Convert.ToInt32(m.Groups["minute"].Value);
                sec = Convert.ToInt32(m.Groups["second"].Value);
                switch (m.Groups["month"].Value)
                {
                    case "Jan":
                        month = 1;
                        break;
                    case "Feb":
                        month = 2;
                        break;
                    case "Mar":
                        month = 3;
                        break;
                    case "Apr":
                        month = 4;
                        break;
                    case "May":
                        month = 5;
                        break;
                    case "Jun":
                        month = 6;
                        break;
                    case "Jul":
                        month = 7;
                        break;
                    case "Aug":
                        month = 8;
                        break;
                    case "Sep":
                        month = 9;
                        break;
                    case "Oct":
                        month = 10;
                        break;
                    case "Nov":
                        month = 11;
                        break;
                    case "Dec":
                        month = 12;
                        break;
                    default:
                        throw new FormatException("Unknown month.");
                }
                string offsetSign = m.Groups["offsetsign"].Value;
                int offsetHours = Convert.ToInt32(m.Groups["offsethours"].Value);
                int offsetMinutes = Convert.ToInt32(m.Groups["offsetminutes"].Value);
                DateTime dt = new DateTime(year, month, day, hour, min, sec);
                if (offsetSign == "+")
                {
                    dt.AddHours(offsetHours);
                    dt.AddMinutes(offsetMinutes);
                }
                else if (offsetSign == "-")
                {
                    dt.AddHours(-offsetHours);
                    dt.AddMinutes(-offsetMinutes);
                }
                return dt;
            }
            throw new FormatException("Incompatible date/time string format");
        }



    }



