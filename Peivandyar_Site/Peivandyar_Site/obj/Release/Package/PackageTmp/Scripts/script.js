$(window).load(function() {
    // Animate loader off screen
    $(".se-pre-con").fadeOut("slow");;
});

// When the DOM is ready, run this function
$(document).ready(function() {
  //Set the carousel options
  $('#quote-carousel').carousel({
    pause: true,
    interval: 1000,
  });
});

/*-------- Scroll To Up -------------------
$(document).ready(function(){
     $(window).scroll(function () {
        if ($(this).scrollTop() > 50) {
            $('#back-to-top').fadeIn();
        } else {
            $('#back-to-top').fadeOut();
        }
    });
    // scroll body to 0px on click
    $('#back-to-top').click(function () {
        $('#back-to-top').tooltip('hide');
        $('body,html').animate({
            scrollTop: 0
        }, 800);
        return false;
    });
    $('#back-to-top').tooltip('show');
});*/

/*-------- For Smoth Dropdown ----------*/
$('.dropdown').on('show.bs.dropdown', function(e){
  $(this).find('.dropdown-menu').first().stop(true, true).slideDown(200);
});

$('.dropdown').on('hide.bs.dropdown', function(e){
  $(this).find('.dropdown-menu').first().stop(true, true).slideUp(200);
});

/*------------------------ Wow Effect --------------------*/
$(document).ready(function(){
    wow = new WOW(
      {
        animateClass: 'animated',
        offset: 100,
        callback: function(box) {
          console.log("WOW: animating <" + box.tagName.toLowerCase() + ">")
        }
      }
    );
    wow.init();
});
/*------------------- Show and Hidden Navbar JS ------------------*/
(function($) {
  "use strict"; // Start of use strict
  // Show the navbar when the page is scrolled up
  var MQL = 992;
  //primary navigation slide-in effect
  if ($(window).width() > MQL) {
    var headerHeight = $('#main-nav').height();
    $(window).on('scroll', {
        previousTop: 0
      },
      function() {
        var currentTop = $(window).scrollTop();
        //check if user is scrolling up
        if (currentTop < this.previousTop) {
          //if scrolling up...
          if (currentTop > 0 && $('#main-nav').hasClass('is-fixed')) {
            $('#main-nav').addClass('is-visible');
          } else {
            $('#main-nav').removeClass('is-visible is-fixed');
          }
        } else if (currentTop > this.previousTop) {
          //if scrolling down...
          $('#main-nav').removeClass('is-visible');
          if (currentTop > headerHeight && !$('#main-nav').hasClass('is-fixed')) $('#main-nav').addClass('is-fixed');
        }
        this.previousTop = currentTop;
      });
  }

})(jQuery); // End of use strict
