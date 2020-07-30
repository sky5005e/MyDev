<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="frontdropdownlistview.aspx.cs" Inherits="RND_frontdropdownlistview"
    Title="Frontdropdownlistview" %>

<%@ Import Namespace="Incentex.DA" %>
<%@ Import Namespace="Incentex.BE" %>
<%@ Import Namespace="System.Data" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" type="text/css" href="../CSS/dd.css" />

    

    <script type="text/javascript" src="../JS/JQuery/jquery.dd.js"></script>

    <script type="text/javascript" src="../JS/JQuery/jquery-ui.min.js"></script>

    <script type="text/javascript" src="../JS/JQuery/image-dropdown.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
        <td>
            <table class="tab95width form_table">
                <tr>
                    <td>
                        <div class="label">
                            Priority</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="websites4" id="websites4">
                                        <%
                                            LookupDA s = new LookupDA();
                                            LookupBE sBe = new LookupBE();
                                            sBe.SOperation = "selectall";
                                            sBe.iLookupCode = "Priority";

                                            DataSet a = s.LookUp(sBe);
                                            foreach (DataRow r in a.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + r["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=r["iLookupID"]%>" title="<%=path%>">
                                            <%=r["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                            Department</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="websites3" id="websites3">
                                        <%
                                            LookupDA s1 = new LookupDA();
                                            LookupBE s1Be = new LookupBE();
                                            s1Be.SOperation = "selectall";
                                            s1Be.iLookupCode = "Department";

                                            DataSet a1 = s1.LookUp(s1Be);
                                            foreach (DataRow r1 in a1.Tables[0].Rows)
                                            {
                                                string path = "admin/Incentex_Used_Icons/" + r1["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=r1["iLookupID"]%>" title="<%=path%>">
                                            <%=r1["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                            Shipping Method</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="incentex" id="incentex">
                                        <%
                                            LookupDA s2 = new LookupDA();
                                            LookupBE s2Be = new LookupBE();
                                            s2Be.SOperation = "selectall";
                                            s2Be.iLookupCode = "ShippingMethod";

                                            DataSet a2 = s2.LookUp(s2Be);
                                            foreach (DataRow r2 in a2.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + r2["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=r2["iLookupID"]%>" title="<%=path%>">
                                            <%=r2["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
            </td>
        </tr>
        <tr>
        <td>
            <table class="tab95width form_table">
                <tr>
                    <td>
                        <div class="label">
                            Product Category </div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="ProdCategory" id="ProdCategory" >
                                        <%
                                            LookupDA sProdCategory = new LookupDA();
                                            LookupBE sProdCategoryBe = new LookupBE();
                                            sProdCategoryBe.SOperation = "selectall";
                                            sProdCategoryBe.iLookupCode = "ProductCategory";

                                            DataSet dsProdCategory = sProdCategory.LookUp(sProdCategoryBe);
                                            foreach (DataRow rsProdCategory in dsProdCategory.Tables[0].Rows)
                                            {
                                                    
                                        %>
                                        <option>
                                            <%=rsProdCategory["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                 </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                            General Status</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="GeneralStatus" id="GeneralStatus">
                                        <%
                                            LookupDA sGS = new LookupDA();
                                            LookupBE sGSBe = new LookupBE();
                                            sGSBe.SOperation = "selectall";
                                            sGSBe.iLookupCode = "GeneralStatus";

                                            DataSet aGS = s1.LookUp(sGSBe);
                                            foreach (DataRow raGS in aGS.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + raGS["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=raGS["iLookupID"]%>" title="<%=path%>">
                                            <%=raGS["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                            Proof Status</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="ProofStatus" id="ProofStatus">
                                        <%
                                            LookupDA sPS = new LookupDA();
                                            LookupBE sPsBe = new LookupBE();
                                            sPsBe.SOperation = "selectall";
                                            sPsBe.iLookupCode = "ProofStatus";

                                            DataSet aPsBe = sPS.LookUp(sPsBe);
                                            foreach (DataRow rPs in aPsBe.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + rPs["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=rPs["iLookupID"]%>" title="<%=path%>">
                                            <%=rPs["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
            </td>
        </tr>
        <tr>
        <td>
        <table class="tab95width form_table">
                <tr>
                    <td>
                        <div class="label">
                            Production Status </div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select id="ProductionStatus" name="ProductionStatus">
                                        <%
                                            LookupDA sProduction = new LookupDA();
                                            LookupBE sProductionBe = new LookupBE();
                                            sProductionBe.SOperation = "selectall";
                                            sProductionBe.iLookupCode = "ProductionStatus";

                                            DataSet aProduction = sProduction.LookUp(sProductionBe);
                                            foreach (DataRow rsProduction in aProduction.Tables[0].Rows)
                                            {
                                                    
                                        %>
                                        <option>
                                            <%=rsProduction["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                 </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                           Employee Rank (Pilots Only)</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="EmployeeRank" id="EmployeeRank">
                                        <%
                                            LookupDA sEmployeeRank = new LookupDA();
                                            LookupBE sEmployeeRankBe = new LookupBE();
                                            sEmployeeRankBe.SOperation = "selectall";
                                            sEmployeeRankBe.iLookupCode = "EmployeeRankPilotsOnly";

                                            DataSet dsEmployeeRank = sEmployeeRank.LookUp(sEmployeeRankBe);
                                            foreach (DataRow raEmployeeRank in dsEmployeeRank.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + raEmployeeRank["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=raEmployeeRank["iLookupID"]%>" title="<%=path%>">
                                            <%=raEmployeeRank["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                           1st Reminder</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="FirstRemindr" id="FirstRemindr">
                                        <%
                                            LookupDA sFirstReminder = new LookupDA();
                                            LookupBE sFirstReminderBe = new LookupBE();
                                            sFirstReminderBe.SOperation = "selectall";
                                            sFirstReminderBe.iLookupCode = "FirstReminder";

                                            DataSet dsFirstReminder = sFirstReminder.LookUp(sFirstReminderBe);
                                            foreach (DataRow radsFirstReminder in dsFirstReminder.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + radsFirstReminder["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=radsFirstReminder["iLookupID"]%>" title="<%=path%>">
                                            <%=radsFirstReminder["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        </tr>
        <tr>
        <td>
         <table class="tab95width form_table">
                <tr>
                     <td>
                        <div class="label">
                           Number Of Months</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="NoOfMonths" id="NoOfMonths">
                                        <%
                                            LookupDA sNumberOfMonths = new LookupDA();
                                            LookupBE sNumberOfMonthsBe = new LookupBE();
                                            sNumberOfMonthsBe.SOperation = "selectall";
                                            sNumberOfMonthsBe.iLookupCode = "NumberOfMonths";

                                            DataSet dsNumberOfMonths = sNumberOfMonths.LookUp(sNumberOfMonthsBe);
                                            foreach (DataRow raNumberOfMonths in dsNumberOfMonths.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + raNumberOfMonths["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=raNumberOfMonths["iLookupID"]%>" title="<%=path%>">
                                            <%=raNumberOfMonths["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                          Employment Status</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                   <select id="EmpStatus" name="EmpStatus">
                                        <%
                                            LookupDA sEmploymentStatus = new LookupDA();
                                            LookupBE sEmploymentStatusBE = new LookupBE();
                                            sEmploymentStatusBE.SOperation = "selectall";
                                            sEmploymentStatusBE.iLookupCode = "EmploymentStatus";

                                            DataSet dsEmploymentStatus = sEmploymentStatus.LookUp(sEmploymentStatusBE);
                                            foreach (DataRow rsEmploymentStatus in dsEmploymentStatus.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + rsEmploymentStatus["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=rsEmploymentStatus["iLookupID"]%>" title="<%=path%>">
                                            <%=rsEmploymentStatus["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                           2nd Reminder</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="SecondRemindr" id="SecondRemindr">
                                        <%
                                            LookupDA sSecondReminder = new LookupDA();
                                            LookupBE sSecondReminderBe = new LookupBE();
                                            sSecondReminderBe.SOperation = "selectall";
                                            sSecondReminderBe.iLookupCode = "SecondReminders";

                                            DataSet dsSecondReminder = sSecondReminder.LookUp(sSecondReminderBe);
                                            foreach (DataRow raSecondReminder in dsSecondReminder.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + raSecondReminder["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=raSecondReminder["iLookupID"]%>" title="<%=path%>">
                                            <%=raSecondReminder["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        </tr>
        <tr>
        <td>
        <table class="tab95width form_table">
                <tr>
                    <td>
                        <div class="label">
                           3rd Reminder</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="TrdReminder" id="TrdReminder">
                                        <%
                                            LookupDA sTrdReminder = new LookupDA();
                                            LookupBE sTrdReminderBe = new LookupBE();
                                            sTrdReminderBe.SOperation = "selectall";
                                            sTrdReminderBe.iLookupCode = "ThirdReminder";

                                            DataSet dsTrdReminder = sTrdReminder.LookUp(sTrdReminderBe);
                                            foreach (DataRow raTrdReminder in dsTrdReminder.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + raTrdReminder["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=raTrdReminder["iLookupID"]%>" title="<%=path%>">
                                            <%=raTrdReminder["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                          Decorating Method</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                   <select id="DecorateMethod" name="DecorateMethod">
                                        <%
                                            LookupDA sDecordatingmethod = new LookupDA();
                                            LookupBE sDecordatingmethodBE = new LookupBE();
                                            sDecordatingmethodBE.SOperation = "selectall";
                                            sDecordatingmethodBE.iLookupCode = "DecoratingMethod";

                                            DataSet dsDecordatingmethod = sDecordatingmethod.LookUp(sDecordatingmethodBE);
                                            foreach (DataRow rsDecordatingmethod in dsDecordatingmethod.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + rsDecordatingmethod["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=rsDecordatingmethod["iLookupID"]%>" title="<%=path%>">
                                            <%=rsDecordatingmethod["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                           Items to be poly bagged</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="ItemsToBePolybagged" id="ItemsToBePolybagged">
                                        <%
                                            LookupDA sItemsToBePolybagged = new LookupDA();
                                            LookupBE sItemsToBePolybaggedBe = new LookupBE();
                                            sItemsToBePolybaggedBe.SOperation = "selectall";
                                            sItemsToBePolybaggedBe.iLookupCode = "ItemsToBePolybagged";

                                            DataSet dsItemsToBePolybagged = sItemsToBePolybagged.LookUp(sItemsToBePolybaggedBe);
                                            foreach (DataRow raItemsToBePolybagged in dsItemsToBePolybagged.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + raItemsToBePolybagged["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=raItemsToBePolybagged["iLookupID"]%>" title="<%=path%>">
                                            <%=raItemsToBePolybagged["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        </tr>
        <tr>
        <td>
        <table class="tab95width form_table">
                <tr>
                    <td>
                        <div class="label">
                           Expiration By Months</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="ExpiratinbyMonth" id="ExpiratinbyMonth">
                                        <%
                                            LookupDA sExpiratinbyMonth = new LookupDA();
                                            LookupBE sExpiratinbyMonthBe = new LookupBE();
                                            sExpiratinbyMonthBe.SOperation = "selectall";
                                            sExpiratinbyMonthBe.iLookupCode = "ExpirationByMonths";

                                            DataSet dsExpiratinbyMonth = sExpiratinbyMonth.LookUp(sExpiratinbyMonthBe);
                                            foreach (DataRow raExpiratinbyMonth in dsExpiratinbyMonth.Tables[0].Rows)
                                            {
                                                %>
                                        <option>
                                            <%=raExpiratinbyMonth["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                          Managing Shipment by</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                   <select id="ManagingShipment" name="ManagingShipment">
                                        <%
                                            LookupDA sManagingShipment = new LookupDA();
                                            LookupBE sManagingShipmentBE = new LookupBE();
                                            sManagingShipmentBE.SOperation = "selectall";
                                            sManagingShipmentBE.iLookupCode = "ManagingShipmentBy";

                                            DataSet dsManagingShipment = sManagingShipment.LookUp(sManagingShipmentBE);
                                            foreach (DataRow rsManagingShipment in dsManagingShipment.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + rsManagingShipment["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=rsManagingShipment["iLookupID"]%>" title="<%=path%>">
                                            <%=rsManagingShipment["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                           Items to have size stickers</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="ItemSizeSticker" id="ItemSizeSticker">
                                        <%
                                            LookupDA sItemsToHaveSizeStickers = new LookupDA();
                                            LookupBE sItemsToHaveSizeStickersBe = new LookupBE();
                                            sItemsToHaveSizeStickersBe.SOperation = "selectall";
                                            sItemsToHaveSizeStickersBe.iLookupCode = "ItemsToHaveSizeStickers";

                                            DataSet dsItemsToHaveSizeStickers = sItemsToHaveSizeStickers.LookUp(sItemsToHaveSizeStickersBe);
                                            foreach (DataRow raItemsToHaveSizeStickers in dsItemsToHaveSizeStickers.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + raItemsToHaveSizeStickers["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=raItemsToHaveSizeStickers["iLookupID"]%>" title="<%=path%>">
                                            <%=raItemsToHaveSizeStickers["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        </tr>
        <tr>
        <td>
        <table class="tab95width form_table">
                <tr>
                    <td>
                        <div class="label">
                           Expiration By Date</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="ExpiratinbyDate" id="ExpiratinbyDate">
                                        <%
                                            LookupDA sExpiratinbyDate = new LookupDA();
                                            LookupBE sExpiratinbyDateBe = new LookupBE();
                                            sExpiratinbyDateBe.SOperation = "selectall";
                                            sExpiratinbyDateBe.iLookupCode = "ExpirationByDate";

                                            DataSet dsExpiratinbyDate = sExpiratinbyDate.LookUp(sExpiratinbyDateBe);
                                            foreach (DataRow raExpiratinbyDate in dsExpiratinbyDate.Tables[0].Rows)
                                            {
                                                %>
                                        <option>
                                            <%=raExpiratinbyDate["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                         Consolidated Shipment </div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                   <select id="ConsolidateShipment" name="ConsolidateShipment">
                                        <%
                                            LookupDA sConsolidatedShipment = new LookupDA();
                                            LookupBE sConsolidatedShipmentBE = new LookupBE();
                                            sConsolidatedShipmentBE.SOperation = "selectall";
                                            sConsolidatedShipmentBE.iLookupCode = "ConsolidatedShipment";

                                            DataSet dsConsolidatedShipment = sConsolidatedShipment.LookUp(sConsolidatedShipmentBE);
                                            foreach (DataRow rsConsolidatedShipment in dsConsolidatedShipment.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + rsConsolidatedShipment["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=rsConsolidatedShipment["iLookupID"]%>" title="<%=path%>">
                                            <%=rsConsolidatedShipment["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                           Items to be packaged using cardboard insert</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="ItemCardboardInsert" id="ItemCardboardInsert">
                                        <%
                                            LookupDA sItemToBePackagedUsingCardboardInsert = new LookupDA();
                                            LookupBE sItemToBePackagedUsingCardboardInsertBe = new LookupBE();
                                            sItemToBePackagedUsingCardboardInsertBe.SOperation = "selectall";
                                            sItemToBePackagedUsingCardboardInsertBe.iLookupCode = "ItemToBePackagedUsingCardboardInsert";

                                            DataSet dsItemToBePackagedUsingCardboardInsert = sItemToBePackagedUsingCardboardInsert.LookUp(sItemToBePackagedUsingCardboardInsertBe);
                                            foreach (DataRow raItemToBePackagedUsingCardboardInsert in dsItemToBePackagedUsingCardboardInsert.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + raItemToBePackagedUsingCardboardInsert["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=raItemToBePackagedUsingCardboardInsert["iLookupID"]%>" title="<%=path%>">
                                            <%=raItemToBePackagedUsingCardboardInsert["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        </tr>
        <tr>
        <td>
         <table class="tab95width form_table">
                <tr>
                    <td>
                        <div class="label">
                           Associate with File Category</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="FileCategoryAssociate" id="FileCategoryAssociate">
                                        <%
                                            LookupDA sFileCategoryAssociate = new LookupDA();
                                            LookupBE sFileCategoryAssociateBe = new LookupBE();
                                            sFileCategoryAssociateBe.SOperation = "selectall";
                                            sFileCategoryAssociateBe.iLookupCode = "AssociateWithFileCategory";

                                            DataSet dsFileCategoryAssociate = sFileCategoryAssociate.LookUp(sFileCategoryAssociateBe);
                                            foreach (DataRow raFileCategoryAssociate in dsFileCategoryAssociate.Tables[0].Rows)
                                            {
                                                %>
                                        <option>
                                            <%=raFileCategoryAssociate["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                         Workgroup </div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                   <select id="Workgroup" name="Workgroup" runat="server">
                                    <option value="0" title="0">-select workgroup-</option>
                                        <%
                                            LookupDA sWorkgroup = new LookupDA();
                                            LookupBE sWorkgroupBE = new LookupBE();
                                            sWorkgroupBE.SOperation = "selectall";
                                            sWorkgroupBE.iLookupCode = "Workgroup";

                                            DataSet dsWorkgroup = sWorkgroup.LookUp(sWorkgroupBE);
                                            foreach (DataRow rsWorkgroup in dsWorkgroup.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + rsWorkgroup["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=rsWorkgroup["iLookupID"]%>" title="<%=path%>">
                                            <%=rsWorkgroup["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                        <div class="label">
                           Items to be packaged using Plastic clips</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="ItemPackedPlasticClips" id="ItemPackedPlasticClips">
                                        <%
                                            LookupDA sItemToBePackagedUsingClips = new LookupDA();
                                            LookupBE sItemToBePackagedUsingClipsBe = new LookupBE();
                                            sItemToBePackagedUsingClipsBe.SOperation = "selectall";
                                            sItemToBePackagedUsingClipsBe.iLookupCode = "ItemToBePackagedUsingPlasticClips";

                                            DataSet dsItemToBePackagedUsingClips = sItemToBePackagedUsingClips.LookUp(sItemToBePackagedUsingClipsBe);
                                            foreach (DataRow raItemToBePackagedUsingClips in dsItemToBePackagedUsingClips.Tables[0].Rows)
                                            {
                                                string path = "../admin/Incentex_Used_Icons/" + raItemToBePackagedUsingClips["sLookupIcon"].ToString();
                                        %>
                                        <option value="calendar<%=raItemToBePackagedUsingClips["iLookupID"]%>" title="<%=path%>">
                                            <%=raItemToBePackagedUsingClips["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                </tr>
            </table>
        </td>
        </tr>
        <tr>
        <td>
         <table class="tab95width form_table">
                <tr>
                    <td>
                        <div class="label">
                           Base Station</div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    <select name="BasedStation" id="BasedStation">
                                        <%
                                            LookupDA sBasedStation = new LookupDA();
                                            LookupBE sBasedStationBe = new LookupBE();
                                            sBasedStationBe.SOperation = "selectall";
                                            sBasedStationBe.iLookupCode = "BaseStation";

                                            DataSet dsBasedStation = sBasedStation.LookUp(sBasedStationBe);
                                            foreach (DataRow raBasedStation in dsBasedStation.Tables[0].Rows)
                                            {
                                                %>
                                        <option>
                                            <%=raBasedStation["sLookupName"].ToString()%></option>
                                        <%} %>
                                    </select>
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>
                    </td>
                    <td>
                     <div class="label">
                          </div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                 
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>   
                    </td>
                    <td>
                     <div class="label">
                          </div>
                        <div>
                            <div class="form_top_co">
                                <span>&nbsp;</span></div>
                            <div class="form_box status_select">
                                <label class="width284">
                                    
                                </label>
                            </div>
                            <div class="form_bot_co">
                                <span>&nbsp;</span></div>
                        </div>   
                    </td>
                </tr>
            </table>
        </td>
        </tr>
    </table>
</asp:Content>
