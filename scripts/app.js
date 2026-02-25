/* app
*
* @type Object
* @description $.app is the main object for the template's app.
*              It's used for implementing functions and options related
*              to the template. Keeping everything wrapped in an object
*              prevents conflict with other plugins and is a better
*              way to organize our code.
*/
$.app = {};
/* ------------------
* - Implementation -
* ------------------
*/
$(function () {
    //Set up the object
    _init();
    // $.app.initClock.startClock();
    $.app.SidebarMenuHandler.sidebarMenu();
    $.app.SidebarMenuHandler.sidebarToggler();
    $.app.GoToTop.handleGoTop();
    $.app.layoutOptions.initTooltip();
    $.app.MasterPageDropDown();
    $.app.initCheckbox();
//    $("html").niceScroll();
//    $('#notificationList').niceScroll();
   
});
/* ----------------------------------
* - Initialize the app Object -
* ----------------------------------
* All app functions are implemented below.
*/
function _init() {
    $.app.MasterPageDropDown = function () {
        $('[id*=ddlMonths]').selectpicker({
            style: 'btn btn-outline red ',
            width: '60px'
        });
        $('[id*=ddlFYear]').selectpicker({
            style: 'btn btn-outline red ',
            width: '100px'
        });
        $('button[data-id="ddlMonths"]').next('.dropdown-menu').width('80px');
        $('button[data-id="ddlFYear"]').next('.dropdown-menu').width('150px');
    }
    $.app.GoToTop = {

        // Handles the go to top button at the footer
        handleGoTop: function () {
            var offset = 300;
            var duration = 500;
            // var the = $(this);
            $(window).scroll(function () {
                if ($(this).scrollTop() > offset) {
                    $('.go-to-top').fadeIn(duration);
                } else {
                    $('.go-to-top').fadeOut(duration);
                }
            });
            $('.go-to-top').click(function (e) {
                e.preventDefault();
                $('html, body').animate({
                    scrollTop: 0
                }, duration);
                return false;
            });
        }
    };
    // Handle sidebar menu
    $.app.SidebarMenuHandler = {
        sidebarMenu: function () {
            if ($.cookie && $.cookie('activeURL') === $(location).attr('href').split('/')[4]) {
                var activeURL = $.cookie('activeURL');
                var el = $("[href='" + activeURL + "']");
                el.parents('li').each(function () {
                    $(this).addClass('active');
                    $(this).find('> a > span.arrow').addClass('open');

                    if ($(this).parent('ul.sidebar-menu').size() === 1) {
                        $(this).find('> a').append('<span class="selected"></span>');
                    }

                    if ($(this).children('ul.sub-menu').size() === 1) {
                        $(this).addClass('open');
                    }
                });
            }
            $('.sidebar').on('click', 'li > a', function (e) {
                if ($.app.WindowSize.getWindowDimension().width >= $.app.ScreenSizes.screenSizes('md') && $(this).parents('.sidebar-menu-hover-submenu').size() === 1) { // exit of hover sidebar menu
                    $.app.SidebarMenuHandler.setActiveState($(this));
                    return;
                }

                if ($(this).next().hasClass('sub-menu') === false) {
                    if ($.app.WindowSize.getWindowDimension().width < $.app.ScreenSizes.screenSizes('md') && $('.sidebar').hasClass("in")) { // close the menu on mobile view while laoding a page 
                        $.app.SidebarMenuHandler.setActiveState($(this));
                        $('.header .responsive-toggler').click();
                    }
                    return;
                }

                if ($(this).next().hasClass('sub-menu always-open')) {
                    $.app.SidebarMenuHandler.setActiveState($(this));
                    return;
                }

                var parent = $(this).parent().parent();
                var the = $(this);
                var menu = $('.sidebar-menu');
                var sub = $(this).next();

                var autoScroll = menu.data("auto-scroll");
                var slideSpeed = parseInt(menu.data("slide-speed"));
                var keepExpand = menu.data("keep-expanded");

                if (keepExpand !== true) {
                    parent.children('li.open').children('a').children('.arrow').removeClass('open');
                    parent.children('li.open').children('.sub-menu:not(.always-open)').slideUp(slideSpeed);
                    parent.children('li.open').removeClass('open');
                }

                var slideOffeset = -200;

                if (sub.is(":visible")) {
                    $('.arrow', $(this)).removeClass("open");
                    $(this).parent().removeClass("open");
                    sub.slideUp(slideSpeed, function () {
                        if (autoScroll === true && $('body').hasClass('sidebar-closed') === false) {
                            $.app.Scroller.scrollTo(the, slideOffeset);
                        }
                    });
                } else {
                    $('.arrow', $(this)).addClass("open");
                    $(this).parent().addClass("open");
                    sub.slideDown(slideSpeed, function () {
                        if (autoScroll === true && $('body').hasClass('sidebar-closed') === false) {
                            $.app.Scroller.scrollTo(the, slideOffeset);
                        }
                    });
                }

                e.preventDefault();
            });
        },
        // Hanles sidebar toggler
        sidebarToggler: function () {
            var body = $('body');
            if ($.cookie && $.cookie('sidebar_closed') === '1' && $.app.WindowSize.getWindowDimension().width >= $.app.ScreenSizes.screenSizes('md')) {
                $('body').addClass('sidebar-closed');
                $('.sidebar-menu').addClass('sidebar-menu-closed');
                $('#menu-trigger').removeClass('open');
            }

            // handle sidebar show/hide
            $('body').on('click', '.sidebar-toggler', function (e) {
                var sidebar = $('.sidebar');
                var sidebarMenu = $('.sidebar-menu');
                var menuToggler = $(this);
                if (body.hasClass("sidebar-closed")) {
                    body.removeClass("sidebar-closed");
                    sidebarMenu.removeClass("sidebar-menu-closed");
                    menuToggler.addClass('open');
                    if ($.cookie) {
                        $.cookie('sidebar_closed', '0');
                    }
                } else {
                    body.addClass("sidebar-closed");
                    sidebarMenu.addClass("sidebar-menu-closed");
                    menuToggler.removeClass('open');
                    if ($.cookie) {
                        $.cookie('sidebar_closed', '1');
                    }
                }

                $(window).trigger('resize');
            });

        },

        setActiveState: function (el) {
            var menu = $('.sidebar-menu');
            var linkURL = el.attr('href');
            if (!el || el.size() == 0) {
                return;
            }

            if (linkURL.toLowerCase() === 'javascript:;' || linkURL.toLowerCase() === '#') {
                return;
            }

            // disable active states
            menu.find('li.active').removeClass('active');
            menu.find('li > a > .selected').remove();

            if (menu.hasClass('sidebar-menu-hover-submenu') === false) {
                menu.find('li.open').each(function () {
                    if ($(this).children('.sub-menu').size() === 0) {
                        $(this).removeClass('open');
                        $(this).find('> a > .arrow.open').removeClass('open');
                    }
                });
            } else {
                menu.find('li.open').removeClass('open');
            }

            el.parents('li').each(function () {
                $(this).addClass('active');
                $(this).find('> a > span.arrow').addClass('open');

                if ($(this).parent('ul.sidebar-menu').size() === 1) {
                    $(this).find('> a').append('<span class="selected"></span>');
                }

                if ($(this).children('ul.sub-menu').size() === 1) {
                    $(this).addClass('open');
                }
                if ($.cookie) {
                    $.cookie('activeURL', linkURL);
                }
            });

        }
    };

    $.app.WindowSize = {
        getWindowDimension: function () {
            var e = window,
                a = 'inner';
            if (!('innerWidth' in window)) {
                a = 'client';
                e = document.documentElement || document.body;
            }

            return {
                width: e[a + 'Width'],
                height: e[a + 'Height']
            };
        }
    };
    $.app.ScreenSizes = {
        screenSizes: function (size) {
            // bootstrap screen sizes
            var sizes = {
                'xs': 480,     // extra small
                'sm': 768,     // small
                'md': 991,     // medium
                'lg': 1200     // large
            };

            return sizes[size] ? sizes[size] : 0;
        }
    };
    $.app.layoutOptions = {
        initTooltip: function () {
            $('[data-toggle="tooltip"]').tooltip();
            $('[data-tooltip="tooltip"]').tooltip();
        }
        //        activateradiobutton: function () {


        //            
        //        }


    };
    $.app.Scroller = {

        //scroll to top
        scrollTo: function (el, offeset) {
            var pos = (el && el.size() > 0) ? el.offset().top : 0;

            if (el) {
                if ($('body').hasClass('header-fixed')) {
                    pos = pos - $('.header').height();
                }
                pos = pos + (offeset ? offeset : -1 * el.height());
            }

            $('html,body').animate({
                scrollTop: pos
            }, 'slow');
        },
        // function to scroll to the top
        scrollTop: function () {
            scrollTo();
        }
    };

    $.fn.scrollTo = function (target, options, callback) {
        if (typeof options == 'function' && arguments.length == 2) { callback = options; options = target; }
        var settings = $.extend({
            scrollTarget: target,
            offsetTop: 50,
            duration: 500,
            easing: 'swing'
        }, options);
        return this.each(function () {
            var scrollPane = $(this);
            var scrollTarget = (typeof settings.scrollTarget == "number") ? settings.scrollTarget : $(settings.scrollTarget);
            var scrollY = (typeof scrollTarget == "number") ? scrollTarget : scrollTarget.offset().top + scrollPane.scrollTop() - parseInt(settings.offsetTop);
            scrollPane.animate({ scrollTop: scrollY }, parseInt(settings.duration), settings.easing, function () {
                if (typeof callback == 'function') { callback.call(this); }
            });
        });
    }


    // **** Collapsible panel button click **** //
    $('body').on('click', '.control-panel > .control-panel-title > .tools > .collapse, .control-panel .control-panel-title > .tools > .expand', function (e) {
        e.preventDefault();
        var el = $(this).closest(".control-panel").children(".control-panel-body");
        if ($(this).hasClass("collapse")) {
            $(this).removeClass("collapse icon icon-arrow-up").addClass("expand icon icon-arrow-down");
            el.slideUp(200);
        } else {
            $(this).removeClass("expand icon icon-arrow-down").addClass("collapse icon icon-arrow-up");
            el.slideDown(200);
        }
    });
    $('body').on('click', '.custom-nav > .nav > .collapse,.custom-nav > .nav > .expand', function (e) {
        e.preventDefault();
        var el = $(this).closest(".custom-nav").children(".tab-content");
        if ($(this).hasClass("collapse")) {
            $(this).removeClass("collapse icon icon-arrow-up").addClass("expand icon icon-arrow-down");
            el.slideUp(200);
        } else {
            $(this).removeClass("expand icon icon-arrow-down").addClass("collapse icon icon-arrow-up");
            el.slideDown(200);
        }
    });


    $('body').on('click', '#responsive-trigger', function (e) {
        e.preventDefault();
        $(this).toggleClass('open');
    });
    $(".right-sidebar-toggler-outside").click(function (i) {
        $("body").toggleClass("right-sidebar-open");
        var icon = $(this).find('i');
        if ($.app.WindowSize.getWindowDimension().width >= $.app.ScreenSizes.screenSizes('md')) {
            if (icon.hasClass("icon-arrow-left")) {
                icon.removeClass("icon-arrow-left");
                icon.addClass("icon-arrow-right")
            } else {
                icon.removeClass("icon-arrow-right");
                icon.addClass("icon-arrow-left")
            }
        } else {
            icon.removeClass();
            icon.addClass("icon-arrow-left");
        }
       
    });
   
    $(".right-sidebar-toggler").click(function (i) {
        $("body").toggleClass("right-sidebar-open");
    });
    $('.right-sidebar-list').niceScroll();

    $('body').on('click', '#top-search > a', function (e) {
        e.preventDefault();

        $('.header').addClass('search-toggled');
    });

    $('body').on('click', '#top-search-close', function (e) {
        e.preventDefault();

        $('.header').removeClass('search-toggled');
    });
    //$('.accordion-toggle').click(function (e) {
        
    //    e.preventDefault();
    //    var icon = $(this).children("i");
    //    if ($(this).hasClass("collapsed")) {
    //        icon.removeClass("icon-arrow-down").addClass("icon-arrow-up");

    //    } else {
    //        icon.removeClass("icon-arrow-up").addClass("icon-arrow-down");

    //    }
    //});
}



