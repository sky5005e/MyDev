<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CommonHead.ascx.cs" Inherits="UserControls_CommonHead" %>
<link id="Link1" href='<%#ResolveClientUrl("~/CSS/style.css")%>' runat="server" rel="stylesheet"
    type="text/css" media="screen" />
<link id="Link3" href='<%#ResolveClientUrl("~/CSS/jquery-ui.css")%>' runat="server"
    rel="stylesheet" type="text/css" media="screen" />
<link id="Link2" href='<%#ResolveClientUrl("~/CSS/prettyPhoto.css")%>' runat="server"
    rel="stylesheet" type="text/css" media="screen" />
<link id="Link4" href='<%#ResolveClientUrl("~/CSS/simplemodal.css")%>' runat="server"
    rel="stylesheet" type="text/css" media="screen" />
<link id="Link5" href='<%#ResolveClientUrl("~/CSS/dd.css")%>' runat="server" rel="stylesheet"
    type="text/css" media="screen" />
<link id="Link7" href='<%#ResolveClientUrl("~/CSS/jquery.jgd.dropdown.css")%>' runat="server"
    rel="stylesheet" type="text/css" media="screen" />
<link id="Link8" href='<%#ResolveClientUrl("~/CSS/jquery.timepicker-1.1.0.css")%>'
    runat="server" rel="stylesheet" type="text/css" media="screen" />

<link rel="icon" type="image/ico" href='<%#ResolveClientUrl("~/icons/favicon.ico")%>'  />
<link rel="shortcut icon" type="image/x-icon" href='<%#ResolveClientUrl("~/icons/favicon.ico")%>'  />


<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery.js")%>' type="text/javascript"
    language="javascript"></script>

<%--<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery-1.3.2.min.js")%>' type="text/javascript"
    language="javascript"></script>--%>

<script src='<%#ResolveClientUrl("~/FusionCharts/jquery-1.4.1.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/general.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery.jgd.dropdown.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery.validate.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/uploader/base.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/uploader/uploader.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/uploader/upload.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery.simplemodal-1.2.3.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery.ui.draggable.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery.alerts.js")%>' type="text/javascript"
    language="javascript"></script>

<%
    if (Request.AppRelativeCurrentExecutionFilePath.ToString().ToLower() == "~/signup.aspx")
    { %>

<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery.Signup_prettyPhoto.js")%>' type="text/javascript"
    language="javascript"></script>

<%}
    else
    { %>

<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery.prettyPhoto.js")%>' type="text/javascript"
    language="javascript"></script>

<%} %>

<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery.form.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery.fileupload.js")%>' type="text/javascript"
    language="javascript"></script>

<%--Javascripts for the image dropdown--%>

<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery.dd.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery-ui.min.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/image-dropdown.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/validationMessages/validationMessage.js")%>'
    type="text/javascript" language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery.imagerotator.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/diQuery-collapsiblePanel.js")%>' type="text/javascript"
    language="javascript"></script>

<script src='<%#ResolveClientUrl("~/JS/JQuery/jquery.timepicker-1.1.0.js")%>' type="text/javascript"
    language="javascript"></script>

<%--    <script src='<%#ResolveClientUrl("~/JS/JQuery/css_browser_selector.js")%>' type="text/javascript"
    language="javascript"></script>--%>
<!--[if IE 6]>
<script src="js/dd_belatedpng.js" type="text/javascript"></script>
<script>
  DD_belatedPNG.fix('*');
</script>
<![endif]-->
