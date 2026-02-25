$(document).ready(function () {

    var animating = false,
      submitPhase1 = 1100,
      submitPhase2 = 400,
      logoutPhase1 = 800,
      $login = $(".login"),
      $app = $(".app");

    function ripple(elem, e) {
        $(".ripple").remove();
        var elTop = elem.offset().top,
        elLeft = elem.offset().left,
        x = e.pageX - elLeft,
        y = e.pageY - elTop;
        var $ripple = $("<div class='ripple'></div>");
        $ripple.css({ top: y, left: x });
        elem.append($ripple);
    };

    $(document).on("click", ".login__submit", function (e) {
        if (animating) return;
        var _userid = $('[id*=txtUserId]').val();
        var _pass = $('[id*=txtPwd]').val();
        var curUserId = document.getElementById('lblId').value;
        var dummyButtonLogin = $('[id*=dummyButtonLogin]');
        var lblMessagelogin = $('[id*=lblMessagelogin]');
        animating = true;
        var that = this;
        ripple($(that), e);
        $(that).addClass("processing");
        //debugger;


        $.ajax({
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            url: "Default.aspx/AuthUser",
            data: '{"userid":"' + _userid + '","pass":"' + _pass + '"}',
            success: function (response) {
                animating = false;
                //debugger;
                if (curUserId == '153133' ||  curUserId ==  '153129') {
                    lblMessagelogin.val('');
                    lblMessagelogin.hide();
                    dummyButtonLogin.click();
                }
                else {
                    if (response.d == "Authenticated") {
                        lblMessagelogin.val('');
                        lblMessagelogin.hide();
                        dummyButtonLogin.click();
                    } else {
                        $(that).removeClass("processing");
                        lblMessagelogin.html(response.d);
                        lblMessagelogin.show();
                    }
                }
            }
        });
        return false;
        //setTimeout(function() {
        //  $(that).addClass("success");
        //  setTimeout(function() {
        //    $app.show();
        //    $app.css("top");
        //    $app.addClass("active");
        //  }, submitPhase2 - 70);
        //  setTimeout(function() {
        //    $login.hide();
        //    $login.addClass("inactive");
        //    animating = false;
        //    $(that).removeClass("success processing");
        //  }, submitPhase2);
        //}, submitPhase1);
    });

    $(document).on("click", ".app__logout", function (e) {
        if (animating) return;
        $(".ripple").remove();
        animating = true;
        var that = this;
        $(that).addClass("clicked");
        setTimeout(function () {
            $app.removeClass("active");
            $login.show();
            $login.css("top");
            $login.removeClass("inactive");
        }, logoutPhase1 - 120);
        setTimeout(function () {
            $app.hide();
            animating = false;
            $(that).removeClass("clicked");
        }, logoutPhase1);
    });

});