// function to indicate loading
$.app.loading = function (options) {
    options = $.extend(true, {}, options);
    var html = '';
    html = '<div class="loading-message ' + (options.boxed ? 'loading-message-boxed' : '') + '">' + '<div class="loader"></div>' + '</div>';
    if (options.target) { // element blocking
        var el = $(options.target);
        if (el.height() <= ($(window).height())) {
            options.centerY = true;
        }
        el.block({
            message: html,
            baseZ: options.zIndex ? options.zIndex : 1000,
            centerY: options.centerY !== undefined ? options.centerY : false,
            css: {
                top: '10%',
                border: '0',
                padding: '0',
                backgroundColor: 'none'
            },
            overlayCSS: {
                backgroundColor: options.overlayColor ? options.overlayColor : '#FAFAFA',
                opacity: options.boxed ? 0.05 : 0.8,
                cursor: 'wait'
            }
        });
    } else { // page blocking
        $.blockUI({
            message: html,
            baseZ: options.zIndex ? options.zIndex : 1000,
            css: {
                border: '0',
                padding: '0',
                backgroundColor: 'none'
            },
            overlayCSS: {
                backgroundColor: options.overlayColor ? options.overlayColor : '#FAFAFA',
                opacity: options.boxed ? 0.05 : 0.8,
                cursor: 'wait'
            }
        });
    }
}

// function to finish loading
$.app.unloading = function (target) {
    if (target) {
        $(target).unblock({
            onUnblock: function () {
                $(target).css('position', '');
                $(target).css('zoom', '');
            }
        });
    } else {
        $.unblockUI();
    }
}

$.app.initCheckbox = function () {
    // **** iCheck plugin initialize **** //
    $('.iCheck').iCheck({
        checkboxClass: 'icheckbox_minimal-blue',
        radioClass: 'iradio_minimal-blue'

    });
};
$.app.initDateTimePicker = function () {
    //****** Date Timepicker initialize *****//
    $('.datetimepicker').datetimepicker({
        pickTime: false
    });
};
//function analogClock() {
//}
//analogClock.prototype.run = function () {
//    var date = new Date();
//    var second = date.getSeconds() * 6;
//    var minute = date.getMinutes() * 6 + second / 60;
//    var hour = ((date.getHours() % 12) / 12) * 360 + 90 + minute / 12;
//    jQuery('#hour').css("transform", "rotate(" + hour + "deg)");
//    jQuery('#minute').css("transform", "rotate(" + minute + "deg)");
//    jQuery('#second').css("transform", "rotate(" + second + "deg)");

//};

