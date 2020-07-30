
/*# AVOID COLLISIONS #*/
;if (window.jQuery) (function ($) {
    /*# AVOID COLLISIONS #*/
    var CurrFileSize = 0;
    // plugin initialization
    $.fn.MultiFile = function (options) {
        if (this.length == 0) return this; // quick fail
        // Handle API methods
        if (typeof arguments[0] == 'string') {
            // Perform API methods on individual elements
            if (this.length > 1) {
                var args = arguments;
                return this.each(function () {
                    $.fn.MultiFile.apply($(this), args);
                });
            };
            // Invoke API method handler
            $.fn.MultiFile[arguments[0]].apply(this, $.makeArray(arguments).slice(1) || []);
            // Quick exit...
            return this;
        };

        // Initialize options for this call
        var options = $.extend(
			{}/* new object */,
			$.fn.MultiFile.options/* default options */,
			options || {} /* just-in-time options */
		);

        // Empty Element Fix!!!
        // this code will automatically intercept native form submissions
        // and disable empty file elements
        $('form')
		.not('MultiFile-intercepted')
		.addClass('MultiFile-intercepted')
		.submit($.fn.MultiFile.disableEmpty);


        // utility method to integrate this plugin with others...
        if ($.fn.MultiFile.options.autoIntercept) {
            $.fn.MultiFile.intercept($.fn.MultiFile.options.autoIntercept /* array of methods to intercept */);
            $.fn.MultiFile.options.autoIntercept = null; /* only run this once */
        };

        // loop through each matched element
        this
		 .not('.MultiFile-applied')
			.addClass('MultiFile-applied')
		.each(function () {
		    //#####################################################################
		    // MAIN PLUGIN FUNCTIONALITY - START
		    //#####################################################################

		    window.MultiFile = (window.MultiFile || 0) + 1;
		    var group_count = window.MultiFile;

		    var MultiFile = { e: this, E: $(this), clone: $(this).clone() };

		    //===

		    //# USE CONFIGURATION
		    if (typeof options == 'number') options = { max: options };
		    var o = $.extend({},
        $.fn.MultiFile.options,
        options || {},
   					($.metadata ? MultiFile.E.metadata() : ($.meta ? MultiFile.E.data() : null)) || {}, /* metadata options */
								{} /* internals */
       );
		    // limit number of files that can be selected?
		    if (!(o.max > 0) /*IsNull(MultiFile.max)*/) {
		        o.max = MultiFile.E.attr('maxlength');
		        if (!(o.max > 0) /*IsNull(MultiFile.max)*/) {
		            o.max = (String(MultiFile.e.className.match(/\b(max|limit)\-([0-9]+)\b/gi) || ['']).match(/[0-9]+/gi) || [''])[0];
		            if (!(o.max > 0)) o.max = -1;
		            else o.max = String(o.max).match(/[0-9]+/gi)[0];
		        }
		    };
		    o.max = new Number(o.max);
		    // limit extensions?
		    o.accept = o.accept || MultiFile.E.attr('accept') || '';
		    o.allowedextensionmsg = o.allowedextensionmsg || MultiFile.E.attr('allowedextensionmsg') || '';
		    if (!o.accept) {
		        o.accept = (MultiFile.e.className.match(/\b(accept\-[\w\|]+)\b/gi)) || '';
		        o.accept = new String(o.accept).replace(/^(accept|ext)\-/i, '');
		    };

		    //===

		    // APPLY CONFIGURATION
		    $.extend(MultiFile, o || {});
		    MultiFile.STRING = $.extend({}, $.fn.MultiFile.options.STRING, MultiFile.STRING);

		    //===

		    //#########################################
		    // PRIVATE PROPERTIES/METHODS
		    $.extend(MultiFile, {
		        n: 0, // How many elements are currently selected?
		        slaves: [], files: [],
		        instanceKey: MultiFile.e.id || 'MultiFile' + String(group_count), // Instance Key?
		        generateID: function (z) { return MultiFile.instanceKey + (z > 0 ? '_F' + String(z) : ''); },
		        trigger: function (event, element) {
		            var handler = MultiFile[event], value = $(element).attr('value');
		            if (handler) {
		                var returnValue = handler(element, value, MultiFile);
		                if (returnValue != null) return returnValue;
		            }
		            return true;
		        }
		    });

		    //===

		    if (String(MultiFile.accept).length > 1) {
		        MultiFile.accept = MultiFile.accept.replace(/\W+/g, '|').replace(/^\W|\W$/g, '');
		        MultiFile.rxAccept = new RegExp('\\.(' + (MultiFile.accept ? MultiFile.accept : '') + ')$', 'gi');
		    };

		    //===

		    // Create wrapper to hold our file list
		    MultiFile.wrapID = MultiFile.instanceKey + '_wrap'; // Wrapper ID?
		    MultiFile.E.wrap('<div class="MultiFile-wrap" id="' + MultiFile.wrapID + '"></div>');
		    MultiFile.wrapper = $('#' + MultiFile.wrapID + '');

		    //===

		    // MultiFile MUST have a name - default: file1[], file2[], file3[]
		    MultiFile.e.name = MultiFile.e.name || 'file' + group_count + '[]';

		    //===

		    if (!MultiFile.list) {
		        // Create a wrapper for the list
		        // * OPERA BUG: NO_MODIFICATION_ALLOWED_ERR ('list' is a read-only property)
		        // this change allows us to keep the files in the order they were selected
		        MultiFile.wrapper.append('<div class="MultiFile-list" id="' + MultiFile.wrapID + '_list"></div>');
		        MultiFile.list = $('#' + MultiFile.wrapID + '_list');
		    };
		    MultiFile.list = $(MultiFile.list);
		    //===

		    // Bind a new element
		    MultiFile.addSlave = function (slave, slave_count) {
		        //if(window.console) console.log('MultiFile.addSlave',slave_count);

		        // Keep track of how many elements have been displayed
		        MultiFile.n++;
		        // Add reference to master element
		        slave.MultiFile = MultiFile;

		        // Clear identifying properties from clones
		        if (slave_count > 0) slave.id = slave.name = '';

		        // Define element's ID and name (upload components need this!)
		        //slave.id = slave.id || MultiFile.generateID(slave_count);
		        if (slave_count > 0) slave.id = MultiFile.generateID(slave_count);

		        slave.name = String(MultiFile.namePattern
		        /*master name*/.replace(/\$name/gi, $(MultiFile.clone).attr('name'))
		        /*master id  */.replace(/\$id/gi, $(MultiFile.clone).attr('id'))
		        /*group count*/.replace(/\$g/gi, group_count)//(group_count>0?group_count:''))
		        /*slave count*/.replace(/\$i/gi, slave_count)//(slave_count>0?slave_count:''))
        );

		        // If we've reached maximum number, disable input slave
		        if ((MultiFile.max > 0) && ((MultiFile.n - 1) > (MultiFile.max)))//{ // MultiFile.n Starts at 1, so subtract 1 to find true count
		            slave.disabled = true;
		        //};

		        // Remember most recent slave
		        MultiFile.current = MultiFile.slaves[slave_count] = slave;

		        // We'll use jQuery from now on
		        slave = $(slave);

		        // Clear value
		        slave.val('').attr('value', '')[0].value = '';

		        //		        alert(slave[0].files[0].size);

		        // Stop plugin initializing on slaves
		        slave.addClass('MultiFile-applied');

		        // Triggered when a file is selected
		        slave.change(function () {
		            //if(window.console) console.log('MultiFile.slave.change',slave_count);
		            CurrFileSize = slave[0].files[0].size;
		            // alert(slave[0].files[0].size);
		            // Lose focus to stop IE7 firing onchange again
		            $(this).blur();

		            //# Trigger Event! onFileSelect
		            if (!MultiFile.trigger('onFileSelect', this, MultiFile)) return false;
		            //# End Event!

		            //# Retrive value of selected file from element
		            var ERROR = '', v = String(this.value || ''/*.attr('value)*/);

                    
		            // check extension
		            if (MultiFile.accept && v && !v.match(MultiFile.rxAccept))//{
		                ERROR = MultiFile.STRING.denied.replace('$ext', String(v.match(/\.\w{1,4}$/gi))).replace('$acpt', MultiFile.allowedextensionmsg);
		                //$acpt
		            //}
		            //};

		            // Disallow duplicates
		            for (var f in MultiFile.slaves)//{
		                if (MultiFile.slaves[f] && MultiFile.slaves[f] != this)//{
		                //console.log(MultiFile.slaves[f],MultiFile.slaves[f].value);
		                    if (MultiFile.slaves[f].value == v)//{
		                        ERROR = MultiFile.STRING.duplicate.replace('$file', v.match(/[^\/\\]+$/gi));
		            //};
		            //};
		            //};

		            // Create a new file input element
		            var newEle = $(MultiFile.clone).clone(); // Copy parent attributes - Thanks to Jonas Wagner
		            //# Let's remember which input we've generated so
		            // we can disable the empty ones before submission
		            newEle.addClass('MultiFile');

		            // Handle error
		            if (ERROR != '') {
		                // Handle error
		                MultiFile.error(ERROR);

		                // Ditch the trouble maker and add a fresh new element
		                MultiFile.n--;
		                MultiFile.addSlave(newEle[0], slave_count);
		                slave.parent().prepend(newEle);
		                slave.remove();
		                return false;
		            };

		            // Hide this element (NB: display:none is evil!)
		            $(this).css({ position: 'absolute', top: '-3000px' });
		            //alert(MultiFile.attr("class");
		            $(this).removeClass("filupCtrl");
		            // Add new element to the form
		            slave.after(newEle);

		            // Update list
		            MultiFile.addToList(this, slave_count);

		            // Bind functionality
		            MultiFile.addSlave(newEle[0], slave_count + 1);

		            //# Trigger Event! afterFileSelect
		            if (!MultiFile.trigger('afterFileSelect', this, MultiFile)) return false;
		            //# End Event!

		        }); // slave.change()

		        // Save control to element
		        $(slave).data('MultiFile', MultiFile);


		    }; // MultiFile.addSlave
		    // Bind a new element



		    // Add a new file to the list
		    MultiFile.addToList = function (slave, slave_count) {
		        //if(window.console) console.log('MultiFile.addToList',slave_count);

		        //# Trigger Event! onFileAppend
		        if (!MultiFile.trigger('onFileAppend', slave, MultiFile)) return false;
		        //# End Event!

		        // Create label elements
                var DefaultIcon = "asset-img.jpg";
                var InputTextBoxCount = $(".custominputtextbox").length;
                var CurrentExtension = String(slave.value.match(/\.\w{1,4}$/gi));
		        var 
                 //r = $('<div class="file-box"><div class="file-box-head cf"><span class="files alignleft">Files(<span class="filecount"></span>)</span><span class="file-size alignright">Size(<span class="totalsizecount"></span>)</span></div></div>'),
                // sm = $('<div class="files-list"></div>'),
                 hd = $('<span class="sizecount" style="display:none;">' + CurrFileSize + '</span>'),
		         v = String(slave.value || ''/*.attr('value)*/),
                // a = $('<span class="MultiFile-title" title="' + MultiFile.STRING.selected.replace('$file', v) + '">' + MultiFile.STRING.file.replace('$file', v.match(/[^\/\\]+$/gi)[0]) + '</span>'),
                 //b = $('<a class="MultiFile-remove" href="#' + MultiFile.wrapID + '">' + MultiFile.STRING.remove + '</a>'),
                // b = $( '<a class="MultiFile-remove" href="#' + MultiFile.wrapID + '">' + MultiFile.STRING.remove + '<img src="../../NewDesign/img/asset-ico3.png" width="20" height="20"></a>'),
                 hover = '<ul class="asset-imagehover cf"><li style="display:none;"><a href="javascript: void(0);"><img src="../../NewDesign/img/asset-ico2.png" width="20" height="20"></a></li><li><a class="MultiFile-remove" href="' + MultiFile.wrapID + '"><img src="../../NewDesign/img/asset-ico3.png" width="20" height="20"></a></li><li style="display:none;"><a href="javascript: void(0);"><img src="../../NewDesign/img/asset-ico1.png" width="20" height="20"></a></li></ul>',
                // szHtml = $('<span style="float:right;margin: 0px 60px 0 0;"></span>'),
		        // sz = GetFileSize(CurrFileSize);

                r = $('<div class="asset-img-block"></div>'),
                sm = '';
                
                var DefaultTitle = "Image Name";
                if(CurrentExtension.toLowerCase() == ".mp4")
                    DefaultTitle = "Video Name";
                    
                if(CurrentExtension.toLowerCase() == ".jpg" || CurrentExtension.toLowerCase() == ".jpeg" || CurrentExtension.toLowerCase() == ".gif" || CurrentExtension.toLowerCase() == ".mp4")
                    sm = $('<div class="asset-image clientside"><div class="asset-innerimage"><img src="../../NewDesign/img/asset-img.jpg" width="69" height="69" alt="' + MultiFile.STRING.file.replace('$file', v.match(/[^\/\\]+$/gi)[0]) + '" title="' + MultiFile.STRING.selected.replace('$file', v) + '">'+ hover + '</div><strong>' + DefaultTitle + '</strong><input type="text" id="txtFileName' + (InputTextBoxCount + 1) + '" name="txtFileName' + (InputTextBoxCount + 1) + '" class="input-field-small input-55 custominputtextbox" value="" onkeypress="return AllowForRenamingUploadedFile(event);"></div>');
                else
                    sm = $('<div class="asset-image clientside"><div class="assetdoc-innerimage"><img src="../../NewDesign/img/file-ico.png" width="79" height="79" alt="' + MultiFile.STRING.file.replace('$file', v.match(/[^\/\\]+$/gi)[0]) + '" title="' + MultiFile.STRING.selected.replace('$file', v) + '">'+ hover + '</div><strong>' + 'Name' + '</strong><input type="text" id="txtFileName' + (InputTextBoxCount + 1) + '" name="txtFileName' + (InputTextBoxCount + 1) + '" class="input-field-small input-55 custominputtextbox" value="" onkeypress="return AllowForRenamingUploadedFile(event);"></div>');                                              
                
		        var FileCount = $(".files-list").length;
		        // Insert label
                
                //alert($(MultiFile.list).attr("class") +"===" + $(MultiFile.list).hasClass("multifilediv"));
		        if(!$(MultiFile.list).hasClass("multifilediv"))
		            MultiFile.list = $(MultiFile.list).parent().parent().parent().parent().next("div");
		        //var dHTMLTAG = $(MultiFile.list).parent().parent().parent().parent().parent();
		       // alert($(MultiFile.list).attr("class") +"===" + $(MultiFile.list).hasClass("multifilediv"));
		        //alert(MultiFile.list.find(".asset-img-block").html() + "==" + MultiFile.list.find(".asset-img-block").val());
                if (MultiFile.list.find(".asset-img-block").html() == undefined )
                //|| MultiFile.list.find(".asset-img-block").html().trim() == ''
		            //MultiFile.list.append(r.append(sm.append(a, ' ', hd, ' ', szHtml.append(sz), ' ', b)));
		            MultiFile.list.append(r.append(sm));
		        else
		            //MultiFile.list.find(".file-box").append(sm.append(a, ' ', hd, ' ', szHtml.append(sz), ' ', b));
		            MultiFile.list.find(".asset-img-block").append(sm);

		        //MultiFile.list.find(".filecount").html((FileCount + 1));
		        //MultiFile.list.find(".totalsizecount").html(TotalSize());


		        MultiFile.list.find("a.MultiFile-remove").click(function () {

//                    debugger;
                    //alert("remove click");
		            //# Trigger Event! onFileRemove
		            if (!MultiFile.trigger('onFileRemove', slave, MultiFile)) return false;
		            //# End Event!

		           // var FileCount = $(".files-list").length;

		            MultiFile.n--;
		            MultiFile.current.disabled = false;

		            // Remove element, remove label, point to current
		            MultiFile.slaves[slave_count] = null;
		            $(slave).remove();
		            $(this).parent().parent().parent().parent().remove();

		            // Show most current element again (move into view) and clear selection
		            $(MultiFile.current).css({ position: '', top: '' });
		            $(MultiFile.current).reset().val('').attr('value', '')[0].value = '';

		            //# Trigger Event! afterFileRemove
		            if (!MultiFile.trigger('afterFileRemove', slave, MultiFile)) return false;
		            //# End Event!
                   
                    if($(MultiFile.list).find(".asset-image").length == 0)
                        $(MultiFile.list).find(".asset-img-block").remove();
		           
		            //MultiFile.list.find(".filecount").html((FileCount - 1));
		            //MultiFile.list.find(".totalsizecount").html(TotalSize());
		            return false;
		        });

		        //# Trigger Event! afterFileAppend
		        if (!MultiFile.trigger('afterFileAppend', slave, MultiFile)) return false;
		        //# End Event!
                
		    }; // MultiFile.addToList
		    // Add element to selected files list



		    // Bind functionality to the first element
		    if (!MultiFile.MultiFile) MultiFile.addSlave(MultiFile.e, 0);

		    // Increment control count
		    //MultiFile.I++; // using window.MultiFile
		    MultiFile.n++;

		    // Save control to element
		    MultiFile.E.data('MultiFile', MultiFile);


		    //#####################################################################
		    // MAIN PLUGIN FUNCTIONALITY - END
		    //#####################################################################
		}); // each element
    };

    /*--------------------------------------------------------*/

    /*
    ### Core functionality and API ###
    */
    $.extend($.fn.MultiFile, {
        /**
        * This method removes all selected files
        *
        * Returns a jQuery collection of all affected elements.
        *
        * @name reset
        * @type jQuery
        * @cat Plugins/MultiFile
        * @author Diego A. (http://www.fyneworks.com/)
        *
        * @example $.fn.MultiFile.reset();
        */
        reset: function () {
            var settings = $(this).data('MultiFile');
            //if(settings) settings.wrapper.find('a.MultiFile-remove').click();
//            alert(3456);
            if (settings) settings.list.find('a.MultiFile-remove').click();
            return $(this);
        },


        disableEmpty: function (klass) {
            klass = String(klass || 'mfD');
            var o = [];
            $('input:file').each(function () { if ($(this).val() == '') o[o.length] = this; });
            return $(o).each(function () { this.disabled = true }).addClass(klass);
        },


        reEnableEmpty: function (klass) {
            klass = String(klass || 'mfD');
            return $('input:file.' + klass).removeClass(klass).each(function () { this.disabled = false });
        },


        intercepted: {},
        intercept: function (methods, context, args) {
            var method, value; args = args || [];
            if (args.constructor.toString().indexOf("Array") < 0) args = [args];
            if (typeof (methods) == 'function') {
                $.fn.MultiFile.disableEmpty();
                value = methods.apply(context || window, args);
                //SEE-http://code.google.com/p/jquery-multifile-plugin/issues/detail?id=27
                setTimeout(function () { $.fn.MultiFile.reEnableEmpty() }, 1000);
                return value;
            };
            if (methods.constructor.toString().indexOf("Array") < 0) methods = [methods];
            for (var i = 0; i < methods.length; i++) {
                method = methods[i] + ''; // make sure that we have a STRING
                if (method) (function (method) { // make sure that method is ISOLATED for the interception
                    $.fn.MultiFile.intercepted[method] = $.fn[method] || function () { };
                    $.fn[method] = function () {
                        $.fn.MultiFile.disableEmpty();
                        value = $.fn.MultiFile.intercepted[method].apply(this, arguments);
                        setTimeout(function () { $.fn.MultiFile.reEnableEmpty() }, 1000);
                        return value;
                    }; // interception
                })(method); // MAKE SURE THAT method IS ISOLATED for the interception
            }; // for each method
        }
    });

    /*--------------------------------------------------------*/

    /*
    ### Default Settings ###
    eg.: You can override default control like this:
    $.fn.MultiFile.options.accept = 'gif|jpg';
    */
    $.fn.MultiFile.options = { //$.extend($.fn.MultiFile, { options: {
        accept: '', // accepted file extensions
        max: -1,    // maximum number of selectable files
        allowedextensionmsg: '',
        // name to use for newly created elements
        namePattern: '$name', // same name by default (which creates an array)

        // STRING: collection lets you show messages in different languages
        STRING: {
            remove: 'x',
            denied: 'You cannot select a $ext file. <br />Only $acpt are allowed',
            file: '$file',
            selected: 'File selected: $file',
            duplicate: 'This file has already been selected:\n$file'
        },

        // name of methods that should be automcatically intercepted so the plugin can disable
        // extra file elements that are empty before execution and automatically re-enable them afterwards
        autoIntercept: ['submit', 'ajaxSubmit', 'ajaxForm', 'validate' /* array of methods to intercept */],

        // error handling function
        error: function (s) {
            /*
            ERROR! blockUI is not currently working in IE
            if($.blockUI){
            $.blockUI({
            message: s.replace(/\n/gi,'<br/>'),
            css: { 
            border:'none', padding:'15px', size:'12.0pt',
            backgroundColor:'#900', color:'#fff',
            opacity:'.8','-webkit-border-radius': '10px','-moz-border-radius': '10px'
            }
            });
            window.setTimeout($.unblockUI, 2000);
            }
            else//{// save a byte!
            */
            GeneralAlertMsg(s);
            //}// save a byte!
        }
    }; //} });

    /*--------------------------------------------------------*/

    /*
    ### Additional Methods ###
    Required functionality outside the plugin's scope
    */

    // Native input reset method - because this alone doesn't always work: $(element).val('').attr('value', '')[0].value = '';
    $.fn.reset = function () { return this.each(function () { try { this.reset(); } catch (e) { } }); };

    /*--------------------------------------------------------*/

    /*
    ### Default implementation ###
    The plugin will attach itself to file inputs
    with the class 'multi' when the page loads
    */
    $(function () {
        //$("input:file.multi").MultiFile();
        $("input[type=file].multi").MultiFile();
    });



    /*# AVOID COLLISIONS #*/
})(jQuery);

function TotalSize() {
    var Total = 0;
    $(".sizecount").each(function () {
        Total += parseInt($(this).html());
    });
    return GetFileSize(Total);
}
function GetFileSize(CurrSize) {
    var iSize = (CurrSize / 1024);
    if (iSize / 1024 > 1) {
        if (((iSize / 1024) / 1024) > 1) {
            iSize = (Math.round(((iSize / 1024) / 1024) * 100) / 100) + "Gb";
        }
        else {
            iSize = (Math.round((iSize / 1024) * 100) / 100) + "Mb";
        }
    }
    else {
        iSize = (Math.round(iSize * 100) / 100) + "kb";
    }
    return iSize;
}
/*# AVOID COLLISIONS #*/
