//custom
//start

//Important  Note onchange="setTimeout('assigndesign();',250);"//
// This assigndesign is called on onchange of dropdownlsit of asp control where selection 
// event is fired.
assigndesign = function() {
    $("#BaseStationForCountry").msDropDown({ mainCSS: 'dd2' });
    
    /*
   
    $("#Department").msDropDown({ mainCSS: 'dd2' });
    $("#Gender").msDropDown({ mainCSS: 'dd2' });
    $("#Workgroup").msDropDown({ mainCSS: 'dd2' });
    $("#Region").msDropDown({ mainCSS: 'dd2' });*/
    
    //alert('m here');
    //custom select box js
    $(".custom-sel").find("select").css({ opacity: 0.0 });
    $(".custom-sel").append("<span class='slc'> <span class='src'></span></span>");
    $(".custom-sel select").each(function() {

        var sel = $(this).find("option[selected]").text();

        $(this).next("span.slc").find("span.src").html(sel);
    });
    
    //custom select box js  
};
//end

$(document).ready(function() {
    //custom check box js

    $(".custom-checkbox ,.custom-checkbox_checked , .station_checkbox ,.station_checkbox_checked , .checkout_checkbox , .checkout_checkbox_checked , .wheather_check , .wheather_checked ").find("input").css({ opacity: 0.0 });
    $(".custom-checkbox ,.custom-checkbox_checked , .station_checkbox ,.station_checkbox_checked , .checkout_checkbox , .checkout_checkbox_checked , .wheather_check , .wheather_checked ").append("<span> </span>");
    $(".custom-checkbox ,.custom-checkbox_checked , .station_checkbox ,.station_checkbox_checked , .checkout_checkbox , .checkout_checkbox_checked , .wheather_check , .wheather_checked ").find("span").addClass("checkbox-on");
    $(".icheckbox_flat,.custom-checkbox input , .custom-checkbox_checked input , .station_checkbox input , .station_checkbox_checked input, .checkout_checkbox input , .checkout_checkbox_checked input , .wheather_check input , .station_checkbox_checked input , .wheather_checked input ").click(function() {        
        $(this).next().toggleClass("checkbox-off");
        $(this).next().toggleClass("checkbox-on");
        if ($(this).parent().hasClass(".gridheader"))
            SetContentCheckbox(this);
        else if ($(this).parent().hasClass(".gridcontent"))
            SetHeaderCheckbox(this);
        else if ($(this).parent().hasClass(".chkHeader"))
            SetAccessRightsCheckbox(this, "header");
        else if ($(this).parent().hasClass(".chkLine"))
            SetAccessRightsCheckbox(this, "line");


    });
    ////added by prashant
    function SetAccessRightsCheckbox(CheckBoxCtrl, type) {
        if (type == "header") {
            var HeaderID = $(CheckBoxCtrl).attr("id");
            var chkID = HeaderID.substring(HeaderID.indexOf("chk"), HeaderID.indexOf("All"));
            //             alert($(CheckBoxCtrl).is(":checked"));
            if ($(CheckBoxCtrl).is(":checked")) {
                $(".chkLine :checkbox[id$='" + chkID + "']").attr("checked", true);
                $(".chkLine :checkbox[id$='" + chkID + "']").siblings("span").toggleClass("checkbox-off", true);
                $(".chkLine :checkbox[id$='" + chkID + "']").siblings("span").toggleClass("checkbox-on", false);
            }
            else {
                $(".chkLine :checkbox[id$='" + chkID + "']").attr("checked", false);
                $(".chkLine :checkbox[id$='" + chkID + "']").siblings("span").toggleClass("checkbox-off", false);
                $(".chkLine :checkbox[id$='" + chkID + "']").siblings("span").toggleClass("checkbox-on", true);
            }

        }
        else if (type == "line") {
            var LineID = $(CheckBoxCtrl).attr("id");
            var chkID = LineID.substring(LineID.indexOf("chk"), LineID.length);
            var totalCheckbox = $(".chkLine :checkbox[id$='" + chkID + "']").length;
            var totalCheckedCheckbox = $(".chkLine :checkbox[id$='" + chkID + "']:checked").length;
            chkID = chkID + 'All';
            //alert(totalCheckbox +"=="+totalCheckedCheckbox);
            //alert(chkID);
            if (totalCheckbox == totalCheckedCheckbox) {
                $(".chkHeader :checkbox[id$='" + chkID + "']").attr("checked", true);
                $(".chkHeader :checkbox[id$='" + chkID + "']").siblings("span").toggleClass("checkbox-off", true);
                $(".chkHeader :checkbox[id$='" + chkID + "']").siblings("span").toggleClass("checkbox-on", false);
            }
            else {
                $(".chkHeader :checkbox[id$='" + chkID + "']").attr("checked", false);
                $(".chkHeader :checkbox[id$='" + chkID + "']").siblings("span").toggleClass("checkbox-off", false);
                $(".chkHeader :checkbox[id$='" + chkID + "']").siblings("span").toggleClass("checkbox-on", true);
            }
        }

    }
    function SetContentCheckbox(HeaderCheckBox) {
        if ($(HeaderCheckBox).is(":checked")) {
            $(HeaderCheckBox).parents(".orderreturn_box").find(".gridcontent").each(function() {
                $(this).children("input").attr("checked", "checked");
                $(this).children("span").toggleClass("checkbox-off", true);
                $(this).children("span").toggleClass("checkbox-on", false);
            });
        }
        else {
            $(HeaderCheckBox).parents(".orderreturn_box").find(".gridcontent").each(function() {
                $(this).children("input").removeAttr("checked");
                $(this).children("span").toggleClass("checkbox-off", false);
                $(this).children("span").toggleClass("checkbox-on", true);
            });
        }
    }
    function SetHeaderCheckbox(ContentCheckBox) {
        var TotalRows = $(ContentCheckBox).parents(".orderreturn_box").find(".gridcontent").length;
        var CheckedCount = 0;

        $(ContentCheckBox).parents(".orderreturn_box").find(".gridcontent").each(function() {
            if ($(this).children("input").is(":checked"))
                CheckedCount = CheckedCount + 1;
        });
        if (TotalRows == CheckedCount) {
            $(ContentCheckBox).parents(".orderreturn_box").find(".gridheader").children("input").attr("checked", "checked");
            $(ContentCheckBox).parents(".orderreturn_box").find(".gridheader").children("span").toggleClass("checkbox-off", true);
            $(ContentCheckBox).parents(".orderreturn_box").find(".gridheader").children("span").toggleClass("checkbox-on", false);
        }
        else {
            $(ContentCheckBox).parents(".orderreturn_box").find(".gridheader").children("input").removeAttr("checked");
            $(ContentCheckBox).parents(".orderreturn_box").find(".gridheader").children("span").toggleClass("checkbox-off", false);
            $(ContentCheckBox).parents(".orderreturn_box").find(".gridheader").children("span").toggleClass("checkbox-on", true);
        }
    }
    ////added by prashant

    //custom select box js
    $(".custom-sel").find("select").css({ opacity: 0.0 });
    $(".custom-sel").append("<span class='slc'> <span class='src'></span></span>");
    $(".custom-sel select").each(function() {

        var sel = $(this).find("option[selected]").text();

        $(this).next("span.slc").find("span.src").html(sel);
    });

    $(".custom-sel select").change(function() {

        var sel = $(this).find("option[selected]").text();

        var optionValueText = jQuery.trim(jQuery($(this) + 'select:selected').text());

        // alert(optionValueText);

        //$(this).next("span.slc").find("span.src").html(sel);
    });
    //custom select box js


    //--------------

    $(".custom_sel_name").find("select").css({ opacity: 0.0 });
    $(".custom_sel_name").append("<span class='slc'> <span class='src'></span></span>");
    $(".custom_sel_name select").each(function() {
        var sel = $(this).find("option[selected]").text();
        $(this).next("span.slc").find("span.src").html(sel);
    });

    $(".custom_sel_name select").change(function() {
        var sel = $(this).find("option[selected]").text();

        $(this).next("span.slc").find("span.src").html(sel);
    });

    //--------------


    $(".custom_sel_qty").find("select").css({ opacity: 0.0 });
    $(".custom_sel_qty").append("<span class='slc'> <span class='src'></span></span>");
    $(".custom_sel_qty select").each(function() {
        // var sel = $(this).val();
        var sel = $(this).find("option[selected]").text();

        $(this).next("span.slc").find("span.src").html(sel);
    });

    $(".custom_sel_qty select").change(function() {
        // var sel = $(this).val();
        var sel = $(this).find("option[selected]").text();

        $(this).next("span.slc").find("span.src").html(sel);

    });


    // custom radio button

    $(".custom_radio").find("input").css({ opacity: 0.0 });
    $(".custom_radio").append("<span> </span>");
    $(".custom_radio").find("span").addClass("radio-on");
    $(".custom_radio input ").click(function() {
        $(this).next().toggleClass("radio-off");
    });

});


