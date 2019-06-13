/*-------- For Smoth Dropdown ----------*/
$('.dropdown').on('show.bs.dropdown', function(e){
  $(this).find('.dropdown-menu').first().stop(true, true).slideDown(100);
});

$('.dropdown').on('hide.bs.dropdown', function(e){
  $(this).find('.dropdown-menu').first().stop(true, true).slideUp(100);
});

/*-------- Multi Dropdown-Subdown -------------*/
$('.dropdown-menu a.dropdown-toggle').on('click', function(e) {
  if (!$(this).next().hasClass('show')) {
    $(this).parents('.dropdown-menu').first().find('.show').removeClass("show");
  }
  var $subMenu = $(this).next(".dropdown-menu");
  $subMenu.toggleClass('show');

  $(this).parents('li.nav-item.dropdown.show').on('hidden.bs.dropdown', function(e) {
    $('.dropdown-submenu .show').removeClass("show");
  });

  return false;
});

/*-----------------------------------------------*/
$(document).ready(function () {
    "use strict";
    $('.menu > ul > li:has( > ul)').addClass('menu-dropdown-icon');

    $('.menu > ul > li > ul:not(:has(ul))').addClass('normal-sub');
    $(".menu > ul").before("<a href=\"#\" class=\"menu-mobile\"></a>");
    $(".menu > ul > li").hover(function (e) {
        if ($(window).width() > 943) {
            $(this).children("ul").stop(true, false).fadeToggle(150);
            e.preventDefault();
        }
    });
    $(".menu > ul > li").click(function () {
        if ($(window).width() <= 943) {
            $(this).children("ul").fadeToggle(150);
        }
    });
    $(".menu-mobile").click(function (e) {
        $(".menu > ul").toggleClass('show-on-mobile');
        e.preventDefault();
    });
});