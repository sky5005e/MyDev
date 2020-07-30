var ua = navigator.userAgent;
window['eventVar'] = '';

if (ua.match('/iPhone|iPod|iPad/i') || ua.match('iPhone;|iPod;|iPad;'))
   window['eventVar'] = "touchstart";
else
   window['eventVar'] = "click";
   
function ExecuteAnchorHref(href) {
    window.location = href;
}

function OpenGallery(Ctrl,DivID,href)
{
    
  if($(Ctrl).parent().find(".filetype").html() == "Video")
    return;
  
  $("#" + DivID).css('top', '0');
  $("#" + DivID).find("img").attr("src",href);
  
  var ImgWidth = $("#" + DivID).find("img").width();
  var ImgHeight = $("#" + DivID).find("img").height();
  
  if(ImgWidth < 900 && ImgHeight < 700)
    $("#" + DivID).find(".popupInner").width(ImgWidth).height(ImgHeight);
  else{
    var DivWidth = $("#" + DivID).find(".popupInner").width();
    var DivHeight = $("#" + DivID).find(".popupInner").height();
    $("#" + DivID).find("img").width(DivWidth).height(DivHeight);
  }
  SetPopUpAtTop();
}
function ExecuteLinkHref(link, value) {
    var _href = link;
    var _id = document.getElementById(value);
    _id.href = _href;
    //window.location = _href;
    //document.body.appendChild(_id);
    _id.click();
}


