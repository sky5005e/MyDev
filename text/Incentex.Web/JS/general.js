$(document).ready(function() {
    //custom check box js

    $(".custom-checkbox , .station_checkbox , .wheather_check ").find("input").css({ opacity: 0.0 });
    $(".custom-checkbox , .station_checkbox , .wheather_check ").append("<span> </span>");
    $(".custom-checkbox , .station_checkbox , .wheather_check").find("span").addClass("checkbox-on");
    $(".custom-checkbox input , .station_checkbox input , .wheather_check input").click(function() {
        alert("2call");
        $(this).next().toggleClass("checkbox-off");
        $(this).next().toggleClass("checkbox-on");
    });
	

    //custom select box js
    $(".custom-sel").find("select").css({ opacity: 0.0 });
    $(".custom-sel").append("<span class='slc'> <span class='src'></span></span>");
    $(".custom-sel select").each(function() {
        var sel = $(this).val();
        $(this).next("span.slc").find("span.src").html(sel);
    });

    $(".custom-sel select").change(function() {
        var sel = $(this).val();

        $(this).next("span.slc").find("span.src").html(sel);
    });
    //custom select box js


    //--------------

    $(".custom_sel_name").find("select").css({ opacity: 0.0 });
    $(".custom_sel_name").append("<span class='slc'> <span class='src'></span></span>");
    $(".custom_sel_name select").each(function() {
        var sel = $(this).val();
        $(this).next("span.slc").find("span.src").html(sel);
    });

    $(".custom_sel_name select").change(function() {
        var sel = $(this).val();

        $(this).next("span.slc").find("span.src").html(sel);
    });

    //--------------

    $(".custom_sel_qty").find("select").css({ opacity: 0.0 });
    $(".custom_sel_qty").append("<span class='slc'> <span class='src'></span></span>");
    $(".custom_sel_qty select").each(function() {
        var sel = $(this).val();
        $(this).next("span.slc").find("span.src").html(sel);
    });

    $(".custom_sel_qty select").change(function() {
        var sel = $(this).val();

        $(this).next("span.slc").find("span.src").html(sel);
    });

});



$(function() {
    $(".datepicker").datepicker({
        buttonText: 'DatePicker',
        showOn: 'button',
        buttonImage: 'images/calender-icon.jpg',
        buttonImageOnly: true
    });
});


$(function() {
    $(".datepickersmall").datepicker({
        buttonText: 'DatePicker',
        showOn: 'button',
        buttonImage: 'images/datepicker-i-small.png',
        buttonImageOnly: true
    });
});

//-------------------Validation For Date Start And End Date------------------------------------//

//-------------------------------------------------------//



//------------------ Custom Textarea Javascript ------------------------//


function scrolltextarea(Scrollarea, scrollTop2, Scrollbottom) {
    Scrollarea = $(Scrollarea), scrollTop2 = $(scrollTop2), Scrollbottom = $(Scrollbottom);

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


