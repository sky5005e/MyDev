(function($) {
    $.fn.extend({
        collapsiblePanel: function() {
            // Call the ConfigureCollapsiblePanel function for the selected element
            return $(this).each(ConfigureCollapsiblePanel);
        }
    });
    
    $.fn.extend({
        collapsibleInnerPanel: function() {
            // Call the ConfigureCollapsibleInnerPanel function for the selected element
            return $(this).each(ConfigureCollapsibleInnerPanel);
        }
    });
    
    $.fn.extend({
        collapsibleSubInnerPanel: function() {
            // Call the ConfigureCollapsibleInnerPanel function for the selected element
            return $(this).each(ConfigureCollapsibleSubInnerPanel);
        }
    });
})(jQuery);

function ConfigureCollapsiblePanel() {
    $(this).addClass("ui-widget");

    // Wrap the contents of the container within a new div.
    $(this).children().wrapAll("<div class='collapsibleContainerContent'></div>");

    // Create a new div as the first item within the container.  Put the title of the panel in here.
    $("<div class='collapsibleContainerTitle ui-widget-header'><div class='tab_headergr_r'><div" + ($(this).attr("total") != null ? " style='float:left;'" : "") + ">" + $(this).attr("title") + "</div>" + ($(this).attr("total") != null ? "<div style='text-align:right;color:red;'>" + $(this).attr("total") + "</div>" : "") +"</div>" + "</div>").prependTo($(this));

    // Assign a call to CollapsibleContainerTitleOnClick for the click event of the new title div.
    $(".collapsibleContainerTitle", this).click(CollapsibleContainerTitleOnClick);
}

function ConfigureCollapsibleInnerPanel() {
    $(this).addClass("ui-widget");

    // Wrap the contents of the container within a new div.
    $(this).children().wrapAll("<div class='collapsibleContainerInnerContent'></div>");

    // Create a new div as the first item within the container.  Put the title of the panel in here.
    $("<div class='collapsibleContainerInnerTitle ui-widget-header-dark'><div class='tab_headergr_r_dark'><div" + ($(this).attr("total") != null ? " style='float:left;'" : "") + ">" + $(this).attr("title") + "</div>" + ($(this).attr("total") != null ? "<div style='text-align:right;color:red;'>" + $(this).attr("total") + "</div>" : "") +"</div>" + "</div>").prependTo($(this));

    // Assign a call to CollapsibleContainerTitleOnClick for the click event of the new title div.
    $(".collapsibleContainerInnerTitle", this).click(CollapsibleContainerInnerTitleOnClick);
}

function ConfigureCollapsibleSubInnerPanel() {
    $(this).addClass("ui-widget");

    // Wrap the contents of the container within a new div.
    $(this).children().wrapAll("<div class='collapsibleContainerSubInnerContent'></div>");

    // Create a new div as the first item within the container.  Put the title of the panel in here.
    $("<div class='collapsibleContainerSubInnerTitle ui-widget-header-dark'><div class='tab_headergr_r_dark'><div" + ($(this).attr("total") != null ? " style='float:left;'" : "") + ">" + $(this).attr("title") + "</div>" + ($(this).attr("total") != null ? "<div style='text-align:right;color:red;'>" + $(this).attr("total") + "</div>" : "") +"</div>" + "</div>").prependTo($(this));

    // Assign a call to CollapsibleContainerTitleOnClick for the click event of the new title div.
    $(".collapsibleContainerSubInnerTitle", this).click(CollapsibleContainerSubInnerTitleOnClick);
}

function CollapsibleContainerTitleOnClick() {
    // The item clicked is the title div... get this parent (the overall container) and toggle the content within it.
    $(".collapsibleContainerContent", $(this).parent()).slideToggle();
}

function CollapsibleContainerInnerTitleOnClick() {
    // The item clicked is the title div... get this parent (the overall container) and toggle the content within it.
    $(".collapsibleContainerInnerContent", $(this).parent()).slideToggle();
}

function CollapsibleContainerSubInnerTitleOnClick() {
    // The item clicked is the title div... get this parent (the overall container) and toggle the content within it.
    $(".collapsibleContainerSubInnerContent", $(this).parent()).slideToggle();
}