$(document).ready(function() {
    $("#SubFields ul li:odd").removeClass("alignleft").addClass("alignright");
    $("#system-data-popup .specs-popup .table-grid tr:first-child").addClass("first");
    $("#Accounting_SubFields ul li:nth-child(2n)").removeClass("alignleft").addClass("alignright");
    
    // Utilizing the Modernizr object created to implement placeholder functionality
//    if (typeof (Modernizr) != 'undefined' && !Modernizr.input.placeholder) {
//        $('input[type="text"]').each(function() {
//            var phAttr = $(this).attr('placeholder');
//            if (typeof (phAttr) != 'undefined' && phAttr != false) {
//                if (phAttr != null && phAttr != '') {
//                    $(this).addClass('default_title_text');
//                    $(this).val(phAttr);
//                    $(this).focus(function() {
//                        $(this).removeClass('default_title_text');
//                        if ($(this).val() == $(this).attr('placeholder')) {
//                            $(this).val('');
//                        }
//                    });
//                    $(this).blur(function() {
//                        if ($(this).val() == '') {
//                            $(this).val($(this).attr('placeholder'));
//                            $(this).addClass('default_title_text');
//                        }
//                    });
//                }
//            }
//        });
//    }

    $('.store-footer .store-title').click(function() {
        $("html, body").animate({ scrollTop: 0 }, 600);
        return false;
    });


    //    $(document).ready(function() {
    //        $(".table-grid tr:first").addClass("");
    //        $(".table-grid tr:odd").addClass("odd");
    //        $(".table-grid tr:not(.odd)").addClass("even");
    //        $(".table-grid tr:first-child").removeClass("odd");
    //        $(".table-grid tr:first-child").removeClass("even");
    //        $(".table-grid tr.no-border").removeClass("odd");
    //        $(".table-grid tr.no-border").removeClass("even");
    //    });


    // Custorm Dropdown Menu
    //  $('.default').dropkick();

    $(function() {
        $(".default").uniform();
    });


    //$('.default').jqTransSelect();



    // set focus Login & Register POPUP
    $(".login-field .input-txt").focus(function() {
        $(this).parent().addClass("active");
    });

    $(".login-field .input-txt").blur(function() {
        $(this).parent().removeClass("active");
    });

    $(".password-field .input-txt").focus(function() {
        $(this).parent().addClass("active");
        //$(".forgot-btn").hide();
    });

    $(".password-field .input-txt").blur(function() {
        $(this).parent().removeClass("active");
        $(".forgot-btn").show();
    });

    $(".left-form li .input-field , .left-form li .input-field-month , .left-form li .input-field-day , .left-form li .input-field-year ").focus(function() {
        $(this).addClass("highlight-field");
    });

    $(".left-form li .input-field , .left-form li .input-field-month , .left-form li .input-field-day , .left-form li .input-field-year ").blur(function() {
        $(this).removeClass("highlight-field");
    });

    // set focus on Account Page
    $(".input-field-all").focus(function() {
        $(this).addClass("highlight");
    });

    $(".input-field-all").blur(function() {
        $(this).removeClass("highlight");
    });

    $(".input-textarea").focus(function() {
        $(this).addClass("highlight");
    });

    $(".input-textarea").blur(function() {
        $(this).removeClass("highlight");
    });

    $(".input-field-small").focus(function() {
        $(this).addClass("highlight");
    });

    $(".input-field-small").blur(function() {
        $(this).removeClass("highlight");
    });


    $(".cart-btn-block .input-qty").focus(function() {
        $(this).addClass("highlight");
    });

    $(".cart-btn-block .input-qty").blur(function() {
        $(this).removeClass("highlight");
    });

    //generating tabs    
    $.tabs();

    SetPopUpAtTop();

    $(window).resize(function() {
        SetPopUpAtTop(true);
    });

    $('#footer a').click(function() {
        $('#term-block').css('top', '0');
        $('#term-block .term-content').show();
        SetPopUpAtTop();
    });

    // POPUP Callout Function
    $('#popup-register').click(function() {
        $('#register-block').css('top', '0');
        $('#register-block .register-content').show();
        $('.sucess-block').hide();
        SetPopUpAtTop();
    });

    $('.register-header .video-btn').click(function() {
        $('#video-block').css('top', '0');
        $('#register-block').css('top', '-9999px');
        SetPopUpAtTop();
    });

    $('#video-block .hide-popup').click(function() {

        $('#video-block').css('top', '-9999px');
        $('#register-block').css('top', '0px');
        SetPopUpAtTop();
    });

    $('.pending-btn .login-btn').click(function() {
        $(this).closest('td').find('#pending-popup').css('top', '0');
        SetPopUpAtTop();
    });

    $('.header-cart-btn .currency').click(function() {
        $('#currency-block').css('top', '0');
        $('#currency-block .currency-content').show();
        SetPopUpAtTop();
    });

    $('.cart-caption').click(function() {
        $('#shop-popup').css('top', '0');
        $('#shop-popup .shop-popup').show();
        SetPopUpAtTop();
    });

    // Resolve My Profile for iPod / iPhone  and iPad
    SetLinkforIpadForAnchorTag($('.innernNavigation li ul li a, .innernNavigation li div a, .row-link a,.order-links li a,.close-cart'), window['eventVar']);

    $(".header-cart-btn a").bind("touchend", function(e) {
        var el = $(this);
        var link = el.attr('href');
        window.location = link;
    });

    function SetLinkforIpadForAnchorTag(object, event) {
        $(object).bind(event, function(e) {
            var el = $(this);
            var link = el.attr('href');
            if($(".ctrlinfochanged").val().trim().toLowerCase() == "false")
            {   
                window.location = link;
            }
        });
    }

    // POPUP Close function
    if ($(".popupInner").size() > 0)
        BindPopUpCloseEvents();

    //$(".popupInner").mouseup(function() {
    // return false
    //});

    //    $('.popup-outer').mouseup(function(e) {
    //        if ($(e.target).parent("a.popup-openlink").length == 0) {
    //            $('.popup-outer').css('top', '-9999px');
    //        }
    //    });
    //    $('.close-btn').click(function() {
    //        $('.popup-outer').css('top', '-9999px');
    //    });
    //    $('.cancel-btn').click(function() {
    //        $('.popup-outer').css('top', '-9999px');
    //    });

    //    // Accordion and Sub Menu Both Work
    //    $('#accordion li').children('ul').hide();
    //    $('#accordion li.first').addClass('active').find('ul:first').show();
    //    $('#accordion li.active').find('ul').show();
    //    $('#accordion a').click(function() {
    //        $(this).parent().siblings('.active').removeClass('active').find('ul').slideUp('fast');
    //        if ($(this).parent().hasClass('active')) {
    //            $(this).next('ul').slideUp('fast');
    //            $(this).parent().removeClass('active');
    //        } else {
    //            $(this).next('ul').slideDown('fast');
    //            $(this).parent().addClass('active');
    //        }
    //    });

    $('.admin-linkslist li').click(function() {
        $('.admin-linkslist li').toggleClass("active", false);
        // if ($(this).parents('div').hasClass('asset-inner'))
        var pdiv = $(this).parents('div');
        if (!pdiv.hasClass('asset-inner'))
            $('.asset-inner').toggle(false);
        $(this).toggleClass("active", true);
        var TagClassName = $(this).attr("data-content");
        $("." + TagClassName).slideToggle();
    });

    $('#request-quote-popup').css('top', '0');
    $('#request-quote-popup .request-content').show();

    var tabindex = 1;
    $('.warranty-form').find('input,select,textarea').each(function() {
        if (this.type != "hidden") {
            var $input = $(this);
            $input.attr("tabindex", tabindex);

            // select the first one.
            if (tabindex == 1) {
                $input.select();
            }
            tabindex++;
        }
    });

    Array.prototype.contains = function(element) {
        return this.indexOf(element) > -1;
    };
    
    $("div.parentdiv").on("click", function (e) {
         $(this).next(".childdiv").slideToggle('slow');
    });

    $(".onlynumber").bind('cut copy paste contextmenu', function (e) {
        e.preventDefault();
    }).autocomplete({ 
        disabled: true 
    }).on('keypress', function(evt) {
        return $.onlyNumber(evt);
    });
});     //$(document).ready(function  ends here...

