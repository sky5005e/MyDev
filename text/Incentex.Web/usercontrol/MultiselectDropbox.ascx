<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MultiselectDropbox.ascx.cs"
    Inherits="usercontrol_MultiselectDropbox" %>
<meta content="False" name="vs_snapToGrid" />
<meta content="True" name="vs_showGrid" />
<div id="dvTop" class="DT">
    <asp:TextBox ID="tbm" ReadOnly="True" runat="server" class="MSTBM" onmouseover="DisplayTitle(this);">-Select-</asp:TextBox>
    <img id="imgm" class="DDA" onclick="SHMulSel(<%= ControlClientID %>, event)" />
</div>
<div id="divMain" class="DVMain" runat="server">
    <asp:Repeater ID="rp1" runat="server">
        <ItemTemplate>
            <a class="LI">
                <div id="DVLI" class="LI" onclick="event.cancelBubble=true;" style="margin-left:5px;">
                    <input id="cb1" type="checkbox" runat="server" value='<%# DataBinder.Eval(Container, dataItem) %>'
                        name="cb1" />
                    <asp:Literal ID="lt1" runat="server" Text="test"></asp:Literal>
                </div>
            </a>
        </ItemTemplate>
    </asp:Repeater>
</div>
<asp:hiddenfield id="hsiv" runat="server" />
