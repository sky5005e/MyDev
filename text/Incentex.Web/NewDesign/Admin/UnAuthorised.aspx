<%@ Page Language="C#" MasterPageFile="~/NewDesign/FrontMasterPage.master" AutoEventWireup="true"
    CodeFile="UnAuthorised.aspx.cs" Inherits="NewDesign_Admin_UnAuthorised" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <% if (!Request.IsLocal)
       { %>
    <section id="container" class="cf filter-page">
    <% } %>
    <div id="container" class="cf filter-page">
    <div id="media-div" style="width:100%">
    <div class="widecolumn alignright" style="width: 100%" id="DocumentStoragecenter">
            <div class="filter-content">
                <div class="filter-header cf" >
                    <span class="title-txt">Unauthorised...</span> <em id="totalcount_em"
                        runat="server" visible="false"></em>
                </div>
             </div>
             <br /><br /><br />
            <div class="filter-content">
                    <div class="filter-header cf" class="MediaFilter">
                        <div class="unautherisedblock" >
                        
                        Unauthorised...<br />
                        You are not authorised to access this module.
                        
                        
                        </div>
                    </div>
             </div>
        </div>
        
        </div>
    </div> 
        <% if (!Request.IsLocal)
           { %>
    </section>
    <%}
    %>
</asp:Content>
