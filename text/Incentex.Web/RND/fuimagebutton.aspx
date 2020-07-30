<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fuimagebutton.aspx.cs" Inherits="fuimagebutton" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" xml:lang="en" lang="en">
<head>
    <meta http-equiv="content-type" content="text/html; charset=utf-8" />
    <title>Styling File Inputs with CSS and the DOM</title>
    <%--JS File to open file dialog box on image click event--%>
    <script type="text/javascript" src="../fileinput/si.files.js"></script>
    <%--CSS file for the same--%>
    <link href="../fileinput/FileInputs.css" rel="stylesheet" type="text/css" />
  <%--  <style type="text/css" title="text/css">
        /* <![CDATA[ */
        
        /* ]]> */</style>--%>
</head>
<body>
    <form action="#" runat="server">
    <label class="cabinet" id="fileupload" >
        <input type="file" class="file" runat="server" id="test" />
    </label>
    <br />
    <label class="cabinet" id="fileupload1">
        <input type="file" class="file" runat="server" id="test1" />
    </label>
    
    <label class="cabinet" id="fileupload3" >
    <a href="#" class="greyicon_btn btn" title="Program Agreement"><input type="file" class="file" runat="server" id="File1" /></a>
    </label>
      
    
    <asp:Button ID="btntest" runat="server" OnClick="btntest_Click" />
    </form>

    <script type="text/javascript" language="javascript">
        // <![CDATA[

        SI.Files.stylizeAll();
        

        /*
        --------------------------------
        Known to work in:
        --------------------------------
        - IE 5.5+
        - Firefox 1.5+
        - Safari 2+
                          
        --------------------------------
        Known to degrade gracefully in:
        --------------------------------
        - Opera
        - IE 5.01

        --------------------------------
        Optional configuration:

        Change before making method calls.
        --------------------------------
        SI.Files.htmlClass = 'SI-FILES-STYLIZED';
        SI.Files.fileClass = 'file';
        SI.Files.wrapClass = 'cabinet';

        --------------------------------
        Alternate methods:
        --------------------------------
        SI.Files.stylizeById('input-id');
        SI.Files.stylize(HTMLInputNode);

        --------------------------------
        */

        // ]]>
    </script>

</body>
</html>
