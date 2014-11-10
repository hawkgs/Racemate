$(function() {
    "use strict";

    var $profileMenu = $("#profile-menu"),
        $notifications = $("#notifications"),
        $notificationBtn = $(".nav-item.notifications"),
        CLICK_CLASS = "click",
        CLICKED_CLASS = "clicked",
        MENU_SPEED = 250;

    $("button, input[type='submit'], a.button")
        .mousedown(function() {
            $(this).addClass(CLICK_CLASS);
        })
        .mouseup(function() {
            $(this).removeClass(CLICK_CLASS);
        });

    $("#main-header").find(".profile-info").click(function() {
        $profileMenu.slideToggle(MENU_SPEED);

        return false;
    });

    $notificationBtn.click(function() {
        $notifications.slideToggle(MENU_SPEED, function() {
            if ($(this).css("display") === "none") {
                $notificationBtn.removeClass(CLICKED_CLASS);
            } else {
                $notificationBtn.addClass(CLICKED_CLASS);
            }
        });

        return false;
    });

    $(document).click(function(e) {
        if (!($(e.target).closest(".header-menu-holder").length > 0)) {
            $profileMenu.hide();
            $notifications.hide();

            if ($notificationBtn.hasClass(CLICKED_CLASS)) {
                $notificationBtn.removeClass(CLICKED_CLASS);
            }

            return true;
        }
    });
});