$(function () {
    "use strict";

    // Register all header pages with their routes and corresponding classes
    var pages = [{
        route: "/Administration/Reports",
        _class: "reports"
    }, {
        route: "/User/Race/List",
        _class: "races"
    }];

    pages.forEach(function (page) {
        if (window.location.pathname.indexOf(page.route) !== -1) {
            $(".nav-item." + page._class).addClass("clicked");
        }
    });
});