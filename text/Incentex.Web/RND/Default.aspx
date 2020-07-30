<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" Culture="auto" meta:resourcekey="PageResource1"
    UICulture="auto" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="form1">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="width: 1140px">
       <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>--%>
                <div style="width: 100%" class="fl">
                    <table>
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <%--Currency From--%>
                                        </td>
                                        <td>
                                            <%-- <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                            <ContentTemplate>--%>
                                            <asp:DropDownList ID="ddlCurrencyFrom" runat="server" AutoPostBack="true" Visible="false">
                                                <asp:ListItem Text="US Dollar" Value="USD" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="EU euro" Value="EUR"></asp:ListItem>
                                                <asp:ListItem Text="Canadian dollar" Value="CAD"></asp:ListItem>
                                            </asp:DropDownList>
                                            <%--   </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%--Currency To--%>
                                        </td>
                                        <td>
                                            <%-- <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>--%>
                                            <asp:DropDownList ID="ddlCurrencyTo" runat="server" AutoPostBack="true" Visible="false">
                                                <asp:ListItem Text="US Dollar" Value="USD" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="EU euro" Value="EUR"></asp:ListItem>
                                                <asp:ListItem Text="Canadian dollar" Value="CAD"></asp:ListItem>
                                            </asp:DropDownList>
                                            <%--  </ContentTemplate>
                                        </asp:UpdatePanel>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right">
                                            <asp:Button ID="btnChange" Text="Convert" runat="server" OnClick="btnChange_Click"
                                                Visible="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" align="right" style="color: Red;">
                                            <asp:Label ID="lblConvertionRate" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                    <asp:GridView ID="grdView" runat="server" 
            AutoGenerateColumns="False" onrowdatabound="grdView_RowDataBound" >
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblUserName" runat="server" Text='<%# Eval("iProductId") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblUserType" runat="server" Text='<%# Eval("iProductName") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblProdPrize" runat="server" Text='<%# Eval("dProductePrize") %>' ></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
               
                <%-- <table width="1140" cellpadding="7" bordercolor="#999999" border="1" bgcolor="#f6f6f6"
                    style="border-collapse: collapse;">
                    <colgroup>
                        <col span="1" width="64">
                    </colgroup>
                    <tbody>
                        <tr>
                            <th width="100" valign="bottom" bgcolor="#d6e3eb" align="left">
                                Currency Name
                            </th>
                            <th width="41" valign="bottom" bgcolor="#d6e3eb" align="left">
                                Code
                            </th>
                            <th align="left" bgcolor="#d6e3eb" valign="bottom" width="100">
                                Currency Name
                            </th>
                            <th align="left" bgcolor="#d6e3eb" valign="bottom" width="41">
                                Code
                            </th>
                            <th align="left" bgcolor="#d6e3eb" valign="bottom" width="100">
                                Currency Name
                            </th>
                            <th align="left" bgcolor="#d6e3eb" valign="bottom" width="41">
                                Code
                            </th>
                            <th align="left" bgcolor="#d6e3eb" valign="bottom" width="100">
                                Currency Name
                            </th>
                            <th align="left" bgcolor="#d6e3eb" valign="bottom" width="41">
                                Code
                            </th>
                        </tr>
                        <tr>
                            <td width="150">
                                Afghanistan afghani
                            </td>
                            <td width="41">
                                AFA
                            </td>
                            <td width="150">
                                Ethiopian birr
                            </td>
                            <td width="41">
                                ETB
                            </td>
                            <td width="150">
                                Lebanese pound
                            </td>
                            <td width="41">
                                LBP
                            </td>
                            <td width="150">
                                Silver (ounce)
                            </td>
                            <td width="41">
                                XAG
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Albanian lek
                            </td>
                            <td width="41">
                                ALL
                            </td>
                            <td width="150">
                                EU euro
                            </td>
                            <td width="41">
                                EUR
                            </td>
                            <td width="150">
                                Lesotho loti
                            </td>
                            <td width="41">
                                LSL
                            </td>
                            <td width="150">
                                Singapore dollar
                            </td>
                            <td width="41">
                                SGD
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Algerian dinar
                            </td>
                            <td width="41">
                                DZD
                            </td>
                            <td width="150">
                                Falkland Islands pound
                            </td>
                            <td width="41">
                                FKP
                            </td>
                            <td width="41">
                                Liberian dollar
                            </td>
                            <td width="150">
                                LRD
                            </td>
                            <td width="150">
                                Solomon Islands dollar
                            </td>
                            <td width="41">
                                SBD
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Angolan kwanza reajustado
                            </td>
                            <td width="41">
                                AOR
                            </td>
                            <td width="150">
                                Fiji dollar
                            </td>
                            <td width="41">
                                FJD
                            </td>
                            <td width="41">
                                Libyan dinar
                            </td>
                            <td width="150">
                                LYD
                            </td>
                            <td width="150">
                                Somali shilling
                            </td>
                            <td width="41">
                                SOS
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Argentine peso
                            </td>
                            <td width="41">
                                ARS
                            </td>
                            <td width="150">
                                Gambian dalasi
                            </td>
                            <td width="41">
                                GMD
                            </td>
                            <td width="41">
                                Lithuanian litas
                            </td>
                            <td width="150">
                                LTL
                            </td>
                            <td width="150">
                                South African rand
                            </td>
                            <td width="41">
                                ZAR
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Armenian dram
                            </td>
                            <td width="41">
                                AMD
                            </td>
                            <td width="150">
                                Georgian lari
                            </td>
                            <td width="41">
                                GEL
                            </td>
                            <td width="41">
                                Macao SAR pataca
                            </td>
                            <td width="150">
                                MOP
                            </td>
                            <td width="150">
                                South Korean won
                            </td>
                            <td width="41">
                                KRW
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Aruban guilder
                            </td>
                            <td width="41">
                                AWG
                            </td>
                            <td width="150">
                                Ghanaian new cedi
                            </td>
                            <td width="41">
                                GHS
                            </td>
                            <td width="41">
                                Macedonian denar
                            </td>
                            <td width="150">
                                MKD
                            </td>
                            <td width="150">
                                Sri Lanka rupee
                            </td>
                            <td width="41">
                                LKR
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Australian dollar
                            </td>
                            <td width="41">
                                AUD
                            </td>
                            <td width="150">
                                Gibraltar pound
                            </td>
                            <td width="41">
                                GIP
                            </td>
                            <td width="41">
                                Malagasy ariary
                            </td>
                            <td width="150">
                                MGA
                            </td>
                            <td width="150">
                                Sudanese pound
                            </td>
                            <td width="41">
                                SDG
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Azerbaijanian new manat
                            </td>
                            <td width="41">
                                AZN
                            </td>
                            <td width="150">
                                Gold (ounce)
                            </td>
                            <td width="41">
                                XAU
                            </td>
                            <td width="41">
                                Malawi kwacha
                            </td>
                            <td width="150">
                                MWK
                            </td>
                            <td width="150">
                                Suriname dollar
                            </td>
                            <td width="41">
                                SRD
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Bahamian dollar
                            </td>
                            <td width="41">
                                BSD
                            </td>
                            <td width="150">
                                Gold franc
                            </td>
                            <td width="41">
                                XFO
                            </td>
                            <td width="41">
                                Malaysian ringgit
                            </td>
                            <td width="150">
                                MYR
                            </td>
                            <td width="150">
                                Swaziland lilangeni
                            </td>
                            <td width="41">
                                SZL
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Bahraini dinar
                            </td>
                            <td width="41">
                                BHD
                            </td>
                            <td width="150">
                                Guatemalan quetzal
                            </td>
                            <td width="41">
                                GTQ
                            </td>
                            <td width="41">
                                Maldivian rufiyaa
                            </td>
                            <td width="150">
                                MVR
                            </td>
                            <td width="150">
                                Swedish krona
                            </td>
                            <td width="41">
                                SEK
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Bangladeshi taka
                            </td>
                            <td width="41">
                                BDT
                            </td>
                            <td width="150">
                                Guinean franc
                            </td>
                            <td width="41">
                                GNF
                            </td>
                            <td width="41">
                                Mauritanian ouguiya
                            </td>
                            <td width="150">
                                MRO
                            </td>
                            <td width="150">
                                Swiss franc
                            </td>
                            <td width="41">
                                CHF
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Barbados dollar
                            </td>
                            <td width="41">
                                BBD
                            </td>
                            <td width="150">
                                Guyana dollar
                            </td>
                            <td width="41">
                                GYD
                            </td>
                            <td width="41">
                                Mauritius rupee
                            </td>
                            <td width="150">
                                MUR
                            </td>
                            <td width="150">
                                Syrian pound
                            </td>
                            <td width="41">
                                SYP
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Belarusian ruble
                            </td>
                            <td width="41">
                                BYR
                            </td>
                            <td width="150">
                                Haitian gourde
                            </td>
                            <td width="41">
                                HTG
                            </td>
                            <td width="41">
                                Mexican peso
                            </td>
                            <td width="150">
                                MXN
                            </td>
                            <td width="150">
                                Taiwan New dollar
                            </td>
                            <td width="41">
                                TWD
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Belize dollar
                            </td>
                            <td width="41">
                                BZD
                            </td>
                            <td width="150">
                                Honduran lempira
                            </td>
                            <td width="41">
                                HNL
                            </td>
                            <td width="41">
                                Moldovan leu
                            </td>
                            <td width="150">
                                MDL
                            </td>
                            <td width="150">
                                Tajik somoni
                            </td>
                            <td width="41">
                                TJS
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Bermudian dollar
                            </td>
                            <td width="41">
                                BMD
                            </td>
                            <td width="150">
                                Hong Kong SAR dollar
                            </td>
                            <td width="41">
                                HKD
                            </td>
                            <td width="41">
                                Mongolian tugrik
                            </td>
                            <td width="150">
                                MNT
                            </td>
                            <td width="150">
                                Tanzanian shilling
                            </td>
                            <td width="41">
                                TZS
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Bhutan ngultrum
                            </td>
                            <td width="41">
                                BTN
                            </td>
                            <td width="150">
                                Hungarian forint
                            </td>
                            <td width="41">
                                HUF
                            </td>
                            <td width="41">
                                Moroccan dirham
                            </td>
                            <td width="150">
                                MAD
                            </td>
                            <td width="150">
                                Thai baht
                            </td>
                            <td width="41">
                                THB
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Bolivian boliviano
                            </td>
                            <td width="41">
                                BOB
                            </td>
                            <td width="150">
                                Icelandic krona
                            </td>
                            <td width="41">
                                ISK
                            </td>
                            <td width="41">
                                Mozambique new metical
                            </td>
                            <td width="150">
                                MZN
                            </td>
                            <td width="150">
                                Tongan pa&#39;anga
                            </td>
                            <td width="41">
                                TOP
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Botswana pula
                            </td>
                            <td width="41">
                                BWP
                            </td>
                            <td width="150">
                                IMF special drawing right
                            </td>
                            <td width="41">
                                XDR
                            </td>
                            <td width="41">
                                Myanmar kyat
                            </td>
                            <td width="150">
                                MMK
                            </td>
                            <td width="150">
                                Trinidad and Tobago dollar
                            </td>
                            <td width="41">
                                TTD
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Brazilian real
                            </td>
                            <td width="41">
                                BRL
                            </td>
                            <td width="150">
                                Indian rupee
                            </td>
                            <td width="41">
                                INR
                            </td>
                            <td width="41">
                                Namibian dollar
                            </td>
                            <td width="150">
                                NAD
                            </td>
                            <td width="150">
                                Tunisian dinar
                            </td>
                            <td width="41">
                                TND
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                British pound
                            </td>
                            <td width="41">
                                GBP
                            </td>
                            <td width="41">
                                Indonesian rupiah
                            </td>
                            <td width="150">
                                IDR
                            </td>
                            <td width="41">
                                Indonesian rupiahNepalese rupee
                            </td>
                            <td width="150">
                                NPR
                            </td>
                            <td width="150">
                                Turkish lira
                            </td>
                            <td width="41">
                                TRY
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Brunei dollar
                            </td>
                            <td width="41">
                                BND
                            </td>
                            <td width="41">
                                Iranian rial
                            </td>
                            <td width="150">
                                IRR
                            </td>
                            <td width="41">
                                Iranian rialNetherlands Antillian guilder
                            </td>
                            <td width="150">
                                ANG
                            </td>
                            <td width="150">
                                Turkmen new manat
                            </td>
                            <td width="41">
                                TMT
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Bulgarian lev
                            </td>
                            <td width="41">
                                BGN
                            </td>
                            <td width="41">
                                Iraqi dinar
                            </td>
                            <td width="150">
                                IQD
                            </td>
                            <td width="41">
                                Iraqi dinarNew Zealand dollar
                            </td>
                            <td width="150">
                                NZD
                            </td>
                            <td width="150">
                                UAE dirham
                            </td>
                            <td width="41">
                                AED
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Burundi franc
                            </td>
                            <td width="41">
                                BIF
                            </td>
                            <td width="41">
                                Israeli new shekel
                            </td>
                            <td width="150">
                                ILS
                            </td>
                            <td width="41">
                                Israeli new shekelNicaraguan cordoba oro
                            </td>
                            <td width="150">
                                NIO
                            </td>
                            <td width="150">
                                Uganda new shilling
                            </td>
                            <td width="41">
                                UGX
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Cambodian riel
                            </td>
                            <td width="41">
                                KHR
                            </td>
                            <td width="41">
                                Jamaican dollar
                            </td>
                            <td width="150">
                                JMD
                            </td>
                            <td width="41">
                                Jamaican dollarNigerian naira
                            </td>
                            <td width="150">
                                NGN
                            </td>
                            <td width="150">
                                UIC franc
                            </td>
                            <td width="41">
                                XFU
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Canadian dollar
                            </td>
                            <td width="41">
                                CAD
                            </td>
                            <td width="41">
                                Japanese yen
                            </td>
                            <td width="150">
                                JPY
                            </td>
                            <td width="41">
                                Japanese yenNorth Korean won
                            </td>
                            <td width="150">
                                KPW
                            </td>
                            <td width="150">
                                Ukrainian hryvnia
                            </td>
                            <td width="41">
                                UAH
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Cape Verde escudo
                            </td>
                            <td width="41">
                                CVE
                            </td>
                            <td width="41">
                                Jordanian dinar
                            </td>
                            <td width="150">
                                JOD
                            </td>
                            <td width="41">
                                Jordanian dinarNorwegian krone
                            </td>
                            <td width="150">
                                NOK
                            </td>
                            <td width="150">
                                Uruguayan peso uruguayo
                            </td>
                            <td width="41">
                                UYU
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Cayman Islands dollar
                            </td>
                            <td width="41">
                                KYD
                            </td>
                            <td width="41">
                                Kazakh tenge
                            </td>
                            <td width="150">
                                KZT
                            </td>
                            <td width="41">
                                Kazakh tengeOmani rial
                            </td>
                            <td width="150">
                                OMR
                            </td>
                            <td width="150">
                                US dollar
                            </td>
                            <td width="41">
                                USD
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                CFA franc BCEAO
                            </td>
                            <td width="41">
                                XOF
                            </td>
                            <td width="41">
                                Kenyan shilling
                            </td>
                            <td width="150">
                                KES
                            </td>
                            <td width="41">
                                Pakistani rupee
                            </td>
                            <td width="150">
                                PKR
                            </td>
                            <td width="150">
                                Uzbekistani sum
                            </td>
                            <td width="41">
                                UZS
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                CFA franc BEAC
                            </td>
                            <td width="41">
                                XAF
                            </td>
                            <td width="41">
                                Kuwaiti dinar
                            </td>
                            <td width="150">
                                KWD
                            </td>
                            <td width="41">
                                Palladium (ounce)
                            </td>
                            <td width="150">
                                XPD
                            </td>
                            <td width="150">
                                Vanuatu vatu
                            </td>
                            <td width="41">
                                VUV
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                CFP franc
                            </td>
                            <td width="41">
                                XPF
                            </td>
                            <td width="41">
                                Kyrgyz som
                            </td>
                            <td width="150">
                                KGS
                            </td>
                            <td width="41">
                                Panamanian balboa
                            </td>
                            <td width="150">
                                PAB
                            </td>
                            <td width="150">
                                Venezuelan bolivar fuerte
                            </td>
                            <td width="41">
                                VEF
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Chilean peso
                            </td>
                            <td width="41">
                                CLP
                            </td>
                            <td width="41">
                                Lao kip
                            </td>
                            <td width="150">
                                LAK
                            </td>
                            <td width="41">
                                Papua New Guinea kina
                            </td>
                            <td width="150">
                                PGK
                            </td>
                            <td width="150">
                                Vietnamese dong
                            </td>
                            <td width="41">
                                VND
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Chinese yuan renminbi
                            </td>
                            <td width="41">
                                CNY
                            </td>
                            <td width="41">
                                Latvian lats
                            </td>
                            <td width="150">
                                LVL
                            </td>
                            <td width="41">
                                Paraguayan guarani
                            </td>
                            <td width="150">
                                PYG
                            </td>
                            <td width="150">
                                Yemeni rial
                            </td>
                            <td width="41">
                                YER
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Colombian peso
                            </td>
                            <td width="41">
                                COP
                            </td>
                            <td width="41">
                                Lebanese pound
                            </td>
                            <td width="150">
                                LBP
                            </td>
                            <td width="41">
                                Peruvian nuevo sol
                            </td>
                            <td width="150">
                                PEN
                            </td>
                            <td width="150">
                                Zambian kwacha
                            </td>
                            <td width="41">
                                ZMK
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Comoros franc
                            </td>
                            <td width="41">
                                KMF
                            </td>
                            <td width="41">
                                Lesotho loti
                            </td>
                            <td width="150">
                                LSL
                            </td>
                            <td width="41">
                                Philippine peso
                            </td>
                            <td width="150">
                                PHP
                            </td>
                            <td width="150">
                                Zimbabwe dollar
                            </td>
                            <td width="41">
                                ZWL
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Congolese franc
                            </td>
                            <td width="41">
                                CDF
                            </td>
                            <td width="41">
                                Indonesian rupiah
                            </td>
                            <td width="150">
                                IDR
                            </td>
                            <td width="41">
                                Platinum (ounce)
                            </td>
                            <td width="150">
                                XPT
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Costa Rican colon
                            </td>
                            <td width="41">
                                CRC
                            </td>
                            <td width="41">
                                Iranian rial
                            </td>
                            <td width="150">
                                IRR
                            </td>
                            <td width="41">
                                Polish zloty
                            </td>
                            <td width="150">
                                PLN
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Croatian kuna
                            </td>
                            <td width="41">
                                HRK
                            </td>
                            <td width="41">
                                Iraqi dinar
                            </td>
                            <td width="150">
                                IQD
                            </td>
                            <td width="41">
                                Qatari rial
                            </td>
                            <td width="150">
                                QAR
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Cuban peso
                            </td>
                            <td width="41">
                                CUP
                            </td>
                            <td width="41">
                                Israeli new shekel
                            </td>
                            <td width="150">
                                ILS
                            </td>
                            <td width="41">
                                Romanian new leu
                            </td>
                            <td width="150">
                                RON
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Czech koruna
                            </td>
                            <td width="41">
                                CZK
                            </td>
                            <td width="41">
                                Jamaican dollar
                            </td>
                            <td width="150">
                                JMD
                            </td>
                            <td width="41">
                                Russian ruble
                            </td>
                            <td width="150">
                                RUB
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Danish krone
                            </td>
                            <td width="41">
                                DKK
                            </td>
                            <td width="41">
                                Japanese yen
                            </td>
                            <td width="150">
                                JPY
                            </td>
                            <td width="41">
                                Rwandan franc
                            </td>
                            <td width="150">
                                RWF
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Djibouti franc
                            </td>
                            <td width="41">
                                DJF
                            </td>
                            <td width="41">
                                Jordanian dinar
                            </td>
                            <td width="150">
                                JOD
                            </td>
                            <td width="41">
                                Saint Helena pound
                            </td>
                            <td width="150">
                                SHP
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Dominican peso
                            </td>
                            <td width="41">
                                DOP
                            </td>
                            <td width="41">
                                Kazakh tenge
                            </td>
                            <td width="150">
                                KZT
                            </td>
                            <td width="41">
                                Samoan tala
                            </td>
                            <td width="150">
                                WST
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                East Caribbean dollar
                            </td>
                            <td width="41">
                                XCD
                            </td>
                            <td width="41">
                                Kenyan shilling
                            </td>
                            <td width="150">
                                KES
                            </td>
                            <td width="41">
                                Sao Tome and Principe dobra
                            </td>
                            <td width="150">
                                STD
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Egyptian pound
                            </td>
                            <td width="41">
                                EGP
                            </td>
                            <td width="41">
                                Kuwaiti dinar
                            </td>
                            <td width="150">
                                KWD
                            </td>
                            <td width="41">
                                Saudi riyal
                            </td>
                            <td width="150">
                                SAR
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                El Salvador colon
                            </td>
                            <td width="41">
                                SVC
                            </td>
                            <td width="41">
                                Kyrgyz som
                            </td>
                            <td width="150">
                                KGS
                            </td>
                            <td width="41">
                                Serbian dinar
                            </td>
                            <td width="150">
                                RSD
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Eritrean nakfa
                            </td>
                            <td width="41">
                                ERN
                            </td>
                            <td width="41">
                                Lao kip
                            </td>
                            <td width="150">
                                LAK
                            </td>
                            <td width="41">
                                Seychelles rupee
                            </td>
                            <td width="150">
                                SCR
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                Estonian kroon
                            </td>
                            <td width="41">
                                EEK
                            </td>
                            <td width="41">
                                Latvian lats
                            </td>
                            <td width="150">
                                LVL
                            </td>
                            <td width="41">
                                Sierra Leone leone
                            </td>
                            <td width="150">
                                SLL
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td width="150">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="150">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="150">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                            <td width="41">
                                &nbsp;
                            </td>
                        </tr>
                    </tbody>
                </table>--%>
                <%--<div class="fl">
                        AFA-Afghanistan Afghani<br />
                        ALL-Albanian Lek<br />
                        DZD-Algerian Dinar<br />
                        ARS-Argentine Peso<br />
                        AWG-Aruba Florin<br />
                        AUD-Australian Dollar<br />
                        BSD-Bahamian Dollar<br />
                        BHD-Bahraini Dinar<br />
                        BDT-Bangladesh Taka<br />
                        BBD-Barbados Dollar<br />
                        BZD-Belize Dollar<br />
                        BMD-Bermuda Dollar<br />
                        BTN-Bhutan Ngultrum<br />
                        BOB-Bolivian Boliviano<br />
                        BWP-Botswana Pula<br />
                        BRL-Brazilian Real<br />
                        GBP-British Pound<br />
                        BND-Brunei Dollar<br />
                        BIF-Burundi Franc<br />
                        XOF-CFA Franc (BCEAO)<br />
                        XAF-CFA Franc (BEAC)<br />
                        KHR-Cambodia Riel<br />
                        CAD-Canadian Dollar<br />
                        CVE-Cape Verde Escudo<br />
                        KYD-Cayman Islands Dollar<br />
                        CLP-Chilean Peso<br />
                        CNY-Chinese Yuan<br />
                        COP-Colombian Peso<br />
                        KMF-Comoros Franc<br />
                        CRC-Costa Rica Colon<br />
                        HRK-Croatian Kuna<br />
                        CUP-Cuban Peso<br />
                        CYP-Cyprus Pound<br />
                        CZK-Czech Koruna<br />
                        DKK-Danish Krone<br />
                        DJF-Dijibouti Franc<br />
                        DOP-Dominican Peso<br />
                        XCD-East Caribbean Dollar<br />
                        EGP-Egyptian Pound<br />
                        SVC-El Salvador Colon<br />
                        EEK-Estonian Kroon<br />
                        ETB-Ethiopian Birr<br />
                        EUR-Euro<br />
                        FKP-Falkland Islands Pound<br />
                        GMD-Gambian Dalasi<br />
                        GHC-Ghanian Cedi<br />
                        GIP-Gibraltar Pound<br />
                        XAU-Gold Ounces<br />
                        GTQ-Guatemala Quetzal<br />
                        GNF-Guinea Franc<br />
                        GYD-Guyana Dollar<br />
                        HTG-Haiti Gourde<br />
                        HNL-Honduras Lempira<br />
                        HKD-Hong Kong Dollar<br />
                        HUF-Hungarian Forint<br />
                        ISK-Iceland Krona<br />
                        INR-Indian Rupee<br />
                        IDR-Indonesian Rupiah<br />
                        IQD-Iraqi Dinar<br />
                        ILS-Israeli Shekel<br />
                        JMD-Jamaican Dollar<br />
                        JPY-Japanese Yen<br />
                        JOD-Jordanian Dinar<br />
                        KZT-Kazakhstan Tenge<br />
                        KES-Kenyan Shilling<br />
                        KRW-Korean Won<br />
                        KWD-Kuwaiti Dinar<br />
                        LAK-Lao Kip<br />
                        LVL-Latvian Lat<br />
                        LBP-Lebanese Pound<br />
                        LSL-Lesotho Loti<br />
                        LRD-Liberian Dollar<br />
                        LYD-Libyan Dinar<br />
                        LTL-Lithuanian Lita<br />
                        MOP-Macau Pataca<br />
                        MKD-Macedonian Denar<br />
                        MGF-Malagasy Franc<br />
                        MWK-Malawi Kwacha<br />
                        MYR-Malaysian Ringgit<br />
                        MVR-Maldives Rufiyaa<br />
                        MTL-Maltese Lira<br />
                        MRO-Mauritania Ougulya<br />
                        MUR-Mauritius Rupee<br />
                        MXN-Mexican Peso<br />
                        MDL-Moldovan Leu<br />
                        MNT-Mongolian Tugrik<br />
                        MAD-Moroccan Dirham<br />
                        MZM-Mozambique Metical<br />
                        MMK-Myanmar Kyat<br />
                        NAD-Namibian Dollar<br />
                        NPR-Nepalese Rupee<br />
                        ANG-Neth Antilles Guilder<br />
                        NZD-New Zealand Dollar<br />
                        NIO-Nicaragua Cordoba<br />
                        NGN-Nigerian Naira<br />
                        KPW-North Korean Won<br />
                        NOK-Norwegian Krone<br />
                        OMR-Omani Rial<br />
                        XPF-Pacific Franc<br />
                        PKR-Pakistani Rupee<br />
                        XPD-Palladium Ounces<br />
                        PAB-Panama Balboa<br />
                        PGK-Papua New Guinea Kina<br />
                        PYG-Paraguayan Guarani<br />
                        PEN-Peruvian Nuevo Sol<br />
                        PHP-Philippine Peso<br />
                        XPT-Platinum Ounces<br />
                        PLN-Polish Zloty<br />
                        QAR-Qatar Rial<br />
                        ROL-Romanian Leu<br />
                        RUB-Russian Rouble<br />
                        WST-Samoa Tala<br />
                        STD-Sao Tome Dobra<br />
                        SAR-Saudi Arabian Riyal<br />
                        SCR-Seychelles Rupee<br />
                        SLL-Sierra Leone Leone<br />
                        XAG-Silver Ounces<br />
                        SGD-Singapore Dollar<br />
                        SKK-Slovak Koruna<br />
                        SIT-Slovenian Tolar<br />
                        SBD-Solomon Islands Dollar<br />
                        SOS-Somali Shilling<br />
                        ZAR-South African Rand<br />
                        LKR-Sri Lanka Rupee<br />
                        SHP-St Helena Pound<br />
                        SDD-Sudanese Dinar<br />
                        SRG-Surinam Guilder<br />
                        SZL-Swaziland Lilageni<br />
                        SEK-Swedish Krona<br />
                        TRY-Turkey Lira<br />
                        CHF-Swiss Franc<br />
                        SYP-Syrian Pound<br />
                        TWD-Taiwan Dollar<br />
                        TZS-Tanzanian Shilling<br />
                        THB-Thai Baht<br />
                        TOP-Tonga Pa'anga<br />
                        TTD-Trinidad&amp;Tobago Dollar<br />
                        TND-Tunisian Dinar<br />
                        TRL-Turkish Lira<br />
                        USD-U.S. Dollar<br />
                        AED-UAE Dirham<br />
                        UGX-Ugandan Shilling<br />
                        UAH-Ukraine Hryvnia<br />
                        UYU-Uruguayan New Peso<br />
                        VUV-Vanuatu Vatu<br />
                        VEB-Venezuelan Bolivar<br />
                        VND-Vietnam Dong<br />
                        YER-Yemen Riyal<br />
                        YUM-Yugoslav Dinar<br />
                        ZMK-Zambian Kwacha<br />
                        ZWD-Zimbabwe Dollar<br />
                    </div>--%>
           <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </div>
    </form>
</asp:Content>