/*
// Copy the following functions on page wherever required

$(function() {
$(".datepicker").datepicker({
buttonText: 'Date',
showOn: 'button',
buttonImage: 'images/calender-icon.jpg',
buttonImageOnly: true
});
});


$(function() {
$(".datepickersmall").datepicker({
buttonText: 'Date',
showOn: 'button',
buttonImage: 'images/datepicker-i-small.png',
buttonImageOnly: true
});
});


*/


//------------------ Custom Textarea Javascript ------------------------//


function scrolltextarea(Scrollarea, scrollTop2, Scrollbottom) {
    Scrollarea = $(Scrollarea), scrollTop2 = $(scrollTop2), Scrollbottom = 

$(Scrollbottom);

    stextarey = Scrollarea.height();
    stextareyTo = Scrollarea.get(0).scrollHeight;
    //alert(stextarey+" "+stextareyTo);
    if (stextarey == stextareyTo) {
        scrollTop2.hide();
        Scrollbottom.hide();
    };

    Scrollarea.live('keypress', function() {
        var textarey = Scrollarea.height();
        textareyTo = Scrollarea.get(0).scrollHeight;
        currentScrollPosition = Scrollarea.scrollTop();
        getScrollValue = textareyTo - textarey;
        Scrollarea.stop();
        //alert(currentScrollPosition+" "+textareyTo +", "+getScrollValue);
        //scrollTop2.show();
        if (currentScrollPosition == getScrollValue) {
            //alert('for test');
            scrollTop2.show();
            Scrollbottom.hide();
        }
        if (currentScrollPosition == 0) {
            //alert('for test');
            scrollTop2.hide();
            Scrollbottom.show();
        };
    });

    Scrollbottom.live('mousedown', function() {
        var textarey = Scrollarea.height();
        var textareyTo = Scrollarea.get(0).scrollHeight;
        currentScrollPosition = Scrollarea.scrollTop();
        getScrollValue = textareyTo - textarey;
        //alert(currentScrollPosition);
        $(Scrollarea).animate({ scrollTop: getScrollValue }, textareyTo * 4);



    });
    Scrollbottom.mouseup(function() {
        //alert("out")
        //var i = 0;
        var textarey = Scrollarea.height();
        textareyTo = Scrollarea.get(0).scrollHeight;
        currentScrollPosition = Scrollarea.scrollTop();
        getScrollValue = textareyTo - textarey;
        Scrollarea.stop();
        //alert(currentScrollPosition+" "+textareyTo +", "+getScrollValue);
        scrollTop2.show();
        if (currentScrollPosition == getScrollValue) {
            //alert('for test');
            Scrollbottom.hide();
        };
        //scrolltextarea().stop();

    });

    scrollTop2.live('mousedown', function() {
        var textarey = Scrollarea.height();
        var textareyTo = Scrollarea.get(0).scrollHeight;
        currentScrollPosition = Scrollarea.scrollTop();
        getScrollValue = textareyTo - textarey;

        Scrollarea.animate({ scrollTop: -getScrollValue }, textareyTo * 4);

    });
    scrollTop2.mouseup(function() {
        //alert("out")
        //var i = 0;
        var textarey = Scrollarea.height();
        textareyTo = Scrollarea.get(0).scrollHeight;
        currentScrollPosition = Scrollarea.scrollTop();
        getScrollValue = textareyTo - textarey;
        //alert(currentScrollPosition+" "+textareyTo +", "+getScrollValue);
        Scrollarea.stop();
        Scrollbottom.show();
        if (currentScrollPosition == 0) {

            scrollTop2.hide();
        };
        //scrolltextarea().stop();
    });

};



$(document).ready(function() {
    $(".gallery a[rel^='prettyPhoto']").prettyPhoto({ theme: 'facebook' });
});

//added by prashant/devraj to Get the Country Code for ZipCode Validation
function GetCountryCode(value)
{
    if (value == "United States")//hard coded for United States
        return "US";
    else if (value == "Canada")//hard coded for Canada
        return "CA";
    else
        return "default";
}