$.generatePassword = function (pwdLength) {    
    var pwd = "";
    var pwdChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNPQRSTUVWXYZ123456789!@#$%^&*";
    
    while ((pwdLength--) > 0) {
        pwd += pwdChars[~~(Math.ceil(Math.random() * pwdChars.length - 1))];
    }
      
    return pwd;
}

$.properCase = function (ele) {    
    var capText = ele.value.toLowerCase();
    
    if (capText.length > 0 && capText.indexOf(' ') > 0) {    
        var wordArray = capText.split(' ');

        var numWords = wordArray.length;

        for (x = 0; x < numWords; x++) {        
            wordArray[x] = wordArray[x].replace(wordArray[x].charAt(0), wordArray[x].charAt(0).toUpperCase());
    	    
            if (x == 0 && capText.length > 0)
	            capText = wordArray[x] + " ";
            else if (x != numWords - 1)
                capText = capText + wordArray[x] + " ";
            else if (x == numWords - 1)
	            capText = capText+wordArray[x];
        }
    }
    else {
        capText = capText.replace(capText.charAt(0), capText.charAt(0).toUpperCase());
    }
    
    ele.value = capText;    
}


function SetPopUpAtTop(IsWindowResize) {
    $('.popupInner').each(function() {
        $(this).css('margin-top', '78px');
    });
    if(!IsWindowResize)
		scrollTo(0,0);
}

$.tabs = function () {
    $(".tabs li.active").each(function() {
       $("." + $(this).attr("tab-id")).addClass("active").show();        
    });

    //window['eventVar'] will be "touchstart" for Apple Product,
    //and it will be "click" for Normal Browser
    $(".tabs li").on(window['eventVar'], function() {    
        $("#hdnClickedTab").val($(this).attr("tab-id"));        
        $.changeTab(this, !$(this).hasClass("checkchanges"));
    });
}

$.changeTab = function(liCtrl, ignoreChanges) {
    if($("#hdnTabInfoChanged").val().trim().toLowerCase() != "true" || ignoreChanges) {
        $(liCtrl).siblings("li").removeClass("active");
        $(liCtrl).addClass("active");

        var currDiv = $(liCtrl).attr("tab-id");

        if (!$(liCtrl).parent("ul").hasClass("structdiff"))
            $(liCtrl).parent("ul").siblings("div.current-tab").removeClass("active").hide();
        else 
            $(liCtrl).parents("div.narrowcolumn").siblings("div.widecolumn").find("div.current-tab").removeClass("active").hide();
            
        $("." + currDiv).addClass("active").fadeIn(1000);
        $("#hdnTabInfoChanged").val("false");
        
        var activeTabSection = $("." + currDiv).find("li.tab-section.active");
        
        if ($(liCtrl).hasClass("tab-section") || $(activeTabSection).length > 0) {            
            var hdnClickedSection = $("input[type=hidden][id$='hdnCurrentSection']");
            if ($(hdnClickedSection) != null && $(hdnClickedSection).length == 1) {
                if ($(liCtrl).hasClass("tab-section"))
                    $(hdnClickedSection).val(currDiv);
                else
                    $(hdnClickedSection).val($(activeTabSection).attr("tab-id"));
            }
        }
    }
    else {
        GeneralConfirmationMsg("Would you like to save changes before leaving?");
    }
}
 
$.onlyNumber = function(evt) {
    var k = evt.which || evt.keyCode;
    if( (k > 47 && k <58) || k == 46 || k == 144 || k == 145 || (k >= 8 && k <= 13) || (k >= 35 && k <= 40) || (k >= 112 && k <= 123))
        return true;
    else
        return false;
}