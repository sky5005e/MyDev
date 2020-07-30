using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
/// Summary description for UploadFile
/// </summary>
[Serializable()]
public class UploadFile
{
    public UploadFile()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public long AttachmentID { get; set; }
    public string OnlyFileName { get; set; }
    public string SavedFileName { get; set; }
}

[Serializable()]
public class UploadImage
{
    public UploadImage()
    {
        _ID = Guid.NewGuid();
    }
    private Guid _ID;
    public string imageName { get; set; }
    public string imageOnly { get; set; }
}
