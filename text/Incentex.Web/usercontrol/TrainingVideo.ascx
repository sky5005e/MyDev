<%@ Control Language="C#" AutoEventWireup="true"  CodeFile="TrainingVideo.ascx.cs" Inherits="usercontrol_TrainingVideo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="at" %>
<script type="text/javascript">
    $(window).load(function() {
        $("#dvVideoLoader").hide();
    });
</script>
<script type="text/javascript" language="javascript">
    $().ready(function() {
        $("#dvVideoLoader").show();
        /*Function to get X and Y co-ordinates of a browser*/

        posY = getPopScreenCenterY();
        posX = getPopScreenCenterX();

        $("#ctl00_ctl03_hfPopY").val(posY);
        $("#ctl00_ctl03_hfPopX").val(posX);

        function getPopScreenCenterY() {
            var y = 0;
            y = getPopScrollOffset() + (getPopInnerHeight() / 2);
            return (y);
        }

        function getPopScreenCenterX() {
            return (document.body.clientWidth / 2);
        }

        function getPopInnerHeight() {
            var y;
            if (self.innerHeight) // all except Explorer
            {
                y = self.innerHeight;
            }
            else if (document.documentElement &&
                    document.documentElement.clientHeight)
            // Explorer 6 Strict Mode
            {
                y = document.documentElement.clientHeight;
            }
            else if (document.body) // other Explorers
            {
                y = document.body.clientHeight;
            }
            return (y);
        }

        function getPopScrollOffset() {
            var y;
            if (self.pageYOffset) // all except Explorer
            {
                y = self.pageYOffset;
            }
            else if (document.documentElement &&
                    document.documentElement.scrollTop)
            // Explorer 6 Strict
            {
                y = document.documentElement.scrollTop;
            }
            else if (document.body) // all other Explorers
            {
                y = document.body.scrollTop;
            }
            return (y);
        }

        /*End*/
    });

</script>
<link media="screen" rel="stylesheet" href='<%=ConfigurationSettings.AppSettings["siteurl"] %>CSS/colorbox.css' />
<input type="hidden" id="hfPopX" value="0" runat="server" />
<input type="hidden" id="hfPopY" value="0" runat="server" />
<asp:LinkButton ID="lnkDummyVideo" class="grey2_btn alignright" runat="server" Style="display: none"></asp:LinkButton>
<at:ModalPopupExtender BehaviorID="mpeBehavior" ID="mpeATrainigVideo" TargetControlID="lnkDummyVideo" BackgroundCssClass="modalBackground"
    DropShadow="false" runat="server" CancelControlID="cboxClose" PopupControlID="pnlTrainigVidei">
</at:ModalPopupExtender>

<asp:PlaceHolder runat="server" ID="phScriptManager"></asp:PlaceHolder>
<asp:Panel ID="pnlTrainigVidei" runat="server" Style="display: none;">
    <div class="cboxWrapper" style="display: block; width: 850px; position: fixed; left: 8%;
        top: 15%;">
        <div style="">
            <div id="cboxTopLeft" style="float: left;">
            </div>
            <div id="cboxTopCenter" style="float: left; width: 800px;">
            </div>
            <div id="cboxTopRight" style="float: left;">
            </div>
        </div>
        <div style="clear: left;">
            <div id="cboxMiddleLeft" style="float: left; height: 445px;">
            </div>
            <div id="cboxContent" style="float: left; display: block;height:445px;">
                 <div id="cboxLoadedContent" style="display: block;margin:0;">
                     <div id="cboxClose" style="right:2px;" onclick="javascript:window.location=window.location.href;">
                         close</div>
                     <div>
                         <div runat="server" id="dvVideoTag">
                         </div>
                         <div id="dvVideoLoader" style="display: block; position: absolute; z-index: 2; left: 50%;
                             top: 50%;">
                             <img alt="Loading" src='<%=ConfigurationSettings.AppSettings["siteurl"] %>Images/ajaxbtn.gif' />
                         </div>
                        <% if (IsForGeneralVideo)
                            {%>
                         <div class="spacer10">
                         </div>
                         <div class="centeralign">
                             <asp:LinkButton runat="server" ID="lnkbtnWatchLatter" CssClass="grey2_btn" OnClick="lnkbtnWatchLatter_Click"><span>Watch Later</span></asp:LinkButton>
                         </div>
                         <%} %>
                     </div>
                 </div>
            </div>
            <div id="cboxMiddleRight" style="float: left; height: 445px;">
            </div>
        </div>
        <div style="clear: left;">
            <div id="cboxBottomLeft" style="float: left;">
            </div>
            <div id="cboxBottomCenter" style="float: left; width: 800px;">
            </div>
            <div id="cboxBottomRight" style="float: left;">
            </div>
        </div>
    </div>
</asp:Panel>
