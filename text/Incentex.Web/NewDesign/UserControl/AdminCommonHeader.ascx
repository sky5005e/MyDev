<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AdminCommonHeader.ascx.cs" Inherits="NewDesign_UserControl_AdminCommonHeader" %>
<link rel="apple-touch-icon-precomposed" sizes="144x144" href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/ico/icon-144.png")%>' />
<link rel="apple-touch-icon-precomposed" sizes="114x114" href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/ico/icon-114.png")%>' />
<link rel="apple-touch-icon-precomposed" sizes="72x72" href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/ico/icon-72.png")%>' />
<link rel="apple-touch-icon-precomposed" sizes="57x57" href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/ico/icon-57.png")%>' />
<link rel="apple-touch-icon-precomposed" href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/ico/icon-57.png")%>' />
<link rel="icon" type="image/ico" href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/ico/favicon.ico")%>'  />
<link rel="shortcut icon" type="image/x-icon" href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/ico/favicon.ico")%>'  />
 <!-- BEGIN PLUGIN CSS -->
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/fullcalendar/fullcalendar.css")%>' rel="stylesheet" type="text/css" media="screen" />
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/pace/pace-theme-flash.css")%>' rel="stylesheet" type="text/css" media="screen" />
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/gritter/css/jquery.gritter.css")%>' rel="stylesheet" type="text/css" />
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/bootstrap-datepicker/css/datepicker.css")%>' rel="stylesheet" type="text/css" />
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-ricksaw-chart/css/rickshaw.css")%>' type="text/css" media="screen" />
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-morris-chart/css/morris.css")%>' type="text/css" media="screen" />
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-slider/css/jquery.sidr.light.css")%>' rel="stylesheet"
        type="text/css" media="screen" />
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/bootstrap-select2/select2.css")%>' rel="stylesheet" type="text/css"
        media="screen" />
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-jvectormap/css/jquery-jvectormap-1.2.2.css")%>' rel="stylesheet"
        type="text/css" media="screen" />
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/boostrap-checkbox/css/bootstrap-checkbox.css")%>' rel="stylesheet"
        type="text/css" media="screen" />
    <!-- END PLUGIN CSS -->
    <!-- BEGIN CORE CSS FRAMEWsORK -->
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/boostrapv3/css/bootstrap.min.css")%>' rel="stylesheet" type="text/css" />
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/boostrapv3/css/bootstrap-theme.min.css")%>' rel="stylesheet"
        type="text/css" />
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/font-awesome/css/font-awesome.css")%>' rel="stylesheet" type="text/css" />
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/css/animate.min.css")%>' rel="stylesheet" type="text/css" />
    <!-- END CORE CSS FRAMEWORK -->
    <!-- BEGIN CSS TEMPLATE -->
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/css/style.css")%>' rel="stylesheet" type="text/css" />
<link href='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/css/custom-icon-set.css")%>' rel="stylesheet" type="text/css" />
    <!-- END CSS TEMPLATE -->
    <!-- BEGIN CORE JS FRAMEWORK-->

<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-1.8.3.min.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-ui/jquery-ui-1.10.1.custom.min.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/bootstrap/js/bootstrap.min.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/breakpoints.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-unveil/jquery.unveil.min.js")%>'></script>
<!-- END CORE JS FRAMEWORK -->
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-slider/jquery.sidr.min.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-numberAnimate/jquery.animateNumbers.js")%>'></script>
 <script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-slimscroll/jquery.slimscroll.min.js")%>'></script>
<!-- END CORE PLUGINS -->
<!-- BEGIN PAGE LEVEL PLUGINS -->
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/pace/pace.min.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-block-ui/jqueryblockui.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/bootstrap-datepicker/js/bootstrap-datepicker.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/bootstrap-timepicker/js/bootstrap-timepicker.min.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-inputmask/jquery.inputmask.min.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/jquery-autonumeric/autoNumeric.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/ios-switch/ios7-switch.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/bootstrap-select2/select2.min.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/bootstrap-wysihtml5/wysihtml5-0.3.0.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/bootstrap-wysihtml5/bootstrap-wysihtml5.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/bootstrap-tag/bootstrap-tagsinput.min.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/plugins/dropzone/dropzone.min.js")%>'></script>
<!-- END PAGE LEVEL PLUGINS -->
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/js/form_elements.js")%>'></script>
<!-- BEGIN PAGE LEVEL SCRIPTS -->
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/js/core.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/js/chat.js")%>'></script>
<script type="text/javascript" src='<%= Page.ResolveClientUrl("~/NewDesign/StaticContents/Admin/assets/js/demo.js")%>'></script>
<!-- END PAGE LEVEL SCRIPTS -->
<!-- END JAVASCRIPTS -->