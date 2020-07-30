// JScript File

(function ($) {
    //added by Prakash (17-July-09)
    $.fn.hint = function (blurClass) {
      if (!blurClass) { 
        blurClass = 'blur';
      }
        
      return this.each(function () {
        // get jQuery version of 'this'
        var $input = $(this),
        
        // capture the rest of the variable to allow for reuse
          title = $input.attr('title'),
          $form = $(this.form),
          $win = $(window);

        function remove() {
          if ($input.val() === title && $input.hasClass(blurClass)) {
            $input.val('').removeClass(blurClass);
          }
        }

        // only apply logic if the element has the attribute
        if (title) { 
          // on blur, set value to title attr if text is blank
          $input.blur(function () {
            if (this.value === '') {
              $input.val(title).addClass(blurClass);
            }
          }).focus(remove).blur(); // now change all inputs to title
          
          // clear the pre-defined text when form is submitted
          $form.submit(remove);
          $win.unload(remove); // handles Firefox's autocomplete
        }
      });
    },
    //added by Prakash (03-Jun-09)
    $.showprogress = function(progTit, progText, progImg)
    {
        $.hideprogress();
        $("BODY").append('<div id="processing_overlay"></div>');
        $("BODY").append(
		  '<div id="processing_container">' +
		    '<h1 id="processing_title">' + progTit + '</h1>' +
		    '<div id="processing_content">' +
		      '<div id="processing_message">'+ progText +'<br/><br/><img src="images/ajax-loader.gif" style="border: none" alt="Loading..." /></div>' +
			'</div>' +
		  '</div>');
		 
		var pos = ($.browser.msie && parseInt($.browser.version) <= 6 ) ? 'absolute' : 'fixed'; 
		
		$("#processing_overlay").css({
		    opacity:0.5
		});    
		$("#processing_container").css({
			position: pos,
			zIndex: 99999,
			padding: 0,
			margin: 0
		});
		
		$("#processing_container").css({
			minWidth: $("#processing_container").outerWidth(),
			maxWidth: $("#processing_container").outerWidth()
		});
		  
		var top = (($(window).height() / 2) - ($("#processing_container").outerHeight() / 2)) + (-75);
		var left = (($(window).width() / 2) - ($("#processing_container").outerWidth() / 2)) + 0;
		if( top < 0 ) top = 0;
		if( left < 0 ) left = 0;
		
		// IE6 fix
		if( $.browser.msie && parseInt($.browser.version) <= 6 ) top = top + $(window).scrollTop();
		
		$("#processing_container").css({
			top: top + 'px',
			left: left + 'px'
		});
		$("#processing_overlay").height( $(document).height() );
		$("BODY select").hide();
    },
    
    $.hideprogress = function()
    {
        $("#processing_container").remove();
        $("#processing_overlay").remove();
        $("BODY select").show();
    },
    
    //added by Prakash (03-Jun-09)
    $.showmsg = function(msgEle,msgType,msgText,autoHide){
        var tblMsg;
        
        if(msgType == 'error')
            tblMsg = '<div class="error_lftmargin"><img src="images/a1_error_leftbg.gif" width="17" height="37" alt="" class="fl" /><div class="fl error_midbg error_midw error_red"><img src="images/a1_red_error_icn.gif" width="28" height="28" alt="" class="fl error_rgticn" />' + msgText + '</div><div class="fl error_rgtbg"></div></div><div class="cl"></div><br />';
        else if(msgType == 'info')
            tblMsg = '<div class="error_lftmargin"><img src="images/a1_error_leftbg.gif" width="17" height="37" alt="" class="fl" /><div class="fl error_midbg error_midw error_gray"><img src="images/a1_alert_icn.gif" width="28" height="28" alt="" class="fl error_rgticn" />' + msgText + '</div><div class="fl error_rgtbg"></div></div><div class="cl"></div><br />';
        else if(msgType == 'infomsg')
            tblMsg = '<div class="error_lftmargin"><img src="images/a1_error_leftbg.gif" width="17" height="37" alt="" class="fl" /><div class="fl error_midbg error_midw error_gray"><img src="images/a1_alert_icn.gif" width="28" height="28" alt="" class="fl error_rgticn" />' + msgText + '</div><div class="fl error_rgtbg"></div></div><div class="cl"></div><br />';    
        else if(msgType == 'success')
            tblMsg = '<div class="error_lftmargin"><img src="images/a1_error_leftbg.gif" width="17" height="37" alt="" class="fl" /><div class="fl error_midbg error_midw error_green"><img src="images/a1_tick_icn.gif" width="28" height="28" alt="" class="fl error_rgticn" />' + msgText + '</div><div class="fl error_rgtbg"></div></div><div class="cl"></div><br />';
            
        //alert(tblMag);
        $("#" + msgEle).html(tblMsg);
        $("#" + msgEle).show();
        if(autoHide)
        {
            setTimeout(function(){
                $('#' + msgEle).fadeOut('normal')},10000    
	        );
        }
    }
})(jQuery);    