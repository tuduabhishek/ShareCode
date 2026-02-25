/*jshint browser:true*/

//
// jquery.sessionTimeout.js
//
// After a set amount of time, a dialog is shown to the user with the option
// to either log out now, or stay connected. If log out now is selected,
// the page is redirected to a logout URL. If stay connected is selected,
// a keep-alive URL is requested through AJAX. If no options is selected
// after another set amount of time, the page is automatically redirected
// to a timeout URL.
//
//
// USAGE
//
//   1. Include jQuery
//   2. Include jQuery UI (for dialog)
//   3. Include jquery.sessionTimeout.js
//   4. Call $.sessionTimeout(); after document ready
//
//
// OPTIONS
//
//   title
//     Title for the dialog.
//     Default: 'Your session is about to expire!'
//
//   message
//     Text shown to user in dialog after warning period.
//     Default: 'Your session is about to expire.'
//
//   titleMessage
//     Text shown in the browser title/tab bar via the via html/head/title attribute.
//     Default: 'Warning: Time Out'
//
//   stayConnectedBtn
//     Default: 'Stay connected'
//
//   logoutBtn
//     Default: 'Logout'
//
//   closeModals
//     Array of modla IDs to close prior to opening the time-out modal	
//     Default: 'array = []'
//
//   keepAliveUrl
//     URL to call through AJAX to keep session alive. This resource should do something innocuous that would keep the session alive, which will depend on your server-side platform.
//     Default: '/keep-alive'
//
//   redirUrl
//     URL to take browser to if no action is take after warning period
//     Default: '/timed-out'
//
//   logoutUrl
//     URL to take browser to if user clicks "Log Out Now"
//     Default: '/log-out'
//
//   warnAfter
//     Time in milliseconds after page is opened until warning dialog is opened
//     Default: 900000 (15 minutes)
//
//   redirAfter
//     Time in milliseconds after page is opened until browser is redirected to redirUrl
//     Default: 1200000 (20 minutes)
//
(function ($) {
    jQuery.sessionTimeout = function (options) {
        // DEFINE AN ARRAY TO STORE OTEHR MODALS TO CLOSE
        var otherModals = [];
        // DEFAULT CONFIG
        var defaults = {
            title: 'Your session is about to expire!',
            message: 'Your session is about to expire.',
            titleMessage: 'Warning: Time Out',
            stayConnectedBtn: 'Stay connected',
            logoutBtn: 'Logout',
            closeModals: otherModals,
            keepAliveUrl: '/keep-alive',
            redirUrl: '/timed-out',
            logoutUrl: '/log-out',
            warnAfter: 900000, // 15 minutes
            redirAfter: 1200000 // 20 minutes
        };

        // Extend user-set options over defaults
        var o = defaults,
				dialogTimer,
				redirTimer,
                countdownTimer;

        if (options) { o = $.extend(defaults, options); }

        // cache a reference to the countdown element so we don't have to query the DOM for it on each ping.
        var $countdown;
        // Create timeout warning dialog
        $('body').append('<div class="modal fade" id="sessionTimeout-dialog" data-bs-backdrop="static">'
							+ '<div class="modal-dialog modal-small">'
							+ '<div class="modal-content">'
							+ '<div class="modal-header">'
							+ '<h4 class="modal-title">' + o.title + '</h4>'
							+ '</div>'
							+ '<div class="modal-body"><p><i class="fa fa-warning text-danger"></i>' + o.message + ' in <span id="session-timeout-counter"></span> seconds.</p></div>'
							+ '<div class="modal-footer">'
							//+ '<button id="sessionTimeout-dialog-logout" type="button" class="btn btn-default">' + o.logoutBtn + '</button>'
							+ '<button id="sessionTimeout-dialog-keepalive" type="button" class="btn btn-primary" data-bs-dismiss="modal">' + o.stayConnectedBtn + '</button>'
							+ '</div>'
							+ '</div>'
							+ '</div>'
							+ '</div>');
        $('#sessionTimeout-dialog-logout').on('click', function () {
            window.location = o.logoutUrl;
        });

        $('#sessionTimeout-dialog').on('hide.bs.modal', function () {
            $.ajax({
                type: "POST",
                url: o.keepAliveUrl,
                data: '{name: "ReInit" }',
                contentType: "application/json; charset=utf-8"
            });
            // Stop redirect timer and restart warning timer
            controlRedirTimer('stop');
            controlDialogTimer('start');
        });

        function controlDialogTimer(action) {
            switch (action) {
                case 'start':
                    // After warning period, show dialog and start redirect timer
                    dialogTimer = setTimeout(function () {
                        // CLOSE ANY POTENTIALLY OPEN MODALS
                        $.each(o.closeModals, function (i, val) {
                            var mEl = document.getElementById(val);
                            if (mEl) {
                                var m = bootstrap.Modal.getInstance(mEl);
                                if (m) m.hide();
                            }
                        });

                        // SET HEAD TITLE MESSAGE
                        //document.title = o.titleMessage;
                        $countdown = $('#session-timeout-counter');
                        $countdown.html("");
                        var myModal = new bootstrap.Modal(document.getElementById('sessionTimeout-dialog'));
                        myModal.show();
                        controlRedirTimer('start');
                    }, o.warnAfter);
                    break;

                case 'stop':
                    clearTimeout(dialogTimer);
                    break;
            }
        }

        function controlRedirTimer(action) {
            switch (action) {
                case 'start':
                    var seconds = (o.redirAfter - o.warnAfter) / 1000;
                    countdownTimer = setInterval(function () {
                        seconds--;
                        $countdown.html(seconds);
                    }, 1000);
                    // Dialog has been shown, if no action taken during redir period, redirect
                    redirTimer = setTimeout(function () {
                        window.location = o.redirUrl;
                    }, o.redirAfter - o.warnAfter);
                    break;

                case 'stop':
                    clearInterval(countdownTimer);
                    clearTimeout(redirTimer);
                    break;
            }
        }

        // Begin warning period
        controlDialogTimer('start');
    };
})(jQuery);
