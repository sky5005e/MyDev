<%@ Page Title="" Language="C#" MasterPageFile="~/NewDesign/HeaderMaster.master" AutoEventWireup="true" CodeFile="Contactus.aspx.cs" Inherits="Contactus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMaster" Runat="Server">
  
<div class="contact-block">
    	<h3>Contact Us</h3>
      <div class="contact-txt cf"><span>Client Support: 772-453-2764</span><a href="#" class="live-chat-off">LIVE CHAT NOT AVAILABLE</a></div>
      <div class="contact-form">
      	<ul class="cf">
        	<li><span class="lbl-txt">Login Email</span><input type="text" class="input-contact" placeholder="me@email.com" title="me@email.com"></li>
          <li><span class="lbl-txt">Reason</span><input type="text" class="input-contact" ></li>
          <li><span class="lbl-txt">Subject</span><input type="text" class="input-contact" ></li>
          <li><span class="lbl-txt">Message</span><textarea class="textarea-contact"></textarea></li>
          <li><a href="#" class="contact-btn" title="SUBMIT">SUBMIT</a></li>
        </ul>
      </div>
    </div>
</asp:Content>

