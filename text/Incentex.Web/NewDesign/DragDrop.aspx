<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DragDrop.aspx.cs" Inherits="NewDesign_DragDrop" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="StaticContents/css/DragDropStyle.css" rel="stylesheet" type="text/css" />
   
    <script src="../JS/JQuery/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
    $(document).ready( function()
    {
    
    
    function GetParent(ParValue) {
       
    parent.Success(ParValue);
    }
    
    
    //$("#AjaxFileUpload1_Html5DropZone").html('Drag or paste you file here. Or <span style="color:red">browse</span> for a file to upload');
    $('.removeButton').click( function () {
    
                $('#AjaxFileUpload1_Footer').hide();
                $('.filename').hide();
                $('.filetype').hide();
                $('.uploadedState').hide();
                $('#AjaxFileUpload1_QueueContainer').hide();
    });
    
   
    });
    function Success(sender, args) {

    $('#AjaxFileUpload1_Footer').hide();
    //alert($('.filetype').html());
    var filetype=$('.filetype').html();
    var filename=$('.filename').html();
    
    $('.filename').hide();
    $('.filetype').hide();
    $('.uploadedState').hide();
    $('#AjaxFileUpload1_QueueContainer').hide();
    $('.ajax__fileupload_fileItemInfo').remove();
    parent.Success(filetype,filename);
   
    }
    
    function Error()
    {
    alert('Not Able To Upload');
    }
    
    
    
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" />
    <div style="border:solid 0px red;height:120px;overflow:hidden;">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:AjaxFileUpload ID="AjaxFileUpload1" runat="server" ThrobberID="loader" Width="100%"
                    MaximumNumberOfFiles="1" OnUploadComplete="UploadComplete" OnClientUploadComplete="Success"
                    ContextKeys="fred"/>
                <asp:Image ID="loader" runat="server" ImageUrl="~/Images/ajax-loader.gif" Style="display: None" />
                <asp:HiddenField ID="hfIsValid" runat="server" Value="0" />
            </ContentTemplate>
        </asp:UpdatePanel>
         </div>
    </form>
</body>
</html>
