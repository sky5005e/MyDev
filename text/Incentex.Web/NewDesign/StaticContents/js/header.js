$(document).ready(function() {
    // niceScroll
    function cartscroll() {
        $("#header-cart").niceScroll(
			{
			    touchbehavior: false,
			    cursorcolor: "#666",
			    cursoropacitymax: 0.7,
			    cursorwidth: 6,
			    cursorborder: "1px solid #666",
			    cursorborderradius: "8px",
			    autohidemode: "scroll"
			});
    }

    $(".innernNavigation > li").hover(function() {
        cartscroll();
        $(this).children('.header-cartNav').show();
    }, function() {
        $('.header-cartNav').hide();
    })
    $(".close-cart").click(function() {
        $('.header-cartNav').hide();

    });



    function cartscroll1() {
        $("#search-showblock").niceScroll(
			{
			    touchbehavior: false,
			    cursorcolor: "#666",
			    cursoropacitymax: 0.7,
			    cursorwidth: 6,
			    cursorborder: "1px solid #666",
			    cursorborderradius: "8px",
			    autohidemode: "scroll"
			});
    }

    $(".search-field input").focus(function() {
        $(this).closest('.search-field').addClass('active');
        cartscroll1();
        $(".search-showblock").css('visibility', 'visible');
        $(this).attr('title', 'TYPING...');
        $(this).attr('placeholder', 'TYPING...');
    });

    $(".close-pop").click(function(e) {
        e.preventDefault();
        e.stopPropagation();
        e.stopImmediatePropagation();
        $(".search-field input").val('');
        $(".search-field input").attr('title', 'Search...');
        $(".search-field input").attr('placeholder', 'Search...');
    });

    $(".search-field input").blur(function() {
        $(this).closest('.search-field').removeClass('active');
        cartscroll1();
        $(".search-showblock").css('visibility', 'hidden');
        $(this).attr('title', 'SEARCH SYSTEM...');
        $(this).attr('placeholder', 'SEARCH SYSTEM...');
    });
});


