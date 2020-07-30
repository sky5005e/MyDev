<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UPSPackageTracking.ascx.cs"
    Inherits="usercontrol_UPSPackageTracking" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<style type="text/css">
    .order_detail td
    {
    	font-size:12px;
    }
</style>

<link media="screen" rel="stylesheet" href='<%=ConfigurationSettings.AppSettings["siteurl"] %>CSS/colorbox.css' />
<asp:LinkButton ID="lnkDummyButton" runat="server" Style="display: none"></asp:LinkButton>
<at:ModalPopupExtender BehaviorID="mpeBehavior" ID="mpePackageTracking" TargetControlID="lnkDummyButton"
    BackgroundCssClass="modalBackground" DropShadow="false" runat="server" CancelControlID="cboxClose"
    PopupControlID="pnlPackageTracking">
</at:ModalPopupExtender>
<asp:PlaceHolder runat="server" ID="phScriptManager"></asp:PlaceHolder>
<asp:Panel ID="pnlPackageTracking" runat="server" Style="display: none;">
    <div class="cboxWrapper" style="display: block; width: 550px;left:30%;top:20%;position:fixed;">
        <div style="">
            <div id="cboxTopLeft" style="float: left;">
            </div>
            <div id="cboxTopCenter" style="float: left; width: 500px;">
            </div>
            <div id="cboxTopRight" style="float: left;">
            </div>
        </div>
        <div style="clear: left;">
            <div id="cboxMiddleLeft" style="float: left; height: 595px;">
            </div>
            <div id="cboxContent" style="float: left; display: block; height: 595px;width:500px;">
                <div id="cboxLoadedContent" style="display: block; margin: 0;">
                    <div id="cboxClose" style="right: 2px;">
                        close</div>
                    <div>
                        <table cellpadding="5" cellspacing="5" class="order_detail">
                            <tr>
                                <td>
                                    Tracking Number :
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblTrackingNumber"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Shipped By :
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblShippedBy"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Delivered On :
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblDeliveredOn"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Left At :
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblLeftAt"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Signed By :
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblSignedBy"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    ShipTo Address :
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lblShipToAddress"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <h3 style="color:#72757C;margin:10px 0px;">
                            Shipment Progress</h3>
                        <div runat="server" id="dvShipmentProgress" style="height:415px;overflow:auto;">
                        </div>
                    </div>
                </div>
            </div>
            <div id="cboxMiddleRight" style="float: left; height: 595px;">
            </div>
        </div>
        <div style="clear: left;">
            <div id="cboxBottomLeft" style="float: left;">
            </div>
            <div id="cboxBottomCenter" style="float: left; width: 500px;">
            </div>
            <div id="cboxBottomRight" style="float: left;">
            </div>
        </div>
    </div>
</asp:Panel>
