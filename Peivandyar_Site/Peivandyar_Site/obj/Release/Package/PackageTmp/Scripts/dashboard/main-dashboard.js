
$(document).ready(function() {
    $('[data-toggle="offcanvas"]').click(function() {
      $('#side-menu').toggleClass('hidden-xs');
    });
});

/*-------- For Smoth Dropdown ----------*/
$('.dropdown').on('show.bs.dropdown', function(e){
  $(this).find('.dropdown-menu').first().stop(true, true).slideDown(200);
});

$('.dropdown').on('hide.bs.dropdown', function(e){
  $(this).find('.dropdown-menu').first().stop(true, true).slideUp(200);
});

/*----------------------------------------*/


/*------------------- For Change width in mobile view ---------------------*/

function myFunction(){
    var element = document.getElementById("main-body");
    if (element.classList) {
        element.classList.toggle("display-table-row");
    } else {
        var classes = element.className.split(" ");
        var i = classes.indexOf("display-table");

        if (i >= 0) 
            classes.splice(i, 1);
        else 
            classes.push("display-table");
            element.className = classes.join(" "); 
    }
}
/*------------------- Clickable Table Row ---------------------*/
jQuery(document).ready(function($) {
    $('.clickable-row').click(function() {
        window.location = $(this).data("href");
    });
});

/*------------------------ For Multi Choose Dropbox with Checkbox ----------------*/
        $('ul.dropdown-menu.my-check').on('click', function(event){
            var events = $._data(document, 'events') || {};
            events = events.click || [];
            for(var i = 0; i < events.length; i++) {
                if(events[i].selector) {

                    //Check if the clicked element matches the event selector
                    if($(event.target).is(events[i].selector)) {
                        events[i].handler.call(event.target, event);
                    }

                    // Check if any of the clicked element parents matches the 
                    // delegated event selector (Emulating propagation)
                    $(event.target).parents(events[i].selector).each(function(){
                        events[i].handler.call(this, event);
                    });
                }
            }
            event.stopPropagation(); //Always stop propagation
        });

/*------------------------ Chart JavaCode ----------------*/



/*------------------------ Star Function -----------------*/
