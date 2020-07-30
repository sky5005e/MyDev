<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewFlaggedNotes.aspx.cs" Inherits="AssetFrameItems_ViewFlaggedNotes" %>
<%@ Register TagPrefix="uc" TagName="CommonHeader" Src="~/NewDesign/UserControl/NewCommonHeader.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <uc:CommonHeader ID="ucCommonHead" runat="server" />
    <script type="text/javascript">
     $(document).ready(function () {            
            $(window).ValidationUI();
            BindPopUpCloseEvents();
     });
     function RedirectURL(UrlPath)
     {
        window.parent.location = UrlPath;
     }
    </script>
</head>
<body class="NoClass">
    <form id="form1" runat="server">
        <div class="assetspoupup-content cf">
			                
			                <h2>View Note</h2>
			                <div class="message-container">
					                <div id="boxscroll">
						                <div class="viewnote-listbox">
							                <ul class="viewnote-list cf">
							                    <asp:Repeater ID="rpBasicViewNote" runat="server">
							                        <ItemTemplate>
							                            <li>
								                            <h5 class="cf"><span><%# Eval("FirstName") + " " + Eval("LastName")%></span><em><%# Eval("DateNTime") %></em></h5>
								                            <p><%# Eval("Notecontents") %></p>
								                        </li>    
							                        </ItemTemplate>
							                    </asp:Repeater>
							                    <li id="nobasicnotes_li" runat="server" style="text-align:center;">
							                        <p>No data found.</p>
							                    </li>
                                                <li class="text-area">
                                                  <textarea id="txtFlaggedNoteDetails" runat="server" class="input-textarea checkvalidation"></textarea>
                                                  <asp:RequiredFieldValidator ID="rfvFlaggedNoteDetails" runat="server" ControlToValidate="txtFlaggedNoteDetails" SetFocusOnError="True"
                                                  CssClass="error" Display="Dynamic" ErrorMessage="Please enter note" ValidationGroup="flagAssetNote"></asp:RequiredFieldValidator>
                                                </li>
							                </ul>
                                            <div class="notes-btn-block cf">
                                                <a class="cancel-popup small-gray-btn iframeCancel" href="javascript: void(0);"><span>Cancel</span></a> 
                                                <asp:LinkButton ID="lnkbtnAssetsNote" runat="server" CssClass="small-blue-btn submit" OnClick="lnkbtnAssetsNote_Click" ToolTip="Add Note" ValidationGroup="flagAssetNote" call="flagAssetNote" OnClientClick="window.parent.ShowDefaultLoader();"><span>Add Note</span></asp:LinkButton>
                                                <asp:LinkButton ID="lnkbtnAssetsUnflagged" runat="server" CssClass="small-gray-btn" OnClick="lnkbtnAssetsUnflagged_Click" ToolTip="Unflagged" OnClientClick="window.parent.ShowDefaultLoader();"><span>Unflagged</span></asp:LinkButton>
                                                <asp:LinkButton ID="lnkbtnViewAsset" runat="server" CssClass="small-blue-btn" OnClick="lnkbtnViewAsset_Click" ToolTip="View Asset" OnClientClick="window.parent.ShowDefaultLoader();"><span>View Asset</span></asp:LinkButton>
                                                
                                            </div>

						                </div>
				                  </div>					
			                </div>
		                </div>
    </form>
</body>
</html>
