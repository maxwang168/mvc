
function showLoading(root) {
    var img = root + 'Content/Images/Wait.gif';
    $.blockUI({ message: '<h1><img src="' + img + '" /> 請稍候...</h1>' });
}

function hideLoading() {
    $.unblockUI();
}

function numFormat(num) {
    if (num == null || num == "") return "";
    num = num + "";
    var re = /(-?\d+)(\d{3})/;
    while (re.test(num)) {
        num = num.replace(re, "$1,$2");
    }
    return num;
}

$(document).ready(function (e) {
    if (navigator.appVersion.indexOf("MSIE 8") >= 0) {
        if (g_IsInternet != 'Y') {
            if (window.History && window.History.pushState) {
                $(window).on('popstate', function () {
                    window.History.pushState('forward', null, '#');
                    window.History.forward(1);
                });
            }
            window.History.pushState('forward', null, '#');
            window.History.forward(1);
        }
    }
    else {
        if (g_IsInternet != 'Y') {
            var counter = 0;
            if (window.history && window.history.pushState) {
                $(window).on('popstate', function () {
                    window.history.pushState('forward', null, '#');
                    window.history.forward(1);
                });
            }
            window.history.pushState('forward', null, '#');
            window.history.forward(1);
        }
    }
});
